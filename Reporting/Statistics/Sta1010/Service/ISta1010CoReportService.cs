using Reporting.Mappers.Common;
using Reporting.Statistics.Sta1010.Models;

namespace Reporting.Statistics.Sta1010.Service;

public interface ISta1010CoReportService
{
    CommonReportingRequestModel GetSta1010ReportingData(CoSta1010PrintConf printConf, int hpId);
}
