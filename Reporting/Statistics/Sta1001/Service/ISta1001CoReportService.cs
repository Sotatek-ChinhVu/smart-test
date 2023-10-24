using Reporting.Mappers.Common;
using Reporting.Statistics.Sta1001.Models;

namespace Reporting.Statistics.Sta1001.Service;

public interface ISta1001CoReportService
{
    CommonReportingRequestModel GetSta1001ReportingData(CoSta1001PrintConf printConf, int hpId);

    CommonExcelReportingModel ExportCsv(CoSta1001PrintConf printConf, int dateFrom, int dateTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow);
}
