using Domain.Models.PtFamily;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtFamilyRepository : IPtFamilyRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtFamilyRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtFamilyModel> GetList(long ptId, int hpId)
    {
        var ptFamilys = _tenantDataContext.PtFamilys.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0).Select(x => new PtFamilyModel(
                x.FamilyId,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.ZokugaraCd,
                x.SortNo,
                x.ParentId,
                x.FamilyPtId,
                x.KanaName ?? String.Empty,
                x.Name ?? String.Empty,
                x.Sex,
                x.Birthday,
                x.IsDead,
                x.IsSeparated,
                x.Biko ?? String.Empty,
                x.IsDeleted,
                x.CreateDate,
                x.CreateId,
                x.CreateMachine ?? String.Empty,
                x.UpdateDate,
                x.UpdateId,
                x.UpdateMachine ?? String.Empty
            ));
        return ptFamilys.ToList();
    }
}
