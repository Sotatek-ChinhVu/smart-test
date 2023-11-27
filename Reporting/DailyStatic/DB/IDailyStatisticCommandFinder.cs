using Domain.Common;
using Reporting.DailyStatic.Model;

namespace Reporting.DailyStatic.DB;

public interface IDailyStatisticCommandFinder : IRepositoryBase
{
    ConfigStatisticModel GetDailyConfigStatisticMenu(int hpId, int menuId);
}
