using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2010.Models;

namespace Reporting.Statistics.Sta2010.Service
{
    public interface ISta2010CoReportService
    {
        CommonReportingRequestModel GetSta2010ReportingData(CoSta2010PrintConf printConf, int hpId);
    }
}
