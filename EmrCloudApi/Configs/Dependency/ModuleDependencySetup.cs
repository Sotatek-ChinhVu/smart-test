using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using Domain.Models.PatientGroupMst;
using Domain.Models.PatientInfor;
using Domain.Models.RaiinKubunMst;
using Domain.Models.Reception;
using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using Domain.Models.SetMst;
using Domain.Models.User;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.Diseases;
using Interactor.Insurance;
using Interactor.KarteInfs;
using Interactor.OrdInfs;
using Interactor.PatientGroupMst;
using Interactor.PatientInfor;
using Interactor.RaiinKubunMst;
using Interactor.Reception;
using Interactor.SetKbnMst;
using Interactor.SetMst;
using Interactor.User;
using UseCase.Core.Builder;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Insurance.GetList;
using UseCase.KarteInfs.GetLists;
using UseCase.OrdInfs.GetListTrees;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using UseCase.RaiinKubunMst.GetList;
using UseCase.Reception.Get;
using UseCase.Reception.GetList;
using UseCase.SetKbnMst.GetList;
using UseCase.SetMst.GetList;
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
            services.AddTransient<IPtDiseaseRepository, DiseaseRepository>();
            services.AddTransient<IOrdInfRepository, OrdInfRepository>();
            services.AddTransient<IReceptionRepository, ReceptionRepository>();
            services.AddTransient<IInsuranceRepository, InsuranceRepository>();
            services.AddTransient<IPatientInforRepository, PatientInforRepository>();
            services.AddTransient<IKarteInfRepository, KarteInfRepository>();
            services.AddTransient<IRaiinKubunMstRepository, RaiinKubunMstRepository>();
            services.AddTransient<ISetMstRepository, SetMstRepository>();
            services.AddTransient<ISetGenerationMstRepository, SetGenerationMstRepository>();
            services.AddTransient<ISetKbnMstRepository, SetKbnMstRepository>();
            services.AddTransient<IPatientGroupMstRepository, PatientGroupMstRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<CreateUserInputData, CreateUserInteractor>();
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();

            //PtByomeis
            busBuilder.RegisterUseCase<GetPtDiseaseListInputData, GetPtDiseaseListInteractor>();

            //Order Info
            busBuilder.RegisterUseCase<GetOrdInfListTreeInputData, GetOrdInfListTreeInteractor>();

            //Reception
            busBuilder.RegisterUseCase<GetReceptionInputData, GetReceptionInteractor>();
            busBuilder.RegisterUseCase<GetReceptionListInputData, GetReceptionListInteractor>();

            //Insurance
            busBuilder.RegisterUseCase<GetInsuranceListInputData, GetInsuranceListInteractor>();
            //Karte
            busBuilder.RegisterUseCase<GetListKarteInfInputData, GetListKarteInfInteractor>();

            // PatientInfor
            busBuilder.RegisterUseCase<GetPatientInforByIdInputData, GetPatientInforByIdInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoSimpleInputData, SearchPatientInfoSimpleInteractor>();
            busBuilder.RegisterUseCase<GetListPatientGroupMstInputData, GetListPatientGroupMstInteractor>();

            //RaiinKubun
            busBuilder.RegisterUseCase<GetRaiinKubunMstListInputData, GetRaiinKubunMstListInteractor>();

            //Set
            busBuilder.RegisterUseCase<GetSetMstListInputData, GetSetMstListInteractor>();

            //SetKbn
            busBuilder.RegisterUseCase<GetSetKbnMstListInputData, GetSetKbnMstListInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
