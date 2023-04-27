using Reporting.Mappers.Common;

namespace Reporting.Statistics.Sta1001.Service
{
    public interface ISta1001CoReportService
    {
        CommonReportingRequestModel GetSta1001ReportingData(int hpId, int menuId, int dateFrom, int dateTo, int timeFrom, int timeTo);
    }
}
