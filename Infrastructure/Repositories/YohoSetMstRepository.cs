using Domain.Constant;
using Domain.Models.OrdInfDetails;
using Domain.Models.YohoSetMst;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<YohoSetMstModel>> GetByItemCd(int hpId, string itemcd , int startDate)
        {
            var tenMst = await _tenantDataContext.TenMsts.FirstOrDefaultAsync(x => x.HpId == hpId && x.ItemCd.Equals(itemcd) && x.StartDate == startDate);
            if(tenMst is null)
                return Enumerable.Empty<YohoSetMstModel>();

            var yohoMsts = await _tenantDataContext.YohoSetMsts.Where(x => x.IsDeleted == DeleteStatus.None && x.ItemCd.Equals(itemcd)).ToListAsync();

            return yohoMsts.Select(x => new YohoSetMstModel(tenMst.Name ?? string.Empty, tenMst.YohoKbn, x.SetId, x.UserId, x.ItemCd));
        }
    }
}
