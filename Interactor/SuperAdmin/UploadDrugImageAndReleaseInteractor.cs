using Domain.SuperAdminModels.SystemChangeLog;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using UseCase.SuperAdmin.UploadDrugImage;

namespace Interactor.SuperAdmin;

public class UploadDrugImageAndReleaseInteractor : IUploadDrugImageAndReleaseInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IConfiguration _configuration;
    private readonly ISystemChangeLogRepository _systemChangeLogRepository;
    private IMessenger? _messenger;
    private bool isStopProgress = false;
    private readonly List<string> fileUploaded = new();
    private string filename = string.Empty, folderName = string.Empty;
    private int successCount = 0, totalFile = 0;

    public UploadDrugImageAndReleaseInteractor(IAmazonS3Service amazonS3Service, IConfiguration configuration, ISystemChangeLogRepository systemChangeLogRepository)
    {
        _amazonS3Service = amazonS3Service;
        _configuration = configuration;
        _systemChangeLogRepository = systemChangeLogRepository;
    }

    public UploadDrugImageAndReleaseOutputData Handle(UploadDrugImageAndReleaseInputData inputData)
    {
        string pathFolderUpdateDataTenant = _configuration["PathFolderUpdateDataTenant"] ?? string.Empty;
        string pathFile7z = $"{pathFolderUpdateDataTenant}\\{Guid.NewGuid()}.7z";
        string pathFileExtract7z = $"{pathFolderUpdateDataTenant}\\7Z-{Guid.NewGuid()}";
        string uploadHousouFilePath = string.Empty, uploadZaiKeiFilePath = string.Empty, uploadFilePath = string.Empty;
        List<string> housouFileList = new(), zaikeiFileList = new(), releaseFileList = new();
        try
        {
            _messenger = inputData.Messenger;

            if (inputData.FileUpdateData == null || !string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
            {
                SendMessager(new UploadDrugImageAndReleaseStatus(true, 0, 0, string.Empty, string.Empty, "This file is null or not in the correct format."));
                return new UploadDrugImageAndReleaseOutputData();
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

            // check exist folder 
            pathFileExtract7z = $"{pathFileExtract7z}\\{CommonConstants.Updfile}";
            var allFolder = Directory.GetDirectories(pathFileExtract7z).ToList();

            // get folder to upload drug image
            if (allFolder.Contains($"{pathFileExtract7z}\\{CommonConstants.Drug_photo_05}"))
            {
                string drugImage7zPage = $"{pathFileExtract7z}\\{CommonConstants.Drug_photo_05}";
                string housouPath = $"{drugImage7zPage}\\{CommonConstants.HouSou}";
                string zaikeiPath = $"{drugImage7zPage}\\{CommonConstants.ZaiKei}";
                uploadFilePath = $"{CommonConstants.Image}/{CommonConstants.Reference}/{CommonConstants.DrugPhoto}";
                uploadHousouFilePath = $"{uploadFilePath}/{CommonConstants.HouSou}/";
                uploadZaiKeiFilePath = $"{uploadFilePath}/{CommonConstants.ZaiKei}/";

                housouFileList = Directory.GetFiles(housouPath).ToList();
                zaikeiFileList = Directory.GetFiles(zaikeiPath).ToList();
            }

            // get folder to upload release file
            if (allFolder.Contains($"{pathFileExtract7z}\\{CommonConstants.Release_99}"))
            {
                string releaseFile7zPath = $"{pathFileExtract7z}\\{CommonConstants.Release_99}";
                uploadFilePath = $"{CommonConstants.Common}/{CommonConstants.Release_Version}/";

                releaseFileList = Directory.GetFiles(releaseFile7zPath).ToList();
            }

            totalFile = housouFileList.Count + zaikeiFileList.Count + releaseFileList.Count;

            // upload release file action
            folderName = CommonConstants.Release_99;
            UploadFileAction(releaseFileList, uploadFilePath);

            // check if continue progress
            if (!isStopProgress)
            {
                // upload housou drug image
                folderName = CommonConstants.Drug_photo_05 + "/" + CommonConstants.HouSou;
                UploadFileAction(housouFileList, uploadHousouFilePath);
            }

            // check if continue progress
            if (!isStopProgress)
            {
                // upload zaikei drug image
                folderName = CommonConstants.Drug_photo_05 + "/" + CommonConstants.ZaiKei;
                UploadFileAction(zaikeiFileList, uploadZaiKeiFilePath);

                // return success message
                SendMessager(new UploadDrugImageAndReleaseStatus(true, totalFile, successCount, folderName, filename, string.Empty));
            }

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
            SendMessager(new UploadDrugImageAndReleaseStatus(false, totalFile, successCount, folderName, filename, ex.Message));
        }
        finally
        {
            _amazonS3Service.Dispose();
            _systemChangeLogRepository.ReleaseResource();

            //delete file temp
            File.Delete(pathFile7z);
            Directory.Delete(pathFileExtract7z.Replace($"\\{CommonConstants.Updfile}", string.Empty), true);
        }
        return new UploadDrugImageAndReleaseOutputData();
    }

    /// <summary>
    /// UploadFileAction
    /// </summary>
    /// <param name="sourceFileList"></param>
    /// <param name="uploadFilePath"></param>
    private void UploadFileAction(List<string> sourceFileList, string uploadFilePath)
    {
        foreach (var strPath in sourceFileList)
        {
            // Check is stop progerss
            var statusCallBack = _messenger!.SendAsync(new StopUploadDrugImageAndRelease());
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

            // return message
            SendMessager(new UploadDrugImageAndReleaseStatus(false, totalFile, successCount, folderName, filename, string.Empty));
        }
    }

    /// <summary>
    /// Send Message to controller
    /// </summary>
    /// <param name="status"></param>
    private void SendMessager(UploadDrugImageAndReleaseStatus status)
    {
        _messenger!.Send(status);
    }
}
