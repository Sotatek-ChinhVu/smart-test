using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3080.Models;

namespace Reporting.Statistics.Sta3080.Service;

public interface ISta3080CoReportService
{
    CommonReportingRequestModel GetSta3080ReportingData(CoSta3080PrintConf printConf, int hpId, CoFileType outputFileType);

    CommonExcelReportingModel ExportCsv(CoSta3080PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
}
