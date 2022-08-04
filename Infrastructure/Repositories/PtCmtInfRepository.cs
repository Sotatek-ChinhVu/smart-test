using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtCmtInfRepository : IPtCmtInfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtCmtInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public void Upsert(long ptId, string text)
    {
        var ptCmt = _tenantDataContext.PtCmtInfs
            .OrderByDescending(p => p.UpdateDate)
            .FirstOrDefault(p => p.PtId == ptId);
        if (ptCmt is null)
        {
            _tenantDataContext.PtCmtInfs.Add(new PtCmtInf
            {
                HpId = 1,
                PtId = ptId,
                Text = text
            });
        }
        else
        {
            ptCmt.Text = text;
            ptCmt.UpdateDate = DateTime.UtcNow;
        }

        _tenantDataContext.SaveChanges();
    }
}
