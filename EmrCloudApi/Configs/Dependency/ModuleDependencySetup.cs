using Domain.Models.Reception;
using Domain.Models.User;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.PatientInfor;
using Interactor.Reception;
using Interactor.User;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using UseCase.Core.Builder;
using UseCase.PatientInformation.GetById;
using UseCase.PatientInformation.GetList;
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
            services.AddTransient<IPatientInforRepository, PatientInforRepository>();
            services.AddTransient<IReceptionRepository, ReceptionRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<CreateUserInputData, CreateUserInteractor>();
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();

            // PatientInfor
            busBuilder.RegisterUseCase<GetAllInputData, GetListPatientInforInteractor>();
            busBuilder.RegisterUseCase<GetPatientInforByIdInputData, GetPatientInforByIdInteractor>();


            //Reception
            busBuilder.RegisterUseCase<GetReceptionInputData, GetReceptionInteractor>();

            var bus = busBuilder.Build();   
            services.AddSingleton(bus);
        } 
    }
}
