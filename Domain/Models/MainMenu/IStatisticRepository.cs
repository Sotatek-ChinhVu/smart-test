﻿using Domain.Common;

namespace Domain.Models.MainMenu;

public interface IStatisticRepository : IRepositoryBase
{
    List<StatisticMenuModel> GetStatisticMenu(int hpId, int grpId);

    List<StaGrpModel> GetStaGrp(int hpId, int grpId);

    (int menuIdTemp, bool success) SaveStatisticMenu(int hpId, int userId, List<StatisticMenuModel> statisticMenuModelList);

    List<StaCsvMstModel> GetStaCsvMstModels(int hpId);
}
