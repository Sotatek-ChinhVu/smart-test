using Domain.Models.RaiinFilterMst;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RaiinFilterMstRepository : IRaiinFilterMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RaiinFilterMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<RaiinFilterMstModel> GetList()
    {
        var query =
            from mst in _tenantDataContext.RaiinFilterMsts
            where mst.IsDeleted == DeleteTypes.None
            select new
            {
                mst,
                sorts = _tenantDataContext.RaiinFilterSorts
                    .Where(s => s.FilterId == mst.FilterId && s.IsDeleted == DeleteTypes.None)
                    .ToList()
            };

        var mstWithSorts = query.ToList();
        return mstWithSorts.Select(x => new RaiinFilterMstModel(
            x.mst.FilterId,
            x.mst.SortNo,
            x.mst.FilterName,
            x.mst.SelectKbn,
            x.mst.Shortcut,
            columnSortInfos: x.sorts.Select(s => new RaiinFilterSortModel(
                s.Id,
                s.FilterId,
                s.SeqNo,
                s.Priority,
                s.ColumnName,
                s.KbnCd,
                s.SortKbn
            )).ToList()
        )).ToList();
    }
}
