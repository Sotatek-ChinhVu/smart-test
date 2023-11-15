
using AWSSDK.Interfaces;
using AWSSDK.Services;
using Domain.SuperAdminModels.Admin;
using Domain.SuperAdminModels.Tenant;
using Infrastructure.Common;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Infrastructure.Services;
using Infrastructure.SuperAdminRepositories;
using Interactor.SuperAdmin;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UseCase.Core.Builder;
using UseCase.SuperAdmin.Login;
using UseCase.SuperAdmin.UpgradePremium;

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

            //Init follow transient so no need change transient
            //services.AddScoped<ILoggingHandler, LoggingHandler>();

            //services.AddScoped<ISystemStartDbService, SystemStartDbService>();
        }

        private void SetupRepositories(IServiceCollection services)
        {
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            busBuilder.RegisterUseCase<LoginInputData, LoginInteractor>();
            busBuilder.RegisterUseCase<UpgradePremiumInputData, UpgradePremiumInteractor>();

            //SystemStartDb 
            //busBuilder.RegisterUseCase<SystemStartDbInputData, SystemStartDbInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
