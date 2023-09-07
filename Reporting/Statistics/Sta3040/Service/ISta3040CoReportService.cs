using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3040.Models;

namespace Reporting.Statistics.Sta3040.Service;

public interface ISta3040CoReportService
{
    CommonReportingRequestModel GetSta3040ReportingData(CoSta3040PrintConf printConf, int hpId, CoFileType outputFileType);

    CommonExcelReportingModel ExportCsv(CoSta3040PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType);
}
