using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3010.Models;

namespace Reporting.Statistics.Sta3010.Service;

public interface ISta3010CoReportService
{
    CommonReportingRequestModel GetSta3010ReportingData(CoSta3010PrintConf printConf, int hpId, CoFileType outputFileType);
}
