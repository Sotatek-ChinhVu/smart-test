using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2002.Models;

namespace Reporting.Statistics.Sta2002.Service
{
    public interface ISta2002CoReportService
    {
        CommonReportingRequestModel GetSta2002ReportingData(CoSta2002PrintConf printConf, int hpId);
    }
}
