using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta1010.Models;

namespace Reporting.Statistics.Sta1010.Service;

public interface ISta1010CoReportService
{
    CommonReportingRequestModel GetSta1010ReportingData(CoSta1010PrintConf printConf, int hpId);

    CommonExcelReportingModel ExportCsv(CoSta1010PrintConf printConf, int dateFrom, int dateTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
}
