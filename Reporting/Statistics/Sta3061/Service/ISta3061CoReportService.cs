using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3061.Models;

namespace Reporting.Statistics.Sta3061.Service;

public interface ISta3061CoReportService
{
    CommonReportingRequestModel GetSta3061ReportingData(CoSta3061PrintConf printConf, int hpId);
}
