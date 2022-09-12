using Domain.CalculationInf;
using Domain.Models.ColumnSetting;
using Domain.Models.Diseases;
using Domain.Models.DrugDetail;
using Domain.Models.FlowSheet;
using Domain.Models.GroupInf;
using Domain.Models.InputItem;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.JsonSetting;
using Domain.Models.KaMst;
using Domain.Models.KarteFilterMst;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.MstItem;
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
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.SystemConf;
using Domain.Models.UketukeSbtDayInf;
using Domain.Models.UketukeSbtMst;
using Domain.Models.User;
using Domain.Models.UserConf;
using Domain.Models.VisitingListSetting;
using EmrCloudApi.Realtime;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.SpecialNote;
using Infrastructure.Services;
using Interactor.CalculationInf;
using Interactor.ColumnSetting;
using Interactor.Diseases;
using Interactor.DrugDetail;
using Interactor.FlowSheet;
using Interactor.GrpInf;
using Interactor.InputItem;
using Interactor.Insurance;
using Interactor.InsuranceMst;
using Interactor.JsonSetting;
using Interactor.KaMst;
using Interactor.KarteFilter;
using Interactor.KarteInfs;
using Interactor.KohiHokenMst;
using Interactor.MedicalExamination;
using Interactor.MstItem;
using Interactor.OrdInfs;
using Interactor.PatientGroupMst;
using Interactor.PatientInfor;
using Interactor.PatientRaiinKubun;
using Interactor.RaiinFilterMst;
using Interactor.RaiinKubunMst;
using Interactor.Reception;
using Interactor.ReceptionInsurance;
using Interactor.ReceptionSameVisit;
using Interactor.Schema;
using Interactor.SetKbnMst;
using Interactor.SetMst;
using Interactor.SpecialNote;
using Interactor.UketukeSbtMst;
using Interactor.User;
using Interactor.VisitingList;
using UseCase.CalculationInf;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.Core.Builder;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Diseases.Upsert;
using UseCase.DrugDetail;
using UseCase.FlowSheet.GetList;
using UseCase.FlowSheet.Upsert;
using UseCase.GroupInf.GetList;
using UseCase.InputItem.Search;
using UseCase.InputItem.UpdateAdopted;
using UseCase.Insurance.GetList;
using UseCase.InsuranceMst.Get;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.Upsert;
using UseCase.KaMst.GetList;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;
using UseCase.KarteInfs.GetLists;
using UseCase.KohiHokenMst.Get;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.GetFoodAlrgy;
using UseCase.OrdInfs.GetListTrees;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using UseCase.PatientRaiinKubun.Get;
using UseCase.RaiinFilterMst.GetList;
using UseCase.RaiinFilterMst.SaveList;
using UseCase.RaiinKubunMst.GetList;
using UseCase.Reception.Get;
using UseCase.Reception.GetList;
using UseCase.Reception.GetSettings;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;
using UseCase.Schema.GetListImageTemplates;
using UseCase.SearchHokensyaMst.Get;
using UseCase.SetKbnMst.GetList;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SpecialNote.Get;
using UseCase.UketukeSbtMst.GetBySinDate;
using UseCase.UketukeSbtMst.GetList;
using UseCase.UketukeSbtMst.GetNext;
using UseCase.User.GetByLoginId;
using UseCase.User.GetList;
using UseCase.User.UpsertList;
using UseCase.VisitingList.SaveSettings;

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
            services.AddTransient<IWebSocketService, WebSocketService>();
            services.AddTransient<IAmazonS3Service, AmazonS3Service>();
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
            services.AddTransient<IKarteKbnMstRepository, KarteKbnMstRepository>();
            services.AddTransient<IPatientGroupMstRepository, PatientGroupMstRepository>();
            services.AddTransient<ISetMstRepository, SetMstRepository>();
            services.AddTransient<ISetGenerationMstRepository, SetGenerationMstRepository>();
            services.AddTransient<ISetKbnMstRepository, SetKbnMstRepository>();
            services.AddTransient<IPatientGroupMstRepository, PatientGroupMstRepository>();
            services.AddTransient<IInsuranceMstRepository, InsuranceMstRepository>();
            services.AddTransient<IRaiinFilterMstRepository, RaiinFilterMstRepository>();
            services.AddTransient<IPatientRaiinKubunReponsitory, PatientRaiinKubunReponsitory>();
            services.AddTransient<IPtCmtInfRepository, PtCmtInfRepository>();
            services.AddTransient<IUketukeSbtDayInfRepository, UketukeSbtDayInfRepository>();
            services.AddTransient<ICalculationInfRepository, CalculationInfRepository>();
            services.AddTransient<IImportantNoteRepository, ImportantNoteRepository>();
            services.AddTransient<IPatientInfoRepository, PatientInfoRepository>();
            services.AddTransient<ISummaryInfRepository, SummaryInfRepository>();
            services.AddTransient<IPtCmtInfRepository, PtCmtInfRepository>();
            services.AddTransient<IUserConfRepository, UserConfRepository>();
            services.AddTransient<IFlowSheetRepository, FlowSheetRepository>();
            services.AddTransient<ISystemConfRepository, SystemConfRepository>();
            services.AddTransient<IReceptionInsuranceRepository, ReceptionInsuranceRepository>();
            services.AddTransient<IColumnSettingRepository, ColumnSettingRepository>();
            services.AddTransient<IReceptionSameVisitRepository, ReceptionSameVisitRepository>();
            services.AddTransient<IFlowSheetRepository, FlowSheetRepository>();
            services.AddTransient<ISystemConfRepository, SystemConfRepository>();
            services.AddTransient<IReceptionInsuranceRepository, ReceptionInsuranceRepository>();
            services.AddTransient<IKarteFilterMstRepository, KarteFilterMstRepository>();
            services.AddTransient<IColumnSettingRepository, ColumnSettingRepository>();
            services.AddTransient<IReceptionSameVisitRepository, ReceptionSameVisitRepository>();
            services.AddTransient<IInputItemRepository, InputItemRepository>();
            services.AddTransient<IVisitingListSettingRepository, VisitingListSettingRepository>();
            services.AddTransient<IDrugDetailRepository, DrugDetailRepository>();
            services.AddTransient<IJsonSettingRepository, JsonSettingRepository>();
            services.AddTransient<IMstItemRepository, MstItemRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();
            busBuilder.RegisterUseCase<UpsertUserListInputData, UpsertUserListInteractor>();
            busBuilder.RegisterUseCase<GetUserByLoginIdInputData, GetUserByLoginIdInteractor>();

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

            // Visiting
            busBuilder.RegisterUseCase<SaveVisitingListSettingsInputData, SaveVisitingListSettingsInteractor>();

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

            //SetMst
            busBuilder.RegisterUseCase<GetSetMstListInputData, GetSetMstListInteractor>();
            busBuilder.RegisterUseCase<SaveSetMstInputData, SaveSetMstInteractor>();
            busBuilder.RegisterUseCase<ReorderSetMstInputData, ReorderSetMstInteractor>();
            busBuilder.RegisterUseCase<CopyPasteSetMstInputData, CopyPasteSetMstInteractor>();

            //Medical Examination
            busBuilder.RegisterUseCase<GetMedicalExaminationHistoryInputData, GetMedicalExaminationHistoryInteractor>();

            //SetKbn
            busBuilder.RegisterUseCase<GetSetKbnMstListInputData, GetSetKbnMstListInteractor>();

            //Insurance Mst
            busBuilder.RegisterUseCase<GetInsuranceMstInputData, GetInsuranceMstInteractor>();

            // RaiinFilter
            busBuilder.RegisterUseCase<GetRaiinFilterMstListInputData, GetRaiinFilterMstListInteractor>();
            busBuilder.RegisterUseCase<SaveRaiinFilterMstListInputData, SaveRaiinFilterMstListInteractor>();

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
            busBuilder.RegisterUseCase<UpsertFlowSheetInputData, UpsertFlowSheetInteractor>();

            // UketukeSbtDayInf
            busBuilder.RegisterUseCase<GetReceptionInsuranceInputData, ReceptionInsuranceInteractor>();

            // KarteFilter
            busBuilder.RegisterUseCase<GetKarteFilterInputData, GetKarteFilterMstsInteractor>();

            busBuilder.RegisterUseCase<SaveKarteFilterInputData, SaveKarteFilterMstsInteractor>();

            // ColumnSetting
            busBuilder.RegisterUseCase<SaveColumnSettingListInputData, SaveColumnSettingListInteractor>();
            busBuilder.RegisterUseCase<GetColumnSettingListInputData, GetColumnSettingListInteractor>();

            // JsonSetting
            busBuilder.RegisterUseCase<GetJsonSettingInputData, GetJsonSettingInteractor>();
            busBuilder.RegisterUseCase<UpsertJsonSettingInputData, UpsertJsonSettingInteractor>();

            // Reception Same Visit
            busBuilder.RegisterUseCase<GetReceptionSameVisitInputData, GetReceptionSameVisitInteractor>();

            //Special note
            busBuilder.RegisterUseCase<GetSpecialNoteInputData, GetSpecialNoteInteractor>();
            busBuilder.RegisterUseCase<GetFoodAlrgyInputData, GetFoodAlrgyInteractor>();

            //Input Item
            busBuilder.RegisterUseCase<SearchInputItemInputData, SearchInputItemInteractor>();
            busBuilder.RegisterUseCase<UpdateAdoptedInputItemInputData, UpdateAdoptedInputItemInteractor>();
            busBuilder.RegisterUseCase<GetDosageDrugListInputData, GetDosageDrugListInteractor>();

            // Disease
            busBuilder.RegisterUseCase<UpsertPtDiseaseListInputData, UpsertPtDiseaseListInteractor>();

            // Drug Infor - Data Menu and Detail 
            busBuilder.RegisterUseCase<GetDrugDetailInputData, GetDrugDetailInteractor>();

            // Get HokenMst by FutansyaNo
            busBuilder.RegisterUseCase<GetKohiHokenMstInputData, GetKohiHokenMstInteractor>();

            // Schema
            busBuilder.RegisterUseCase<GetListImageTemplatesInputData, GetListImageTemplatesInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
