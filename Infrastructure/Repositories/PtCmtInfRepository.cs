using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PtCmtInfRepository : RepositoryBase, IPtCmtInfRepository
{
    public PtCmtInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<PtCmtInfModel> GetList(long ptId, int hpId)
    {
        var ptCmts = NoTrackingDataContext.PtCmtInfs.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0).OrderByDescending(p => p.UpdateDate)
            .Select(x => new PtCmtInfModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.Text ?? String.Empty,
                x.IsDeleted,
                x.Id
            ));

        return ptCmts.ToList();
    }

    public PtCmtInfModel GetPtCmtInfo(int hpId, long ptId)
    {
        var result = NoTrackingDataContext.PtCmtInfs
                           .Where(u => u.HpId == hpId && u.PtId == ptId && u.IsDeleted == 0)
                           .OrderByDescending(u => u.UpdateDate)
                           .AsEnumerable()
                           .Select(u => new PtCmtInfModel(
                                u.HpId,
                                u.PtId,
                                u.SeqNo,
                                u.Text ?? string.Empty,
                                u.IsDeleted,
                                u.Id
                               ))
                           .FirstOrDefault();
        return result ?? new PtCmtInfModel();
    }

    public void Upsert(long ptId, string text, int userId)
    {
        var ptCmtList = TrackingDataContext.PtCmtInfs.AsTracking()
            .Where(p => p.PtId == ptId && p.IsDeleted != 1)
            .ToList();

        if (ptCmtList.Count != 1)
        {
            foreach (var ptCmt in ptCmtList)
            {
                ptCmt.IsDeleted = 1;
            }

            TrackingDataContext.PtCmtInfs.Add(new PtCmtInf
            {
                HpId = 1,
                PtId = ptId,
                Text = text,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
                CreateId = userId
            });
        }
        else
        {
            var ptCmt = ptCmtList[0];

            ptCmt.Text = text;
            ptCmt.UpdateDate = CIUtil.GetJapanDateTimeNow();
            ptCmt.UpdateId = userId;
        }

        TrackingDataContext.SaveChanges();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
