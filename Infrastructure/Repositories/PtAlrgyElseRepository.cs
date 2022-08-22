using Domain.Models.PtAlrgyElse;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtAlrgyElseRepository : IPtAlrgyElseRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtAlrgyElseRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtAlrgyElseModel> GetList(long ptId)
    {
        var ptAlrgyElses = _tenantDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyElseModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.AlrgyName ?? String.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? String.Empty,
                x.IsDeleted
            ));
        return ptAlrgyElses.ToList();
    }
}
