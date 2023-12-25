
using AWSSDK.Interfaces;
using AWSSDK.Services;
using Domain.SuperAdminModels.Admin;
using Domain.SuperAdminModels.Logger;
using Domain.SuperAdminModels.MigrationTenantHistory;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using EmrCloudApi.ScheduleTask;
using Infrastructure.Common;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Infrastructure.SuperAdminRepositories;
using Interactor.Realtime;
using Interactor.SuperAdmin;
using Interactor.SuperAdmin.AuditLog;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SuperAdminAPI.Services;
using UseCase.Core.Builder;
using UseCase.SuperAdmin.AuditLog;
using UseCase.SuperAdmin.ExportCsvTenantList;
using UseCase.SuperAdmin.GetNotification;
using UseCase.SuperAdmin.GetTenant;
using UseCase.SuperAdmin.GetTenantDetail;
using UseCase.SuperAdmin.Login;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;
using UseCase.SuperAdmin.RestoreTenant;
using UseCase.SuperAdmin.RevokeInsertPermission;
using UseCase.SuperAdmin.StopedTenant;
using UseCase.SuperAdmin.TenantOnboard;
using UseCase.SuperAdmin.TerminateTenant;
using UseCase.SuperAdmin.UpdateDataTenant;
using UseCase.SuperAdmin.UpdateNotification;
using UseCase.SuperAdmin.UpgradePremium;
using UseCase.UserToken.GetInfoRefresh;
using UseCase.UserToken.SiginRefresh;
using UseCase.SystemStartDbs;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;
using Microsoft.Extensions.Caching.Memory;
using UseCase.SuperAdmin.ExportCsvTenantList;
using UseCase.SuperAdmin.ExportCsvLogList;
using Helper.Messaging;
using UseCase.SuperAdmin.DeleteJunkFileS3;
using SuperAdminAPI.ScheduleTask;
using UseCase.SuperAdmin.UploadDrugImage;
using Domain.SuperAdminModels.SystemChangeLog;
using Helper.Messaging;

namespace SuperAdmin.Configs.Dependency
{
    public class ModuleDependencySetup : IDependencySetup
    {
        public void Run(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilters>();
            });

            SetupRepositories(services);
            SetupInterfaces(services);
            SetupUseCase(services);
        }

        private void SetupInterfaces(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITenantProvider, TenantProvider>();
            services.AddTransient<IAmazonS3Service, AmazonS3Service>();
            services.AddTransient<IAwsSdkService, AwsSdkService>();
            services.AddTransient<IWebSocketService, WebSocketService>();
            services.AddTransient<ISystemChangeLogRepository, SystemChangeLogRepository>();
            services.AddTransient<IMessenger, Messenger>();


            //Init follow transient so no need change transient
            //services.AddScoped<ILoggingHandler, LoggingHandler>();

            services.AddScoped<ISystemStartDbService, SystemStartDbService>();
            services.AddScoped<IDeleteJunkFileS3Service, DeleteJunkFileS3Service>();
        }

        private void SetupRepositories(IServiceCollection services)
        {
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<IAdminAuditLogRepository, AdminAuditLogRepository>();
            services.AddTransient<IMigrationTenantHistoryRepository, MigrationTenantHistoryRepository>();

            services.AddSingleton<IHostedService, TaskScheduleRevokeInsertPermission>();
            services.AddSingleton<IHostedService, TaskScheduleDeleteJunkFileS3>();

            services.AddTransient<INotificationRepository, NotificationRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            busBuilder.RegisterUseCase<LoginInputData, LoginInteractor>();
            busBuilder.RegisterUseCase<UpdateTenantInputData, UpdateTenantInteractor>();
            busBuilder.RegisterUseCase<TenantOnboardInputData, TenantOnboardInteractor>();
            busBuilder.RegisterUseCase<GetAuditLogListInputData, GetAuditLogListInteractor>();
            busBuilder.RegisterUseCase<TerminateTenantInputData, TerminateTenantInteractor>();
            busBuilder.RegisterUseCase<GetTenantInputData, GetTenantInteractor>();
            busBuilder.RegisterUseCase<GetTenantDetailInputData, GetTenantDetailInteractor>();
            busBuilder.RegisterUseCase<GetNotificationInputData, GetNotificationInteractor>();
            busBuilder.RegisterUseCase<UpdateNotificationInputData, UpdateNotificationInteractor>();
            busBuilder.RegisterUseCase<ToggleTenantInputData, ToggleTenantInteractor>();
            busBuilder.RegisterUseCase<RestoreTenantInputData, RestoreTenantInteractor>();
            busBuilder.RegisterUseCase<SigninRefreshTokenInputData, SigInRefreshTokenInteractor>();
            busBuilder.RegisterUseCase<RefreshTokenByUserInputData, RefreshTokenByUserInteractor>();
            busBuilder.RegisterUseCase<RestoreObjectS3TenantInputData, RestoreObjectS3TenantInteractor>();
            busBuilder.RegisterUseCase<UpdateDataTenantInputData, UpdateDataTenantInteractor>();
            busBuilder.RegisterUseCase<ExportCsvTenantListInputData, ExportCsvTenantListInteractor>();
            busBuilder.RegisterUseCase<ExportCsvLogListInputData, ExportCsvLogListInteractor>();
            busBuilder.RegisterUseCase<UploadDrugImageInputData, UploadDrugImageInteractor>();

            //SystemStartDb 
            //busBuilder.RegisterUseCase<SystemStartDbInputData, SystemStartDbInteractor>();

            busBuilder.RegisterUseCase<RevokeInsertPermissionInputData, RevokeInsertPermissionInteractor>();
            busBuilder.RegisterUseCase<DeleteJunkFileS3InputData, DeleteJunkFileS3Interactor>();

            services.AddMemoryCache();
            var bus = busBuilder.Build();
            services.AddSingleton(bus);

        }
    }
}
