using Reporting.OutDrug.Model.Output;

namespace Reporting.OutDrug.Service;

public interface IOutDrugCoReportService
{
    CoOutDrugReportingOutputData GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo);
}
