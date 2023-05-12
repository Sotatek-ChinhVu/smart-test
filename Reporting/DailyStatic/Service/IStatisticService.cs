using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;

namespace Reporting.DailyStatic.Service;

public interface IStatisticService
{
    CommonReportingRequestModel PrintExecute(int hpId, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1);
}
