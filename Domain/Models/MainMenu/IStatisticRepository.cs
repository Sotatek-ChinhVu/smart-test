using Domain.Common;

namespace Domain.Models.MainMenu;

public interface IStatisticRepository : IRepositoryBase
{
    List<StatisticMenuModel> GetStatisticMenu(int hpId, int grpId);

    List<StaGrpModel> GetStaGrp(int hpId, int grpId);

    (int menuIdTemp, bool success) SaveStatisticMenu(int hpId, int userId, List<StatisticMenuModel> statisticMenuModelList);

    bool SaveStaConfMenu(int hpId, int userId, StatisticMenuModel statisticMenu);

    List<StatisticMenuModel> GetStatisticMenuModels(int hpId);

    List<StaCsvMstModel> GetStaCsvMstModels(int hpId);

    void SaveStaCsvMst(int hpId, int userId, List<StaCsvMstModel> staCsvMstModels);
}
