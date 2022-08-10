using Domain.Models.PtAlrgyElse;
using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtAlrgyElseRepository : IPtAlrgyElseRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtAlrgyElseRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtAlrgyElseModel> GetList(long ptId)
    {
        var ptAlrgyElses = _tenantDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyElseModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.AlrgyName,
                x.StartDate,
                x.EndDate,
                x.Cmt,
                x.IsDeleted
            ));
        return ptAlrgyElses.ToList();
    }
}
