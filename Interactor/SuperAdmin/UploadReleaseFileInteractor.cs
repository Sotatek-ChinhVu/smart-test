using Domain.SuperAdminModels.SystemChangeLog;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using UseCase.SuperAdmin.UploadReleaseFile;

namespace Interactor.SuperAdmin;

public class UploadReleaseFileInteractor : IUploadReleaseFileInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IConfiguration _configuration;
    private readonly ISystemChangeLogRepository _systemChangeLogRepository;
    private IMessenger? _messenger;
    private bool isStopProgress = false;
    private readonly List<string> fileUploaded = new();
    private string filename = string.Empty;
    private int successCount = 0, totalFile = 0;

    public UploadReleaseFileInteractor(IAmazonS3Service amazonS3Service, IConfiguration configuration, ISystemChangeLogRepository systemChangeLogRepository)
    {
        _amazonS3Service = amazonS3Service;
        _configuration = configuration;
        _systemChangeLogRepository = systemChangeLogRepository;
    }

    public UploadReleaseFileOutputData Handle(UploadReleaseFileInputData inputData)
    {
        string pathFolderUpdateDataTenant = _configuration["PathFolderUpdateDataTenant"] ?? string.Empty;
        string pathFile7z = $"{pathFolderUpdateDataTenant}\\{Guid.NewGuid()}.7z";
        string pathFileExtract7z = $"{pathFolderUpdateDataTenant}\\7Z-{Guid.NewGuid()}";
        try
        {
            _messenger = inputData.Messenger;

            if (inputData.FileUpdateData == null || !string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
            {
                SendMessager(new UploadReleaseFileStatus(true, 0, 0, string.Empty, "This file is null or not in the correct format."));
                return new UploadReleaseFileOutputData();
            }

            // Save file 7z
            using (var fileStream = new FileStream(pathFile7z, FileMode.Create))
            {
                inputData.FileUpdateData.CopyTo(fileStream);
            }

            // Extract file 7z
            using (var archive = SevenZipArchive.Open(pathFile7z))
            {
                archive.ExtractToDirectory(pathFileExtract7z);
            }

            // get folder to upload file
            string uploadFilePath = $"{CommonConstants.Common}/{CommonConstants.Release_Version}/";
            pathFileExtract7z = $"{pathFileExtract7z}\\{CommonConstants.Release_99}";

            var releaseFileList = Directory.GetFiles(pathFileExtract7z).ToList();
            totalFile = releaseFileList.Count;

            // upload release file
            UploadImageFileAction(releaseFileList, uploadFilePath);

            // return success message
            SendMessager(new UploadReleaseFileStatus(true, totalFile, successCount, filename, string.Empty));

            // if stop progress, revert data
            if (isStopProgress)
            {
                foreach (var deletedFile in fileUploaded)
                {
                    var response = _amazonS3Service.DeleteObjectAsync(deletedFile);
                    response.Wait();
                }
            }
        }
        catch (Exception ex)
        {
            // return error message
            SendMessager(new UploadReleaseFileStatus(false, totalFile, successCount, filename, ex.Message));
        }
        finally
        {
            _amazonS3Service.Dispose();
            _systemChangeLogRepository.ReleaseResource();

            //delete file temp
            File.Delete(pathFile7z);
            Directory.Delete(pathFileExtract7z.Replace($"\\{CommonConstants.Release_99}", string.Empty), true);
        }
        return new UploadReleaseFileOutputData();
    }

    private void UploadImageFileAction(List<string> releaseFileList, string uploadFilePath)
    {
        foreach (var strPath in releaseFileList)
        {
            // Check is stop progerss
            var statusCallBack = _messenger!.SendAsync(new StopUploadReleaseFile());
            isStopProgress = statusCallBack.Result.Result;
            if (isStopProgress)
            {
                break;
            }

            // upload file to S3
            filename = Path.GetFileName(strPath);
            fileUploaded.Add($"{uploadFilePath}{filename}");
            byte[] file = File.ReadAllBytes(strPath);
            MemoryStream memory = new MemoryStream(file);
            var response = _amazonS3Service.UploadObjectAsync(uploadFilePath, filename, memory);
            response.Wait();

            successCount++;
            if (totalFile == successCount)
            {
                break;
            }

            // return message to controller
            SendMessager(new UploadReleaseFileStatus(false, totalFile, successCount, filename, string.Empty));
        }
    }

    private void SendMessager(UploadReleaseFileStatus status)
    {
        _messenger!.Send(status);
    }
}
