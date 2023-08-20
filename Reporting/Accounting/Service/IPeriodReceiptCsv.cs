using Reporting.Mappers.Common;

namespace Reporting.Accounting.Service
{
    public interface IPeriodReceiptCsv
    {
        CommonExcelReportingModel GetPeriodReceiptCsv(int hpId, int startDate, int endDate,List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn);
    }
}
