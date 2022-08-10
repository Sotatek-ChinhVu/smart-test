using Domain.Models.PtCmtInf;
using Domain.Models.PtKioReKi;
using Domain.Models.PtSupple;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtSuppleRepository : IPtSuppleRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtSuppleRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtSuppleModel> GetList(long ptId)
    {
        var ptKioRekis = _tenantDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtSuppleModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.IndexCd,
                x.IndexWord,
                x.StartDate,
                x.EndDate,
                x.Cmt,
                x.IsDeleted
            ));
        return ptKioRekis.ToList();
    }
}
