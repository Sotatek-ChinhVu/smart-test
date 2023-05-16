using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3070.Models;

namespace Reporting.Statistics.Sta3070.Service;

public interface ISta3070CoReportService
{
    CommonReportingRequestModel GetSta3070ReportingData(CoSta3070PrintConf printConf, int hpId, CoFileType outputFileType);
}
