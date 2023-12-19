using Reporting.Mappers.Common;

namespace Reporting.OutDrug.Service;

public interface IOutDrugCoReportService
{
    CommonReportingRequestModel GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo);
}
