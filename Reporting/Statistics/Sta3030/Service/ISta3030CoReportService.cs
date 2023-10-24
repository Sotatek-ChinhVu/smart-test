using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3030.Models;

namespace Reporting.Statistics.Sta3030.Service;

public interface ISta3030CoReportService
{
    CommonReportingRequestModel GetSta3030ReportingData(CoSta3030PrintConf printConf, int hpId, CoFileType outputFileType);

    CommonExcelReportingModel ExportCsv(CoSta3030PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
}
