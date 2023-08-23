using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;

namespace Reporting.DailyStatic.Service
{
    public interface IStaticsticExportCsvService
    {
        CommonExcelReportingModel ExportCsv(int hpId, string formName, string menuName, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0, bool? isPutColName = false);
    }
}
