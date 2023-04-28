using Domain.Common;

namespace Domain.Models.MainMenu;

public interface IStatisticRepository : IRepositoryBase
{
    List<StatisticMenuModel> GetDailyStatisticMenu(int hpId, int groupId);
}
