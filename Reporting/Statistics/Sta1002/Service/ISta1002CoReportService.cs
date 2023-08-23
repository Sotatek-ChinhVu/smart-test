using Reporting.Mappers.Common;
using Reporting.Statistics.Sta1002.Models;

namespace Reporting.Statistics.Sta1002.Service;

public interface ISta1002CoReportService
{
    CommonReportingRequestModel GetSta1002ReportingData(CoSta1002PrintConf printConf, int hpId);

    CommonExcelReportingModel ExportCsv(CoSta1002PrintConf printConf, int dateFrom, int dateTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow);
}
