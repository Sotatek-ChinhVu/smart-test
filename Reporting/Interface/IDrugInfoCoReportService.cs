using Reporting.DrugInfo.Model;

namespace Reporting.Interface
{
    public interface IDrugInfoCoReportService
    {
        DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo);
    }
}
