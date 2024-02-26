using Reporting.Mappers.Common;

namespace Reporting.Yakutai.Service
{
    public interface IYakutaiCoReportService
    {
        CommonReportingRequestModel GetYakutaiReportingData(int hpId, long ptId, int sinDate, long raiinNo);
    }
}
