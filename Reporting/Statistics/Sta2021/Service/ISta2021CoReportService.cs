using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta2021.Models;

namespace Reporting.Statistics.Sta2021.Service
{
    public interface ISta2021CoReportService
    {
        CommonReportingRequestModel GetSta2021ReportingData(CoSta2021PrintConf printConf, int hpId);

        CommonExcelReportingModel ExportCsv(CoSta2021PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
    }
}
