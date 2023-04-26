using Reporting.DailyStatic.Model;

namespace Reporting.DailyStatic.DB;

public interface IDailyStatisticCommandFinder
{
    ConfigStatisticModel GetDailyConfigStatisticMenu(int hpId, int menuId);
}
