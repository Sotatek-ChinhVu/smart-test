using Domain.CalculationInf;
using Domain.Models.ColumnSetting;
using Domain.Models.Diseases;
using Domain.Models.FlowSheet;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.KaMst;
using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using Domain.Models.PatientGroupMst;
using Domain.Models.PatientInfor;
using Domain.Models.PatientRaiinKubun;
using Domain.Models.PtCmtInf;
using Domain.Models.RaiinCmtInf;
using Domain.Models.RaiinFilterMst;
using Domain.Models.RaiinKbnInf;
using Domain.Models.RaiinKubunMst;
using Domain.Models.Reception;
using Domain.Models.ReceptionInsurance;
using Domain.Models.ReceptionSameVisit;
using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using Domain.Models.SetMst;
using Domain.Models.SystemConf;
using Domain.Models.UketukeSbtDayInf;
using Domain.Models.UketukeSbtMst;
using Domain.Models.User;
using Domain.Models.UserConf;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.CalculationInf;
using Interactor.ColumnSetting;
using Interactor.Diseases;
using Interactor.FlowSheet;
using Interactor.GrpInf;
using Interactor.Insurance;
using Interactor.InsuranceMst;
using Interactor.KaMst;
using Interactor.KarteInfs;
using Interactor.OrdInfs;
using Interactor.PatientGroupMst;
using Interactor.PatientInfor;
using Interactor.PatientRaiinKubun;
using Interactor.RaiinFilterMst;
using Interactor.RaiinKubunMst;
using Interactor.Reception;
using Interactor.ReceptionInsurance;
using Interactor.ReceptionSameVisit;
using Interactor.SetKbnMst;
using Interactor.SetMst;
using Interactor.UketukeSbtMst;
using Interactor.User;
using UseCase.CalculationInf;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.Core.Builder;
using UseCase.Diseases.GetDiseaseList;
using UseCase.FlowSheet.GetList;
using UseCase.GroupInf.GetList;
using UseCase.Insurance.GetList;
using UseCase.InsuranceMst.Get;
using UseCase.KaMst.GetList;
using UseCase.KarteInfs.GetLists;
using UseCase.OrdInfs.GetListTrees;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using UseCase.PatientRaiinKubun.Get;
using UseCase.RaiinFilterMst.GetList;
using UseCase.RaiinKubunMst.GetList;
using UseCase.Reception.Get;
using UseCase.Reception.GetList;
using UseCase.Reception.GetSettings;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;
using UseCase.SearchHokensyaMst.Get;
using UseCase.SetKbnMst.GetList;
using UseCase.SetMst.GetList;
using UseCase.UketukeSbtMst.GetBySinDate;
using UseCase.UketukeSbtMst.GetList;
using UseCase.UketukeSbtMst.GetNext;
using UseCase.User.GetList;
using UseCase.User.UpsertList;
using Domain.Models.KarteFilterDetail;
using Domain.Models.KarteFilterMst;
using Interactor.KarteFilter;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.PatientInfor.SearchAdvanced;

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
            services.AddTransient<IRaiinCmtInfRepository, RaiinCmtInfRepository>();
            services.AddTransient<IUketukeSbtMstRepository, UketukeSbtMstRepository>();
            services.AddTransient<IKaMstRepository, KaMstRepository>();
            services.AddTransient<IRaiinKbnInfRepository, RaiinKbnInfRepository>();
            services.AddTransient<ICalculationInfRepository, CalculationInfRepository>();
            services.AddTransient<IGroupInfRepository, GroupInfRepository>();
            services.AddTransient<ISetMstRepository, SetMstRepository>();
            services.AddTransient<ISetGenerationMstRepository, SetGenerationMstRepository>();
            services.AddTransient<ISetKbnMstRepository, SetKbnMstRepository>();
            services.AddTransient<IPatientGroupMstRepository, PatientGroupMstRepository>();
            services.AddTransient<IInsuranceMstRepository, InsuranceMstRepository>();
            services.AddTransient<IRaiinFilterMstRepository, RaiinFilterMstRepository>();
            services.AddTransient<IPatientRaiinKubunReponsitory, PatientRaiinKubunReponsitory>();
            services.AddTransient<IPtCmtInfRepository, PtCmtInfRepository>();
            services.AddTransient<IUketukeSbtDayInfRepository, UketukeSbtDayInfRepository>();
            services.AddTransient<IFlowSheetRepository, FlowSheetRepository>();
            services.AddTransient<IUserConfRepository, UserConfRepository>();
            services.AddTransient<ISystemConfRepository, SystemConfRepository>();
            services.AddTransient<IReceptionInsuranceRepository, ReceptionInsuranceRepository>();
            services.AddTransient<IKarteFilterDetailRepository, KarteFilterDetailRepository>();
            services.AddTransient<IKarteFilterMstRepository, KarteFilterMstRepository>();
            services.AddTransient<IColumnSettingRepository, ColumnSettingRepository>();
            services.AddTransient<IReceptionSameVisitRepository, ReceptionSameVisitRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();
            busBuilder.RegisterUseCase<UpsertUserListInputData, UpsertUserListInteractor>();

            //PtByomeis
            busBuilder.RegisterUseCase<GetPtDiseaseListInputData, GetPtDiseaseListInteractor>();

            //Order Info
            busBuilder.RegisterUseCase<GetOrdInfListTreeInputData, GetOrdInfListTreeInteractor>();

            //Reception
            busBuilder.RegisterUseCase<GetReceptionInputData, GetReceptionInteractor>();
            busBuilder.RegisterUseCase<GetReceptionListInputData, GetReceptionListInteractor>();
            busBuilder.RegisterUseCase<UpdateReceptionStaticCellInputData, UpdateReceptionStaticCellInteractor>();
            busBuilder.RegisterUseCase<UpdateReceptionDynamicCellInputData, UpdateReceptionDynamicCellInteractor>();
            busBuilder.RegisterUseCase<GetReceptionSettingsInputData, GetReceptionSettingsInteractor>();
            busBuilder.RegisterUseCase<GetPatientRaiinKubunInputData, GetPatientRaiinKubunInteractor>();

            //Insurance
            busBuilder.RegisterUseCase<GetInsuranceListInputData, GetInsuranceListInteractor>();
            //Karte
            busBuilder.RegisterUseCase<GetListKarteInfInputData, GetListKarteInfInteractor>();

            // PatientInfor
            busBuilder.RegisterUseCase<GetPatientInforByIdInputData, GetPatientInforByIdInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoSimpleInputData, SearchPatientInfoSimpleInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoAdvancedInputData, SearchPatientInfoAdvancedInteractor>();
            busBuilder.RegisterUseCase<GetListPatientGroupMstInputData, GetListPatientGroupMstInteractor>();

            //RaiinKubun
            busBuilder.RegisterUseCase<GetRaiinKubunMstListInputData, GetRaiinKubunMstListInteractor>();

            //Calculation Inf
            busBuilder.RegisterUseCase<CalculationInfInputData, CalculationInfInteractor>();
            //Group Inf
            busBuilder.RegisterUseCase<GetListGroupInfInputData, GroupInfInteractor>();

            //Set
            busBuilder.RegisterUseCase<GetSetMstListInputData, GetSetMstListInteractor>();

            //SetKbn
            busBuilder.RegisterUseCase<GetSetKbnMstListInputData, GetSetKbnMstListInteractor>();

            //Insurance Mst
            busBuilder.RegisterUseCase<GetInsuranceMstInputData, GetInsuranceMstInteractor>();

            // RaiinFilter
            busBuilder.RegisterUseCase<GetRaiinFilterMstListInputData, GetRaiinFilterMstListInteractor>();

            // Ka
            busBuilder.RegisterUseCase<GetKaMstListInputData, GetKaMstListInteractor>();

            // UketukeSbt
            busBuilder.RegisterUseCase<GetUketukeSbtMstListInputData, GetUketukeSbtMstListInteractor>();
            busBuilder.RegisterUseCase<GetUketukeSbtMstBySinDateInputData, GetUketukeSbtMstBySinDateInteractor>();
            busBuilder.RegisterUseCase<GetNextUketukeSbtMstInputData, GetNextUketukeSbtMstInteractor>();

            // HokensyaMst
            busBuilder.RegisterUseCase<SearchHokensyaMstInputData, SearchHokensyaMstInteractor>();

            // Flowsheet
            busBuilder.RegisterUseCase<GetListFlowSheetInputData, GetListFlowSheetInteractor>();

            // UketukeSbtDayInf
            busBuilder.RegisterUseCase<GetReceptionInsuranceInputData, ReceptionInsuranceInteractor>();

            // KarteFilter
            busBuilder.RegisterUseCase<GetKarteFilterInputData, GetKarteFilterMstsInteractor>();
       
            // ColumnSetting
            busBuilder.RegisterUseCase<SaveColumnSettingListInputData, SaveColumnSettingListInteractor>();
            busBuilder.RegisterUseCase<GetColumnSettingListInputData, GetColumnSettingListInteractor>();

            // Reception Same Visit
            busBuilder.RegisterUseCase<GetReceptionSameVisitInputData, GetReceptionSameVisitInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
