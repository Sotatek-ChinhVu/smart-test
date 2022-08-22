using Domain.Models.PtKioReki;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtKioReKiRepository : IPtKioRekiRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtKioReKiRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtKioRekiModel> GetList(long ptId)
    {
        var ptKioRekis = _tenantDataContext.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(p => p.SortNo).Select(x => new PtKioRekiModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd ?? String.Empty,
               x.ByotaiCd ?? String.Empty,
               x.Byomei ?? String.Empty,
               x.StartDate,
               x.Cmt ?? String.Empty,
               x.IsDeleted
            ));

        return ptKioRekis.ToList();
    }
}
