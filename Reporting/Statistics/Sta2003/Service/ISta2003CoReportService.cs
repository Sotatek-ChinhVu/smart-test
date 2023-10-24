using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2003.Models;

namespace Reporting.Statistics.Sta2003.Service;

public interface ISta2003CoReportService
{
    CommonReportingRequestModel GetSta2003ReportingData(CoSta2003PrintConf printConf, int hpId);

    CommonExcelReportingModel ExportCsv(CoSta2003PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
}
