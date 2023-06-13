using Domain.Models.AccountDue;
using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.CommonMasters.Enums;
using Reporting.DrugInfo.Model;
using Reporting.Karte1.Mapper;
using Reporting.Mappers.Common;
using Reporting.OrderLabel.Model;
using Reporting.OutDrug.Model.Output;
using Reporting.ReceiptList.Model;

namespace Reporting.ReportServices;

public interface IReportService
{
    CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate);

    CommonReportingRequestModel GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);

    DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo);

    CommonReportingRequestModel GetByomeiReportingData(int hpId, long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds);

    CommonReportingRequestModel GetSijisenReportingData(int hpId, int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr);

    CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels);

    CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate);

    CommonReportingRequestModel GetReceiptCheckCoReportService(int hpId, List<long> ptIds, int seikyuYm);

    CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels);

    CoOutDrugReportingOutputData GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo);

    AccountingResponse GetAccountingReportingData(
               int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0,
               int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0,
               bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false,
               int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "");

    AccountingResponse GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
            int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn,
            int hakkoDay, string memo, string formFileName);

    AccountingResponse GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels);

    AccountingResponse GetAccountingData(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai);

    bool CheckOpenReportingForm(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai);

    AccountingResponse GetAccountingReportingData(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess = false);

    CommonReportingRequestModel GetStatisticReportingData(int hpId, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0);

    CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId);

    CommonReportingRequestModel GetPatientManagement(int hpId, int menuId);

    CommonReportingRequestModel GetSyojyoSyokiReportingData(int hpId, long ptId, int seikyuYm, int hokenId);

    CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd);

    CommonReportingRequestModel GetMemoMsgReportingData(string reportName, string title, List<string> listMessage);

    CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt);

    CommonReportingRequestModel GetReceTargetPrint(int hpId, int seikyuYm);

    CommonReportingRequestModel GetDrugNoteSealPrintData(int hpId, long ptId, int sinDate, long raiinNo);

    CommonReportingRequestModel GetYakutaiReportingData(int hpId, long ptId, int sinDate, int raiinNo);

    CommonReportingRequestModel GetAccountingCardReportingData(int hpId, long ptId, int sinYm, int hokenId, bool includeOutDrug);
}
