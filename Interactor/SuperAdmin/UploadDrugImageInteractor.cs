using Domain.SuperAdminModels.SystemChangeLog;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using UseCase.SuperAdmin.UploadDrugImage;

namespace Interactor.SuperAdmin;

public class UploadDrugImageInteractor : IUploadDrugImageInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IConfiguration _configuration;
    private readonly ISystemChangeLogRepository _systemChangeLogRepository;
    private readonly AmazonS3Options _options;
    private IMessenger? _messenger;
    bool isStopProgress = false;

    public UploadDrugImageInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, IConfiguration configuration, ISystemChangeLogRepository systemChangeLogRepository)
    {
        _options = optionsAccessor.Value;
        _amazonS3Service = amazonS3Service;
        _configuration = configuration;
        _systemChangeLogRepository = systemChangeLogRepository;
    }

    public UploadDrugImageOutputData Handle(UploadDrugImageInputData inputData)
    {
        string pathFolderUpdateDataTenant = _configuration["PathFolderUpdateDataTenant"] ?? string.Empty;
        string pathFile7z = $"{pathFolderUpdateDataTenant}\\{Guid.NewGuid()}.7z";
        string pathFileExtract7z = $"{pathFolderUpdateDataTenant}\\7Z-{Guid.NewGuid()}";
        List<string> fileUploaded = new();
        string filename = string.Empty;
        int successCount = 0;
        try
        {
            _messenger = inputData.Messenger;

            if (inputData.FileUpdateData == null || !string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
            {
                SendMessager(new UploadDrugImageStatus(true, 0, 0, string.Empty, string.Empty, "This file is null or not in the correct format."));
                return new UploadDrugImageOutputData();
            }

            //// Save file 7z
            //using (var fileStream = new FileStream(pathFile7z, FileMode.Create))
            //{
            //    inputData.FileUpdateData.CopyTo(fileStream);
            //}

            //// Extract file 7z
            //using (var archive = SevenZipArchive.Open(pathFile7z))
            //{
            //    archive.ExtractToDirectory(pathFileExtract7z);
            //}

            pathFileExtract7z = "D:\\7Z-82dc264a-f049-4896-bb66-74d357dae3e5";
            // get folder to upload file
            pathFileExtract7z = $"{pathFileExtract7z}\\{CommonConstants.Drug_photo_05}";
            string housouPath = $"{pathFileExtract7z}\\{CommonConstants.Housou}";
            string zaikeiPath = $"{pathFileExtract7z}\\{CommonConstants.ZaiKei}";

            string uploadFilePath = $"{CommonConstants.Image}/{CommonConstants.Reference}/{CommonConstants.DrugPhoto}";
            string uploadHousouFilePath = $"{uploadFilePath}/{CommonConstants.Housou}/";
            string uploadZaiKeiFilePath = $"{uploadFilePath}/{CommonConstants.ZaiKei}/";

            var housouFileList = Directory.GetFiles(housouPath).ToList();
            var zaikeiFileList = Directory.GetFiles(zaikeiPath).ToList();
            var totalFile = housouFileList.Count + zaikeiFileList.Count;

            // upload housouFile
            foreach (var strPath in housouFileList)
            {
                // Check is stop progerss
                var statusCallBack = _messenger!.SendAsync(new StopUploadDrugImage());
                isStopProgress = statusCallBack.Result.Result;
                if (isStopProgress)
                {
                    break;
                }
                filename = Path.GetFileName(strPath);
                fileUploaded.Add($"{uploadHousouFilePath}{filename}");
                byte[] file = File.ReadAllBytes(strPath);
                MemoryStream memory = new MemoryStream(file);
                var response = _amazonS3Service.UploadObjectAsync(uploadHousouFilePath, filename, memory);
                response.Wait();

                successCount++;
                if (totalFile == successCount)
                {
                    break;
                }
                SendMessager(new UploadDrugImageStatus(false, totalFile, successCount, CommonConstants.Housou, filename, string.Empty));
            }

            // upload zaikeiFile
            if (!isStopProgress)
            {
                foreach (var strPath in zaikeiFileList)
                {
                    // Check is stop progerss
                    var statusCallBack = _messenger!.SendAsync(new StopUploadDrugImage());
                    isStopProgress = statusCallBack.Result.Result;
                    if (isStopProgress)
                    {
                        break;
                    }

                    filename = Path.GetFileName(strPath);
                    fileUploaded.Add($"{uploadZaiKeiFilePath}{filename}");
                    byte[] file = File.ReadAllBytes(strPath);
                    MemoryStream memory = new MemoryStream(file);
                    var response = _amazonS3Service.UploadObjectAsync(uploadZaiKeiFilePath, filename, memory);
                    response.Wait();

                    successCount++;
                    if (totalFile == successCount)
                    {
                        break;
                    }
                    SendMessager(new UploadDrugImageStatus(false, totalFile, successCount, CommonConstants.ZaiKei, filename, string.Empty));
                }
                SendMessager(new UploadDrugImageStatus(true, totalFile, successCount, CommonConstants.Housou, filename, string.Empty));
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
            return new UploadDrugImageOutputData();
        }
        finally
        {
            _amazonS3Service.Dispose();
            _systemChangeLogRepository.ReleaseResource();

            // delete file temp
            //File.Delete(pathFile7z);
            //Directory.Delete(pathFileExtract7z.Replace($"\\{CommonConstants.Drug_photo_05}", string.Empty), true);
        }
    }

    private void SendMessager(UploadDrugImageStatus status)
    {
        _messenger!.Send(status);
    }
}
