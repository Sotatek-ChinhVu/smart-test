using Amazon.Runtime.Internal;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.AccountingCardList.Model;
using Reporting.CommonMasters.Enums;
using Reporting.DrugInfo.Model;
using Reporting.GrowthCurve.Model;
using Reporting.KensaLabel.Model;
using Reporting.Mappers.Common;
using Reporting.OrderLabel.Model;
using Reporting.OutDrug.Model.Output;
using Reporting.PatientManagement.Models;
using Reporting.ReceiptList.Model;
using Reporting.ReceiptPrint.Service;
using Reporting.Statistics.Sta9000.Models;

namespace Reporting.ReportServices;

public interface IReportService
{
    CommonReportingRequestModel GetNameLabelReportingData(int hpId, long ptId, string kanjiName, int sinDate);

    CommonReportingRequestModel GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);

    DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo);

    CommonReportingRequestModel GetByomeiReportingData(int hpId, long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds);

    CommonReportingRequestModel GetSijisenReportingData(int hpId, int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr);

    CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels);

    CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate);

    CommonReportingRequestModel GetReceiptCheckCoReportService(int hpId, List<long> ptIds, int seikyuYm);

    CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels);

    CommonReportingRequestModel GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo);

    AccountingResponse GetAccountingReportingData(
               int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0,
               int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0,
               bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false,
               int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "");

    AccountingResponse GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
            int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn,
            int hakkoDay, string memo, string formFileName);

    AccountingResponse GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels);

    AccountingResponse GetAccountingReportingData(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess = false);

    AccountingResponse GetAccountingData(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai);

    CommonReportingRequestModel GetStatisticReportingData(int hpId, string formName, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0);

    CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, int seikyuYm, int hokenKbn, bool isIncludeOutDrug, bool isModePrint, bool isOpenedFromAccounting);

    CommonReportingRequestModel GetPatientManagement(int hpId, PatientManagementModel patientManagementModel);

    CommonReportingRequestModel GetSyojyoSyokiReportingData(int hpId, long ptId, int seikyuYm, int hokenId);

    CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd);

    CommonReportingRequestModel GetMemoMsgReportingData(string reportName, string title, List<string> listMessage);

    CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, long ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, int hokenKbn, ReseputoShubetsuModel selectedReseputoShubeusu, int departmentId, int doctorId, int printNoFrom, int printNoTo, bool includeTester, bool includeOutDrug, int sort, List<long> printPtIds);

    CommonReportingRequestModel GetReceTargetPrint(int hpId, int seikyuYm);

    CommonReportingRequestModel GetDrugNoteSealPrintData(int hpId, long ptId, int sinDate, long raiinNo);
    CommonReportingRequestModel GetKensaHistoryPrint(int hpId, int userId, long ptId, int setId, int iraiDate, int startDate, int endDate, bool showAbnormalKbn, int sinDate);
    CommonReportingRequestModel GetKensaResultMultiPrint(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn, int sinDate);
    CommonReportingRequestModel GetInDrugPrintData(int hpId, long ptId, int sinDate, long raiinNo);
    CommonReportingRequestModel GetGrowthCurveA4PrintData(int hpId, GrowthCurveConfig growthCurveConfig);
    CommonReportingRequestModel GetGrowthCurveA5PrintData(int hpId, GrowthCurveConfig growthCurveConfig);
    CommonReportingRequestModel GetYakutaiReportingData(int hpId, long ptId, int sinDate, long raiinNo);
    CommonReportingRequestModel GetKensaLabelPrintData(int hpId, long ptId, long raiinNo, int sinDate, KensaPrinterModel printerModel);
    CommonReportingRequestModel GetAccountingCardReportingData(int hpId, long ptId, int sinYm, int hokenId, bool includeOutDrug);
    List<CoAccountingParamModel> PrintWithoutThread(bool ryoshusho, bool meisai, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> accountDueListModels, CoAccountDueListModel selectedAccountDueListModel, bool isPrintMonth, int sinDate, long oyaRaiinNo, List<CoAccountDueListModel>? nyukinModels = null);
    CommonReportingRequestModel GetKarte3ReportingData(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi);
    CommonReportingRequestModel GetAccountingCardListReportingData(int hpId, List<TargetItem> targets, bool includeOutDrug, string kaName, string tantoName, string uketukeSbt, string hoken);
    CommonExcelReportingModel GetReceiptPrintExcel(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int seikyuYm);
    CommonExcelReportingModel GetReceiptListExcel(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput receiptListModel, bool isIsExportTitle);
    AccountingResponse GetPeriodPrintData(int hpId, int startDate, int endDate, List<PtInfInputItem> sourcePt, List<(int grpId, string grpCd)> grpConditions, int printSort, bool isPrintList, bool printByMonth, bool printByGroup, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, int hakkoDay, string memo, string formFileName, bool nyukinBase);
    List<string> OutputExcelForPeriodReceipt(int hpId, int startDate, int endDate, List<Tuple<long, int>> ptConditions, List<Tuple<int, string>> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn);
    CommonExcelReportingModel ExportCsv(int hpId, string menuName, int menuId, int timeFrom, int timeTo, int? monthFrom = 0, int? monthTo = 0, int? dateFrom = 0, int? dateTo = 0, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0, bool? isPutColName = false, CoFileType? coFileType = null);

    (string message, CoPrintExitCode code, List<string> data) OutPutFileSta900(int hpId, List<string> outputColumns, bool isPutColName, CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf, CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf, CoSta9000KensaConf? kensaConf, List<long> ptIds, int sortOrder, int sortOrder2, int sortOrder3);

    void ReleaseResource();
    void Instance(int service);
}
