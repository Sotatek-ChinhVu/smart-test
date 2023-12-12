using AWSSDK.Constants;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using UseCase.SuperAdmin.StopedTenant;

namespace Interactor.SuperAdmin
{
    public class ToggleTenantInteractor : IToggleTenantInputPort
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;

        public ToggleTenantInteractor(
            ITenantRepository tenantRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask
            )
        {
            _tenantRepository = tenantRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
        }

        public ToggleTenantOutputData Handle(ToggleTenantInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;

                if (inputData.TenantId <= 0)
                {
                    return new ToggleTenantOutputData(false, ToggleTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new ToggleTenantOutputData(false, ToggleTenantStatus.TenantDoesNotExist);
                }

                if (tenant.Status != ConfigConstant.StatusTenantDictionary()["available"] && inputData.Type==0)
                {
                    return new ToggleTenantOutputData(false, ToggleTenantStatus.TenantNotAvailable);
                }

                if (tenant.Status != ConfigConstant.StatusTenantDictionary()["stoped"] && inputData.Type == 1)
                {
                    return new ToggleTenantOutputData(false, ToggleTenantStatus.TenantNotStoped);
                }

                if (inputData.Type == 0)
                {
                    _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["stopping"]);
                }
                else
                {
                    _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["starting"]);
                }

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(() =>
                {
                    string typeName = inputData.Type == 0 ? "stoped" : "start";
                    try
                    {
                        bool result;
                        if (inputData.Type == 0)
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

                            // Add info tenant for notification
                            notification.SetTenantId(tenant.TenantId);
                            notification.SetStatusTenant(tenant.StatusTenant);

                            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);

                            cts.Cancel();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        var messenge = $"{tenant.EndSubDomain} is {typeName} failed. Error: {ex.Message}.";
                        var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);

                        // Add info tenant for notification
                        notification.SetTenantId(tenant.TenantId);
                        notification.SetStatusTenant(tenant.StatusTenant);

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

                return new ToggleTenantOutputData(true, ToggleTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
