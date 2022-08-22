using Domain.Models.PtAlrgyFood;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtAlrgyFoodRepository : IPtAlrgyFoodRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtAlrgyFoodRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtAlrgyFoodModel> GetList(long ptId)
    {
        var aleFoodKbns = _tenantDataContext.M12FoodAlrgyKbn.ToList();
        var ptAlrgyFoods = _tenantDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).ToList();
        var query = from ale in ptAlrgyFoods
                    join mst in aleFoodKbns on ale.AlrgyKbn equals mst.FoodKbn
                    select new PtAlrgyFoodModel
                    (
                          ale.HpId,
                          ale.PtId,
                          ale.SeqNo,
                          ale.SortNo,
                          ale.AlrgyKbn ?? String.Empty,
                          ale.StartDate,
                          ale.EndDate,
                          ale.Cmt ?? String.Empty,
                          ale.IsDeleted,
                          mst.FoodName
                    );

        return query.ToList();
    }
}
