using AWSSDK.Constants;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using UseCase.SuperAdmin.UpdateDataTenant;

namespace Interactor.SuperAdmin
{
    public class UpdateDataTenantInteractor : IUpdateDataTenantInputPort
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;

        public UpdateDataTenantInteractor(
            ITenantRepository tenantRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask
            )
        {
            _tenantRepository = tenantRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
        }
        public UpdateDataTenantOutputData Handle(UpdateDataTenantInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;

                if (inputData.TenantId <= 0)
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new UpdateDataTenantOutputData(false, UpdateDataTenantStatus.TenantDoesNotExist);
                }

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(() =>
                {
                    string typeName = 0 == 0 ? "stoped" : "start";
                    try
                    {
                        bool result;
                        if (0 == 0)
                        {
                            result = _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["stoped"]);
                        }
                        else
                        {
                            result = _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                        }
                        if (result)
                        {
                            var messenge = $"{tenant.EndSubDomain} is {typeName} successfully.";
                            var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);

                            cts.Cancel();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        var messenge = $"{tenant.EndSubDomain} is {typeName} failed. Error: {ex.Message}.";
                        var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                    finally
                    {
                        _tenantRepositoryRunTask.ReleaseResource();
                        _notificationRepositoryRunTask.ReleaseResource();
                    }
                });

                return new UpdateDataTenantOutputData(true, UpdateDataTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
