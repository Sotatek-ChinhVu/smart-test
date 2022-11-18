using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtCmtInfRepository : IPtCmtInfRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtCmtInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtCmtInfModel> GetList(long ptId, int hpId)
    {
        var ptCmts = _tenantDataContext.PtCmtInfs.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0).OrderByDescending(p => p.UpdateDate)
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

    public void Upsert(long ptId, string text, int userId)
    {
        var ptCmtList = _tenantDataContext.PtCmtInfs.AsTracking()
            .Where(p => p.PtId == ptId && p.IsDeleted != 1)
            .ToList();

        if (ptCmtList.Count != 1)
        {
            foreach (var ptCmt in ptCmtList)
            {
                ptCmt.IsDeleted = 1;
            }

            _tenantDataContext.PtCmtInfs.Add(new PtCmtInf
            {
                HpId = 1,
                PtId = ptId,
                Text = text,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                UpdateId = userId,
                CreateId = userId
            });
        }
        else
        {
            var  ptCmt = ptCmtList[0];

            ptCmt.Text = text;
            ptCmt.UpdateDate = DateTime.UtcNow;
            ptCmt.UpdateId = userId;
        }

        _tenantDataContext.SaveChanges();
    }
}
