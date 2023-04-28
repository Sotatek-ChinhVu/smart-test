using Domain.Models.MainMenu;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class StatisticRepository : RepositoryBase, IStatisticRepository
{
    public StatisticRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<StatisticMenuModel> GetDailyStatisticMenu(int hpId, int groupId)
    {
        var result = NoTrackingDataContext.StaMenus.Where(item => item.HpId == hpId
                                                                  && (groupId == 0 || item.GrpId == groupId)
                                                                  && item.IsDeleted == 0)
                                                   .OrderBy(item => item.SortNo)
                                                   .Select(item => new StatisticMenuModel(
                                                                       item.MenuId,
                                                                       item.GrpId,
                                                                       item.ReportId,
                                                                       item.SortNo,
                                                                       item.MenuName ?? string.Empty,
                                                                       item.IsPrint))
                                                   .ToList();

        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
