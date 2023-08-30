using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;

namespace Reporting.DailyStatic.Service
{
    public interface IStaticsticExportCsvService
    {
        CommonExcelReportingModel ExportCsv(int hpId, string menuName, int menuId, int timeFrom, int timeTo, int? monthFrom = 0, int? monthTo = 0, int? dateFrom = 0, int? dateTo = 0, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0, bool? isPutColName = false, CoFileType? coFileType = null);
    }
}
