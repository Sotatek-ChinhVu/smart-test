using Reporting.Mappers.Common;

namespace Reporting.InDrug.Service
{
    public interface IInDrugCoReportService
    {
        CommonReportingRequestModel GetInDrugPrintData(int hpId, long ptId, int sinDate, long raiinNo);
    }
}
