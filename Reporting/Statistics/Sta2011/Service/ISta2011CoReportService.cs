using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2011.Models;

namespace Reporting.Statistics.Sta2011.Service
{
    public interface ISta2011CoReportService
    {
        CommonReportingRequestModel GetSta2011ReportingData(CoSta2011PrintConf printConf, int hpId);
    }
}
