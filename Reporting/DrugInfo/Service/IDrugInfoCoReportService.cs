using Reporting.DrugInfo.Model;

namespace Reporting.DrugInfo.Service;

public interface IDrugInfoCoReportService
{
    DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo);

    void ReleaseResource();
}
