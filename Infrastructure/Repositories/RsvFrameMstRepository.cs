using Domain.Models.PtCmtInf;
using Domain.Models.PtInfection;
using Domain.Models.RsvFrameMst;
using Domain.Models.RsvInfo;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RsvFrameMstRepository : IRsvFrameMstRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RsvFrameMstRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<RsvFrameMstModel> GetList(int hpId)
    {
        var ptRsvFrameMsts = _tenantDataContext.RsvFrameMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0).Select(x => new RsvFrameMstModel(
                x.HpId,
                x.RsvFrameId,
                x.RsvGrpId,
                x.SortKey,
                x.RsvFrameName ?? String.Empty,
                x.TantoId,
                x.KaId,
                x.MakeRaiin,
                x.IsDeleted
            ));
        return ptRsvFrameMsts.ToList();
    }
}
