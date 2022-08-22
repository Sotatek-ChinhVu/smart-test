using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
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
                Text = text,
                CreateDate = DateTime.UtcNow,
                CreateId = TempIdentity.UserId,
                CreateMachine = TempIdentity.ComputerName
            });
        }
        else
        {
            ptCmt.Text = text;
            ptCmt.UpdateDate = DateTime.UtcNow;
            ptCmt.UpdateId = TempIdentity.UserId;
            ptCmt.UpdateMachine = TempIdentity.ComputerName;
        }

        _tenantDataContext.SaveChanges();
    }
}
