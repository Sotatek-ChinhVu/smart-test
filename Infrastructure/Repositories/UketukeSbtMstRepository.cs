using Domain.Models.UketukeSbtMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UketukeSbtMstRepository : RepositoryBase, IUketukeSbtMstRepository
{
    public UketukeSbtMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public UketukeSbtMstModel? GetByKbnId(int kbnId)
    {
        var entity = NoTrackingDataContext.UketukeSbtMsts.Where(u => u.KbnId == kbnId && u.IsDeleted == DeleteTypes.None).FirstOrDefault();
        return entity is null ? null : ToModel(entity);
    }

    public List<UketukeSbtMstModel> GetList()
    {
        return NoTrackingDataContext.UketukeSbtMsts
            .Where(u => u.IsDeleted == DeleteTypes.None)
            .OrderBy(u => u.SortNo).AsEnumerable()
            .Select(u => ToModel(u)).ToList();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    private UketukeSbtMstModel ToModel(UketukeSbtMst u)
    {
        return new UketukeSbtMstModel(
            u.KbnId,
            u.KbnName ?? string.Empty,
            u.SortNo,
            u.IsDeleted);
    }
}
