﻿using Domain.SuperAdminModels.SystemChangeLog;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using UseCase.SuperAdmin.UploadDrugImage;

namespace Interactor.SuperAdmin;

public class UploadDrugImageInteractor : IUploadDrugImageInputPort
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly IConfiguration _configuration;
    private readonly ISystemChangeLogRepository _systemChangeLogRepository;
    private IMessenger? _messenger;
    private bool isStopProgress = false;
    private readonly List<string> fileUploaded = new();
    private string filename = string.Empty, folderName = string.Empty;
    private int successCount = 0, totalFile = 0;

    public UploadDrugImageInteractor(IAmazonS3Service amazonS3Service, IConfiguration configuration, ISystemChangeLogRepository systemChangeLogRepository)
    {
        _amazonS3Service = amazonS3Service;
        _configuration = configuration;
        _systemChangeLogRepository = systemChangeLogRepository;
    }

    public UploadDrugImageOutputData Handle(UploadDrugImageInputData inputData)
    {
        string pathFolderUpdateDataTenant = _configuration["PathFolderUpdateDataTenant"] ?? string.Empty;
        string pathFile7z = $"{pathFolderUpdateDataTenant}\\{Guid.NewGuid()}.7z";
        string pathFileExtract7z = $"{pathFolderUpdateDataTenant}\\7Z-{Guid.NewGuid()}";
        try
        {
            _messenger = inputData.Messenger;

            if (inputData.FileUpdateData == null || !string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
            {
                SendMessager(new UploadDrugImageStatus(true, 0, 0, string.Empty, string.Empty, "This file is null or not in the correct format."));
                return new UploadDrugImageOutputData();
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
            pathFileExtract7z = $"{pathFileExtract7z}\\{CommonConstants.Drug_photo_05}";
            string housouPath = $"{pathFileExtract7z}\\{CommonConstants.HouSou}";
            string zaikeiPath = $"{pathFileExtract7z}\\{CommonConstants.ZaiKei}";

            string uploadFilePath = $"{CommonConstants.Image}/{CommonConstants.Reference}/{CommonConstants.DrugPhoto}";
            string uploadHousouFilePath = $"{uploadFilePath}/{CommonConstants.HouSou}/";
            string uploadZaiKeiFilePath = $"{uploadFilePath}/{CommonConstants.ZaiKei}/";

            var housouFileList = Directory.GetFiles(housouPath).ToList();
            var zaikeiFileList = Directory.GetFiles(zaikeiPath).ToList();
            totalFile = housouFileList.Count + zaikeiFileList.Count;

            // set folder name
            folderName = CommonConstants.HouSou;

            // upload housouFile
            UploadImageFileAction(housouFileList, uploadHousouFilePath);

            // check if continue progress
            if (!isStopProgress)
            {
                // set folder name
                folderName = CommonConstants.ZaiKei;

                // upload zaikeiFile
                UploadImageFileAction(zaikeiFileList, uploadZaiKeiFilePath);

                // return success message
                SendMessager(new UploadDrugImageStatus(true, totalFile, successCount, folderName, filename, string.Empty));
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
            SendMessager(new UploadDrugImageStatus(false, totalFile, successCount, folderName, filename, ex.Message));
        }
        finally
        {
            _amazonS3Service.Dispose();
            _systemChangeLogRepository.ReleaseResource();

            //delete file temp
            File.Delete(pathFile7z);
            Directory.Delete(pathFileExtract7z.Replace($"\\{CommonConstants.Drug_photo_05}", string.Empty), true);
        }
        return new UploadDrugImageOutputData();
    }

    private void UploadImageFileAction(List<string> sourceFileList, string uploadFilePath)
    {
        foreach (var strPath in sourceFileList)
        {
            // Check is stop progerss
            var statusCallBack = _messenger!.SendAsync(new StopUploadDrugImage());
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
            SendMessager(new UploadDrugImageStatus(false, totalFile, successCount, folderName, filename, string.Empty));
        }
    }

    private void SendMessager(UploadDrugImageStatus status)
    {
        _messenger!.Send(status);
    }
}
