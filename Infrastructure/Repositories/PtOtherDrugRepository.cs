using Domain.Models.PtOtherDrug;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtOtherDrugRepository : IPtOtherDrugRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtOtherDrugRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtOtherDrugModel> GetList(long ptId)
    {
        var ptOtherDrugs = _tenantDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtherDrugModel(
              x.HpId,
              x.PtId,
              x.SeqNo,
              x.SortNo,
              x.ItemCd ?? String.Empty,
              x.DrugName ?? String.Empty,
              x.StartDate,
              x.EndDate,
              x.Cmt ?? String.Empty,
              x.IsDeleted
            ));
        return ptOtherDrugs.ToList();
    }
}
