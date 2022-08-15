using Domain.Models.SeikaturekiInf;
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
        var seikaturekiInfs = _tenantDataContext.SeikaturekiInfs.Where(x => x.PtId == ptId && x.HpId == hpId).OrderByDescending(x => x.UpdateDate).Select(x => new SeikaturekiInfModel(
                x.Id,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.Text ?? String.Empty
            ));
        return seikaturekiInfs.ToList();
    }
}
