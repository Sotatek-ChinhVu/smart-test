using Domain.Models.KarteInfs;
using Domain.Models.KarteKbn;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.RaiinKubunMst;
using Domain.Models.Reception;
using Domain.Models.Set;
using Domain.Models.SetGeneration;
using Domain.Models.SetKbn;
using Domain.Models.User;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.MedicalExamination;
using Interactor.MedicalExamination.KarteInfs;
using Interactor.MedicalExamination.OrdInfs;
using Interactor.PatientInfor;
using Interactor.RaiinKubunMst;
using Interactor.Reception;
using Interactor.Set;
using Interactor.SetKbn;
using Interactor.User;
using UseCase.Core.Builder;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.KarteInfs.GetLists;
using UseCase.MedicalExamination.OrdInfs.GetListTrees;
using UseCase.PatientInformation.GetById;
using UseCase.RaiinKubunMst.GetList;
using UseCase.Reception.Get;
using UseCase.Set.GetList;
using UseCase.SetKbn.GetList;
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
            services.AddTransient<IReceptionRepository, ReceptionRepository>();
            services.AddTransient<IPatientInforRepository, PatientInforRepository>();
            services.AddTransient<IKarteInfRepository, KarteInfRepository>();
            services.AddTransient<IRaiinKubunMstRepository, RaiinKubunMstRepository>();
            services.AddTransient<IKarteKbnRepository, KarteKbnRepository>();
            services.AddTransient<ISetRepository, SetRepository>();
            services.AddTransient<ISetGenerationRepository, SetGenerationRepository>();
            services.AddTransient<ISetKbnMstRepository, SetKbnRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<CreateUserInputData, CreateUserInteractor>();
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();

            //Order Info
            busBuilder.RegisterUseCase<GetOrdInfListTreeInputData, GetOrdInfListTreeInteractor>();

            //Reception
            busBuilder.RegisterUseCase<GetReceptionInputData, GetReceptionInteractor>();

            //Karte
            busBuilder.RegisterUseCase<GetListKarteInfInputData, GetListKarteInfInteractor>();

            // PatientInfor
            busBuilder.RegisterUseCase<GetPatientInforByIdInputData, GetPatientInforByIdInteractor>();

            //RaiinKubun
            busBuilder.RegisterUseCase<GetRaiinKubunMstListInputData, GetRaiinKubunMstListInteractor>();

            //Medical Examination
            busBuilder.RegisterUseCase<GetMedicalExaminationHistoryInputData, GetMedicalExaminationHistoryInteractor>();

            //Set
            busBuilder.RegisterUseCase<GetSetListInputData, GetSetListInteractor>();

            //SetKbn
            busBuilder.RegisterUseCase<GetSetKbnListInputData, GetSetKbnListInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
