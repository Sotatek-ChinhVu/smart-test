using Domain.Models.PtAlrgyElse;
using Domain.Models.PtAlrgyFood;
using Domain.Models.PtCmtInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtAlrgyFoodRepository : IPtAlrgyFoodRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtAlrgyFoodRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtAlrgyFoodModel> GetList(long ptId)
    {
        var ptAlrgyFoods = _tenantDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyFoodModel(
              x.HpId,
              x.PtId,
              x.SeqNo,
              x.SortNo,
              x.AlrgyKbn ?? String.Empty,
              x.StartDate,
              x.EndDate,
              x.Cmt ?? String.Empty,
              x.IsDeleted
            ));
        return ptAlrgyFoods.ToList();
    }
}
