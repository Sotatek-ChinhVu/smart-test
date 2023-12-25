
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Interactor.Realtime;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;

namespace Interactor.SuperAdmin
{
    public class RestoreObjectS3TenantInteractor : IRestoreObjectS3TenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationRepository _notificationTaskRunRepository;
        public RestoreObjectS3TenantInteractor(
            IAwsSdkService awsSdkService,
            INotificationRepository notificationRepository,
            INotificationRepository notificationTaskRunRepository)
        {
            _awsSdkService = awsSdkService;
            _notificationRepository = notificationRepository;
            _notificationTaskRunRepository = notificationTaskRunRepository;
        }
        public RestoreObjectS3TenantOutputData Handle(RestoreObjectS3TenantInputData inputData)
        {
            IWebSocketService _webSocketService;
            _webSocketService = (IWebSocketService)inputData.WebSocketService;
            try
            {
                if (string.IsNullOrEmpty(inputData.ObjectName))
                {
                    return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Failed);
                }
                Task.Run(() =>
                {
                    try
                    {
                        var restoreObjectS3 = _awsSdkService.CopyObjectsInFolderAsync(
                        ConfigConstant.SourceBucketName,
                        inputData.ObjectName,
                        ConfigConstant.DestinationBucketName,
                        inputData.Type, inputData.IsPrefixDelete);
                        restoreObjectS3.Wait();
                          var message = $"医療機関{inputData.ObjectName} のS3 データが復元されました。";
                        var notification = _notificationTaskRunRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, message);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                    }
                    catch (Exception ex)
                    {
                        var message = $"医療機関{inputData.ObjectName} のS3 データの回復に失敗しました: {ex.Message}";
                        var notification = _notificationTaskRunRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                    }
                    finally
                    {
                        _notificationTaskRunRepository.ReleaseResource();
                    }
                });

                var message = $"医療機関{inputData.ObjectName} のS3 データを復元しています。";
                var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiInfo, message);
                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Success);
            }
            catch (Exception ex)
            {
                var message = inputData.ObjectName + $" is restore data S3 failed. {ex.Message}";
                var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, message);
                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                return new RestoreObjectS3TenantOutputData(RestoreObjectS3TenantStatus.Failed);
            }
            finally
            {
                _notificationRepository.ReleaseResource();
            }
        }
    }
}
