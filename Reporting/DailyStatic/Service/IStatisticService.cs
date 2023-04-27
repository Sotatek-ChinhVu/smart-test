using Reporting.Mappers.Common;

namespace Reporting.DailyStatic.Service;

public interface IStatisticService
{
    CommonReportingRequestModel PrintExecute(int hpId, int menuId, int dateFrom, int dateTo, int timeFrom, int timeTo);
}
