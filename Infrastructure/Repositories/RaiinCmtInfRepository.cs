using Domain.Models.RaiinCmtInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RaiinCmtInfRepository : RepositoryBase, IRaiinCmtInfRepository
{
    public RaiinCmtInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int cmtKbn, string text, int userId)
    {
        var raiinCmt = TrackingDataContext.RaiinCmtInfs.FirstOrDefault(r =>
            r.HpId == hpId
            && r.RaiinNo == raiinNo
            && r.CmtKbn == cmtKbn
            && r.IsDelete == DeleteTypes.None);
        if (raiinCmt is null)
        {
            // Insert
            TrackingDataContext.RaiinCmtInfs.Add(new RaiinCmtInf
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo,
                CmtKbn = cmtKbn,
                Text = text,
                UpdateDate = DateTime.UtcNow,
                UpdateId = userId,
                CreateDate = DateTime.UtcNow,
                CreateId = userId
            });
        }
        else
        {
            // Update
            raiinCmt.Text = text;
            raiinCmt.UpdateDate = DateTime.UtcNow;
            raiinCmt.UpdateId = userId;
        }

        TrackingDataContext.SaveChanges();
    }
}