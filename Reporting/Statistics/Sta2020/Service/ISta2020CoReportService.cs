using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2020.Models;

namespace Reporting.Statistics.Sta2020.Service
{
    public interface ISta2020CoReportService
    {
        CommonReportingRequestModel GetSta2020ReportingData(CoSta2020PrintConf printConf, int hpId);

        CommonExcelReportingModel ExportCsv(CoSta2020PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow);
    }
}
