using Domain.Models.RsvGrpMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RsvGrpMstRepository : IRsvGrpMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RsvGrpMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<RsvGrpMstModel> GetList(int hpId)
    {
        var ptRsvGrpMsts = _tenantDataContext.RsvGrpMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0).Select(x => new RsvGrpMstModel(
               x.HpId,
               x.RsvGrpId,
               x.SortKey,
               x.RsvGrpName,
               x.IsDeleted
            ));
        return ptRsvGrpMsts.ToList();
    }
}
