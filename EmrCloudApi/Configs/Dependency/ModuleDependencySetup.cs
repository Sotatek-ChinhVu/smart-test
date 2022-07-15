﻿using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Reception;
using Domain.Models.User;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.OrdInfs;
using Interactor.Reception;
using Interactor.User;
using UseCase.Core.Builder;
using UseCase.OrdInfs.GetListTrees;
using UseCase.Reception.Get;
using UseCase.User.Create;
using UseCase.User.GetList;

namespace EmrCloudApi.Configs.Dependency
{
    public class ModuleDependencySetup : IDependencySetup
    {
        public void Run(IServiceCollection services)
        {
            SetupRepositories(services);
            SetupInterfaces(services);
            SetupUseCase(services);
        }

        private void SetupInterfaces(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ITenantProvider, TenantProvider>();
        }

        private void SetupRepositories(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrdInfRepository, OrdInfRepository>();
            services.AddTransient<IOrdInfDetailRepository, OrdInfDetailRepository>();
            services.AddTransient<IReceptionRepository, ReceptionRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<CreateUserInputData, CreateUserInteractor>();
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();
            busBuilder.RegisterUseCase<GetOrdInfListTreeInputData, GetOrdInfListTreeInteractor>();


            //Reception
            busBuilder.RegisterUseCase<GetReceptionInputData, GetReceptionInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
