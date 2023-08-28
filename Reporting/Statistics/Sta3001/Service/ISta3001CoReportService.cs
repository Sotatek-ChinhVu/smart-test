using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3001.Models;

namespace Reporting.Statistics.Sta3001.Service
{
    public interface ISta3001CoReportService
    {
        CommonReportingRequestModel GetSta3001ReportingData(CoSta3001PrintConf printConf, int hpId);

        CommonExcelReportingModel ExportCsv(CoSta3001PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
    }
}
