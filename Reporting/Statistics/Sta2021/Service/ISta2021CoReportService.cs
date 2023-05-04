using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2021.Models;

namespace Reporting.Statistics.Sta2021.Service
{
    public interface ISta2021CoReportService
    {
        CommonReportingRequestModel GetSta2021ReportingData(CoSta2021PrintConf printConf, int hpId);
    }
}
