using AWSSDK.Common;
using AWSSDK.Constants;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Helper.Messaging;
using Helper.Messaging.Data;
using Interactor.Realtime;
using Microsoft.Extensions.Configuration;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using System.Runtime.InteropServices;
using UseCase.SuperAdmin.UpdateDataTenant;
using ReaderOptions = SharpCompress.Readers.ReaderOptions;

namespace Interactor.SuperAdmin
{
    public class UpdateDataTenantInteractor : IUpdateDataTenantInputPort
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IConfiguration _configuration;
        private readonly INotificationRepository _notificationRepository;
        private IMessenger? _messenger;
        public UpdateDataTenantInteractor(
            ITenantRepository tenantRepository,
             IConfiguration configuration,
              INotificationRepository notificationRepository
            )
        {
            _tenantRepository = tenantRepository;
            _configuration = configuration;
            _notificationRepository = notificationRepository;
        }
        public UpdateDataTenantOutputData Handle(UpdateDataTenantInputData inputData)
        {
            try
            {
                _messenger = inputData.Messenger;
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;
                string pathFolderUpdateDataTenant = _configuration["PathFolderUpdateDataTenant"] ?? string.Empty;
                pathFolderUpdateDataTenant = "D:\\";
                string passwordFile7z = _configuration["PasswordFile7z"] ?? string.Empty;

                if (inputData.TenantId <= 0)
                {
                    // send error message
                    _messenger!.Send(new UpdateDataTenantResult(false, string.Empty, 0, 0, "医療機関が無効です。", 0));
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant == null || tenant.TenantId <= 0)
                {
                    // send error message
                    _messenger!.Send(new UpdateDataTenantResult(false, string.Empty, 0, 0, "医療機関が無効です。", 0));
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.TenantDoesNotExist);
                }

                if (!string.Equals(Path.GetExtension(inputData.FileUpdateData.FileName), ".7z", StringComparison.OrdinalIgnoreCase))
                {
                    // send error message
                    _messenger!.Send(new UpdateDataTenantResult(false, string.Empty, 0, 0, "アップロードファイルが不正です。", 0));
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.UploadFileIncorrectFormat7z);
                }

                if (tenant.Status != ConfigConstant.StatusTenantDictionary()["available"] && tenant.Status != ConfigConstant.StatusTenantDictionary()["stoped"] && tenant.Status != ConfigConstant.StatusTenantDictionary()["storage-full"])
                {
                    // send error message
                    _messenger!.Send(new UpdateDataTenantResult(false, string.Empty, 0, 0, "医療機関は更新する準備ができません。", 0));
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.TenantNotReadyToUpdate);
                }


                string pathFile7z = $"{pathFolderUpdateDataTenant}{tenant.SubDomain}-{Guid.NewGuid()}.7z";
                string pathFileExtract7z = $"{pathFolderUpdateDataTenant}7Z-{tenant.SubDomain}-{Guid.NewGuid()}";
                string pathFolderScript = $"{pathFileExtract7z}\\{UpdateConst.UPD_FILE_FOLDER}\\{UpdateConst.UPDATE_SQL}";
                string pathFolderMaster = $"{pathFileExtract7z}\\{UpdateConst.UPD_FILE_FOLDER}\\{UpdateConst.UPDATE_MASTER}";


                // Replace path file linux
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    pathFile7z = pathFile7z.Replace("\\", "/");
                    pathFolderScript = pathFolderScript.Replace("\\", "/");
                    pathFolderMaster = pathFolderMaster.Replace("\\", "/");
                }

                // Save file 7z
                using (var fileStream = new FileStream(pathFile7z, FileMode.Create))
                {
                    inputData.FileUpdateData.CopyTo(fileStream);
                }

                // Extract file 7z
                using (var archive = SevenZipArchive.Open(pathFile7z, new ReaderOptions() { Password = passwordFile7z }))
                {
                    archive.ExtractToDirectory(pathFileExtract7z);
                }
                // Check if extraction was successful
                if (!Directory.Exists(pathFolderScript) || !Directory.Exists(pathFolderMaster))
                {
                    // send error message
                    _messenger!.Send(new UpdateDataTenantResult(false, string.Empty, 0, 0, ".7zファイルの展開に失敗しました。", 0));
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.UnzipFile7zError);
                }

                int totalFileExcute = 0;
                // File script in folder 02_script
                string[] listFileScriptSql = Directory.GetFiles(pathFolderScript)
                    .Where(file => Path.GetExtension(file).Equals(".sql", StringComparison.OrdinalIgnoreCase))
                .ToArray();

                totalFileExcute += listFileScriptSql.Count();

                // Subfolder in folder 03_master
                string[] subFoldersMasters = Directory.GetDirectories(pathFolderMaster);

                if (subFoldersMasters.Length <= 0)
                {
                    // send error message
                    _messenger!.Send(new UpdateDataTenantResult(false, string.Empty, 0, 0, $"{pathFolderMaster} にはサブフォルダが存在しません。", 0));
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.MasterFolderHasNoSubfolder);
                }

                int totalHFiles = subFoldersMasters
               .Select(subFolder => Directory.GetFiles(subFolder, "*.h").Length)
               .Sum();

                int totalSqlFiles = subFoldersMasters
              .Select(subFolder => Directory.GetFiles(subFolder, "*.sql").Length)
              .Sum();

                totalFileExcute = totalFileExcute + totalHFiles + totalSqlFiles;

                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["updating"]);
                _messenger!.Send(new UpdateDataTenantResult(false, string.Empty, totalFileExcute, 0, "", 1));
                var result = UpdateDataTenant.ExcuteUpdateDataTenant(listFileScriptSql, subFoldersMasters, tenant.EndPointDb, ConfigConstant.PgPostDefault, tenant.Db,
                     tenant.UserConnect, tenant.PasswordConnect, inputData.CancellationToken, _messenger, totalFileExcute, pathFile7z, pathFolderUpdateDataTenant);

                var statusCallBack = _messenger!.SendAsync(new StopUpdateDataTenantStatus());
                bool isStopCalc = statusCallBack.Result.Result;
                // Check stop api update data tenant
                if (!isStopCalc)
                {
                    if (result)
                    {
                        var messenge = $"{tenant.EndSubDomain} のデータアップデートが完了しました。";
                        var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, tenant.Status);
                        // Add info tenant for notification
                        notification.SetTenantId(tenant.TenantId);
                        notification.SetStatusTenant(ConfigConstant.StatusTenantRunning);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        return new UpdateDataTenantOutputData(true, UpdateDataTenantStatus.Successed);
                    }

                    else
                    {
                        var messenge = $"{tenant.EndSubDomain} のデータアップデートに失敗しました。エラー";
                        var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["failed"]);
                        // Add info tenant for notification
                        notification.SetTenantId(tenant.TenantId);
                        notification.SetStatusTenant(ConfigConstant.StatusTenantRunning);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.Failed);

                    }
                }
                else
                {
                    // Cancel api,  update status tenant 
                    _tenantRepository.UpdateStatusTenant(inputData.TenantId, tenant.Status);
                }
                return new UpdateDataTenantOutputData(true, UpdateDataTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }
    }
}
