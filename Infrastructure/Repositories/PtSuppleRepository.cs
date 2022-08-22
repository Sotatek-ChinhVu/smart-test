using Domain.Models.PtSupple;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtSuppleRepository : IPtSuppleRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtSuppleRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtSuppleModel> GetList(long ptId)
    {
        var ptSupples = _tenantDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtSuppleModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.IndexCd ?? String.Empty,
                x.IndexWord ?? String.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? String.Empty,
                x.IsDeleted
            ));
        return ptSupples.ToList();
    }
}
