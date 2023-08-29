using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Statistics.Sta3071.Models;

namespace Reporting.Statistics.Sta3071.Service;

public interface ISta3071CoReportService
{
    CommonReportingRequestModel GetSta3071ReportingData(CoSta3071PrintConf printConf, int hpId, CoFileType outputFileType, bool isPutTotalRow);

    CommonExcelReportingModel ExportCsv(CoSta3071PrintConf printConf, int hpId, bool isPutTotalRow, bool isPutColName, int monthFrom, int monthTo, string menuName, CoFileType? coFileType);
}