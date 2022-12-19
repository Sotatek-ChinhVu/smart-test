using Domain.Constant;
using Domain.Models.OrdInfDetails;
using Domain.Models.YohoSetMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class YohoSetMstRepository : IYohoSetMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public YohoSetMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<YohoSetMstModel> GetByItemCd(int hpId, string itemcd, int startDate)
        {
            var tenMst = _tenantDataContext.TenMsts.FirstOrDefault(x => x.HpId == hpId && x.ItemCd.Equals(itemcd) && x.StartDate == startDate);
            if (tenMst is null)
                return Enumerable.Empty<YohoSetMstModel>();

            var yohoMsts = _tenantDataContext.YohoSetMsts.Where(x => x.IsDeleted == DeleteStatus.None && (x.ItemCd != null && x.ItemCd.Equals(itemcd))).ToList();

            return yohoMsts.Select(x => new YohoSetMstModel(tenMst.Name ?? string.Empty, tenMst.YohoKbn, x.SetId, x.UserId, x.ItemCd ?? string.Empty));
        }
    }
}
