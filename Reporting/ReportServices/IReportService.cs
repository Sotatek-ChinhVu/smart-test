using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.DrugInfo.Model;
using Reporting.Karte1.Mapper;
using Reporting.Mappers.Common;
using Reporting.OrderLabel.Model;
using Reporting.ReceiptList.Model;
using Reporting.OutDrug.Model.Output;

namespace Reporting.ReportServices;

public interface IReportService
{
    CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate);

    Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);

    DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo);

    CommonReportingRequestModel GetByomeiReportingData(long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds);

    CommonReportingRequestModel GetSijisenReportingData(int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr);

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

    AccountingResponse GetAccountingReportingData(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess = false);
}
