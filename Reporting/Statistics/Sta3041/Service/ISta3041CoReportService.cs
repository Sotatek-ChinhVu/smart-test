using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3041.Models;

namespace Reporting.Statistics.Sta3041.Service;

public interface ISta3041CoReportService
{
    CommonReportingRequestModel GetSta3041ReportingData(CoSta3041PrintConf printConf, int hpId, CoFileType outputFileType);
}
