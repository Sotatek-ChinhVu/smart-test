using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3020.Models;

namespace Reporting.Statistics.Sta3020.Service
{
    public interface ISta3020CoReportService
    {
        CommonReportingRequestModel GetSta3020ReportingData(CoSta3020PrintConf printConf, int hpId);
    }
}
