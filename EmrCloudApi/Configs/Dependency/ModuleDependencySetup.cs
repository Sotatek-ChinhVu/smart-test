using CommonChecker.Caches;
using CommonChecker.Caches.Interface;
using CommonChecker.DB;
using CommonChecker.Services;
using Domain.CalculationInf;
using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.AuditLog;
using Domain.Models.ByomeiSetGenerationMst;
using Domain.Models.ChartApproval;
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
using Domain.Models.KensaIrai;
using Domain.Models.KensaSet;
using Domain.Models.ListSetMst;
using Domain.Models.Lock;
using Domain.Models.MainMenu;
using Domain.Models.MaxMoney;
using Domain.Models.Medical;
using Domain.Models.MedicalExamination;
using Domain.Models.MonshinInf;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.Online;
using Domain.Models.OrdInfs;
using Domain.Models.PatientGroupMst;
using Domain.Models.PatientInfor;
using Domain.Models.PtCmtInf;
using Domain.Models.PtGroupMst;
using Domain.Models.PtTag;
using Domain.Models.RaiinCmtInf;
using Domain.Models.RaiinFilterMst;
using Domain.Models.RaiinKubunMst;
using Domain.Models.RaiinListSetting;
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
using Domain.Models.SmartKartePort;
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
using Domain.Models.Todo;
using Domain.Models.UketukeSbtDayInf;
using Domain.Models.UketukeSbtMst;
using Domain.Models.UsageTreeSet;
using Domain.Models.User;
using Domain.Models.UserConf;
using Domain.Models.UserToken;
using Domain.Models.VisitingListSetting;
using Domain.Models.YohoSetMst;
using EmrCloudApi.Realtime;
using EmrCloudApi.Services;
using EventProcessor.Interfaces;
using EventProcessor.Service;
using Helper.Messaging;
using Infrastructure.Common;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Infrastructure.Repositories;
using Infrastructure.Repositories.SpecialNote;
using Infrastructure.Services;
using Interactor.AccountDue;
using Interactor.Accounting;
using Interactor.AuditTrailLog;
using Interactor.Byomei;
using Interactor.CalculateService;
using Interactor.CalculationInf;
using Interactor.ChartApproval;
using Interactor.ColumnSetting;
using Interactor.CommonChecker;
using Interactor.CommonChecker.CommonMedicalCheck;
using Interactor.Diseases;
using Interactor.Document;
using Interactor.Document.CommonGetListParam;
using Interactor.DrugDetail;
using Interactor.DrugDetailData;
using Interactor.DrugInfor;
using Interactor.DrugInfor.CommonDrugInf;
using Interactor.Family;
using Interactor.Family.ValidateFamilyList;
using Interactor.FlowSheet;
using Interactor.GrpInf;
using Interactor.HokenMst;
using Interactor.Holiday;
using Interactor.Insurance;
using Interactor.InsuranceMst;
using Interactor.JsonSetting;
using Interactor.Ka;
using Interactor.KarteFilter;
using Interactor.KarteInf;
using Interactor.KarteInfs;
using Interactor.KensaHistory;
using Interactor.KohiHokenMst;
using Interactor.LastDayInformation;
using Interactor.ListSetMst;
using Interactor.Lock;
using Interactor.Logger;
using Interactor.MainMenu;
using Interactor.MaxMoney;
using Interactor.MedicalExamination;
using Interactor.MedicalExamination.HistoryCommon;
using Interactor.MedicalExamination.KensaIraiCommon;
using Interactor.MonshinInf;
using Interactor.MstItem;
using Interactor.NextOrder;
using Interactor.Online;
using Interactor.OrdInfs;
using Interactor.PatientGroupMst;
using Interactor.PatientInfor;
using Interactor.PatientInfor.PtKyuseiInf;
using Interactor.PatientInfor.SortPatientCommon;
using Interactor.PatientManagement;
using Interactor.PtGroupMst;
using Interactor.RaiinFilterMst;
using Interactor.RaiinKubunMst;
using Interactor.RaiinListSetting;
using Interactor.Receipt;
using Interactor.ReceiptCheck;
using Interactor.Reception;
using Interactor.ReceptionInsurance;
using Interactor.ReceptionSameVisit;
using Interactor.ReceptionVisiting;
using Interactor.ReceSeikyu;
using Interactor.Santei;
using Interactor.Schema;
using Interactor.SetKbnMst;
using Interactor.SetMst;
using Interactor.SetMst.CommonSuperSet;
using Interactor.SetSendaiGeneration;
using Interactor.SinKoui;
using Interactor.SmartKartePort;
using Interactor.SpecialNote;
using Interactor.StickyNote;
using Interactor.SuperSetDetail;
using Interactor.SwapHoken;
using Interactor.SystemConf;
using Interactor.SystemGenerationConf;
using Interactor.TimeZoneConf;
using Interactor.Todo;
using Interactor.UketukeSbtMst;
using Interactor.UsageTreeSet;
using Interactor.User;
using Interactor.UserConf;
using Interactor.UserToken;
using Interactor.VisitingList;
using Interactor.WeightedSetConfirmation;
using Interactor.YohoSetMst;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Reporting.Accounting.DB;
using Reporting.Accounting.Service;
using Reporting.AccountingCard.DB;
using Reporting.AccountingCard.Service;
using Reporting.AccountingCardList.DB;
using Reporting.AccountingCardList.Service;
using Reporting.Byomei.DB;
using Reporting.Byomei.Service;
using Reporting.Calculate.Implementation;
using Reporting.Calculate.Interface;
using Reporting.CommonMasters.Common;
using Reporting.CommonMasters.Common.Interface;
using Reporting.CommonMasters.Config;
using Reporting.DailyStatic.DB;
using Reporting.DailyStatic.Service;
using Reporting.DrugInfo.DB;
using Reporting.DrugInfo.Service;
using Reporting.DrugNoteSeal.DB;
using Reporting.DrugNoteSeal.Service;
using Reporting.GrowthCurve.DB;
using Reporting.GrowthCurve.Service;
using Reporting.InDrug.DB;
using Reporting.InDrug.Service;
using Reporting.Karte1.DB;
using Reporting.Karte1.Service;
using Reporting.Karte3.DB;
using Reporting.Karte3.Service;
using Reporting.KensaHistory.DB;
using Reporting.KensaHistory.Service;
using Reporting.KensaLabel.DB;
using Reporting.KensaLabel.Service;
using Reporting.Kensalrai.DB;
using Reporting.Kensalrai.Service;
using Reporting.MedicalRecordWebId.DB;
using Reporting.MedicalRecordWebId.Service;
using Reporting.Memo.Service;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Service;
using Reporting.OrderLabel.DB;
using Reporting.OrderLabel.Service;
using Reporting.OutDrug.DB;
using Reporting.OutDrug.Service;
using Reporting.PatientManagement.DB;
using Reporting.PatientManagement.Service;
using Reporting.ReadRseReportFile.Service;
using Reporting.Receipt.DB;
using Reporting.Receipt.Service;
using Reporting.ReceiptCheck.DB;
using Reporting.ReceiptCheck.Service;
using Reporting.ReceiptList.DB;
using Reporting.ReceiptList.Service;
using Reporting.ReceiptPrint.Service;
using Reporting.ReceTarget.DB;
using Reporting.ReceTarget.Service;
using Reporting.ReportServices;
using Reporting.Sijisen.DB;
using Reporting.Sijisen.Service;
using Reporting.Sokatu.AfterCareSeikyu.DB;
using Reporting.Sokatu.AfterCareSeikyu.Service;
using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.HikariDisk.DB;
using Reporting.Sokatu.HikariDisk.Service;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Service;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Service;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Service;
using Reporting.Sokatu.Syaho.DB;
using Reporting.Sokatu.Syaho.Service;
using Reporting.Sokatu.WelfareDisk.Service;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Service;
using Reporting.Statistics.Sta1001.DB;
using Reporting.Statistics.Sta1001.Service;
using Reporting.Statistics.Sta1002.DB;
using Reporting.Statistics.Sta1002.Service;
using Reporting.Statistics.Sta1010.DB;
using Reporting.Statistics.Sta1010.Service;
using Reporting.Statistics.Sta2001.DB;
using Reporting.Statistics.Sta2001.Service;
using Reporting.Statistics.Sta2002.DB;
using Reporting.Statistics.Sta2002.Service;
using Reporting.Statistics.Sta2003.DB;
using Reporting.Statistics.Sta2003.Service;
using Reporting.Statistics.Sta2010.DB;
using Reporting.Statistics.Sta2010.Service;
using Reporting.Statistics.Sta2011.DB;
using Reporting.Statistics.Sta2011.Service;
using Reporting.Statistics.Sta2020.DB;
using Reporting.Statistics.Sta2020.Service;
using Reporting.Statistics.Sta2021.DB;
using Reporting.Statistics.Sta2021.Service;
using Reporting.Statistics.Sta3001.DB;
using Reporting.Statistics.Sta3001.Service;
using Reporting.Statistics.Sta3010.DB;
using Reporting.Statistics.Sta3010.Service;
using Reporting.Statistics.Sta3020.DB;
using Reporting.Statistics.Sta3020.Service;
using Reporting.Statistics.Sta3030.DB;
using Reporting.Statistics.Sta3030.Service;
using Reporting.Statistics.Sta3040.DB;
using Reporting.Statistics.Sta3040.Service;
using Reporting.Statistics.Sta3041.DB;
using Reporting.Statistics.Sta3041.Service;
using Reporting.Statistics.Sta3050.DB;
using Reporting.Statistics.Sta3050.Service;
using Reporting.Statistics.Sta3060.DB;
using Reporting.Statistics.Sta3060.Service;
using Reporting.Statistics.Sta3061.DB;
using Reporting.Statistics.Sta3061.Service;
using Reporting.Statistics.Sta3070.DB;
using Reporting.Statistics.Sta3070.Service;
using Reporting.Statistics.Sta3071.DB;
using Reporting.Statistics.Sta3071.Service;
using Reporting.Statistics.Sta3080.DB;
using Reporting.Statistics.Sta3080.Service;
using Reporting.Statistics.Sta9000.DB;
using Reporting.Statistics.Sta9000.Service;
using Reporting.SyojyoSyoki.DB;
using Reporting.SyojyoSyoki.Service;
using Reporting.Yakutai.DB;
using Reporting.Yakutai.Service;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.AccountDue.IsNyukinExisted;
using UseCase.AccountDue.SaveAccountDueList;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.CheckOpenAccounting;
using UseCase.Accounting.GetAccountingFormMst;
using UseCase.Accounting.GetAccountingHeader;
using UseCase.Accounting.GetAccountingInf;
using UseCase.Accounting.GetAccountingSystemConf;
using UseCase.Accounting.GetListHokenSelect;
using UseCase.Accounting.GetPtByoMei;
using UseCase.Accounting.GetSinMei;
using UseCase.Accounting.PaymentMethod;
using UseCase.Accounting.Recaculate;
using UseCase.Accounting.SaveAccounting;
using UseCase.Accounting.UpdateAccountingFormMst;
using UseCase.Accounting.WarningMemo;
using UseCase.ByomeiSetMst.UpdateByomeiSetMst;
using UseCase.CalculationInf;
using UseCase.ChartApproval.CheckSaveLogOut;
using UseCase.ChartApproval.GetApprovalInfList;
using UseCase.ChartApproval.SaveApprovalInfList;
using UseCase.ColumnSetting.GetColumnSettingByTableNameList;
using UseCase.ColumnSetting.GetList;
using UseCase.ColumnSetting.SaveList;
using UseCase.CommonChecker;
using UseCase.ContainerMasterUpdate;
using UseCase.Core.Builder;
using UseCase.Diseases.GetAllByomeiByPtId;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Diseases.GetSetByomeiTree;
using UseCase.Diseases.Upsert;
using UseCase.Diseases.Validation;
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
using UseCase.DrugInfor.GetDataPrintDrugInfo;
using UseCase.DrugInfor.GetSinrekiFilterMstList;
using UseCase.DrugInfor.SaveSinrekiFilterMstList;
using UseCase.Family.GetFamilyList;
using UseCase.Family.GetFamilyReverserList;
using UseCase.Family.GetMaybeFamilyList;
using UseCase.Family.SaveFamilyList;
using UseCase.Family.ValidateFamilyList;
using UseCase.FlowSheet.GetList;
using UseCase.FlowSheet.GetTooltip;
using UseCase.FlowSheet.Upsert;
using UseCase.GroupInf.GetList;
using UseCase.HokenMst.GetDetail;
using UseCase.HokenMst.GetHokenMst;
using UseCase.Holiday.SaveHoliday;
using UseCase.Insurance.FindHokenInfByPtId;
using UseCase.Insurance.FindPtHokenList;
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
using UseCase.InsuranceMst.DeleteHokenMaster;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.GetHokenMasterReadOnly;
using UseCase.InsuranceMst.GetHokenSyaMst;
using UseCase.InsuranceMst.GetInfoCloneInsuranceMst;
using UseCase.InsuranceMst.GetMasterDetails;
using UseCase.InsuranceMst.GetSelectMaintenance;
using UseCase.InsuranceMst.SaveHokenMaster;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using UseCase.InsuranceMst.SaveOrdInsuranceMst;
using UseCase.IsUsingKensa;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.GetAll;
using UseCase.JsonSetting.Upsert;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetKacodeMstYossi;
using UseCase.Ka.GetKacodeYousikiMst;
using UseCase.Ka.GetList;
using UseCase.Ka.SaveList;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;
using UseCase.KarteInf.ConvertTextToRichText;
using UseCase.KarteInf.GetList;
using UseCase.KensaHistory.GetListKensaCmtMst;
using UseCase.KensaHistory.GetListKensaCmtMst.GetKensaInfDetailByIraiCd;
using UseCase.KensaHistory.GetListKensaInfDetail;
using UseCase.KensaHistory.GetListKensaSet;
using UseCase.KensaHistory.GetListKensaSetDetail;
using UseCase.KensaHistory.UpdateKensaInfDetail;
using UseCase.KensaHistory.UpdateKensaSet;
using UseCase.KohiHokenMst.Get;
using UseCase.LastDayInformation.GetLastDayInfoList;
using UseCase.LastDayInformation.SaveSettingLastDayInfoList;
using UseCase.ListSetMst.UpdateListSetMst;
using UseCase.Lock.Add;
using UseCase.Lock.Check;
using UseCase.Lock.CheckExistFunctionCode;
using UseCase.Lock.CheckIsExistedOQLockInfo;
using UseCase.Lock.ExtendTtl;
using UseCase.Lock.Get;
using UseCase.Lock.GetLockInf;
using UseCase.Lock.Remove;
using UseCase.Lock.Unlock;
using UseCase.Logger;
using UseCase.Logger.WriteListLog;
using UseCase.MainMenu.CreateDataKensaIraiRenkei;
using UseCase.MainMenu.DeleteKensaInf;
using UseCase.MainMenu.GetKensaCenterMstList;
using UseCase.MainMenu.GetKensaInf;
using UseCase.MainMenu.GetKensaIrai;
using UseCase.MainMenu.GetKensaIraiLog;
using UseCase.MainMenu.GetListQualification;
using UseCase.MainMenu.GetOdrSetName;
using UseCase.MainMenu.GetStaCsvMstModel;
using UseCase.MainMenu.GetStatisticMenu;
using UseCase.MainMenu.ImportKensaIrai;
using UseCase.MainMenu.KensaIraiReport;
using UseCase.MainMenu.RsvInfToConfirm;
using UseCase.MainMenu.SaveOdrSet;
using UseCase.MainMenu.SaveStaCsvMst;
using UseCase.MainMenu.SaveStatisticMenu;
using UseCase.MaxMoney.GetMaxMoney;
using UseCase.MaxMoney.GetMaxMoneyByPtId;
using UseCase.MaxMoney.SaveMaxMoney;
using UseCase.MedicalExamination.AddAutoItem;
using UseCase.MedicalExamination.AutoCheckOrder;
using UseCase.MedicalExamination.Calculate;
using UseCase.MedicalExamination.ChangeAfterAutoCheckOrder;
using UseCase.MedicalExamination.CheckedAfter327Screen;
using UseCase.MedicalExamination.CheckedExpired;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.CheckOpenTrialAccounting;
using UseCase.MedicalExamination.ConvertFromHistoryTodayOrder;
using UseCase.MedicalExamination.ConvertInputItemToTodayOdr;
using UseCase.MedicalExamination.ConvertItem;
using UseCase.MedicalExamination.ConvertNextOrderToTodayOdr;
using UseCase.MedicalExamination.GetAddedAutoItem;
using UseCase.MedicalExamination.GetCheckDisease;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.MedicalExamination.GetContainerMst;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHeaderVistitDate;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.GetHistoryFollowSindate;
using UseCase.MedicalExamination.GetHistoryIndex;
using UseCase.MedicalExamination.GetKensaAuditTrailLog;
using UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint;
using UseCase.MedicalExamination.GetOrdersForOneOrderSheetGroup;
using UseCase.MedicalExamination.GetOrderSheetGroup;
using UseCase.MedicalExamination.GetSinkouCountInMonth;
using UseCase.MedicalExamination.GetValidGairaiRiha;
using UseCase.MedicalExamination.GetValidJihiYobo;
using UseCase.MedicalExamination.InitKbnSetting;
using UseCase.MedicalExamination.SaveKensaIrai;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.MedicalExamination.SearchHistory;
using UseCase.MedicalExamination.SummaryInf;
using UseCase.MedicalExamination.TrailAccounting;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.MonshinInfor.GetList;
using UseCase.MonshinInfor.Save;
using UseCase.MstItem.CheckIsTenMstUsed;
using UseCase.MstItem.CheckJihiSbtExistsInTenMst;
using UseCase.MstItem.CompareTenMst;
using UseCase.MstItem.ConvertStringChkJISKj;
using UseCase.MstItem.DeleteOrRecoverTenMst;
using UseCase.MstItem.DiseaseNameMstSearch;
using UseCase.MstItem.DiseaseSearch;
using UseCase.MstItem.ExistUsedKensaItemCd;
using UseCase.MstItem.FindTenMst;
using UseCase.MstItem.GetAdoptedItemList;
using UseCase.MstItem.GetAllCmtCheckMst;
using UseCase.MstItem.GetByomeiByCode;
using UseCase.MstItem.GetCmtCheckMstList;
using UseCase.MstItem.GetDefaultPrecautions;
using UseCase.MstItem.GetDiseaseList;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.GetDrugAction;
using UseCase.MstItem.GetFoodAlrgy;
using UseCase.MstItem.GetJihiSbtMstList;
using UseCase.MstItem.GetListByomeiSetGenerationMst;
using UseCase.MstItem.GetListDrugImage;
using UseCase.MstItem.GetListKensaIjiSetting;
using UseCase.MstItem.GetListResultKensaMst;
using UseCase.MstItem.GetListSetGenerationMst;
using UseCase.MstItem.GetListTenMstOrigin;
using UseCase.MstItem.GetListUser;
using UseCase.MstItem.GetListYohoSetMstModelByUserID;
using UseCase.MstItem.GetParrentKensaMst;
using UseCase.MstItem.GetRenkeiConf;
using UseCase.MstItem.GetRenkeiMst;
using UseCase.MstItem.GetRenkeiTiming;
using UseCase.MstItem.GetSelectiveComment;
using UseCase.MstItem.GetSetDataTenMst;
using UseCase.MstItem.GetSetNameMnt;
using UseCase.MstItem.GetSingleDoseMstAndMedicineUnitList;
using UseCase.MstItem.GetTeikyoByomei;
using UseCase.MstItem.GetTenMstByCode;
using UseCase.MstItem.GetTenMstList;
using UseCase.MstItem.GetTenMstListByItemType;
using UseCase.MstItem.GetTenMstOriginInfoCreate;
using UseCase.MstItem.GetTreeByomeiSet;
using UseCase.MstItem.GetTreeListSet;
using UseCase.MstItem.IsKensaItemOrdering;
using UseCase.MstItem.IsUsingKensa;
using UseCase.MstItem.SaveAddressMst;
using UseCase.MstItem.SaveCompareTenMst;
using UseCase.MstItem.SaveRenkei;
using UseCase.MstItem.SaveSetDataTenMst;
using UseCase.MstItem.SaveSetNameMnt;
using UseCase.MstItem.SearchOTC;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchSupplement;
using UseCase.MstItem.SearchTenItem;
using UseCase.MstItem.SearchTenMstItem;
using UseCase.MstItem.UpdateAdopted;
using UseCase.MstItem.UpdateAdoptedByomei;
using UseCase.MstItem.UpdateAdoptedItemList;
using UseCase.MstItem.UpdateByomeiMst;
using UseCase.MstItem.UpdateCmtCheckMst;
using UseCase.MstItem.UpdateJihiSbtMst;
using UseCase.MstItem.UpdateKensaStdMst;
using UseCase.MstItem.UpdateSingleDoseMst;
using UseCase.MstItem.UpdateYohoSetMst;
using UseCase.MstItem.UploadImageDrugInf;
using UseCase.NextOrder.Check;
using UseCase.NextOrder.CheckNextOrdHaveOdr;
using UseCase.NextOrder.Get;
using UseCase.NextOrder.GetList;
using UseCase.NextOrder.Upsert;
using UseCase.NextOrder.Validation;
using UseCase.Online.GetListOnlineConfirmationHistoryModel;
using UseCase.Online.GetOnlineConsent;
using UseCase.Online.GetRegisterdPatientsFromOnline;
using UseCase.Online.InsertOnlineConfirmation;
using UseCase.Online.InsertOnlineConfirmHistory;
using UseCase.Online.SaveAllOQConfirmation;
using UseCase.Online.SaveOnlineConfirmation;
using UseCase.Online.SaveOQConfirmation;
using UseCase.Online.UpdateOnlineConfirmationHistory;
using UseCase.Online.UpdateOnlineConsents;
using UseCase.Online.UpdateOnlineHistoryById;
using UseCase.Online.UpdateOnlineInRaiinInf;
using UseCase.Online.UpdateOQConfirmation;
using UseCase.Online.UpdatePtInfOnlineQualify;
using UseCase.Online.UpdateRefNo;
using UseCase.OrdInfs.CheckedSpecialItem;
using UseCase.OrdInfs.CheckOrdInfInDrug;
using UseCase.OrdInfs.GetHeaderInf;
using UseCase.OrdInfs.GetListTrees;
using UseCase.OrdInfs.GetMaxRpNo;
using UseCase.OrdInfs.ValidationInputItem;
using UseCase.OrdInfs.ValidationTodayOrd;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientGroupMst.SaveList;
using UseCase.PatientInfor.CheckAllowDeletePatientInfo;
using UseCase.PatientInfor.CheckValidSamePatient;
using UseCase.PatientInfor.DeletePatient;
using UseCase.PatientInfor.GetInsuranceMasterLinkage;
using UseCase.PatientInfor.GetListPatient;
using UseCase.PatientInfor.GetPatientInfoBetweenTimesList;
using UseCase.PatientInfor.GetPtInfByRefNo;
using UseCase.PatientInfor.GetPtInfModelsByName;
using UseCase.PatientInfor.GetPtInfModelsByRefNo;
using UseCase.PatientInfor.GetTokiMstList;
using UseCase.PatientInfor.GetVisitTimesManagementModels;
using UseCase.PatientInfor.PatientComment;
using UseCase.PatientInfor.PtKyuseiInf.GetList;
using UseCase.PatientInfor.Save;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using UseCase.PatientInfor.SavePtKyusei;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchEmptyId;
using UseCase.PatientInfor.SearchPatientInfoByPtIdList;
using UseCase.PatientInfor.SearchPatientInfoByPtNum;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInfor.UpdateVisitTimesManagement;
using UseCase.PatientInfor.UpdateVisitTimesManagementNeedSave;
using UseCase.PatientInformation.GetById;
using UseCase.PatientManagement.GetStaConf;
using UseCase.PatientManagement.SaveStaConf;
using UseCase.PatientManagement.SearchPtInfs;
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
using UseCase.RaiinListSetting.GetDocCategory;
using UseCase.RaiinListSetting.GetFilingcategory;
using UseCase.RaiinListSetting.GetRaiiinListSetting;
using UseCase.RaiinListSetting.SaveRaiinListSetting;
using UseCase.Receipt.CheckExisReceInfEdit;
using UseCase.Receipt.CheckExistsReceInf;
using UseCase.Receipt.CheckExistSyobyoKeika;
using UseCase.Receipt.CreateUKEFile;
using UseCase.Receipt.DoReceCmt;
using UseCase.Receipt.GetDiseaseReceList;
using UseCase.Receipt.GetInsuranceReceInfList;
using UseCase.Receipt.GetListKaikeiInf;
using UseCase.Receipt.GetListReceInf;
using UseCase.Receipt.GetListSokatuMst;
using UseCase.Receipt.GetListSyobyoKeika;
using UseCase.Receipt.GetListSyoukiInf;
using UseCase.Receipt.GetReceByomeiChecking;
using UseCase.Receipt.GetReceCheckOptionList;
using UseCase.Receipt.GetReceCmt;
using UseCase.Receipt.GetReceHenReason;
using UseCase.Receipt.GetReceiCheckList;
using UseCase.Receipt.GetRecePreviewList;
using UseCase.Receipt.GetReceStatus;
using UseCase.Receipt.GetSinDateRaiinInfList;
using UseCase.Receipt.GetSinMeiInMonthList;
using UseCase.Receipt.MedicalDetail;
using UseCase.Receipt.Recalculation;
using UseCase.Receipt.ReceCmtHistory;
using UseCase.Receipt.ReceiptEdit;
using UseCase.Receipt.ReceiptListAdvancedSearch;
using UseCase.Receipt.SaveListReceCmt;
using UseCase.Receipt.SaveListSyobyoKeika;
using UseCase.Receipt.SaveListSyoukiInf;
using UseCase.Receipt.SaveReceCheckCmtList;
using UseCase.Receipt.SaveReceCheckOpt;
using UseCase.Receipt.SaveReceiptEdit;
using UseCase.Receipt.SaveReceStatus;
using UseCase.Receipt.SyobyoKeikaHistory;
using UseCase.Receipt.SyoukiInfHistory;
using UseCase.Receipt.ValidateCreateUKEFile;
using UseCase.ReceiptCheck.Recalculation;
using UseCase.ReceiptCheck.ReceiptInfEdit;
using UseCase.Reception.Delete;
using UseCase.Reception.Get;
using UseCase.Reception.GetHpInf;
using UseCase.Reception.GetLastKarute;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetList;
using UseCase.Reception.GetListRaiinInf;
using UseCase.Reception.GetNextUketukeNoBySetting;
using UseCase.Reception.GetOutDrugOrderList;
using UseCase.Reception.GetRaiinInfBySinDate;
using UseCase.Reception.GetRaiinListWithKanInf;
using UseCase.Reception.GetReceptionDefault;
using UseCase.Reception.GetSettings;
using UseCase.Reception.GetYoyakuRaiinInf;
using UseCase.Reception.InitDoctorCombo;
using UseCase.Reception.Insert;
using UseCase.Reception.ReceptionComment;
using UseCase.Reception.RevertDeleteNoRecept;
using UseCase.Reception.Update;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.Reception.UpdateTimeZoneDayInf;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;
using UseCase.ReceptionVisiting.Get;
using UseCase.ReceSeikyu.CancelSeikyu;
using UseCase.ReceSeikyu.GetList;
using UseCase.ReceSeikyu.GetReceSeikyModelByPtNum;
using UseCase.ReceSeikyu.ImportFile;
using UseCase.ReceSeikyu.Save;
using UseCase.ReceSeikyu.SearchReceInf;
using UseCase.Santei.GetListSanteiInf;
using UseCase.Santei.SaveListSanteiInf;
using UseCase.SaveAuditLog;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.GetListInsuranceScan;
using UseCase.Schema.SaveListFileTodayOrder;
using UseCase.SearchHokensyaMst.Get;
using UseCase.SetKbnMst.GetList;
using UseCase.SetKbnMst.GetSetKbnMstListByGenerationId;
using UseCase.SetKbnMst.Upsert;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.GetListSetGenerationMst;
using UseCase.SetMst.GetToolTip;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SetSendaiGeneration.Add;
using UseCase.SetSendaiGeneration.Delete;
using UseCase.SetSendaiGeneration.GetList;
using UseCase.SetSendaiGeneration.Restore;
using UseCase.SinKoui.GetSinKoui;
using UseCase.SmartKartePort.GetPort;
using UseCase.SmartKartePort.UpdatePort;
using UseCase.SpecialNote.AddAlrgyDrugList;
using UseCase.SpecialNote.Get;
using UseCase.SpecialNote.GetPtWeight;
using UseCase.SpecialNote.GetStdPoint;
using UseCase.SpecialNote.Save;
using UseCase.StickyNote;
using UseCase.SuperSetDetail.GetConversion;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;
using UseCase.SuperSetDetail.SaveConversion;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SuperSetDetail;
using UseCase.SwapHoken.Calculation;
using UseCase.SwapHoken.Save;
using UseCase.SwapHoken.Validate;
using UseCase.SystemConf.Get;
using UseCase.SystemConf.GetDrugCheckSetting;
using UseCase.SystemConf.GetPathAll;
using UseCase.SystemConf.GetSystemConfForPrint;
using UseCase.SystemConf.GetSystemConfList;
using UseCase.SystemConf.GetXmlPath;
using UseCase.SystemConf.SaveDrugCheckSetting;
using UseCase.SystemConf.SavePath;
using UseCase.SystemConf.SaveSystemSetting;
using UseCase.SystemConf.SystemSetting;
using UseCase.SystemGenerationConf.Get;
using UseCase.SystemGenerationConf.GetList;
using UseCase.TimeZoneConf.GetTimeZoneConfGroup;
using UseCase.TimeZoneConf.SaveTimeZoneConf;
using UseCase.Todo.GetListTodoKbn;
using UseCase.Todo.GetTodoGrp;
using UseCase.Todo.GetTodoInfFinder;
using UseCase.Todo.UpsertTodoGrpMst;
using UseCase.Todo.UpsertTodoInf;
using UseCase.UketukeSbtMst.GetBySinDate;
using UseCase.UketukeSbtMst.GetList;
using UseCase.UketukeSbtMst.GetNext;
using UseCase.UketukeSbtMst.Upsert;
using UseCase.UpdateKensaMst;
using UseCase.UpsertMaterialMaster;
using UseCase.UsageTreeSet.GetTree;
using UseCase.User.CheckedLockMedicalExamination;
using UseCase.User.GetAllPermission;
using UseCase.User.GetByLoginId;
using UseCase.User.GetList;
using UseCase.User.GetListFunctionPermission;
using UseCase.User.GetListJobMst;
using UseCase.User.GetListUserByCurrentUser;
using UseCase.User.GetPermissionByScreenCode;
using UseCase.User.GetUserConfList;
using UseCase.User.GetUserConfModelList;
using UseCase.User.MigrateDatabase;
using UseCase.User.Sagaku;
using UseCase.User.SaveListUserMst;
using UseCase.User.UpdateUserConf;
using UseCase.User.UpsertList;
using UseCase.User.UpsertUserConfList;
using UseCase.User.UserInfo;
using UseCase.UserConf.GetListMedicalExaminationConfig;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;
using UseCase.UserConf.UserSettingParam;
using UseCase.UserToken.GetInfoRefresh;
using UseCase.UserToken.SiginRefresh;
using UseCase.VisitingList.ReceptionLock;
using UseCase.VisitingList.SaveSettings;
using UseCase.WeightedSetConfirmation.CheckOpen;
using UseCase.YohoSetMst.GetByItemCd;
using GetDefaultSelectedTimeInputDataOfMedical = UseCase.MedicalExamination.GetDefaultSelectedTime.GetDefaultSelectedTimeInputData;
using GetDefaultSelectedTimeInputDataOfReception = UseCase.Reception.GetDefaultSelectedTime.GetDefaultSelectedTimeInputData;
using GetDefaultSelectedTimeInteractorOfMedical = Interactor.MedicalExamination.GetDefaultSelectedTimeInteractor;
using GetDefaultSelectedTimeInteractorOfReception = Interactor.Reception.GetDefaultSelectedTimeInteractor;
using GetListRaiinInfInputDataOfFamily = UseCase.Family.GetRaiinInfList.GetRaiinInfListInputData;
using GetListRaiinInfInputDataOfReceipt = UseCase.Receipt.GetListRaiinInf.GetListRaiinInfInputData;
using GetListRaiinInfInteractorOfFamily = Interactor.Family.GetRaiinInfListInteractor;
using GetListRaiinInfInteractorOfReceipt = Interactor.Receipt.GetListRaiinInfInteractor;
using GetListRaiinInfInteractorOfReception = Interactor.Reception.GetListRaiinInfInteractor;
using ISokatuCoHpInfFinder = Reporting.Sokatu.Common.DB.ICoHpInfFinder;
using IStatisticCoHpInfFinder = Reporting.Statistics.DB.ICoHpInfFinder;
using SokatuCoHpInfFinder = Reporting.Sokatu.Common.DB.CoHpInfFinder;
using StatisticCoHpInfFinder = Reporting.Statistics.DB.CoHpInfFinder;

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

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilters>();
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

            //Cache data
            services.AddTransient<IUserInfoService, UserInfoService>();
            services.AddTransient<IKaService, KaService>();
            services.AddTransient<ISystemConfigService, SystemConfigService>();

            //Init follow transient so no need change transient
            services.AddScoped<IMasterDataCacheService, MasterDataCacheService>();

            services.AddScoped<IMessenger, Messenger>();
            services.AddScoped<ILoggingHandler, LoggingHandler>();

            ///services.AddScoped<ISystemStartDbService, SystemStartDbService>();

            #region Reporting
            services.AddTransient<IEventProcessorService, EventProcessorService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<ICoDrugInfFinder, CoDrugInfFinder>();
            services.AddTransient<IDrugInfoCoReportService, DrugInfoCoReportService>();
            services.AddTransient<IUserConfReportCommon, UserConfReportCommon>();
            services.AddTransient<IUserMstCache, UserMstCache>();
            services.AddTransient<ISystemConfig, SystemConfig>();
            services.AddTransient<ICoOrderLabelFinder, CoOrderLabelFinder>();
            services.AddTransient<IOrderLabelCoReportService, OrderLabelCoReportService>();
            services.AddTransient<INameLabelService, NameLabelService>();
            services.AddTransient<ISijisenReportService, SijisenReportService>();
            services.AddTransient<IByomeiService, ByomeiService>();
            services.AddTransient<IKarte1Service, Karte1Service>();
            services.AddTransient<IMedicalRecordWebIdReportService, MedicalRecordWebIdReportService>();
            services.AddTransient<ICoMedicalRecordWebIdFinder, CoMedicalRecordWebIdFinder>();
            services.AddTransient<IOutDrugCoReportService, OutDrugCoReportService>();
            services.AddTransient<ICoOutDrugFinder, CoOutDrugFinder>();
            services.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            services.AddTransient<IStatisticCoHpInfFinder, StatisticCoHpInfFinder>();
            services.AddTransient<IReceiptCheckCoReportService, ReceiptCheckCoReportService>();
            services.AddTransient<ICoReceiptCheckFinder, CoReceiptCheckFinder>();
            services.AddTransient<IReceiptListCoReportService, ReceiptListCoReportService>();
            services.AddTransient<ICoReceiptListFinder, CoReceiptListFinder>();
            services.AddTransient<IAccountingCoReportService, AccountingCoReportService>();
            services.AddTransient<ICoAccountingFinder, CoAccountingFinder>();
            services.AddTransient<ISystemConfigProvider, SystemConfigProvider>();
            services.AddTransient<IEmrLogger, EmrLogger>();
            services.AddTransient<IReceiptCoReportService, ReceiptCoReportService>();
            services.AddTransient<ICoReceiptFinder, CoReceiptFinder>();
            services.AddTransient<ISta1002CoReportService, Sta1002CoReportService>();
            services.AddTransient<ICoSta1002Finder, CoSta1002Finder>();
            services.AddTransient<ICoSta1001Finder, CoSta1001Finder>();
            services.AddTransient<IStatisticService, StatisticService>();
            services.AddTransient<IDailyStatisticCommandFinder, DailyStatisticCommandFinder>();
            services.AddTransient<ISta1010CoReportService, Sta1010CoReportService>();
            services.AddTransient<ICoSta1010Finder, CoSta1010Finder>();
            services.AddTransient<ICoSta2001Finder, CoSta2001Finder>();
            services.AddTransient<ISta2001CoReportService, Sta2001CoReportService>();
            services.AddTransient<ICoSta2003Finder, CoSta2003Finder>();
            services.AddTransient<ISta2003CoReportService, Sta2003CoReportService>();
            services.AddTransient<ISta1001CoReportService, Sta1001CoReportService>();
            services.AddTransient<ICoSta2002Finder, CoSta2002Finder>();
            services.AddTransient<ISta2002CoReportService, Sta2002CoReportService>();
            services.AddTransient<ICoSta2010Finder, CoSta2010Finder>();
            services.AddTransient<ISta2010CoReportService, Sta2010CoReportService>();
            services.AddTransient<ICoSta2011Finder, CoSta2011Finder>();
            services.AddTransient<ISta2011CoReportService, Sta2011CoReportService>();
            services.AddTransient<ICoSta2021Finder, CoSta2021Finder>();
            services.AddTransient<ISta2021CoReportService, Sta2021CoReportService>();
            services.AddTransient<ICoSta9000Finder, CoSta9000Finder>();
            services.AddTransient<ISta9000CoReportService, Sta9000CoReportService>();
            services.AddTransient<ICoSta3080Finder, CoSta3080Finder>();
            services.AddTransient<ISta3080CoReportService, Sta3080CoReportService>();
            services.AddTransient<ICoSta3071Finder, CoSta3071Finder>();
            services.AddTransient<ISta3071CoReportService, Sta3071CoReportService>();
            services.AddTransient<IPatientManagementFinder, PatientManagementFinder>();
            services.AddTransient<IPatientManagementService, PatientManagementService>();
            services.AddTransient<ICoSta2020Finder, CoSta2020Finder>();
            services.AddTransient<ISta2020CoReportService, Sta2020CoReportService>();
            services.AddTransient<ISyojyoSyokiCoReportService, SyojyoSyokiCoReportService>();
            services.AddTransient<ICoSyojyoSyokiFinder, CoSyojyoSyokiFinder>();
            services.AddTransient<ICoSta3010Finder, CoSta3010Finder>();
            services.AddTransient<ISta3010CoReportService, Sta3010CoReportService>();
            services.AddTransient<ICoSta3030Finder, CoSta3030Finder>();
            services.AddTransient<ISta3030CoReportService, Sta3030CoReportService>();
            services.AddTransient<ICoSta3001Finder, CoSta3001Finder>();
            services.AddTransient<ISta3001CoReportService, Sta3001CoReportService>();
            services.AddTransient<ICoSta3040Finder, CoSta3040Finder>();
            services.AddTransient<ISta3040CoReportService, Sta3040CoReportService>();
            services.AddTransient<ICoSta3041Finder, CoSta3041Finder>();
            services.AddTransient<ISta3041CoReportService, Sta3041CoReportService>();
            services.AddTransient<ICoSta3050Finder, CoSta3050Finder>();
            services.AddTransient<ISta3050CoReportService, Sta3050CoReportService>();
            services.AddTransient<ICoSta3060Finder, CoSta3060Finder>();
            services.AddTransient<ISta3060CoReportService, Sta3060CoReportService>();
            services.AddTransient<ICoSta3061Finder, CoSta3061Finder>();
            services.AddTransient<ISta3061CoReportService, Sta3061CoReportService>();
            services.AddTransient<ICoSta3070Finder, CoSta3070Finder>();
            services.AddTransient<ISta3070CoReportService, Sta3070CoReportService>();
            services.AddTransient<ICoHokenMstFinder, CoHokenMstFinder>();
            services.AddTransient<ICoHokensyaMstFinder, CoHokensyaMstFinder>();
            services.AddTransient<ISokatuCoHpInfFinder, SokatuCoHpInfFinder>();
            services.AddTransient<ICoKokhoSeikyuFinder, CoKokhoSeikyuFinder>();
            services.AddTransient<ICoKokhoSokatuFinder, CoKokhoSokatuFinder>();
            services.AddTransient<IReceiptPrintService, ReceiptPrintService>();
            services.AddTransient<IMemoMsgCoReportService, MemoMsgCoReportService>();
            services.AddTransient<IP28KokhoSokatuCoReportService, P28KokhoSokatuCoReportService>();
            services.AddTransient<IP11KokhoSokatuCoReportService, P11KokhoSokatuCoReportService>();
            services.AddTransient<IReceTargetCoReportService, ReceTargetCoReportService>();
            services.AddTransient<ICoReceTargetFinder, CoReceTargetFinder>();
            services.AddTransient<IDrugNoteSealCoReportService, DrugNoteSealCoReportService>();
            services.AddTransient<ICoDrugNoteSealFinder, CoDrugNoteSealFinder>();
            services.AddTransient<IHikariDiskCoReportService, HikariDiskCoReportService>();
            services.AddTransient<ICoHikariDiskFinder, CoHikariDiskFinder>();
            services.AddTransient<ICoKoukiSeikyuFinder, CoKoukiSeikyuFinder>();
            services.AddTransient<ICoAfterCareSeikyuFinder, CoAfterCareSeikyuFinder>();
            services.AddTransient<IP28KoukiSeikyuCoReportService, P28KoukiSeikyuCoReportService>();
            services.AddTransient<IP29KoukiSeikyuCoReportService, P29KoukiSeikyuCoReportService>();
            services.AddTransient<IYakutaiCoReportService, YakutaiCoReportService>();
            services.AddTransient<ICoYakutaiFinder, CoYakutaiFinder>();
            services.AddTransient<IAfterCareSeikyuCoReportService, AfterCareSeikyuCoReportService>();
            services.AddTransient<ICoSijisenFinder, CoSijisenFinder>();
            services.AddTransient<ISyahoCoReportService, SyahoCoReportService>();
            services.AddTransient<ICoSyahoFinder, CoSyahoFinder>();
            services.AddTransient<ICoAccountingCardFinder, CoAccountingCardFinder>();
            services.AddTransient<ICoKensaHistoryFinder, CoKensaHistoryFinder>();
            services.AddTransient<IKensaHistoryCoReportService, KensaHistoryCoReportService>();
            services.AddTransient<IKensaResultMultiCoReportService, KensaResultMultiCoReportService>();
            services.AddTransient<IAccountingCardCoReportService, AccountingCardCoReportService>();
            services.AddTransient<IP33KoukiSeikyuCoReportService, P33KoukiSeikyuCoReportService>();
            services.AddTransient<IP34KoukiSeikyuCoReportService, P34KoukiSeikyuCoReportService>();
            services.AddTransient<IP35KoukiSeikyuCoReportService, P35KoukiSeikyuCoReportService>();
            services.AddTransient<IP37KoukiSeikyuCoReportService, P37KoukiSeikyuCoReportService>();
            services.AddTransient<IP40KoukiSeikyuCoReportService, P40KoukiSeikyuCoReportService>();
            services.AddTransient<IP42KoukiSeikyuCoReportService, P42KoukiSeikyuCoReportService>();
            services.AddTransient<IP43KoukiSeikyuCoReportService, P43KoukiSeikyuCoReportService>();
            services.AddTransient<IP45KoukiSeikyuCoReportService, P45KoukiSeikyuCoReportService>();
            services.AddTransient<IP09KoukiSeikyuCoReportService, P09KoukiSeikyuCoReportService>();
            services.AddTransient<IP12KoukiSeikyuCoReportService, P12KoukiSeikyuCoReportService>();
            services.AddTransient<IP13KoukiSeikyuCoReportService, P13KoukiSeikyuCoReportService>();
            services.AddTransient<IP30KoukiSeikyuCoReportService, P30KoukiSeikyuCoReportService>();
            services.AddTransient<IP08KokhoSokatuCoReportService, P08KokhoSokatuCoReportService>();
            services.AddTransient<IP41KoukiSeikyuCoReportService, P41KoukiSeikyuCoReportService>();
            services.AddTransient<IP44KoukiSeikyuCoReportService, P44KoukiSeikyuCoReportService>();
            services.AddTransient<IP08KoukiSeikyuCoReportService, P08KoukiSeikyuCoReportService>();
            services.AddTransient<IP11KoukiSeikyuCoReportService, P11KoukiSeikyuCoReportService>();
            services.AddTransient<IP14KoukiSeikyuCoReportService, P14KoukiSeikyuCoReportService>();
            services.AddTransient<IP17KoukiSeikyuCoReportService, P17KoukiSeikyuCoReportService>();
            services.AddTransient<IP20KoukiSeikyuCoReportService, P20KoukiSeikyuCoReportService>();
            services.AddTransient<IP37KoukiSokatuCoReportService, P37KoukiSokatuCoReportService>();
            services.AddTransient<IP25KokhoSokatuCoReportService, P25KokhoSokatuCoReportService>();
            services.AddTransient<ICoWelfareSeikyuFinder, CoWelfareSeikyuFinder>();
            services.AddTransient<IP41KokhoSokatuCoReportService, P41KokhoSokatuCoReportService>();
            services.AddTransient<IP13WelfareSeikyuCoReportService, P13WelfareSeikyuCoReportService>();
            services.AddTransient<IP08KokhoSeikyuCoReportService, P08KokhoSeikyuCoReportService>();
            services.AddTransient<IP22WelfareSeikyuCoReportService, P22WelfareSeikyuCoReportService>();
            services.AddTransient<IKarte3CoReportService, Karte3CoReportService>();
            services.AddTransient<ICoKarte3Finder, CoKarte3Finder>();
            services.AddTransient<IAccountingCardListCoReportService, AccountingCardListCoReportService>();
            services.AddTransient<ICoAccountingCardListFinder, CoAccountingCardListFinder>();
            services.AddTransient<IP21KoukiSeikyuCoReportService, P21KoukiSeikyuCoReportService>();
            services.AddTransient<IP22KoukiSeikyuCoReportService, P22KoukiSeikyuCoReportService>();
            services.AddTransient<IP23KoukiSeikyuCoReportService, P23KoukiSeikyuCoReportService>();
            services.AddTransient<IP24KoukiSeikyuCoReportService, P24KoukiSeikyuCoReportService>();
            services.AddTransient<IP25KoukiSeikyuCoReportService, P25KoukiSeikyuCoReportService>();
            services.AddTransient<IP27KoukiSeikyuCoReportService, P27KoukiSeikyuCoReportService>();
            services.AddTransient<IP14KokhoSokatuCoReportService, P14KokhoSokatuCoReportService>();
            services.AddTransient<IInDrugCoReportService, InDrugCoReportService>();
            services.AddTransient<ICoInDrugFinder, CoInDrugFinder>();
            services.AddTransient<IGrowthCurveA4CoReportService, GrowthCurveA4CoReportService>();
            services.AddTransient<IGrowthCurveA5CoReportService, GrowthCurveA5CoReportService>();
            services.AddTransient<ISpecialNoteFinder, SpecialNoteFinder>();
            services.AddTransient<IP17KokhoSokatuCoReportService, P17KokhoSokatuCoReportService>();
            services.AddTransient<IP20KokhoSokatuCoReportService, P20KokhoSokatuCoReportService>();
            services.AddTransient<IP22KokhoSokatuCoReportService, P22KokhoSokatuCoReportService>();
            services.AddTransient<IP23KokhoSokatuCoReportService, P23KokhoSokatuCoReportService>();
            services.AddTransient<IP26KokhoSokatuInCoReportService, P26KokhoSokatuInCoReportService>();
            services.AddTransient<IP33KokhoSokatuCoReportService, P33KokhoSokatuCoReportService>();
            services.AddTransient<IP34KokhoSokatuCoReportService, P34KokhoSokatuCoReportService>();
            services.AddTransient<IP35KokhoSokatuCoReportService, P35KokhoSokatuCoReportService>();
            services.AddTransient<IP37KokhoSokatuCoReportService, P37KokhoSokatuCoReportService>();
            services.AddTransient<IP26KokhoSokatuOutCoReportService, P26KokhoSokatuOutCoReportService>();
            services.AddTransient<IP40KokhoSokatuCoReportService, P40KokhoSokatuCoReportService>();
            services.AddTransient<IP42KokhoSokatuCoReportService, P42KokhoSokatuCoReportService>();
            services.AddTransient<IP12KokhoSokatuCoReportService, P12KokhoSokatuCoReportService>();
            services.AddTransient<IP13KokhoSokatuCoReportService, P13KokhoSokatuCoReportService>();
            services.AddTransient<IP43KokhoSokatuCoReportService, P43KokhoSokatuCoReportService>();
            services.AddTransient<IP43KoukiSokatuCoReportService, P43KoukiSokatuCoReportService>();
            services.AddTransient<IP44KokhoSokatuCoReportService, P44KokhoSokatuCoReportService>();
            services.AddTransient<IP45KokhoSokatuCoReportService, P45KokhoSokatuCoReportService>();
            services.AddTransient<IP45KoukiSokatuCoReportService, P45KoukiSokatuCoReportService>();
            services.AddTransient<IP12KokhoSeikyuCoReportService, P12KokhoSeikyuCoReportService>();
            services.AddTransient<IP13KokhoSeikyuCoReportService, P13KokhoSeikyuCoReportService>();
            services.AddTransient<IP14KokhoSeikyuCoReportService, P14KokhoSeikyuCoReportService>();
            services.AddTransient<IP20KokhoSeikyuCoReportService, P20KokhoSeikyuCoReportService>();
            services.AddTransient<IP21KokhoSeikyuCoReportService, P21KokhoSeikyuCoReportService>();
            services.AddTransient<IP22KokhoSeikyuCoReportService, P22KokhoSeikyuCoReportService>();
            services.AddTransient<IP23KokhoSeikyuCoReportService, P23KokhoSeikyuCoReportService>();
            services.AddTransient<IP24KokhoSeikyuCoReportService, P24KokhoSeikyuCoReportService>();
            services.AddTransient<IP24WelfareDiskService, P24WelfareDiskService>();
            services.AddTransient<IReceiptPrintExcelService, ReceiptPrintExcelService>();
            services.AddTransient<IImportCSVCoReportService, ImportCSVCoReportService>();
            services.AddTransient<ICoHpInfFinder, CoHpInfFinder>();
            services.AddTransient<IReceiptPrintService, ReceiptPrintService>();
            services.AddTransient<IP26KokhoSeikyuOutCoReportService, P26KokhoSeikyuOutCoReportService>();
            services.AddTransient<IP27KokhoSeikyuInCoReportService, P27KokhoSeikyuInCoReportService>();
            services.AddTransient<IP27KokhoSeikyuOutCoReportService, P27KokhoSeikyuOutCoReportService>();
            services.AddTransient<IP28KokhoSeikyuCoReportService, P28KokhoSeikyuCoReportService>();
            services.AddTransient<IP29KokhoSeikyuCoReportService, P29KokhoSeikyuCoReportService>();
            services.AddTransient<IP30KokhoSeikyuCoReportService, P30KokhoSeikyuCoReportService>();
            services.AddTransient<IP42KokhoSeikyuCoReportService, P42KokhoSeikyuCoReportService>();
            services.AddTransient<IP43KokhoSeikyuCoReportService, P43KokhoSeikyuCoReportService>();
            services.AddTransient<IP13WelfareSeikyuCoReportService, P13WelfareSeikyuCoReportService>();
            services.AddTransient<IP20WelfareSokatuCoReportService, P20WelfareSokatuCoReportService>();
            services.AddTransient<IP21WelfareSeikyuCoReportService, P21WelfareSeikyuCoReportService>();
            services.AddTransient<IP21WelfareSokatuCoReportService, P21WelfareSokatuCoReportService>();
            services.AddTransient<IP08KokhoSeikyuCoReportService, P08KokhoSeikyuCoReportService>();
            services.AddTransient<IP09KokhoSeikyuCoReportService, P09KokhoSeikyuCoReportService>();
            services.AddTransient<IP22WelfareSeikyuCoReportService, P22WelfareSeikyuCoReportService>();
            services.AddTransient<IP23NagoyaSeikyuCoReportService, P23NagoyaSeikyuCoReportService>();
            services.AddTransient<IP23WelfareSeikyuCoReportService, P23WelfareSeikyuCoReportService>();
            services.AddTransient<IP24WelfareSofuDiskCoReportService, P24WelfareSofuDiskCoReportService>();
            services.AddTransient<IP24WelfareSofuPaperCoReportService, P24WelfareSofuPaperCoReportService>();
            services.AddTransient<IP24WelfareSyomeiCoReportService, P24WelfareSyomeiCoReportService>();
            services.AddTransient<IP24WelfareSyomeiListCoReportService, P24WelfareSyomeiListCoReportService>();
            services.AddTransient<IP24WelfareSyomeiSofuCoReportService, P24WelfareSyomeiSofuCoReportService>();
            services.AddTransient<IP26VaccineSokatuCoReportService, P26VaccineSokatuCoReportService>();
            services.AddTransient<IKarte3CoReportService, Karte3CoReportService>();
            services.AddTransient<ICoKarte3Finder, CoKarte3Finder>();
            services.AddTransient<IP27IzumisanoSeikyuCoReportService, P27IzumisanoSeikyuCoReportService>();
            services.AddTransient<IP35WelfareSeikyuCoReportService, P35WelfareSeikyuCoReportService>();
            services.AddTransient<IP43KikuchiMeisai41CoReportService, P43KikuchiMeisai41CoReportService>();
            services.AddTransient<IP43KikuchiMeisai43CoReportService, P43KikuchiMeisai43CoReportService>();
            services.AddTransient<IP43KikuchiSeikyu41CoReportService, P43KikuchiSeikyu41CoReportService>();
            services.AddTransient<IP43KikuchiSeikyu43CoReportService, P43KikuchiSeikyu43CoReportService>();
            services.AddTransient<IP43KumamotoSeikyuCoReportService, P43KumamotoSeikyuCoReportService>();
            services.AddTransient<IP44WelfareSeikyu84CoReportService, P44WelfareSeikyu84CoReportService>();
            services.AddTransient<IP44WelfareDiskService, P44WelfareDiskService>();
            services.AddTransient<IP14WelfareSeikyuCoReportService, P14WelfareSeikyuCoReportService>();
            services.AddTransient<IP17KokhoSeikyuCoReportService, P17KokhoSeikyuCoReportService>();
            services.AddTransient<IP17WelfareSeikyuCoReportService, P17WelfareSeikyuCoReportService>();
            services.AddTransient<IP11KokhoSeikyuCoReportService, P11KokhoSeikyuCoReportService>();
            services.AddTransient<IP26WelfareSeikyuCoReportService, P26WelfareSeikyuCoReportService>();
            services.AddTransient<IP29WelfareSeikyuCoReportService, P29WelfareSeikyuCoReportService>();
            services.AddTransient<IP40WelfareSeikyuCoReportService, P40WelfareSeikyuCoReportService>();
            services.AddTransient<IP43ReihokuSeikyu41CoReportService, P43ReihokuSeikyu41CoReportService>();
            services.AddTransient<IP43AmakusaSeikyu41CoReportService, P43AmakusaSeikyu41CoReportService>();
            services.AddTransient<IP43AmakusaSeikyu42CoReportService, P43AmakusaSeikyu42CoReportService>();
            services.AddTransient<IP43Amakusa41DiskService, P43Amakusa41DiskService>();
            services.AddTransient<IP46WelfareDiskService, P46WelfareDiskService>();
            services.AddTransient<IP35WelfareSokatuCoReportService, P35WelfareSokatuCoReportService>();
            services.AddTransient<IP25WelfareSeikyuCoReportService, P25WelfareSeikyuCoReportService>();
            services.AddTransient<IP34KokhoSeikyuCoReportService, P34KokhoSeikyuCoReportService>();
            services.AddTransient<IP35KokhoSeikyuCoReportService, P35KokhoSeikyuCoReportService>();
            services.AddTransient<IP37KokhoSeikyuCoReportService, P37KokhoSeikyuCoReportService>();
            services.AddTransient<IP40KokhoSeikyuCityCoReportService, P40KokhoSeikyuCityCoReportService>();
            services.AddTransient<IP40KokhoSeikyuKumiaiCoReportService, P40KokhoSeikyuKumiaiCoReportService>();
            services.AddTransient<IP41KokhoSeikyuCoReportService, P41KokhoSeikyuCoReportService>();
            services.AddTransient<IP44KokhoSeikyuCoReportService, P44KokhoSeikyuCoReportService>();
            services.AddTransient<IP45KokhoSeikyuCoReportService, P45KokhoSeikyuCoReportService>();
            services.AddTransient<IP46KokhoSokatuCoReportService, P46KokhoSokatuCoReportService>();
            services.AddTransient<IKensaIraiRepository, KensaIraiRepository>();
            services.AddTransient<IP33KokhoSeikyuCoReportService, P33KokhoSeikyuCoReportService>();
            services.AddTransient<IP26KoukiSokatuInCoReportService, P26KoukiSokatuInCoReportService>();
            services.AddTransient<IP25KokhoSeikyuCoReportService, P25KokhoSeikyuCoReportService>();
            services.AddTransient<IP46KokhoSeikyuCoReportService, P46KokhoSeikyuCoReportService>();
            services.AddTransient<IP46KoukiSeikyuCoReportService, P46KoukiSeikyuCoReportService>();
            services.AddTransient<IP46WelfareSofu99CoReportService, P46WelfareSofu99CoReportService>();
            services.AddTransient<IP46WelfareSeikyu99CoReportService, P46WelfareSeikyu99CoReportService>();
            services.AddTransient<ICoNameLabelFinder, CoNameLabelFinder>();
            //call Calculate API
            services.AddTransient<ICalculateService, CalculateService>();
            services.AddTransient<ICalcultateCustomerService, CalcultateCustomerService>();
            #endregion Reporting
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
            services.AddTransient<IHistoryCommon, HistoryCommon>();
            services.AddTransient<ISaveMedicalRepository, SaveMedicalRepository>();
            services.AddTransient<IValidateFamilyList, ValidateFamilyList>();
            services.AddTransient<ITodoGrpMstRepository, TodoGrpMstRepository>();
            services.AddTransient<ITodoInfRepository, TodoInfRepository>();
            services.AddTransient<ITodoKbnMstRepository, TodoKbnMstReporitory>();
            services.AddTransient<ILockRepository, LockRepository>();
            services.AddTransient<IStatisticRepository, StatisticRepository>();
            services.AddTransient<ISta3020CoReportService, Sta3020CoReportService>();
            services.AddTransient<ICoSta3020Finder, CoSta3020Finder>();
            services.AddTransient<IKensaIraiCoReportService, KensaIraiCoReportService>();
            services.AddTransient<ICoKensaIraiFinder, CoKensaIraiFinder>();
            services.AddTransient<ISortPatientCommon, SortPatientCommon>();
            services.AddTransient<IRaiinListSettingRepository, RaiinListSettingRepository>();
            services.AddTransient<ICommonSuperSet, CommonSuperSet>();
            services.AddTransient<ICoKarte1Finder, CoKarte1Finder>();
            services.AddTransient<ICoPtByomeiFinder, CoPtByomeiFinder>();
            services.AddTransient<ICheckOpenReportingService, CheckOpenReportingService>();
            services.AddTransient<IUserTokenRepository, UserTokenRepository>();
            services.AddTransient<IKensaLabelCoReportService, KensaLabelCoReportService>();
            services.AddTransient<IKensaLabelFinder, KensaLabelFinder>();
            services.AddTransient<IGetCommonDrugInf, GetCommonDrugInf>();
            services.AddTransient<ICommonReceRecalculation, CommonReceRecalculation>();
            services.AddTransient<IOnlineRepository, OnlineRepository>();
            services.AddTransient<IStaticsticExportCsvService, StaticsticExportCsvService>();
            services.AddTransient<IAuditLogRepository, AuditLogRepository>();
            services.AddTransient<IListSetMstRepository, ListSetMstRepository>();
            services.AddTransient<IKensaSetRepository, KensaSetRepository>();
            services.AddTransient<IByomeiSetGenerationMstRepository, ByomeiSetGenerationMstRepository>();
            services.AddTransient<ISmartKartePortRepository, SmartKartePortRepository>();
            services.AddTransient<IKensaIraiCommon, KensaIraiCommon>();
            ///services.AddTransient<ISystemStartDbRepository, SystemStartDbRepository>();
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
            busBuilder.RegisterUseCase<GetAllPermissionInputData, GetAllPermissionInteractor>();
            busBuilder.RegisterUseCase<GetListUserByCurrentUserInputData, GetListUserByCurrentUserInteractor>();
            busBuilder.RegisterUseCase<GetListJobMstInputData, GetListJobMstInteractor>();
            busBuilder.RegisterUseCase<GetListFunctionPermissionInputData, GetListFunctionPermissionInteractor>();
            busBuilder.RegisterUseCase<SaveListUserMstInputData, SaveListUserMstInteractor>();
            busBuilder.RegisterUseCase<SigninRefreshTokenInputData, SigInRefreshTokenInteractor>();
            busBuilder.RegisterUseCase<RefreshTokenByUserInputData, RefreshTokenByUserInteractor>();
            busBuilder.RegisterUseCase<GetUserInfoInputData, GetUserInfoInteractor>();

            //ApprovalInfo
            busBuilder.RegisterUseCase<GetApprovalInfListInputData, GetApprovalInfListInteractor>();
            busBuilder.RegisterUseCase<SaveApprovalInfListInputData, SaveApprovalInfListInteractor>();
            busBuilder.RegisterUseCase<CheckSaveLogOutInputData, CheckSaveLogOutInteractor>();

            //PtByomeis
            busBuilder.RegisterUseCase<GetPtDiseaseListInputData, GetPtDiseaseListInteractor>();
            busBuilder.RegisterUseCase<ValidationPtDiseaseListInputData, ValidationPtDiseaseListInteractor>();
            busBuilder.RegisterUseCase<GetAllByomeiByPtIdInputData, GetAllByomeiByPtIdInteractor>();
            busBuilder.RegisterUseCase<UpdateByomeiSetMstInputData, UpdateByomeiSetMstInteractor>();

            //Order Info
            busBuilder.RegisterUseCase<GetOrdInfListTreeInputData, GetOrdInfListTreeInteractor>();
            busBuilder.RegisterUseCase<GetMaxRpNoInputData, GetMaxRpNoInteractor>();
            busBuilder.RegisterUseCase<GetHeaderInfInputData, GetHeaderInfInteractor>();
            busBuilder.RegisterUseCase<CheckOrdInfInDrugInputData, CheckOrdInfInDrugInteractor>();

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
            busBuilder.RegisterUseCase<GetInsuranceInfInputData, GetInsuranceInfInteractor>();
            busBuilder.RegisterUseCase<GetMedicalDetailsInputData, GetMedicalDetailsInteractor>();
            busBuilder.RegisterUseCase<DeleteReceptionInputData, DeleteReceptionInteractor>();
            busBuilder.RegisterUseCase<GetRaiinListWithKanInfInputData, GetRaiinListWithKanInfInteractor>();
            busBuilder.RegisterUseCase<RevertDeleteNoReceptInputData, RevertDeleteNoReceptInteractor>();
            busBuilder.RegisterUseCase<GetOutDrugOrderListInputData, GetOutDrugOrderListInteractor>();
            busBuilder.RegisterUseCase<GetYoyakuRaiinInfInputData, GetYoyakuRaiinInfInteractor>();
            busBuilder.RegisterUseCase<GetRaiinInfBySinDateInputData, GetRaiinInfBySinDateInteractor>();
            busBuilder.RegisterUseCase<GetHpInfInputData, GetHpInfInteractor>();

            // Visiting
            busBuilder.RegisterUseCase<SaveVisitingListSettingsInputData, SaveVisitingListSettingsInteractor>();
            busBuilder.RegisterUseCase<GetPatientCommentInputData, GetPatientCommentInteractor>();
            busBuilder.RegisterUseCase<GetReceptionVisitingInputData, GetReceptionVisitingInteractor>();

            //Insurance
            busBuilder.RegisterUseCase<GetInsuranceListInputData, GetInsuranceListInteractor>();
            busBuilder.RegisterUseCase<ValidMainInsuranceInputData, ValidInsuranceMainInteractor>();
            busBuilder.RegisterUseCase<GetInsuranceComboListInputData, GetInsuranceComboListInteractor>();
            busBuilder.RegisterUseCase<ValidHokenInfAllTypeInputData, ValidHokenInfAllTypeInteractor>();
            busBuilder.RegisterUseCase<HokenPatternUsedInputData, HokenPatternUsedInteractor>();
            busBuilder.RegisterUseCase<FindPtHokenListInputData, FindPtHokenListInteractor>();

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
            busBuilder.RegisterUseCase<SearchPatientInfoByPtNumInputData, SearchPatientInfoByPtNumInteractor>();
            busBuilder.RegisterUseCase<GetTokkiMstListInputData, GetTokkiMstListInteractor>();
            busBuilder.RegisterUseCase<CalculationSwapHokenInputData, CalculationSwapHokenInteractor>();
            busBuilder.RegisterUseCase<CheckValidSamePatientInputData, CheckValidSamePatientInteractor>();
            busBuilder.RegisterUseCase<CheckAllowDeletePatientInfoInputData, CheckAllowDeletePatientInfoInteractor>();
            busBuilder.RegisterUseCase<SearchPatientInfoByPtIdListInputData, SearchPatientInfoByPtIdListInteractor>();
            busBuilder.RegisterUseCase<SavePtKyuseiInputData, SavePtKyuseiInteractor>();
            busBuilder.RegisterUseCase<GetPtInfByRefNoInputData, GetPtInfByRefNoInteractor>();
            busBuilder.RegisterUseCase<GetPtInfModelsByNameInputData, GetPtInfModelsByNameInteractor>();
            busBuilder.RegisterUseCase<GetPtInfModelsByRefNoInputData, GetPtInfModelsByRefNoInteractor>();
            busBuilder.RegisterUseCase<GetVisitTimesManagementModelsInputData, GetVisitTimesManagementModelsInteractor>();
            busBuilder.RegisterUseCase<UpdateVisitTimesManagementInputData, UpdateVisitTimesManagementInteractor>();
            busBuilder.RegisterUseCase<UpdateVisitTimesManagementNeedSaveInputData, UpdateVisitTimesManagementNeedSaveInteractor>();

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
            busBuilder.RegisterUseCase<GetConversionInputData, GetConversionInteractor>();
            busBuilder.RegisterUseCase<SaveConversionInputData, SaveConversionInteractor>();
            busBuilder.RegisterUseCase<GetOdrSetNameInputData, GetOdrSetNameInteractor>();
            busBuilder.RegisterUseCase<SaveOdrSetInputData, SaveOdrSetInteractor>();
            busBuilder.RegisterUseCase<GetSetGenerationMstListInputData, GetSetGenerationMstListInteractor>();
            busBuilder.RegisterUseCase<GetSetKbnMstListByGenerationIdInputData, GetSetKbnMstListByGenerationIdInteractor>();

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
            busBuilder.RegisterUseCase<SaveMedicalInputData, SaveMedicalInteractor>();
            busBuilder.RegisterUseCase<GetDefaultSelectedTimeInputDataOfMedical, GetDefaultSelectedTimeInteractorOfMedical>();
            busBuilder.RegisterUseCase<ConvertItemInputData, ConvertItemInteractor>();
            busBuilder.RegisterUseCase<CalculateInputData, CalculateInteractor>();
            busBuilder.RegisterUseCase<GetDataPrintKarte2InputData, GetDataPrintKarte2Interactor>();
            busBuilder.RegisterUseCase<GetHistoryFollowSindateInputData, GetHistoryFollowSindateInteractor>();
            busBuilder.RegisterUseCase<GetOrdersForOneOrderSheetGroupInputData, GetOrdersForOneOrderSheetGroupInteractor>();
            busBuilder.RegisterUseCase<GetOrderSheetGroupInputData, GetOrderSheetGroupInteractor>();
            busBuilder.RegisterUseCase<GetTrialAccountingInputData, GetTrialAccountingInteractor>();
            busBuilder.RegisterUseCase<CheckOpenTrialAccountingInputData, CheckOpenTrialAccountingInteractor>();
            busBuilder.RegisterUseCase<GetKensaAuditTrailLogInputData, GetKensaAuditTrailLogInteractor>();
            busBuilder.RegisterUseCase<GetContainerMstInputData, GetContainerMstInteractor>();
            busBuilder.RegisterUseCase<GetSinkouCountInMonthInputData, GetSinkouCountInMonthInteractor>();
            busBuilder.RegisterUseCase<CheckNextOrdHaveOdrInputData, CheckNextOrdHaveInteractor>();
            busBuilder.RegisterUseCase<GetHeaderVistitDateInputData, GetHeaderVistitDateInteractor>();
            busBuilder.RegisterUseCase<CheckUpsertNextOrderInputData, CheckUpsertNextOrderInteractor>();
            busBuilder.RegisterUseCase<SaveKensaIraiInputData, SaveKensaIraiInteractor>();
            busBuilder.RegisterUseCase<ContainerMasterUpdateInputData, ContainerMasterUpdateInteractor>();
            busBuilder.RegisterUseCase<GetLastDayInfoListInputData, GetLastDayInfoListInteractor>();
            busBuilder.RegisterUseCase<SaveSettingLastDayInfoListInputData, SaveSettingLastDayInfoListInteractor>();

            //SetKbn
            busBuilder.RegisterUseCase<GetSetKbnMstListInputData, GetSetKbnMstListInteractor>();
            busBuilder.RegisterUseCase<UpsertSetKbnMstInputData, UpsertSetKbnMstInteractor>();

            //Insurance Mst
            busBuilder.RegisterUseCase<GetInsuranceMstInputData, GetInsuranceMstInteractor>();
            busBuilder.RegisterUseCase<GetDefaultSelectPatternInputData, GetDefaultSelectPatternInteractor>();
            busBuilder.RegisterUseCase<GetInsuranceMasterDetailInputData, GetInsuranceMasterDetailInteractor>();
            busBuilder.RegisterUseCase<GetSelectMaintenanceInputData, GetSelectMaintenanceInteractor>();
            busBuilder.RegisterUseCase<FindHokenInfByPtIdInputData, FindHokenInfByPtIdInteractor>();


            busBuilder.RegisterUseCase<DeleteHokenMasterInputData, DeleteHokenMasterInteractor>();
            busBuilder.RegisterUseCase<GetInfoCloneInsuranceMstInputData, GetInfoCloneInsuranceMstInteractor>();
            busBuilder.RegisterUseCase<SaveHokenMasterInputData, SaveHokenMasterInteractor>();
            busBuilder.RegisterUseCase<SaveOrdInsuranceMstInputData, SaveOrdInsuranceMstInteractor>();
            busBuilder.RegisterUseCase<GetHokenMasterReadOnlyInputData, GetHokenMasterReadOnlyInteractor>();

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
            busBuilder.RegisterUseCase<GetTooltipInputData, GetTooltipFlowSheetInteractor>();

            // UketukeSbtDayInf
            busBuilder.RegisterUseCase<GetReceptionInsuranceInputData, ReceptionInsuranceInteractor>();

            // KarteFilter
            busBuilder.RegisterUseCase<GetKarteFilterInputData, GetKarteFilterMstsInteractor>();

            busBuilder.RegisterUseCase<SaveKarteFilterInputData, SaveKarteFilterMstsInteractor>();

            // ColumnSetting
            busBuilder.RegisterUseCase<SaveColumnSettingListInputData, SaveColumnSettingListInteractor>();
            busBuilder.RegisterUseCase<GetColumnSettingListInputData, GetColumnSettingListInteractor>();
            busBuilder.RegisterUseCase<GetColumnSettingByTableNameListInputData, GetColumnSettingByTableNameListInteractor>();

            // JsonSetting
            busBuilder.RegisterUseCase<GetJsonSettingInputData, GetJsonSettingInteractor>();
            busBuilder.RegisterUseCase<UpsertJsonSettingInputData, UpsertJsonSettingInteractor>();
            busBuilder.RegisterUseCase<GetAllJsonSettingInputData, GetAllJsonSettingInteractor>();

            // Reception Same Visit
            busBuilder.RegisterUseCase<GetReceptionSameVisitInputData, GetReceptionSameVisitInteractor>();

            //Special note
            busBuilder.RegisterUseCase<GetSpecialNoteInputData, GetSpecialNoteInteractor>();
            busBuilder.RegisterUseCase<SaveSpecialNoteInputData, SaveSpecialNoteInteractor>();
            busBuilder.RegisterUseCase<AddAlrgyDrugListInputData, AddAlrgyDrugListInteractor>();
            busBuilder.RegisterUseCase<GetPtWeightInputData, GetPtWeightInteractor>();
            busBuilder.RegisterUseCase<GetStdPointInputData, GetStdPointInteractor>();

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
            busBuilder.RegisterUseCase<GetAllCmtCheckMstInputData, GetAllCmtCheckMstInteractor>();
            busBuilder.RegisterUseCase<SearchTenMstItemInputData, SearchTenMstItemInteractor>();
            busBuilder.RegisterUseCase<ConvertStringChkJISKjInputData, ConvertStringChkJISKjInteractor>();
            busBuilder.RegisterUseCase<GetTeikyoByomeiInputData, GetTeikyoByomeiInteractor>();
            busBuilder.RegisterUseCase<GetDrugActionInputData, GetDrugActionInteractor>();
            busBuilder.RegisterUseCase<GetDefaultPrecautionsInputData, GetDefaultPrecautionsInteractor>();
            busBuilder.RegisterUseCase<UploadImageDrugInfInputData, UploadImageDrugInfInteractor>();
            busBuilder.RegisterUseCase<GetTenMstListInputData, GetTenMstListInteractor>();
            busBuilder.RegisterUseCase<GetDiseaseListInputData, GetDiseaseListInteractor>();
            busBuilder.RegisterUseCase<GetSingleDoseMstAndMedicineUnitListInputData, GetSingleDoseMstAndMedicineUnitListInteractor>();
            busBuilder.RegisterUseCase<UpdateCmtCheckMstInputData, UpdateCmtCheckMstInteractor>();
            busBuilder.RegisterUseCase<GetParrentKensaMstInputData, GetParrentKensaMstListInteractor>();
            busBuilder.RegisterUseCase<UpdateSingleDoseMstInputData, UpdateSingleDoseMstInteractor>();
            busBuilder.RegisterUseCase<UpdateKensaMstInputData, UpdateKensaMstInteractor>();
            busBuilder.RegisterUseCase<UpdateByomeiMstInputData, UpdateByomeiMstInteractor>();
            busBuilder.RegisterUseCase<F17CommonInputData, F17CommonInteractor>();
            busBuilder.RegisterUseCase<UpdateKensaStdMstInputData, UpdateKensaStdMstInteractor>();
            busBuilder.RegisterUseCase<UpsertMaterialMasterInputData, UpsertMaterialMasterInteractor>();
            busBuilder.RegisterUseCase<GetListKensaMstInputData, GetListKensaMstInteractor>();
            busBuilder.RegisterUseCase<UpdateJihiSbtMstInputData, UpdateJihiSbtMstInteractor>();
            busBuilder.RegisterUseCase<GetListKensaIjiSettingInputData, GetListKensaIjiSettingInteractor>();
            busBuilder.RegisterUseCase<GetSetNameMntInputData, GetSetNameMntInteractor>();
            busBuilder.RegisterUseCase<UpdateYohoSetMstInputData, UpdateYohoSetMstInteractor>();
            busBuilder.RegisterUseCase<GetListYohoSetMstModelByUserIDInputData, GetListYohoSetMstModelByUserIDInteractor>();
            busBuilder.RegisterUseCase<IsUsingKensaInputData, IsUsingKensaInteractor>();
            busBuilder.RegisterUseCase<IsKensaItemOrderingInputData, IsKensaItemOrderingInteractor>();
            busBuilder.RegisterUseCase<ExistUsedKensaItemCdInputData, ExistUsedKensaItemCdInteractor>();
            busBuilder.RegisterUseCase<GetListUserInputData, GetListUserInteractor>();
            busBuilder.RegisterUseCase<GetTenMstByCodeInputData, GetTenMstByCodeInteractor>();
            busBuilder.RegisterUseCase<GetByomeiByCodeInputData, GetByomeiByCodeInteractor>();
            busBuilder.RegisterUseCase<GetNextUketukeNoBySettingInputData, GetNextUketukeNoBySettingInteractor>();
            busBuilder.RegisterUseCase<GetRenkeiTimingInputData, GetRenkeiTimingInteractor>();
            busBuilder.RegisterUseCase<CheckJihiSbtExistsInTenMstInputData, CheckJihiSbtExistsInTenMstInteractor>();

            // Disease
            busBuilder.RegisterUseCase<UpsertPtDiseaseListInputData, UpsertPtDiseaseListInteractor>();
            busBuilder.RegisterUseCase<DiseaseSearchInputData, DiseaseSearchInteractor>();
            busBuilder.RegisterUseCase<GetSetByomeiTreeInputData, GetSetByomeiTreeInteractor>();
            busBuilder.RegisterUseCase<GetTreeByomeiSetInputData, GetTreeByomeiSetInteractor>();
            busBuilder.RegisterUseCase<GetListByomeiSetGenerationMstInputData, GetListByomeiSetGenerationMstInteractor>();

            // Drug Infor - Data Menu and Detail 
            busBuilder.RegisterUseCase<GetDrugDetailInputData, GetDrugDetailInteractor>();
            busBuilder.RegisterUseCase<GetDrugDetailDataInputData, GetDrugDetailDataInteractor>();
            busBuilder.RegisterUseCase<ShowProductInfInputData, ShowProductInfInteractor>();
            busBuilder.RegisterUseCase<ShowKanjaMukeInputData, ShowKanjaMukeInteractor>();
            busBuilder.RegisterUseCase<ShowMdbByomeiInputData, ShowMdbByomeiInteractor>();

            //DrugInfor
            busBuilder.RegisterUseCase<GetDrugInforInputData, GetDrugInforInteractor>();
            busBuilder.RegisterUseCase<GetDataPrintDrugInfoInputData, GetDataPrintDrugInfoInteractor>();

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
            busBuilder.RegisterUseCase<GetLastKaruteInputData, GetLastKaruteInteractor>();

            //System Conf
            busBuilder.RegisterUseCase<GetSystemConfInputData, GetSystemConfInteractor>();
            busBuilder.RegisterUseCase<GetSystemConfListInputData, GetSystemConfListInteractor>();
            busBuilder.RegisterUseCase<GetSystemConfForPrintInputData, GetSystemConfForPrintInteractor>();
            busBuilder.RegisterUseCase<GetDrugCheckSettingInputData, GetDrugCheckSettingInteractor>();
            busBuilder.RegisterUseCase<SaveDrugCheckSettingInputData, SaveDrugCheckSettingInteractor>();
            busBuilder.RegisterUseCase<GetSystemSettingInputData, GetSystemSettingInteractor>();
            busBuilder.RegisterUseCase<SaveSystemSettingInputData, SaveSystemSettingInteractor>();
            busBuilder.RegisterUseCase<GetSystemConfListXmlPathInputData, GetSystemConfListXmlPathInteractor>();
            busBuilder.RegisterUseCase<GetPathAllInputData, GetAllPathInteractor>();
            busBuilder.RegisterUseCase<SavePathInputData, SavePathInteractor>();

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
            busBuilder.RegisterUseCase<IsNyukinExistedInputData, IsNyukinExistedInteractor>();

            //Accounting
            busBuilder.RegisterUseCase<GetAccountingInputData, GetAccountingInteractor>();
            busBuilder.RegisterUseCase<GetPaymentMethodInputData, GetPaymentMethodInteractor>();
            busBuilder.RegisterUseCase<GetWarningMemoInputData, GetWarningMemoInteractor>();
            busBuilder.RegisterUseCase<GetPtByoMeiInputData, GetPtByoMeiInteractor>();
            busBuilder.RegisterUseCase<SaveAccountingInputData, SaveAccountingInteractor>();
            busBuilder.RegisterUseCase<GetAccountingHeaderInputData, GetAccountingHeaderInteractor>();
            busBuilder.RegisterUseCase<CheckAccountingStatusInputData, CheckAccountingStatusInteractor>();
            busBuilder.RegisterUseCase<GetAccountingConfigInputData, GetAccountingConfigInteractor>();
            busBuilder.RegisterUseCase<GetMeiHoGaiInputData, GetMeiHoGaiInteractor>();
            busBuilder.RegisterUseCase<CheckOpenAccountingInputData, CheckOpenAccountingInteractor>();
            busBuilder.RegisterUseCase<RecaculationInputData, RecaculationInteractor>();
            busBuilder.RegisterUseCase<GetListHokenSelectInputData, GetListHokenSelectInteractor>();

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
            busBuilder.RegisterUseCase<GetUserConfigParamInputData, GetUserConfigParamInteractor>();
            busBuilder.RegisterUseCase<GetUserConfModelListInputData, GetUserConfModelListInteractor>();

            //SwapHoken
            busBuilder.RegisterUseCase<SaveSwapHokenInputData, SaveSwapHokenInteractor>();
            busBuilder.RegisterUseCase<ValidateSwapHokenInputData, ValidateSwapHokenInteractor>();

            //System Config Generation 
            busBuilder.RegisterUseCase<GetSystemGenerationConfInputData, GetSystemGenerationConfInteractor>();
            busBuilder.RegisterUseCase<GetSystemGenerationConfListInputData, GetSystemGenerationConfListInteractor>();

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
            busBuilder.RegisterUseCase<ValidateFamilyListInputData, ValidateFamilyListInteractor>();
            busBuilder.RegisterUseCase<GetMaybeFamilyListInputData, GetMaybeFamilyListInteractor>();

            //Receipt
            busBuilder.RegisterUseCase<ReceiptListAdvancedSearchInputData, ReceiptListAdvancedSearchInteractor>();
            busBuilder.RegisterUseCase<GetListSokatuMstInputData, GetListSokatuMstInteractor>();

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
            busBuilder.RegisterUseCase<GetReceCheckOptionListInputData, GetReceCheckOptionListInteractor>();
            busBuilder.RegisterUseCase<SaveReceCheckOptInputData, SaveReceCheckOptInteractor>();
            busBuilder.RegisterUseCase<RecalculationInputData, RecalculationInteractor>();
            busBuilder.RegisterUseCase<ReceCmtHistoryInputData, ReceCmtHistoryInteractor>();
            busBuilder.RegisterUseCase<SyoukiInfHistoryInputData, SyoukiInfHistoryInteractor>();
            busBuilder.RegisterUseCase<SyobyoKeikaHistoryInputData, SyobyoKeikaHistoryInteractor>();
            busBuilder.RegisterUseCase<GetRecePreviewListInputData, GetRecePreviewListInteractor>();
            busBuilder.RegisterUseCase<DoReceCmtInputData, DoReceCmtInteractor>();
            busBuilder.RegisterUseCase<GetReceiptEditInputData, GetReceiptEditInteractor>();
            busBuilder.RegisterUseCase<GetSinMeiInMonthListInputData, GetSinMeiInMonthListInteractor>();
            busBuilder.RegisterUseCase<SaveReceiptEditInputData, SaveReceiptEditInteractor>();
            busBuilder.RegisterUseCase<GetSinDateRaiinInfListInputData, GetSinDateRaiinInfListInteractor>();
            busBuilder.RegisterUseCase<GetReceByomeiCheckingInputData, GetReceByomeiCheckingInteractor>();
            busBuilder.RegisterUseCase<SaveReceStatusInputData, SaveReceStatusInteractor>();
            busBuilder.RegisterUseCase<GetReceStatusInputData, GetReceStatusInteractor>();
            busBuilder.RegisterUseCase<ReceiptCheckRecalculationInputData, ReceCheckRecalculationInteractor>();
            busBuilder.RegisterUseCase<DeleteReceiptInfEditInputData, DeleteReceiptInfEditInteractor>();
            busBuilder.RegisterUseCase<GetListKaikeiInfInputData, GetListKaikeiInfInteractor>();
            busBuilder.RegisterUseCase<CheckExisReceInfEditInputData, CheckExisReceInfEditInteractor>();
            busBuilder.RegisterUseCase<CheckExistsReceInfInputData, CheckExistsReceInfInteractor>();
            busBuilder.RegisterUseCase<CheckExistSyobyoKeikaInputData, CheckExistSyobyoKeikaInteractor>();
            busBuilder.RegisterUseCase<GetListRaiinInfInputDataOfReceipt, GetListRaiinInfInteractorOfReceipt>();

            //ReceSeikyu
            busBuilder.RegisterUseCase<GetListReceSeikyuInputData, GetListReceSeikyuInteractor>();
            busBuilder.RegisterUseCase<SearchReceInfInputData, SearchReceInfInteractor>();
            busBuilder.RegisterUseCase<SaveReceSeiKyuInputData, SaveReceSeiKyuInteractor>();
            busBuilder.RegisterUseCase<ImportFileReceSeikyuInputData, ImportFileReceSeikyuInteractor>();
            busBuilder.RegisterUseCase<CancelSeikyuInputData, CancelSeikyuInteractor>();
            busBuilder.RegisterUseCase<GetReceSeikyModelByPtNumInputData, GetReceSeikyModelByPtNumInteractor>();

            //WeightedSetConfirmation
            busBuilder.RegisterUseCase<IsOpenWeightCheckingInputData, IsOpenWeightCheckingInteractor>();

            //TodoInteractor
            busBuilder.RegisterUseCase<UpsertTodoGrpMstInputData, UpsertTodoGrpMstInteractor>();
            busBuilder.RegisterUseCase<UpsertTodoInfInputData, UpsertTodoInfInteractor>();
            busBuilder.RegisterUseCase<GetTodoInfFinderInputData, GetTodoInfFinderInteractor>();
            busBuilder.RegisterUseCase<GetTodoGrpInputData, GetTodoGrpInteractor>();
            busBuilder.RegisterUseCase<GetTodoKbnInputData, GetTodoKbnInteractor>();

            //CreateUKEFile
            busBuilder.RegisterUseCase<CreateUKEFileInputData, CreateUKEFileInteractor>();
            busBuilder.RegisterUseCase<ValidateCreateUKEFileInputData, ValidateCreateUKEFileInteractor>();

            //TenMstMaintenance
            busBuilder.RegisterUseCase<GetListTenMstOriginInputData, GetListTenMstOriginInteractor>();
            busBuilder.RegisterUseCase<GetTenMstOriginInfoCreateInputData, GetTenMstOriginInfoCreateInteractor>();
            busBuilder.RegisterUseCase<DeleteOrRecoverTenMstInputData, DeleteOrRecoverTenMstInteractor>();
            busBuilder.RegisterUseCase<GetSetDataTenMstInputData, GetSetDataTenMstInteractor>();
            busBuilder.RegisterUseCase<SaveSetDataTenMstInputData, SaveSetDataTenMstInteractor>();
            busBuilder.RegisterUseCase<GetListDrugImageInputData, GetListDrugImageInteractor>();
            busBuilder.RegisterUseCase<GetRenkeiMstInputData, GetRenkeiMstInteractor>();
            busBuilder.RegisterUseCase<CheckIsTenMstUsedInputData, CheckIsTenMstUsedInteractor>();
            busBuilder.RegisterUseCase<GetTenMstListByItemTypeInputData, GetTenMstListByItemTypeInteractor>();

            //Lock
            busBuilder.RegisterUseCase<AddLockInputData, AddLockInteractor>();
            busBuilder.RegisterUseCase<CheckLockInputData, CheckLockInteractor>();
            busBuilder.RegisterUseCase<RemoveLockInputData, RemoveLockInteractor>();
            busBuilder.RegisterUseCase<ExtendTtlLockInputData, ExtendTtlLockInteractor>();
            busBuilder.RegisterUseCase<GetLockInfoInputData, GetLockInfoInteractor>();
            busBuilder.RegisterUseCase<CheckLockVisitingInputData, CheckLockVisitingInteractor>();
            busBuilder.RegisterUseCase<CheckExistFunctionCodeInputData, CheckExistFunctionCodeInteractor>();
            busBuilder.RegisterUseCase<GetLockInfInputData, GetLockInfInteractor>();
            busBuilder.RegisterUseCase<UnlockInputData, UnlockInteractor>();
            busBuilder.RegisterUseCase<CheckIsExistedOQLockInfoInputData, CheckIsExistedOQLockInfoInteractor>();

            // Statistic
            busBuilder.RegisterUseCase<GetStatisticMenuInputData, GetStatisticMenuInteractor>();
            busBuilder.RegisterUseCase<SaveStatisticMenuInputData, SaveStatisticMenuInteractor>();

            // MainMenu
            busBuilder.RegisterUseCase<GetKensaIraiInputData, GetKensaIraiInteractor>();
            busBuilder.RegisterUseCase<GetKensaCenterMstListInputData, GetKensaCenterMstListInteractor>();
            busBuilder.RegisterUseCase<CreateDataKensaIraiRenkeiInputData, CreateDataKensaIraiRenkeiInteractor>();
            busBuilder.RegisterUseCase<GetKensaInfInputData, GetKensaInfInteractor>();
            busBuilder.RegisterUseCase<DeleteKensaInfInputData, DeleteKensaInfInteractor>();
            busBuilder.RegisterUseCase<GetKensaIraiLogInputData, GetKensaIraiLogInteractor>();
            busBuilder.RegisterUseCase<KensaIraiReportInputData, KensaIraiReportInteractor>();
            busBuilder.RegisterUseCase<GetStaCsvMstInputData, GetStaCsvMstInteractor>();
            busBuilder.RegisterUseCase<SaveStaCsvMstInputData, SaveStaCsvMstInteractor>();
            busBuilder.RegisterUseCase<ImportKensaIraiInputData, ImportKensaIraiInteractor>();
            busBuilder.RegisterUseCase<GetRsvInfToConfirmInputData, GetRsvInfToConfirmInteractor>();
            busBuilder.RegisterUseCase<GetListQualificationInfInputData, GetListQualificationInfInteractor>();

            //TimeZoneConfGroup
            busBuilder.RegisterUseCase<GetTimeZoneConfGroupInputData, GetTimeZoneConfGroupInteractor>();
            busBuilder.RegisterUseCase<SaveTimeZoneConfInputData, SaveTimeZoneConfInteractor>();

            //MstItem
            busBuilder.RegisterUseCase<GetJihiSbtMstListInputData, GetJihiMstsInteractor>();
            busBuilder.RegisterUseCase<SaveAddressMstInputData, SaveAddressMstInteractor>();
            busBuilder.RegisterUseCase<GetRenkeiConfInputData, GetRenkeiConfInteractor>();
            busBuilder.RegisterUseCase<SaveRenkeiInputData, SaveRenkeiInteractor>();

            //ListSetMst
            busBuilder.RegisterUseCase<UpdateListSetMstInputData, UpdateListSetMstInteractor>();

            //HolidayMst
            busBuilder.RegisterUseCase<SaveHolidayMstInputData, SaveHolidayMstInteractor>();

            //RaiinListSetting
            busBuilder.RegisterUseCase<GetDocCategoryRaiinInputData, GetDocCategoryRaiinInteractor>();
            busBuilder.RegisterUseCase<GetFilingcategoryInputData, GetFilingcategoryInteractor>();
            busBuilder.RegisterUseCase<GetRaiiinListSettingInputData, GetRaiiinListSettingInteractor>();
            busBuilder.RegisterUseCase<SaveRaiinListSettingInputData, SaveRaiinListSettingInteractor>();

            //SinKoui
            busBuilder.RegisterUseCase<GetListSinKouiInputData, GetListSinKouiInteractor>();

            // Online
            busBuilder.RegisterUseCase<InsertOnlineConfirmHistoryInputData, InsertOnlineConfirmHistoryInteractor>();
            busBuilder.RegisterUseCase<GetRegisterdPatientsFromOnlineInputData, GetRegisterdPatientsFromOnlineInteractor>();
            busBuilder.RegisterUseCase<UpdateOnlineConfirmationHistoryInputData, UpdateOnlineConfirmationHistoryInteractor>();
            busBuilder.RegisterUseCase<UpdateOnlineHistoryByIdInputData, UpdateOnlineHistoryByIdInteractor>();
            busBuilder.RegisterUseCase<UpdateOQConfirmationInputData, UpdateOQConfirmationInteractor>();
            busBuilder.RegisterUseCase<SaveAllOQConfirmationInputData, SaveAllOQConfirmationInteractor>();
            busBuilder.RegisterUseCase<SaveOQConfirmationInputData, SaveOQConfirmationInteractor>();
            busBuilder.RegisterUseCase<UpdateRefNoInputData, UpdateRefNoInteractor>();
            busBuilder.RegisterUseCase<UpdateOnlineInRaiinInfInputData, UpdateOnlineInRaiinInfInteractor>();
            busBuilder.RegisterUseCase<UpdatePtInfOnlineQualifyInputData, UpdatePtInfOnlineQualifyInteractor>();
            busBuilder.RegisterUseCase<GetListOnlineConfirmationHistoryModelInputData, GetListOnlineConfirmationHistoryModelInteractor>();
            busBuilder.RegisterUseCase<GetOnlineConsentInputData, GetOnlineConsentInteractor>();
            busBuilder.RegisterUseCase<UpdateOnlineConsentsInputData, UpdateOnlineConsentsInteractor>();
            busBuilder.RegisterUseCase<UpdateOnlineConfirmationInputData, UpdateOnlineConfirmationInteractor>();
            busBuilder.RegisterUseCase<InsertOnlineConfirmationInputData, InsertOnlineConfirmationInteractor>();

            //AccountingFormMst
            busBuilder.RegisterUseCase<GetAccountingFormMstInputData, GetAccountingFormMstInteractor>();
            busBuilder.RegisterUseCase<UpdateAccountingFormMstInputData, UpdateAccountingFormMstInteractor>();

            //Ka
            busBuilder.RegisterUseCase<GetKacodeMstYossiInputData, GetKaCodeMstYossiInteractor>();
            busBuilder.RegisterUseCase<GetKaCodeYousikiMstInputData, GetKaCodeYousikiMstInteractor>();

            //Audit Log
            busBuilder.RegisterUseCase<SaveAuditTrailLogInputData, SaveAuditTrailLogInteractor>();
            busBuilder.RegisterUseCase<WriteLogInputData, WriteLogInteractor>();
            busBuilder.RegisterUseCase<WriteListLogInputData, WriteListLogInteractor>();

            // Disease Name Mst Seach
            busBuilder.RegisterUseCase<DiseaseNameMstSearchInputData, DiseaseNameMstSearchInteractor>();

            //PatientManagement
            busBuilder.RegisterUseCase<SearchPtInfsInputData, SearchPtInfsInteractor>();
            busBuilder.RegisterUseCase<GetHokenMstInputData, GetHokenMstInteractor>();
            busBuilder.RegisterUseCase<SaveStaConfMenuInputData, SaveStaConfMenuInteractor>();
            busBuilder.RegisterUseCase<GetStaConfMenuInputData, GetStaConfMenuInteractor>();

            //ListSetMst
            busBuilder.RegisterUseCase<GetTreeListSetInputData, GetTreeListSetInteractor>();

            //Get List Set Senkai Generation
            busBuilder.RegisterUseCase<SetSendaiGenerationInputData, SetSendaiGenerationGetListInteractor>();
            busBuilder.RegisterUseCase<DeleteSendaiGenerationInputData, DeleteSetSendaiGenerationInteractor>();
            busBuilder.RegisterUseCase<AddSetSendaiGenerationInputData, AddSetSendaiGenerationInteractor>();
            busBuilder.RegisterUseCase<RestoreSetSendaiGenerationInputData, RestoreSetSendaiGenerationInteractor>();

            // KensaHistory
            busBuilder.RegisterUseCase<UpdateKensaSetInputData, UpdateKensaSetInteractor>();
            busBuilder.RegisterUseCase<GetListKensaSetInputData, GetListKensaSetInteractor>();
            busBuilder.RegisterUseCase<GetListKensaSetDetailInputData, GetListKensaSetDetailInteractor>();
            busBuilder.RegisterUseCase<GetListKensaCmtMstInputData, GetListKensaCmtMstInteractor>();
            busBuilder.RegisterUseCase<UpdateKensaInfDetailInputData, UpdateKensaInfDetailInteractor>();
            busBuilder.RegisterUseCase<GetListKensaInfDetailInputData, GetListKensaInfDetailInteractor>();
            busBuilder.RegisterUseCase<GetKensaInfDetailByIraiCdInputData, GetKensaInfDetailByIraiCdInteractor>();


            //ListSetGeneration
            busBuilder.RegisterUseCase<GetListSetGenerationMstInputData, ListSetGenerationMstInteractor>();

            //Compare TenMst CompareTenMstInputData
            busBuilder.RegisterUseCase<CompareTenMstInputData, CompareTenMstInteractor>();
            busBuilder.RegisterUseCase<SaveCompareTenMstInputData, SaveCompareTenMstInteractor>();
            busBuilder.RegisterUseCase<SaveSetNameMntInputData, SaveSetNameMntInteractor>();

            //SmartKartePort
            busBuilder.RegisterUseCase<UpdatePortInputData, UpdatePortInteractor>();
            busBuilder.RegisterUseCase<GetPortInputData, GetPortInteractor>();

            //PrescriptionHistory
            busBuilder.RegisterUseCase<GetSinrekiFilterMstListInputData, GetSinrekiFilterMstListInteractor>();
            busBuilder.RegisterUseCase<SaveSinrekiFilterMstListInputData, SaveSinrekiFilterMstListInteractor>();
            busBuilder.RegisterUseCase<GetContentDrugUsageHistoryInputData, GetContentDrugUsageHistoryInteractor>();

            //SystemStartDb 
            ///busBuilder.RegisterUseCase<SystemStartDbInputData, SystemStartDbInteractor>();

            var bus = busBuilder.Build();
            services.AddSingleton(bus);
        }
    }
}
