﻿using Domain.Models.Insurance;
﻿using Domain.Models.PatientInfor;
﻿using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.KarteInfs;
using Domain.Models.Reception;
using Domain.Models.Diseases;
using Domain.Models.User;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.Insurance;
using Interactor.OrdInfs;
using Interactor.PatientInfor;
using Interactor.KarteInfs;
using Interactor.Reception;
using Interactor.Diseases;
using Interactor.User;
using UseCase.Core.Builder;
using UseCase.Insurance.GetList;
using UseCase.OrdInfs.GetListTrees;
using UseCase.PatientInformation.GetById;
using UseCase.KarteInfs.GetLists;
using UseCase.Reception.Get;
using UseCase.Reception.GetList;
using UseCase.Diseases.GetDiseaseList;
using UseCase.User.Create;
using UseCase.User.GetList;
using Domain.Models.RaiinKubunMst;
using UseCase.RaiinKubunMst.GetList;
using Interactor.RaiinKubunMst;
using UseCase.GroupInf.GetList;
using Interactor.GrpInf;
using Domain.Models.GroupInf;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientGroupMst.GetList;
using Interactor.PatientGroupMst;
using Domain.Models.PatientGroupMst;

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
            services.AddTransient<IGroupInfRepository, GroupInfRepository>();
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

            //Group Inf
            busBuilder.RegisterUseCase<GetListGroupInfInputData, GroupInfInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
