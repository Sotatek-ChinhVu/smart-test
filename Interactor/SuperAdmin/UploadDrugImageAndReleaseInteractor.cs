using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.SystemChangeLog;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Interactor.Realtime;
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
    private readonly INotificationRepository _notificationRepository;
    private IMessenger? _messenger;
    private bool isStopProgress = false;
    private readonly List<string> fileUploaded = new();
    private string filename = string.Empty, folderName = string.Empty;
    private int successCount = 0, totalFile = 0;

    public UploadDrugImageAndReleaseInteractor(IAmazonS3Service amazonS3Service, IConfiguration configuration, ISystemChangeLogRepository systemChangeLogRepository, INotificationRepository notificationRepository)
    {
        _amazonS3Service = amazonS3Service;
        _configuration = configuration;
        _systemChangeLogRepository = systemChangeLogRepository;
        _notificationRepository = notificationRepository;
    }

    public UploadDrugImageAndReleaseOutputData Handle(UploadDrugImageAndReleaseInputData inputData)
    {
        string pathFolderUpdateDataTenant = _configuration["PathFolderUpdateDataTenant"] ?? string.Empty;
        string pathFile7z = $"{pathFolderUpdateDataTenant}\\{Guid.NewGuid()}.7z";
        string pathFileExtract7z = $"{pathFolderUpdateDataTenant}\\7Z-{Guid.NewGuid()}";
        string uploadHousouFilePath = string.Empty, uploadZaiKeiFilePath = string.Empty, uploadFilePath = string.Empty, errorMessage = string.Empty;
        List<string> housouFileList = new(), zaikeiFileList = new(), releaseFileList = new();
        SystemChangeLogModel systemChangeLog = new();
        int status = 1;
        var _webSocketService = (IWebSocketService)inputData.WebSocketService;

        try
        {
            _messenger = inputData.Messenger;
            if (inputData.FileUpdateData == null || !string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
            {
                SendMessager(new UploadDrugImageAndReleaseStatus(true, 0, 0, string.Empty, string.Empty, "アップロードファイルが不正です。"));
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
            pathFileExtract7z = $"{pathFileExtract7z}\\{CommonConstants.Updfile}";

            // create ReleateInfo
            var releaseInfo = CreateReleateInfo(pathFileExtract7z);
            if (releaseInfo == null)
            {
                // stop upload file if releaseInfo is null
                return new UploadDrugImageAndReleaseOutputData();
            }

            // create system changeLog
            systemChangeLog = AddSystemChangeLog(inputData.FileUpdateData.FileName, releaseInfo);

            // check exist folder 
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

                // set isDrug = 1
                systemChangeLog.UpdateIsDrug(1);
            }

            // get folder to upload release file
            if (allFolder.Contains($"{pathFileExtract7z}\\{CommonConstants.Release_99}"))
            {
                string releaseFile7zPath = $"{pathFileExtract7z}\\{CommonConstants.Release_99}";
                uploadFilePath = $"{CommonConstants.Common}/{CommonConstants.Release_Version}/";

                releaseFileList = Directory.GetFiles(releaseFile7zPath).ToList();

                // set isNote = 1
                systemChangeLog.UpdateIsNote(1);
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
                    var response = _amazonS3Service.DeleteLastestVerObjectAsync(deletedFile);
                    response.Wait();
                }
            }

            // if success, set status = 9
            status = 9;
        }
        catch (Exception ex)
        {
            // return error message, if exception, set status = 8
            errorMessage = ex.Message;
            status = 8;
            SendMessager(new UploadDrugImageAndReleaseStatus(false, totalFile, successCount, folderName, filename, errorMessage));
        }
        finally
        {
            _amazonS3Service.Dispose();
            _systemChangeLogRepository.ReleaseResource();

            // delete file temp
            File.Delete(pathFile7z);
            Directory.Delete(pathFileExtract7z.Replace($"\\{CommonConstants.Updfile}", string.Empty), true);

            // update system change log
            systemChangeLog.UpdateStatus(status, errorMessage);
            _systemChangeLogRepository.SaveSystemChangeLog(systemChangeLog);
        }

        // send notification if success file
        // if status = 9, send message successfully
        if (status == 9)
        {
            var messenge = $"医薬品画像およびリリースノートのアップロードが完了しました。";
            var notification = _notificationRepository.CreateNotification(AWSSDK.Constants.ConfigConstant.StatusNotiSuccess, messenge);
            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
        }
        else
        {
            var notification = _notificationRepository.CreateNotification(AWSSDK.Constants.ConfigConstant.StatusNotifailure, errorMessage);
            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
        }
        return new UploadDrugImageAndReleaseOutputData();
    }

    /// <summary>
    /// Create ReleateInfo
    /// </summary>
    /// <param name="folderName"></param>
    /// <returns></returns>
    private ReleaseInfo? CreateReleateInfo(string folderName)
    {
        string fileName = $"{folderName}\\release.ini";
        string current = string.Empty, newVer = string.Empty, oldVer = string.Empty, releaseVer = string.Empty, type = string.Empty;

        var dictionaryFileList = Directory.GetFiles(folderName).ToList();
        if (!dictionaryFileList.Contains(fileName))
        {
            SendMessager(new UploadDrugImageAndReleaseStatus(true, 0, 0, string.Empty, string.Empty, "release.iniファイルが存在しません。"));
            return null;
        }

        // read file release.ini
        var lines = File.ReadLines(fileName).ToList();
        if (!lines.Contains($"[{CommonConstants.SECTION_RELEASE}]"))
        {
            SendMessager(new UploadDrugImageAndReleaseStatus(true, 0, 0, string.Empty, string.Empty, "release.iniファイルが空またはファイル形式が不正です。"));
            return null;
        }
        foreach (var line in lines)
        {
            if (line.Contains("current"))
            {
                current = line.Substring("current=".Length, line.Length - "current=".Length);
            }

            if (line.Contains("newver"))
            {
                newVer = line.Substring("newver=".Length, line.Length - "newver=".Length);
            }

            if (line.Contains("oldver"))
            {
                oldVer = line.Substring("oldver=".Length, line.Length - "oldver=".Length);
            }

            if (line.Contains("releasever"))
            {
                releaseVer = line.Substring("releasever=".Length, line.Length - "releasever=".Length);
            }

            if (line.Contains("type"))
            {
                type = line.Substring("type=".Length, line.Length - "type=".Length);
            }
        }
        return new ReleaseInfo(current, newVer, oldVer, releaseVer, type);
    }

    private SystemChangeLogModel AddSystemChangeLog(string zipFilePath, ReleaseInfo releaseInfo)
    {
        string updateVersion;
        if (string.IsNullOrEmpty(releaseInfo.NewVer))
        {
            updateVersion = releaseInfo.Current;
        }
        else
        {
            updateVersion = releaseInfo.NewVer;
        }
        return _systemChangeLogRepository.GetSystemChangeLog(zipFilePath, updateVersion);
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

    /// <summary>
    /// private record
    /// </summary>
    /// <param name="Current"></param>
    /// <param name="NewVer"></param>
    /// <param name="OldVer"></param>
    /// <param name="ReleaseVer"></param>
    /// <param name="Type"></param>
    private record ReleaseInfo(string Current, string NewVer, string OldVer, string ReleaseVer, string Type)
    {
        public string Current { get; private set; } = Current;
        public string NewVer { get; private set; } = NewVer;
        public string OldVer { get; private set; } = OldVer;
        public string ReleaseVer { get; private set; } = ReleaseVer;
        public string Type { get; private set; } = Type;
    }
}
