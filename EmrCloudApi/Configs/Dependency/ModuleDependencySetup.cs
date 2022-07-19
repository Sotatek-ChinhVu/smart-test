using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SpecialNote;
using Domain.Models.User;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.PatientInfor;
using Interactor.Reception;
using Interactor.SpecialNote;
using Interactor.User;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using UseCase.Core.Builder;
using UseCase.PatientInformation.GetById;
using UseCase.Reception.Get;
using UseCase.SpecialNote.Read;
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
            services.AddTransient<IReceptionRepository, ReceptionRepository>();
            services.AddTransient<IPatientInforRepository, PatientInforRepository>();
            services.AddTransient<ISpecialNoteRepository, SpecialNoteRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<CreateUserInputData, CreateUserInteractor>();
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();


            //Reception
            busBuilder.RegisterUseCase<GetReceptionInputData, GetReceptionInteractor>();

            // PatientInfor
            busBuilder.RegisterUseCase<GetPatientInforByIdInputData, GetPatientInforByIdInteractor>();


            //SpecialNote
            busBuilder.RegisterUseCase<GetSpecialNoteInputData, GetSpecialNoteInteractor>();

            var bus = busBuilder.Build();   
            services.AddSingleton(bus);
        } 
    }
}
