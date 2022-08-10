using Domain.Models.PtCmtInf;
using Domain.Models.PtKioReki;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtKioReKiRepository : IPtKioRekiRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtKioReKiRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtKioRekiModel> GetList(long ptId)
    {
        var ptKioRekis = _tenantDataContext.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtKioRekiModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd,
               x.ByotaiCd,
               x.Byomei,
               x.StartDate,
               x.Cmt,
               x.IsDeleted
            )).OrderBy(p => p.SortNo);

        return ptKioRekis.ToList();
    }
}
