using AWSSDK.Constants;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using UseCase.SuperAdmin.StopedTenant;

namespace Interactor.SuperAdmin
{
    public class StopedTenantInteractor : IStopedTenantInputPort
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IWebSocketService _webSocketService;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;

        public StopedTenantInteractor(
            ITenantRepository tenantRepository,
            IWebSocketService webSocketService,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask
            )
        {
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
        }

        public StopedTenantOutputData Handle(StopedTenantInputData inputData)
        {
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new StopedTenantOutputData(false, StopedTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant == null || tenant.TenantId <=0)
                {
                    return new StopedTenantOutputData(false, StopedTenantStatus.TenantDoesNotExist);
                }

                if (tenant.Status != ConfigConstant.StatusTenantDictionary()["available"])
                {
                    return new StopedTenantOutputData(false, StopedTenantStatus.TenantNotAvailable);
                }


                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var result = _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["stoped"]);
                        if (result)
                        {
                            var messenge = $"{tenant.EndSubDomain} is stoped successfully.";
                            var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);

                            cts.Cancel();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        var messenge = $"{tenant.EndSubDomain} is stoped failed. Error: {ex.Message}.";
                        var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                    finally
                    {
                        _tenantRepositoryRunTask.ReleaseResource();
                        _notificationRepositoryRunTask.ReleaseResource();
                    }
                });

                return new StopedTenantOutputData(true, StopedTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
