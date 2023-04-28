using Reporting.Mappers.Common;

namespace Reporting.DailyStatic.Service;

public interface IStatisticService
{
    CommonReportingRequestModel PrintExecute(int hpId, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo);
}
