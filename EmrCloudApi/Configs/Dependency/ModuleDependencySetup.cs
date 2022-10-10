using Domain.CalculationInf;
using Domain.Models.ColumnSetting;
using Domain.Models.Diseases;
using Domain.Models.DrugDetail;
using Domain.Models.DrugInfor;
using Domain.Models.FlowSheet;
using Domain.Models.GroupInf;
using Domain.Models.HpMst;
using Domain.Models.HokenMst;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.JsonSetting;
using Domain.Models.Ka;
using Domain.Models.KarteFilterMst;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.MaxMoney;
using Domain.Models.MonshinInf;
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
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using Domain.Models.ReceptionInsurance;
using Domain.Models.ReceptionLock;
using Domain.Models.ReceptionSameVisit;
using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using Domain.Models.SetMst;
using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.SuperSetDetail;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TodayOdr;
using Domain.Models.UketukeSbtDayInf;
using Domain.Models.UketukeSbtMst;
using Domain.Models.UsageTreeSet;
using Domain.Models.User;
using Domain.Models.UserConf;
using Domain.Models.VisitingListSetting;
using EmrCloudApi.Realtime;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.SpecialNote;
using Infrastructure.Services;
using Interactor.Byomei;
using Interactor.CalculationInf;
using Interactor.ColumnSetting;
using Interactor.Diseases;
using Interactor.DrugDetail;
using Interactor.DrugInfor;
using Interactor.FlowSheet;
using Interactor.GrpInf;
using Interactor.HokenMst;
using Interactor.Insurance;
using Interactor.InsuranceMst;
using Interactor.JsonSetting;
using Interactor.Ka;
using Interactor.KarteFilter;
using Interactor.KarteInfs;
using Interactor.KohiHokenMst;
using Interactor.MaxMoney;
using Interactor.MedicalExamination;
using Interactor.MonshinInf;
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
using Interactor.ReceptionVisiting;
using Interactor.Schema;
using Interactor.SetKbnMst;
using Interactor.SetMst;
using Interactor.SpecialNote;
using Interactor.SuperSetDetail;
using Interactor.SystemConf;
using Interactor.UketukeSbtMst;
using Interactor.UsageTreeSet;
using Interactor.User;
using Interactor.VisitingList;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UseCase.CalculationInf;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.Core.Builder;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Diseases.Upsert;
using UseCase.DrugDetail;
using UseCase.DrugInfor.Get;
using UseCase.FlowSheet.GetList;
using UseCase.FlowSheet.Upsert;
using UseCase.GroupInf.GetList;
using UseCase.HokenMst.GetDetail;
using UseCase.Insurance.GetList;
using UseCase.Insurance.ValidateRousaiJibai;
using UseCase.Insurance.ValidKohi;
using UseCase.Insurance.ValidMainInsurance;
using UseCase.Insurance.ValidPatternExpirated;
using UseCase.Insurance.ValidPatternOther;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.Upsert;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetList;
using UseCase.Ka.SaveList;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;
using UseCase.KarteInfs.GetLists;
using UseCase.KohiHokenMst.Get;
using UseCase.MaxMoney.GetMaxMoney;
using UseCase.MaxMoney.SaveMaxMoney;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.MonshinInfor.GetList;
using UseCase.MonshinInfor.Save;
using UseCase.MstItem.DiseaseSearch;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.GetFoodAlrgy;
using UseCase.MstItem.SearchOTC;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchSupplement;
using UseCase.MstItem.SearchTenItem;
using UseCase.MstItem.UpdateAdopted;
using UseCase.MstItem.UpdateAdoptedByomei;
using UseCase.OrdInfs.GetHeaderInf;
using UseCase.OrdInfs.GetListTrees;
using UseCase.OrdInfs.GetMaxRpNo;
using UseCase.OrdInfs.Validation;
using UseCase.OrdInfs.ValidationInputItem;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientGroupMst.SaveList;
using UseCase.PatientInfor.PatientComment;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchEmptyId;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using UseCase.PatientRaiinKubun.Get;
using UseCase.RaiinFilterMst.GetList;
using UseCase.RaiinFilterMst.SaveList;
using UseCase.RaiinKubunMst.GetList;
using UseCase.RaiinKubunMst.LoadData;
using UseCase.Reception.Get;
using UseCase.Reception.GetList;
using UseCase.Reception.GetSettings;
using UseCase.Reception.Insert;
using UseCase.Reception.ReceptionComment;
using UseCase.Reception.Update;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;
using UseCase.ReceptionVisiting.Get;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.SaveImageTodayOrder;
using UseCase.SearchHokensyaMst.Get;
using UseCase.SetKbnMst.GetList;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SpecialNote.AddAlrgyDrugList;
using UseCase.SpecialNote.Get;
using UseCase.SpecialNote.Save;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SuperSetDetail;
using UseCase.SystemConf;
using UseCase.UketukeSbtMst.GetBySinDate;
using UseCase.UketukeSbtMst.GetList;
using UseCase.UketukeSbtMst.GetNext;
using UseCase.UsageTreeSet.GetTree;
using UseCase.User.GetByLoginId;
using UseCase.User.GetList;
using UseCase.User.UpsertList;
using UseCase.VisitingList.ReceptionLock;
using UseCase.VisitingList.SaveSettings;
using UseCase.StickyNote;
using Domain.Models.PtTag;
using Interactor.StickyNote;

namespace EmrCloudApi.Configs.Dependency
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
            services.AddTransient<IKaRepository, KaRepository>();
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
            services.AddTransient<IVisitingListSettingRepository, VisitingListSettingRepository>();
            services.AddTransient<IDrugDetailRepository, DrugDetailRepository>();
            services.AddTransient<IJsonSettingRepository, JsonSettingRepository>();
            services.AddTransient<ISystemGenerationConfRepository, SystemGenerationConfRepository>();
            services.AddTransient<IMstItemRepository, MstItemRepository>();
            services.AddTransient<IReceptionLockRepository, ReceptionLockRepository>();
            services.AddTransient<IDrugInforRepository, DrugInforRepository>();
            services.AddTransient<ISuperSetDetailRepository, SuperSetDetailRepository>();
            services.AddTransient<IUsageTreeSetRepository, UsageTreeSetRepository>();
            services.AddTransient<IMaxmoneyReposiory, MaxmoneyReposiory>();
            services.AddTransient<IMonshinInforRepository, MonshinInforRepository>();
            services.AddTransient<IRaiinListTagRepository, RaiinListTagRepository>();
            services.AddTransient<ISpecialNoteRepository, SpecialNoteRepository>();
            services.AddTransient<IHpInfRepository, HpInfRepository>();
            services.AddTransient<ITodayOdrRepository, TodayOdrRepository>();
            services.AddTransient<IHokenMstRepository, HokenMstRepository>();
            services.AddTransient<IPtTagRepository, PtTagRepository>();
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
            busBuilder.RegisterUseCase<GetMaxRpNoInputData, GetMaxRpNoInteractor>();
            busBuilder.RegisterUseCase<GetHeaderInfInputData, GetHeaderInfInteractor>();

            //Reception
            busBuilder.RegisterUseCase<GetReceptionInputData, GetReceptionInteractor>();
            busBuilder.RegisterUseCase<InsertReceptionInputData, InsertReceptionInteractor>();
            busBuilder.RegisterUseCase<UpdateReceptionInputData, UpdateReceptionInteractor>();
            busBuilder.RegisterUseCase<GetReceptionListInputData, GetReceptionListInteractor>();
            busBuilder.RegisterUseCase<UpdateReceptionStaticCellInputData, UpdateReceptionStaticCellInteractor>();
            busBuilder.RegisterUseCase<UpdateReceptionDynamicCellInputData, UpdateReceptionDynamicCellInteractor>();
            busBuilder.RegisterUseCase<GetReceptionSettingsInputData, GetReceptionSettingsInteractor>();
            busBuilder.RegisterUseCase<GetPatientRaiinKubunInputData, GetPatientRaiinKubunInteractor>();
            busBuilder.RegisterUseCase<GetReceptionCommentInputData, GetReceptionCommentInteractor>();
            busBuilder.RegisterUseCase<GetReceptionLockInputData, GetReceptionLockInteractor>();

            // Visiting
            busBuilder.RegisterUseCase<SaveVisitingListSettingsInputData, SaveVisitingListSettingsInteractor>();
            busBuilder.RegisterUseCase<GetPatientCommentInputData, GetPatientCommentInteractor>();
            busBuilder.RegisterUseCase<GetReceptionVisitingInputData, GetReceptionVisitingInteractor>();

            //Insurance
            busBuilder.RegisterUseCase<GetInsuranceListInputData, GetInsuranceListInteractor>();
            busBuilder.RegisterUseCase<ValidMainInsuranceInputData, ValidInsuranceMainInteractor>();
            busBuilder.RegisterUseCase<ValidInsuranceOtherInputData, ValidInsuranceOtherInteractor>();

            //Karte
            busBuilder.RegisterUseCase<GetListKarteInfInputData, GetListKarteInfInteractor>();

            // PatientInfor
            busBuilder.RegisterUseCase<GetPatientInforByIdInputData, GetPatientInforByIdInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoSimpleInputData, SearchPatientInfoSimpleInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoAdvancedInputData, SearchPatientInfoAdvancedInteractor>();
            busBuilder.RegisterUseCase<GetListPatientGroupMstInputData, GetListPatientGroupMstInteractor>();
            busBuilder.RegisterUseCase<SaveListPatientGroupMstInputData, SaveListPatientGroupMstInteractor>();
            busBuilder.RegisterUseCase<SearchEmptyIdInputData, SearchEmptyIdInteractor>();
            busBuilder.RegisterUseCase<ValidateRousaiJibaiInputData, ValidateRousaiJibaiInteractor>();
            busBuilder.RegisterUseCase<ValidKohiInputData, ValidateKohiInteractor>();

            //RaiinKubun
            busBuilder.RegisterUseCase<GetRaiinKubunMstListInputData, GetRaiinKubunMstListInteractor>();
            busBuilder.RegisterUseCase<LoadDataKubunSettingInputData, LoadDataKubunSettingInteractor>();


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
            busBuilder.RegisterUseCase<UpsertTodayOrdInputData, UpsertTodayOrdInteractor>();

            //SetKbn
            busBuilder.RegisterUseCase<GetSetKbnMstListInputData, GetSetKbnMstListInteractor>();

            //Insurance Mst
            busBuilder.RegisterUseCase<GetInsuranceMstInputData, GetInsuranceMstInteractor>();

            // RaiinFilter
            busBuilder.RegisterUseCase<GetRaiinFilterMstListInputData, GetRaiinFilterMstListInteractor>();
            busBuilder.RegisterUseCase<SaveRaiinFilterMstListInputData, SaveRaiinFilterMstListInteractor>();

            // Ka
            busBuilder.RegisterUseCase<GetKaMstListInputData, GetKaMstListInteractor>();
            busBuilder.RegisterUseCase<GetKaCodeMstInputData, GetKaCodeMstInteractor>();
            busBuilder.RegisterUseCase<SaveKaMstInputData, SaveKaMstInteractor>();

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
            busBuilder.RegisterUseCase<SaveSpecialNoteInputData, SaveSpecialNoteInteractor>();
            busBuilder.RegisterUseCase<AddAlrgyDrugListInputData, AddAlrgyDrugListInteractor>();

            // StickyNote
            busBuilder.RegisterUseCase<GetStickyNoteInputData, GetStickyNoteInteractor>();
            busBuilder.RegisterUseCase<RevertStickyNoteInputData, RevertStickyNoteInteractor>();
            busBuilder.RegisterUseCase<DeleteStickyNoteInputData, DeleteStickyNoteInteractor>();


            //MS Item
            busBuilder.RegisterUseCase<SearchTenItemInputData, SearchTenItemInteractor>();
            busBuilder.RegisterUseCase<UpdateAdoptedTenItemInputData, UpdateAdoptedTenItemInteractor>();
            busBuilder.RegisterUseCase<GetDosageDrugListInputData, GetDosageDrugListInteractor>();
            busBuilder.RegisterUseCase<SearchOTCInputData, SearchOtcInteractor>();
            busBuilder.RegisterUseCase<SearchSupplementInputData, SearchSupplementInteractor>();
            busBuilder.RegisterUseCase<UpdateAdoptedByomeiInputData, UpdateAdoptedByomeiInteractor>();
            busBuilder.RegisterUseCase<GetFoodAlrgyInputData, GetFoodAlrgyInteractor>();
            busBuilder.RegisterUseCase<SearchPostCodeInputData, SearchPostCodeInteractor>();

            // Disease
            busBuilder.RegisterUseCase<UpsertPtDiseaseListInputData, UpsertPtDiseaseListInteractor>();
            busBuilder.RegisterUseCase<DiseaseSearchInputData, DiseaseSearchInteractor>();

            // Drug Infor - Data Menu and Detail 
            busBuilder.RegisterUseCase<GetDrugDetailInputData, GetDrugDetailInteractor>();

            //DrugInfor
            busBuilder.RegisterUseCase<GetDrugInforInputData, GetDrugInforInteractor>();

            // Get HokenMst by FutansyaNo
            busBuilder.RegisterUseCase<GetKohiHokenMstInputData, GetKohiHokenMstInteractor>();

            // Schema
            busBuilder.RegisterUseCase<SaveImageTodayOrderInputData, SaveImageTodayOrderInteractor>();
            busBuilder.RegisterUseCase<GetListImageTemplatesInputData, GetListImageTemplatesInteractor>();

            // SuperSetDetail
            busBuilder.RegisterUseCase<GetSuperSetDetailInputData, GetSuperSetDetailInteractor>();
            busBuilder.RegisterUseCase<SaveSuperSetDetailInputData, SaveSuperSetDetailInteractor>();

            //Validation TodayOrder
            busBuilder.RegisterUseCase<ValidationOrdInfListInputData, ValidationOrdInfListInteractor>();

            //UsageTreeSet
            busBuilder.RegisterUseCase<GetUsageTreeSetInputData, GetUsageTreeSetInteractor>();

            //Maxmoney
            busBuilder.RegisterUseCase<GetMaxMoneyInputData, GetMaxMoneyInteractor>();
            busBuilder.RegisterUseCase<SaveMaxMoneyInputData, SaveMaxMoneyInteractor>();

            //Monshin
            busBuilder.RegisterUseCase<GetMonshinInforListInputData, GetMonshinInforListInteractor>();
            busBuilder.RegisterUseCase<SaveMonshinInputData, SaveMonshinInforListInteractor>();

            // Reception - Valid Pattern Expirated
            busBuilder.RegisterUseCase<ValidPatternExpiratedInputData, ValidPatternExpiratedInteractor>();

            //System Conf
            busBuilder.RegisterUseCase<GetSystemConfInputData, GetSystemConfInteractor>();

            //SaveHokenSya
            busBuilder.RegisterUseCase<SaveHokenSyaMstInputData, SaveHokenSyaMstInteractor>();

            //HokenMst
            busBuilder.RegisterUseCase<GetDetailHokenMstInputData, GetDetailHokenMstInteractor>();

            //Validate InputItem 
            busBuilder.RegisterUseCase<ValidationInputItemInputData, ValidationInputitemInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
