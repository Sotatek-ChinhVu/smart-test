using Domain.Models.PtCmtInf;
using Domain.Models.SeikaturekiInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class SeikaturekiInfRepository : ISeikaturekiInfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public SeikaturekiInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<SeikaturekiInfModel> GetList(long ptId, int hpId)
    {
        var seikaturekiInfs = _tenantDataContext.PtCmtInfs.Where(x => x.PtId == ptId && x.HpId == hpId).Select(x => new SeikaturekiInfModel(
                x.Id,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.Text
            ));
        return seikaturekiInfs.ToList();
    }
}
