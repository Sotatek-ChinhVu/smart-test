using Domain.Models.RaiinCmtInf;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

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
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId
            });
        }
        else
        {
            // Update
            raiinCmt.Text = text;
            raiinCmt.UpdateDate = CIUtil.GetJapanDateTimeNow();
            raiinCmt.UpdateId = userId;
        }

        TrackingDataContext.SaveChanges();
    }
}