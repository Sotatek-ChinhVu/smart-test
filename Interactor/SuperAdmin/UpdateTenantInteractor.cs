using AWSSDK.Constants;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SuperAdmin.StopedTenant;
using UseCase.SuperAdmin.UpdateTenant;

namespace Interactor.SuperAdmin
{
    public class UpdateTenantInteractor : IUpdateTenantInputPort
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;

        public UpdateTenantInteractor(
            ITenantRepository tenantRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask
            )
        {
            _tenantRepository = tenantRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
        }
        public UpdateTenantOutputData Handle(UpdateTenantInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;

                if (inputData.TenantId <= 0)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new UpdateTenantOutputData(false, UpdateTenantStatus.TenantDoesNotExist);
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

                return new UpdateTenantOutputData(true, UpdateTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
