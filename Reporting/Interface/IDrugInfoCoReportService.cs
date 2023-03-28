using Helper.Enum;
using Reporting.DrugInfo.Model;

namespace Reporting.Interface
{
    public interface IDrugInfoCoReportService
    {
        (ReportType, List<DrugInfoModel>) SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo);
    }
}
