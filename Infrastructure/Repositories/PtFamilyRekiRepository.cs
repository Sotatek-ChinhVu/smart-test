using Domain.Models.PtFamily;
using Domain.Models.PtFamilyReki;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtFamilyRekiRepository : IPtFamilyRekiRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtFamilyRekiRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtFamilyRekiModel> GetList(int hpId)
    {
        var ptRekis = _tenantDataContext.PtFamilyRekis.Where(x=> x.HpId == hpId && x.IsDeleted == 0 && !string.IsNullOrEmpty(x.Byomei)).Select(x => new PtFamilyRekiModel(
               x.Id,
               x.HpId,
               x.PtId,
               x.FamilyId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd ?? String.Empty,
               x.ByotaiCd ?? String.Empty,
               x.Byomei ?? String.Empty,
               x.Cmt ?? String.Empty,
               x.IsDeleted,
               x.CreateDate,
               x.CreateId,
               x.CreateMachine ?? String.Empty,
               x.UpdateDate,
               x.UpdateId,
               x.UpdateMachine ?? String.Empty
            ));
        return ptRekis.ToList();
    }
}
