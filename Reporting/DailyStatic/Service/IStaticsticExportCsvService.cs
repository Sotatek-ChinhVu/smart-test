using Reporting.CommonMasters.Enums;

namespace Reporting.DailyStatic.Service
{
    public interface IStaticsticExportCsvService
    {
        List<string> ExportCsv(int hpId, string formName, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0);
    }
}
