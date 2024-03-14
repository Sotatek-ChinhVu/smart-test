using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3062.Models;

namespace Reporting.Statistics.Sta3062.Service;

public interface ISta3062CoReportService
{
    CommonReportingRequestModel GetSta3062ReportingData(CoSta3062PrintConf printConf, int hpId);

    CommonExcelReportingModel ExportCsv(CoSta3062PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
}
