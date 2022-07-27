using Domain.Models.RaiinCmtInf;
using Entity.Tenant;
using Infrastructure.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RaiinCmtInfRepository : IRaiinCmtInfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RaiinCmtInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetDataContext();
    }

    public void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int cmtKbn, string text)
    {
        var raiinCmt = _tenantDataContext.RaiinCmtInfs.FirstOrDefault(r =>
            r.HpId == hpId
            && r.RaiinNo == raiinNo
            && r.CmtKbn == cmtKbn
            && r.IsDelete == DeleteTypes.None);
        if (raiinCmt is null)
        {
            // Insert
            _tenantDataContext.RaiinCmtInfs.Add(new RaiinCmtInf
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo,
                CmtKbn = cmtKbn,
                Text = text
            });
        }
        else
        {
            // Update
            raiinCmt.Text = text;
            raiinCmt.UpdateDate = DateTime.UtcNow;
        }

        _tenantDataContext.SaveChanges();
    }
}