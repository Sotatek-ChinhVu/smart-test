using Domain.Models.PtCmtInf;
using Domain.Models.PtOtherDrug;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtOtherDrugRepository : IPtOtherDrugRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtOtherDrugRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtOtherDrugModel> GetList(long ptId)
    {
        var ptOtherDrugs = _tenantDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtherDrugModel(
              x.HpId,
              x.PtId,
              x.SeqNo,
              x.SortNo,
              x.ItemCd,
              x.DrugName,
              x.StartDate,
              x.EndDate,
              x.Cmt,
              x.IsDeleted
            ));
        return ptOtherDrugs.ToList();
    }
}
