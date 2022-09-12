using Domain.Models.MstItem;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class MstItemRepository : IMstItemRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        private readonly TenantDataContext _tenantDataContextTracking;
        public MstItemRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<DosageDrugModel> GetDosages(List<string> yjCds)
        {
            var result = _tenantDataContext.DosageDrugs.Where(d => yjCds.Contains(d.YjCd));
            return result == null ? new List<DosageDrugModel>() : result.Select(
                    r => new DosageDrugModel(
                            r.YoukaiekiCd,
                            r.DoeiCd,
                            r.DgurKbn,
                            r.KikakiUnit,
                            r.YakkaiUnit,
                            r.RikikaRate,
                            r.RikikaUnit,
                            r.YoukaiekiCd
                   )).ToList();
        }
        public List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData()
        {
            List<FoodAlrgyKbnModel> m12FoodAlrgies = new List<FoodAlrgyKbnModel>();
            int i = 0;
            var aleFoodKbns = _tenantDataContext.M12FoodAlrgyKbn.Select(x => new FoodAlrgyKbnModel()
            {
                FoodKbn = x.FoodKbn,
                FoodName = x.FoodName,
                IsDrugAdditives = int.TryParse(x.FoodKbn, out i) && int.Parse(x.FoodKbn) > 50
            }).OrderBy(x=>x.FoodKbn).ToList();
            return aleFoodKbns;
        }
    }
}
