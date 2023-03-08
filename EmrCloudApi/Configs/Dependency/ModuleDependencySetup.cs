using CommonChecker.DB;
using CommonChecker.Services;
using Domain.CalculationInf;
using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.ApprovalInfo;
using Domain.Models.ColumnSetting;
using Domain.Models.Diseases;
using Domain.Models.Document;
using Domain.Models.DrugDetail;
using Domain.Models.DrugInfor;
using Domain.Models.Family;
using Domain.Models.FlowSheet;
using Domain.Models.GroupInf;
using Domain.Models.HistoryOrder;
using Domain.Models.HokenMst;
using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.JsonSetting;
using Domain.Models.Ka;
using Domain.Models.KarteFilterMst;
using Domain.Models.KarteInfs;
using Domain.Models.KarteKbnMst;
using Domain.Models.MaxMoney;
using Domain.Models.MedicalExamination;
using Domain.Models.MonshinInf;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfs;
using Domain.Models.PatientGroupMst;
using Domain.Models.PatientInfor;
using Domain.Models.PtCmtInf;
using Domain.Models.PtGroupMst;
using Domain.Models.PtTag;
using Domain.Models.RaiinCmtInf;
using Domain.Models.RaiinFilterMst;
using Domain.Models.RaiinKubunMst;
using Domain.Models.RainListTag;
using Domain.Models.Receipt;
using Domain.Models.Reception;
using Domain.Models.ReceptionInsurance;
using Domain.Models.ReceptionLock;
using Domain.Models.ReceptionSameVisit;
using Domain.Models.ReceSeikyu;
using Domain.Models.RsvInf;
using Domain.Models.Santei;
using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using Domain.Models.SetMst;
using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.SuperSetDetail;
using Domain.Models.SwapHoken;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TimeZone;
using Domain.Models.TodayOdr;
using Domain.Models.UketukeSbtDayInf;
using Domain.Models.UketukeSbtMst;
using Domain.Models.UsageTreeSet;
using Domain.Models.User;
using Domain.Models.UserConf;
using Domain.Models.VisitingListSetting;
using Domain.Models.YohoSetMst;
using EmrCloudApi.Realtime;
using EmrCloudApi.Services;
using EventProcessor.Interfaces;
using EventProcessor.Service;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.SpecialNote;
using Infrastructure.Services;
using Interactor.AccountDue;
using Interactor.Accounting;
using Interactor.ApprovalInfo;
using Interactor.Byomei;
using Interactor.CalculationInf;
using Interactor.ColumnSetting;
using Interactor.CommonChecker;
using Interactor.CommonChecker.CommonMedicalCheck;
using Interactor.Diseases;
using Interactor.Document;
using Interactor.Document.CommonGetListParam;
using Interactor.DrugDetail;
using Interactor.DrugDetailData;
using Interactor.DrugInfor;
using Interactor.Family;
using Interactor.FlowSheet;
using Interactor.GrpInf;
using Interactor.HokenMst;
using Interactor.Insurance;
using Interactor.InsuranceMst;
using Interactor.JsonSetting;
using Interactor.Ka;
using Interactor.KarteFilter;
using Interactor.KarteInf;
using Interactor.KarteInfs;
using Interactor.KohiHokenMst;
using Interactor.MaxMoney;
using Interactor.MedicalExamination;
using Interactor.MonshinInf;
using Interactor.MstItem;
using Interactor.NextOrder;
using Interactor.OrdInfs;
using Interactor.PatientGroupMst;
using Interactor.PatientInfor;
using Interactor.PatientInfor.PtKyuseiInf;
using Interactor.PtGroupMst;
using Interactor.RaiinFilterMst;
using Interactor.RaiinKubunMst;
using Interactor.Receipt;
using Interactor.Reception;
using Interactor.ReceptionInsurance;
using Interactor.ReceptionSameVisit;
using Interactor.ReceptionVisiting;
using Interactor.ReceSeikyu;
using Interactor.Santei;
using Interactor.Schema;
using Interactor.SetKbnMst;
using Interactor.SetMst;
using Interactor.SpecialNote;
using Interactor.StickyNote;
using Interactor.SuperSetDetail;
using Interactor.SwapHoken;
using Interactor.SystemConf;
using Interactor.SystemGenerationConf;
using Interactor.UketukeSbtMst;
using Interactor.UsageTreeSet;
using Interactor.User;
using Interactor.UserConf;
using Interactor.VisitingList;
using Interactor.YohoSetMst;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Reporting;
using Reporting.Interface;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.AccountDue.SaveAccountDueList;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.GetAccountingHeader;
using UseCase.Accounting.GetAccountingInf;
using UseCase.Accounting.GetAccountingSystemConf;
using UseCase.Accounting.GetHistoryOrder;
using UseCase.Accounting.GetPtByoMei;
using UseCase.Accounting.PaymentMethod;
using UseCase.Accounting.SaveAccounting;
using UseCase.Accounting.WarningMemo;
using UseCase.ApprovalInfo.GetApprovalInfList;
using UseCase.ApprovalInfo.UpdateApprovalInfList;
using UseCase.CalculationInf;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.CommonChecker;
using UseCase.Core.Builder;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Diseases.GetSetByomeiTree;
using UseCase.Diseases.Upsert;
using UseCase.Document.CheckExistFileName;
using UseCase.Document.ConfirmReplaceDocParam;
using UseCase.Document.DeleteDocCategory;
using UseCase.Document.DeleteDocInf;
using UseCase.Document.DeleteDocTemplate;
using UseCase.Document.DownloadDocumentTemplate;
using UseCase.Document.GetDocCategoryDetail;
using UseCase.Document.GetListDocCategory;
using UseCase.Document.GetListDocComment;
using UseCase.Document.GetListParamTemplate;
using UseCase.Document.MoveTemplateToOtherCategory;
using UseCase.Document.SaveDocInf;
using UseCase.Document.SaveListDocCategory;
using UseCase.Document.SortDocCategory;
using UseCase.Document.UploadTemplateToCategory;
using UseCase.DrugDetail;
using UseCase.DrugDetailData.Get;
using UseCase.DrugDetailData.ShowKanjaMuke;
using UseCase.DrugDetailData.ShowMdbByomei;
using UseCase.DrugDetailData.ShowProductInf;
using UseCase.DrugInfor.Get;
using UseCase.Family.GetFamilyList;
using UseCase.Family.GetFamilyReverserList;
using UseCase.Family.SaveFamilyList;
using UseCase.FlowSheet.GetList;
using UseCase.FlowSheet.Upsert;
using UseCase.GroupInf.GetList;
using UseCase.HokenMst.GetDetail;
using UseCase.Insurance.GetComboList;
using UseCase.Insurance.GetDefaultSelectPattern;
using UseCase.Insurance.GetKohiPriorityList;
using UseCase.Insurance.GetList;
using UseCase.Insurance.HokenPatternUsed;
using UseCase.Insurance.ValidateInsurance;
using UseCase.Insurance.ValidateRousaiJibai;
using UseCase.Insurance.ValidHokenInfAllType;
using UseCase.Insurance.ValidKohi;
using UseCase.Insurance.ValidMainInsurance;
using UseCase.Insurance.ValidPatternExpirated;
using UseCase.Insurance.ValidPatternOther;
using UseCase.InsuranceMst.DeleteHokenMaster;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.GetHokenSyaMst;
using UseCase.InsuranceMst.GetInfoCloneInsuranceMst;
using UseCase.InsuranceMst.GetMasterDetails;
using UseCase.InsuranceMst.GetSelectMaintenance;
using UseCase.InsuranceMst.SaveHokenMaster;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.Upsert;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetList;
using UseCase.Ka.SaveList;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;
using UseCase.KarteInf.ConvertTextToRichText;
using UseCase.KarteInf.GetList;
using UseCase.KohiHokenMst.Get;
using UseCase.MaxMoney.GetMaxMoney;
using UseCase.MaxMoney.GetMaxMoneyByPtId;
using UseCase.MaxMoney.SaveMaxMoney;
using UseCase.MedicalExamination.AddAutoItem;
using UseCase.MedicalExamination.AutoCheckOrder;
using UseCase.MedicalExamination.ChangeAfterAutoCheckOrder;
using UseCase.MedicalExamination.CheckedAfter327Screen;
using UseCase.MedicalExamination.CheckedExpired;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.ConvertFromHistoryTodayOrder;
using UseCase.MedicalExamination.ConvertInputItemToTodayOdr;
using UseCase.MedicalExamination.ConvertNextOrderToTodayOdr;
using UseCase.MedicalExamination.GetAddedAutoItem;
using UseCase.MedicalExamination.GetCheckDisease;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.GetHistoryIndex;
using UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint;
using UseCase.MedicalExamination.GetValidGairaiRiha;
using UseCase.MedicalExamination.GetValidJihiYobo;
using UseCase.MedicalExamination.InitKbnSetting;
using UseCase.MedicalExamination.SearchHistory;
using UseCase.MedicalExamination.SummaryInf;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.MonshinInfor.GetList;
using UseCase.MonshinInfor.Save;
using UseCase.MstItem.DiseaseSearch;
using UseCase.MstItem.FindTenMst;
using UseCase.MstItem.GetAdoptedItemList;
using UseCase.MstItem.GetCmtCheckMstList;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.GetFoodAlrgy;
using UseCase.MstItem.GetSelectiveComment;
using UseCase.MstItem.SearchOTC;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchSupplement;
using UseCase.MstItem.SearchTenItem;
using UseCase.MstItem.UpdateAdopted;
using UseCase.MstItem.UpdateAdoptedByomei;
using UseCase.MstItem.UpdateAdoptedItemList;
using UseCase.NextOrder.Get;
using UseCase.NextOrder.GetList;
using UseCase.NextOrder.Upsert;
using UseCase.NextOrder.Validation;
using UseCase.OrdInfs.CheckedSpecialItem;
using UseCase.OrdInfs.GetHeaderInf;
using UseCase.OrdInfs.GetListTrees;
using UseCase.OrdInfs.GetMaxRpNo;
using UseCase.OrdInfs.ValidationInputItem;
using UseCase.OrdInfs.ValidationTodayOrd;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientGroupMst.SaveList;
using UseCase.PatientInfor.DeletePatient;
using UseCase.PatientInfor.GetInsuranceMasterLinkage;
using UseCase.PatientInfor.GetListPatient;
using UseCase.PatientInfor.GetPatientInfoBetweenTimesList;
using UseCase.PatientInfor.PatientComment;
using UseCase.PatientInfor.PtKyuseiInf.GetList;
using UseCase.PatientInfor.Save;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchEmptyId;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using UseCase.PtGroupMst.CheckAllowDelete;
using UseCase.PtGroupMst.GetGroupNameMst;
using UseCase.PtGroupMst.SaveGroupNameMst;
using UseCase.RaiinFilterMst.GetList;
using UseCase.RaiinFilterMst.SaveList;
using UseCase.RaiinKbn.GetPatientRaiinKubunList;
using UseCase.RaiinKubunMst.GetList;
using UseCase.RaiinKubunMst.GetListColumnName;
using UseCase.RaiinKubunMst.LoadData;
using UseCase.RaiinKubunMst.Save;
using UseCase.RaiinKubunMst.SaveRaiinKbnInfList;
using UseCase.Receipt.GetDiseaseReceList;
using UseCase.Receipt.GetInsuranceReceInfList;
using UseCase.Receipt.GetListSyobyoKeika;
using UseCase.Receipt.GetListSyoukiInf;
using UseCase.Receipt.GetReceCheckOptionList;
using UseCase.Receipt.GetReceCmt;
using UseCase.Receipt.GetReceHenReason;
using UseCase.Receipt.GetReceiCheckList;
using UseCase.Receipt.Recalculation;
using UseCase.Receipt.ReceiptListAdvancedSearch;
using UseCase.Receipt.SaveListReceCmt;
using UseCase.Receipt.SaveListSyobyoKeika;
using UseCase.Receipt.SaveListSyoukiInf;
using UseCase.Receipt.SaveReceCheckCmtList;
using UseCase.Reception.Get;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetList;
using UseCase.Reception.GetListRaiinInfs;
using UseCase.Reception.GetReceptionDefault;
using UseCase.Reception.GetSettings;
using UseCase.Reception.InitDoctorCombo;
using UseCase.Reception.Insert;
using UseCase.Reception.ReceptionComment;
using UseCase.Reception.Update;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.Reception.UpdateTimeZoneDayInf;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;
using UseCase.ReceptionVisiting.Get;
using UseCase.ReceSeikyu.GetList;
using UseCase.Santei.GetListSanteiInf;
using UseCase.Santei.SaveListSanteiInf;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.GetListInsuranceScan;
using UseCase.Schema.SaveListFileTodayOrder;
using UseCase.SearchHokensyaMst.Get;
using UseCase.SetKbnMst.GetList;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.GetToolTip;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SpecialNote.AddAlrgyDrugList;
using UseCase.SpecialNote.Get;
using UseCase.SpecialNote.Save;
using UseCase.StickyNote;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SuperSetDetail;
using UseCase.SwapHoken.Save;
using UseCase.SwapHoken.Validate;
using UseCase.SystemConf.Get;
using UseCase.SystemConf.GetSystemConfForPrint;
using UseCase.SystemConf.GetSystemConfList;
using UseCase.SystemGenerationConf;
using UseCase.UketukeSbtMst.GetBySinDate;
using UseCase.UketukeSbtMst.GetList;
using UseCase.UketukeSbtMst.GetNext;
using UseCase.UketukeSbtMst.Upsert;
using UseCase.UsageTreeSet.GetTree;
using UseCase.User.CheckedLockMedicalExamination;
using UseCase.User.GetByLoginId;
using UseCase.User.GetList;
using UseCase.User.GetPermissionByScreenCode;
using UseCase.User.GetUserConfList;
using UseCase.User.MigrateDatabase;
using UseCase.User.Sagaku;
using UseCase.User.UpdateUserConf;
using UseCase.User.UpsertList;
using UseCase.User.UpsertUserConfList;
using UseCase.UserConf.GetListMedicalExaminationConfig;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;
using UseCase.VisitingList.ReceptionLock;
using UseCase.VisitingList.SaveSettings;
using UseCase.YohoSetMst.GetByItemCd;
using GetDefaultSelectedTimeInputDataOfMedical = UseCase.MedicalExamination.GetDefaultSelectedTime.GetDefaultSelectedTimeInputData;
using GetDefaultSelectedTimeInputDataOfReception = UseCase.Reception.GetDefaultSelectedTime.GetDefaultSelectedTimeInputData;
using GetDefaultSelectedTimeInteractorOfMedical = Interactor.MedicalExamination.GetDefaultSelectedTimeInteractor;
using GetDefaultSelectedTimeInteractorOfReception = Interactor.Reception.GetDefaultSelectedTimeInteractor;
using GetListRaiinInfInputDataOfFamily = UseCase.Family.GetRaiinInfList.GetRaiinInfListInputData;
using GetListRaiinInfInteractorOfFamily = Interactor.Family.GetListRaiinInfInteractor;
using GetListRaiinInfInteractorOfReception = Interactor.Reception.GetListRaiinInfInteractor;

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
            services.AddTransient<IUserService, UserService>();

            //Cache data
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IKaService, KaService>();
            services.AddScoped<ISystemConfigService, SystemConfigService>();

            services.AddTransient<IEventProcessorService, EventProcessorService>();
            services.AddTransient<IReportService, ReportService>();
        }

        private void SetupRepositories(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IApprovalInfRepository, ApprovalinfRepository>();
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
            services.AddTransient<IAccountDueRepository, AccountDueRepository>();
            services.AddTransient<ITimeZoneRepository, TimeZoneRepository>();
            services.AddTransient<ISwapHokenRepository, SwapHokenRepository>();
            services.AddTransient<INextOrderRepository, NextOrderRepository>();
            services.AddTransient<IYohoSetMstRepository, YohoSetMstRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IHistoryOrderRepository, HistoryOrderRepository>();
            services.AddTransient<ICommonGetListParam, CommonGetListParam>();
            services.AddTransient<IGroupNameMstRepository, GroupNameMstRepository>();
            services.AddTransient<IAccountingRepository, AccountingRepository>();
            services.AddTransient<ISanteiInfRepository, SanteiInfRepository>();
            services.AddTransient<IMedicalExaminationRepository, MedicalExaminationRepository>();
            services.AddTransient<ISystemConfigRepository, SystemConfRepostitory>();
            services.AddTransient<IRealtimeOrderErrorFinder, RealtimeOrderErrorFinder>();
            services.AddTransient<IFamilyRepository, FamilyRepository>();
            services.AddTransient<IReceiptRepository, ReceiptRepository>();
            services.AddTransient<IRsvInfRepository, RsvInfRepository>();
            services.AddTransient<ICommonMedicalCheck, CommonMedicalCheck>();
            services.AddTransient<IReceSeikyuRepository, ReceSeikyuRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<GetUserListInputData, GetUserListInteractor>();
            busBuilder.RegisterUseCase<UpsertUserListInputData, UpsertUserListInteractor>();
            busBuilder.RegisterUseCase<GetUserByLoginIdInputData, GetUserByLoginIdInteractor>();
            busBuilder.RegisterUseCase<MigrateDatabaseInputData, MigrateDatabaseInterator>();
            busBuilder.RegisterUseCase<CheckedLockMedicalExaminationInputData, CheckedLockMedicalExaminationInteractor>();
            busBuilder.RegisterUseCase<GetPermissionByScreenInputData, GetPermissionByScreenInteractor>();

            //ApprovalInfo
            busBuilder.RegisterUseCase<GetApprovalInfListInputData, GetApprovalInfListInteractor>();
            busBuilder.RegisterUseCase<UpdateApprovalInfListInputData, UpdateApprovalInfListInteractor>();

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
            busBuilder.RegisterUseCase<GetPatientRaiinKubunListInputData, GetPatientRaiinKubunListInteractor>();
            busBuilder.RegisterUseCase<GetReceptionCommentInputData, GetReceptionCommentInteractor>();
            busBuilder.RegisterUseCase<GetReceptionLockInputData, GetReceptionLockInteractor>();
            busBuilder.RegisterUseCase<GetLastRaiinInfsInputData, GetLastRaiinInfsInteractor>();
            busBuilder.RegisterUseCase<GetReceptionDefaultInputData, GetReceptionDefaultInteractor>();
            busBuilder.RegisterUseCase<InitDoctorComboInputData, InitDoctorComboInteractor>();
            busBuilder.RegisterUseCase<GetListRaiinInfInputDataOfFamily, GetListRaiinInfInteractorOfFamily>();

            // Visiting
            busBuilder.RegisterUseCase<SaveVisitingListSettingsInputData, SaveVisitingListSettingsInteractor>();
            busBuilder.RegisterUseCase<GetPatientCommentInputData, GetPatientCommentInteractor>();
            busBuilder.RegisterUseCase<GetReceptionVisitingInputData, GetReceptionVisitingInteractor>();

            //Insurance
            busBuilder.RegisterUseCase<GetInsuranceListInputData, GetInsuranceListInteractor>();
            busBuilder.RegisterUseCase<ValidMainInsuranceInputData, ValidInsuranceMainInteractor>();
            busBuilder.RegisterUseCase<ValidInsuranceOtherInputData, ValidInsuranceOtherInteractor>();
            busBuilder.RegisterUseCase<GetInsuranceComboListInputData, GetInsuranceComboListInteractor>();
            busBuilder.RegisterUseCase<ValidHokenInfAllTypeInputData, ValidHokenInfAllTypeInteractor>();
            busBuilder.RegisterUseCase<HokenPatternUsedInputData, HokenPatternUsedInteractor>();

            //Karte
            busBuilder.RegisterUseCase<GetListKarteInfInputData, GetListKarteInfInteractor>();
            busBuilder.RegisterUseCase<ConvertTextToRichTextInputData, ConvertTextToRichTextInteractor>();

            // PatientInfor
            busBuilder.RegisterUseCase<GetPatientInforByIdInputData, GetPatientInforByIdInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoSimpleInputData, SearchPatientInfoSimpleInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoAdvancedInputData, SearchPatientInfoAdvancedInteractor>();
            busBuilder.RegisterUseCase<GetListPatientGroupMstInputData, GetListPatientGroupMstInteractor>();
            busBuilder.RegisterUseCase<SaveListPatientGroupMstInputData, SaveListPatientGroupMstInteractor>();
            busBuilder.RegisterUseCase<SearchEmptyIdInputData, SearchEmptyIdInteractor>();
            busBuilder.RegisterUseCase<ValidateRousaiJibaiInputData, ValidateRousaiJibaiInteractor>();
            busBuilder.RegisterUseCase<ValidKohiInputData, ValidateKohiInteractor>();
            busBuilder.RegisterUseCase<GetInsuranceMasterLinkageInputData, GetInsuranceMasterLinkageInteractor>();
            busBuilder.RegisterUseCase<SaveInsuranceMasterLinkageInputData, SaveInsuranceMasterLinkageInteractor>();
            busBuilder.RegisterUseCase<GetPtKyuseiInfInputData, GetPtKyuseiInfInteractor>();
            busBuilder.RegisterUseCase<SavePatientInfoInputData, SavePatientInfoInteractor>();
            busBuilder.RegisterUseCase<DeletePatientInfoInputData, DeletePatientInfoInteractor>();
            busBuilder.RegisterUseCase<ValidateInsuranceInputData, ValidateInsuranceInteractor>();
            busBuilder.RegisterUseCase<GetOrderCheckerInputData, CommonCheckerInteractor>();
            busBuilder.RegisterUseCase<GetListPatientInfoInputData, GetListPatientInfoInteractor>();
            busBuilder.RegisterUseCase<GetPatientInfoBetweenTimesListInputData, GetPatientInfoBetweenTimesListInteractor>();

            //RaiinKubun
            busBuilder.RegisterUseCase<GetRaiinKubunMstListInputData, GetRaiinKubunMstListInteractor>();
            busBuilder.RegisterUseCase<LoadDataKubunSettingInputData, LoadDataKubunSettingInteractor>();
            busBuilder.RegisterUseCase<SaveDataKubunSettingInputData, SaveDataKubunSettingInteractor>();
            busBuilder.RegisterUseCase<GetColumnNameListInputData, GetColumnNameListInteractor>();
            busBuilder.RegisterUseCase<SaveRaiinKbnInfListInputData, SaveRaiinKbnInfListInteractor>();

            //Calculation Inf
            busBuilder.RegisterUseCase<CalculationInfInputData, CalculationInfInteractor>();

            //Group Inf
            busBuilder.RegisterUseCase<GetListGroupInfInputData, GroupInfInteractor>();

            //SetMst
            busBuilder.RegisterUseCase<GetSetMstListInputData, GetSetMstListInteractor>();
            busBuilder.RegisterUseCase<SaveSetMstInputData, SaveSetMstInteractor>();
            busBuilder.RegisterUseCase<ReorderSetMstInputData, ReorderSetMstInteractor>();
            busBuilder.RegisterUseCase<CopyPasteSetMstInputData, CopyPasteSetMstInteractor>();
            busBuilder.RegisterUseCase<GetSetMstToolTipInputData, GetSetMstToolTipInteractor>();

            //Medical Examination
            busBuilder.RegisterUseCase<GetMedicalExaminationHistoryInputData, GetMedicalExaminationHistoryInteractor>();
            busBuilder.RegisterUseCase<UpsertTodayOrdInputData, UpsertTodayOrdInteractor>();
            busBuilder.RegisterUseCase<GetCheckDiseaseInputData, GetCheckDiseaseInteractor>();
            busBuilder.RegisterUseCase<CheckedSpecialItemInputData, CheckedSpecialItemInteractor>();
            busBuilder.RegisterUseCase<GetCheckedOrderInputData, GetCheckedOrderInteractor>();
            busBuilder.RegisterUseCase<GetAddedAutoItemInputData, GetAddedAutoItemInteractor>();
            busBuilder.RegisterUseCase<AddAutoItemInputData, AddAutoItemInteractor>();
            busBuilder.RegisterUseCase<CheckedItemNameInputData, CheckedItemNameInteractor>();
            busBuilder.RegisterUseCase<SearchHistoryInputData, SearchHistoryInteractor>();
            busBuilder.RegisterUseCase<GetValidJihiYoboInputData, GetValidJihiYoboInteractor>();
            busBuilder.RegisterUseCase<GetValidGairaiRihaInputData, GetValidGairaiRihaInteractor>();
            busBuilder.RegisterUseCase<ConvertNextOrderToTodayOrdInputData, ConvertNextOrderTodayOrdInteractor>();
            busBuilder.RegisterUseCase<SummaryInfInputData, SummaryInfInteractor>();
            busBuilder.RegisterUseCase<AutoCheckOrderInputData, AutoCheckOrderInteractor>();
            busBuilder.RegisterUseCase<ChangeAfterAutoCheckOrderInputData, ChangeAfterAutoCheckOrderInteractor>();
            busBuilder.RegisterUseCase<InitKbnSettingInputData, InitKbnSettingInteractor>();
            busBuilder.RegisterUseCase<CheckedAfter327ScreenInputData, CheckedAfter327ScreenInteractor>();
            busBuilder.RegisterUseCase<GetHistoryIndexInputData, GetHistoryIndexInteractor>();
            busBuilder.RegisterUseCase<GetMaxAuditTrailLogDateForPrintInputData, GetMaxAuditTrailLogDateForPrintInteractor>();
            busBuilder.RegisterUseCase<CheckedExpiredInputData, CheckedExpiredInteractor>();
            busBuilder.RegisterUseCase<ConvertFromHistoryTodayOrderInputData, ConvertFromHistoryToTodayOdrInteractor>();
            busBuilder.RegisterUseCase<GetDefaultSelectedTimeInputDataOfMedical, GetDefaultSelectedTimeInteractorOfMedical>();

            //SetKbn
            busBuilder.RegisterUseCase<GetSetKbnMstListInputData, GetSetKbnMstListInteractor>();

            //Insurance Mst
            busBuilder.RegisterUseCase<GetInsuranceMstInputData, GetInsuranceMstInteractor>();
            busBuilder.RegisterUseCase<GetDefaultSelectPatternInputData, GetDefaultSelectPatternInteractor>();
            busBuilder.RegisterUseCase<GetInsuranceMasterDetailInputData, GetInsuranceMasterDetailInteractor>();
            busBuilder.RegisterUseCase<GetSelectMaintenanceInputData, GetSelectMaintenanceInteractor>();


            busBuilder.RegisterUseCase<DeleteHokenMasterInputData, DeleteHokenMasterInteractor>();
            busBuilder.RegisterUseCase<GetInfoCloneInsuranceMstInputData, GetInfoCloneInsuranceMstInteractor>();
            busBuilder.RegisterUseCase<SaveHokenMasterInputData, SaveHokenMasterInteractor>();

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
            busBuilder.RegisterUseCase<UpsertUketukeSbtMstInputData, UpsertUketukeSbtMstInteractor>();

            // HokensyaMst
            busBuilder.RegisterUseCase<SearchHokensyaMstInputData, SearchHokensyaMstInteractor>();
            busBuilder.RegisterUseCase<GetHokenSyaMstInputData, GetHokenSyaMstInteractor>();

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
            busBuilder.RegisterUseCase<SaveStickyNoteInputData, SaveStickyNoteInteractor>();
            busBuilder.RegisterUseCase<GetSettingStickyNoteInputData, GetSettingStickyNoteInteractor>();

            //MS Item
            busBuilder.RegisterUseCase<SearchTenItemInputData, SearchTenItemInteractor>();
            busBuilder.RegisterUseCase<UpdateAdoptedTenItemInputData, UpdateAdoptedTenItemInteractor>();
            busBuilder.RegisterUseCase<GetDosageDrugListInputData, GetDosageDrugListInteractor>();
            busBuilder.RegisterUseCase<SearchOTCInputData, SearchOtcInteractor>();
            busBuilder.RegisterUseCase<SearchSupplementInputData, SearchSupplementInteractor>();
            busBuilder.RegisterUseCase<UpdateAdoptedByomeiInputData, UpdateAdoptedByomeiInteractor>();
            busBuilder.RegisterUseCase<GetFoodAlrgyInputData, GetFoodAlrgyInteractor>();
            busBuilder.RegisterUseCase<SearchPostCodeInputData, SearchPostCodeInteractor>();
            busBuilder.RegisterUseCase<FindTenMstInputData, FindTenMstInteractor>();
            busBuilder.RegisterUseCase<GetAdoptedItemListInputData, GetAdoptedItemListInteractor>();
            busBuilder.RegisterUseCase<UpdateAdoptedItemListInputData, UpdateAdoptedItemListInteractor>();
            busBuilder.RegisterUseCase<GetCmtCheckMstListInputData, GetCmtCheckMstListInteractor>();

            // Disease
            busBuilder.RegisterUseCase<UpsertPtDiseaseListInputData, UpsertPtDiseaseListInteractor>();
            busBuilder.RegisterUseCase<DiseaseSearchInputData, DiseaseSearchInteractor>();
            busBuilder.RegisterUseCase<GetSetByomeiTreeInputData, GetSetByomeiTreeInteractor>();

            // Drug Infor - Data Menu and Detail 
            busBuilder.RegisterUseCase<GetDrugDetailInputData, GetDrugDetailInteractor>();
            busBuilder.RegisterUseCase<GetDrugDetailDataInputData, GetDrugDetailDataInteractor>();
            busBuilder.RegisterUseCase<ShowProductInfInputData, ShowProductInfInteractor>();
            busBuilder.RegisterUseCase<ShowKanjaMukeInputData, ShowKanjaMukeInteractor>();
            busBuilder.RegisterUseCase<ShowMdbByomeiInputData, ShowMdbByomeiInteractor>();

            //DrugInfor
            busBuilder.RegisterUseCase<GetDrugInforInputData, GetDrugInforInteractor>();

            // Get HokenMst by FutansyaNo
            busBuilder.RegisterUseCase<GetKohiHokenMstInputData, GetKohiHokenMstInteractor>();

            // Schema
            busBuilder.RegisterUseCase<GetListImageTemplatesInputData, GetListImageTemplatesInteractor>();
            busBuilder.RegisterUseCase<SaveListFileTodayOrderInputData, SaveListFileInteractor>();

            // SuperSetDetail
            busBuilder.RegisterUseCase<GetSuperSetDetailInputData, GetSuperSetDetailInteractor>();
            busBuilder.RegisterUseCase<SaveSuperSetDetailInputData, SaveSuperSetDetailInteractor>();
            busBuilder.RegisterUseCase<GetSuperSetDetailToDoTodayOrderInputData, GetSuperSetDetailToDoTodayOrderInteractor>();

            //Validation TodayOrder
            busBuilder.RegisterUseCase<ValidationTodayOrdInputData, ValidationTodayOrdInteractor>();

            //UsageTreeSet
            busBuilder.RegisterUseCase<GetUsageTreeSetInputData, GetUsageTreeSetInteractor>();

            //Maxmoney
            busBuilder.RegisterUseCase<GetMaxMoneyInputData, GetMaxMoneyInteractor>();
            busBuilder.RegisterUseCase<SaveMaxMoneyInputData, SaveMaxMoneyInteractor>();
            busBuilder.RegisterUseCase<GetMaxMoneyByPtIdInputData, GetMaxMoneyByPtIdInteractor>();

            //Monshin
            busBuilder.RegisterUseCase<GetMonshinInforListInputData, GetMonshinInforListInteractor>();
            busBuilder.RegisterUseCase<SaveMonshinInputData, SaveMonshinInforListInteractor>();

            // Reception - Valid Pattern Expirated
            busBuilder.RegisterUseCase<ValidPatternExpiratedInputData, ValidPatternExpiratedInteractor>();

            //System Conf
            busBuilder.RegisterUseCase<GetSystemConfInputData, GetSystemConfInteractor>();
            busBuilder.RegisterUseCase<GetSystemConfListInputData, GetSystemConfListInteractor>();
            busBuilder.RegisterUseCase<GetSystemConfForPrintInputData, GetSystemConfForPrintInteractor>();

            //SaveHokenSya
            busBuilder.RegisterUseCase<SaveHokenSyaMstInputData, SaveHokenSyaMstInteractor>();

            //HokenMst
            busBuilder.RegisterUseCase<GetDetailHokenMstInputData, GetDetailHokenMstInteractor>();

            //Validate InputItem 
            busBuilder.RegisterUseCase<ValidationInputItemInputData, ValidationInputitemInteractor>();
            busBuilder.RegisterUseCase<GetSelectiveCommentInputData, GetSelectiveCommentInteractor>();

            //AccoutDue
            busBuilder.RegisterUseCase<GetAccountDueListInputData, GetAccountDueListInteractor>();
            busBuilder.RegisterUseCase<SaveAccountDueListInputData, SaveAccountDueListInteractor>();

            //Accounting
            busBuilder.RegisterUseCase<GetAccountingInputData, GetAccountingInteractor>();
            busBuilder.RegisterUseCase<GetPaymentMethodInputData, GetPaymentMethodInteractor>();
            busBuilder.RegisterUseCase<GetWarningMemoInputData, GetWarningMemoInteractor>();
            busBuilder.RegisterUseCase<GetPtByoMeiInputData, GetPtByoMeiInteractor>();
            busBuilder.RegisterUseCase<SaveAccountingInputData, SaveAccountingInteractor>();
            busBuilder.RegisterUseCase<GetAccountingHistoryOrderInputData, GetAccountingHistoryOrderInteractor>();
            busBuilder.RegisterUseCase<GetAccountingHeaderInputData, GetAccountingHeaderInteractor>();
            busBuilder.RegisterUseCase<CheckAccountingStatusInputData, CheckAccountingStatusInteractor>();
            busBuilder.RegisterUseCase<GetAccountingConfigInputData, GetAccountingConfigInteractor>();

            //TimeZone
            busBuilder.RegisterUseCase<GetDefaultSelectedTimeInputDataOfReception, GetDefaultSelectedTimeInteractorOfReception>();
            busBuilder.RegisterUseCase<UpdateTimeZoneDayInfInputData, UpdateTimeZoneDayInfInteractor>();

            //UserConf
            busBuilder.RegisterUseCase<GetUserConfListInputData, GetUserConfListInteractor>();
            busBuilder.RegisterUseCase<UpdateAdoptedByomeiConfigInputData, UpdateAdoptedByomeiConfigInteractor>();
            busBuilder.RegisterUseCase<UpdateUserConfInputData, UpdateUserConfInteractor>();
            busBuilder.RegisterUseCase<SagakuInputData, SagakuInteractor>();
            busBuilder.RegisterUseCase<UpsertUserConfListInputData, UpsertUserConfListInteractor>();
            busBuilder.RegisterUseCase<GetListMedicalExaminationConfigInputData, GetListMedicalExaminationConfigInteractor>();

            //SwapHoken
            busBuilder.RegisterUseCase<SaveSwapHokenInputData, SaveSwapHokenInteractor>();
            busBuilder.RegisterUseCase<ValidateSwapHokenInputData, ValidateSwapHokenInteractor>();

            //System Config Generation 
            busBuilder.RegisterUseCase<GetSystemGenerationConfInputData, GetSystemGenerationConfInteractor>();

            //Next Order
            busBuilder.RegisterUseCase<GetNextOrderListInputData, GetNextOrderListInteractor>();
            busBuilder.RegisterUseCase<GetNextOrderInputData, GetNextOrderInteractor>();
            busBuilder.RegisterUseCase<UpsertNextOrderListInputData, UpsertNextOrderListInteractor>();
            busBuilder.RegisterUseCase<ValidationNextOrderListInputData, ValidationNextOrderListInteractor>();

            //YohoSetMst
            busBuilder.RegisterUseCase<GetYohoMstByItemCdInputData, GetYohoMstByItemCdInteractor>();

            // Document
            busBuilder.RegisterUseCase<GetListDocCategoryInputData, GetListDocCategoryInteractor>();
            busBuilder.RegisterUseCase<GetDocCategoryDetailInputData, GetDocCategoryDetailInteractor>();
            busBuilder.RegisterUseCase<SaveListDocCategoryInputData, SaveListDocCategoryInteractor>();
            busBuilder.RegisterUseCase<SortDocCategoryInputData, SortDocCategoryInteractor>();
            busBuilder.RegisterUseCase<CheckExistFileNameInputData, CheckExistFileNameInteractor>();
            busBuilder.RegisterUseCase<UploadTemplateToCategoryInputData, UploadTemplateToCategoryInteractor>();
            busBuilder.RegisterUseCase<SaveDocInfInputData, SaveDocInfInteractor>();
            busBuilder.RegisterUseCase<DeleteDocInfInputData, DeleteDocInfInteractor>();
            busBuilder.RegisterUseCase<DeleteDocTemplateInputData, DeleteDocTemplateInteractor>();
            busBuilder.RegisterUseCase<MoveTemplateToOtherCategoryInputData, MoveTemplateToOtherCategoryInteractor>();
            busBuilder.RegisterUseCase<DeleteDocCategoryInputData, DeleteDocCategoryInteractor>();
            busBuilder.RegisterUseCase<GetListParamTemplateInputData, GetListParamTemplateInteractor>();
            busBuilder.RegisterUseCase<DownloadDocumentTemplateInputData, DownloadDocumentTemplateInteractor>();
            busBuilder.RegisterUseCase<GetListDocCommentInputData, GetListDocCommentInteractor>();
            busBuilder.RegisterUseCase<ConfirmReplaceDocParamInputData, ConfirmReplaceDocParamInteractor>();

            //InsuranceScan
            busBuilder.RegisterUseCase<GetListInsuranceScanInputData, GetListInsuranceScanInteractor>();

            //Hoki PriorityList
            busBuilder.RegisterUseCase<GetKohiPriorityListInputData, GetKohiPriorityListInteractor>();

            //PtGroupMaster
            busBuilder.RegisterUseCase<SaveGroupNameMstInputData, SaveGroupNameMstInteractor>();
            busBuilder.RegisterUseCase<GetGroupNameMstInputData, GetGroupNameMstInteractor>();
            busBuilder.RegisterUseCase<CheckAllowDeleteGroupMstInputData, CheckAllowDeleteGroupMstInteractor>();

            //SanteiInf
            busBuilder.RegisterUseCase<GetListSanteiInfInputData, GetListSanteiInfInteractor>();
            busBuilder.RegisterUseCase<SaveListSanteiInfInputData, SaveListSanteiInfInteractor>();

            //Family
            busBuilder.RegisterUseCase<GetFamilyListInputData, GetFamilyListInteractor>();
            busBuilder.RegisterUseCase<SaveFamilyListInputData, SaveFamilyListInteractor>();
            busBuilder.RegisterUseCase<GetFamilyReverserListInputData, GetFamilyReverserListInteractor>();
            busBuilder.RegisterUseCase<GetListRaiinInfInputData, GetListRaiinInfInteractorOfReception>();

            //Receipt
            busBuilder.RegisterUseCase<ReceiptListAdvancedSearchInputData, ReceiptListAdvancedSearchInteractor>();

            //Convert Input Item to Today Order
            busBuilder.RegisterUseCase<ConvertInputItemToTodayOrdInputData, ConvertInputItemToTodayOrderInteractor>();

            // Rece check
            busBuilder.RegisterUseCase<GetReceCmtListInputData, GetReceCmtListInteractor>();
            busBuilder.RegisterUseCase<SaveReceCmtListInputData, SaveReceCmtListInteractor>();
            busBuilder.RegisterUseCase<GetSyoukiInfListInputData, GetSyoukiInfListInteractor>();
            busBuilder.RegisterUseCase<SaveSyoukiInfListInputData, SaveSyoukiInfListInteractor>();
            busBuilder.RegisterUseCase<GetSyobyoKeikaListInputData, GetSyobyoKeikaListInteractor>();
            busBuilder.RegisterUseCase<SaveSyobyoKeikaListInputData, SaveSyobyoKeikaListInteractor>();
            busBuilder.RegisterUseCase<GetReceHenReasonInputData, GetReceHenReasonInteractor>();
            busBuilder.RegisterUseCase<GetReceiCheckListInputData, GetReceiCheckListInteractor>();
            busBuilder.RegisterUseCase<SaveReceCheckCmtListInputData, SaveReceCheckCmtListInteractor>();
            busBuilder.RegisterUseCase<GetInsuranceReceInfListInputData, GetInsuranceReceInfListInteractor>();
            busBuilder.RegisterUseCase<GetDiseaseReceListInputData, GetDiseaseReceListInteractor>();
            busBuilder.RegisterUseCase<RecalculationInputData, RecalculationInteractor>();
            busBuilder.RegisterUseCase<GetReceCheckOptionListInputData, GetReceCheckOptionListInteractor>();

            //ReceSeikyu
            busBuilder.RegisterUseCase<GetListReceSeikyuInputData, GetListReceSeikyuInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
