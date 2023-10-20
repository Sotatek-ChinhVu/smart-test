using Domain.Constant;
using Domain.Models.OrdInfDetails;
using Domain.Models.YohoSetMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class YohoSetMstRepository : RepositoryBase, IYohoSetMstRepository
    {
        public YohoSetMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public IEnumerable<YohoSetMstModel> GetByItemCd(int hpId, int userId, string itemcd, int startDate, int sinDate)
        {
            var tenMst = NoTrackingDataContext.TenMsts.FirstOrDefault(x => x.HpId == hpId && x.ItemCd.Equals(itemcd) && x.StartDate == startDate);
            if (tenMst is null)
                return Enumerable.Empty<YohoSetMstModel>();

            var yohoMsts = NoTrackingDataContext.YohoSetMsts.Where(x => x.HpId == hpId && x.IsDeleted == DeleteStatus.None && x.UserId == userId).ToList();

            var itemCdYohos = yohoMsts?.Select(od => od.ItemCd ?? string.Empty);

            var listTenMst = NoTrackingDataContext.TenMsts.Where(u => u.HpId == hpId &&
                                                                   u.IsNosearch == 0 &&
                                                                   u.StartDate <= sinDate &&
                                                                   u.EndDate >= sinDate &&
                                                                   u.IsDeleted == DeleteTypes.None &&
                                                                   u.SinKouiKbn == tenMst.SinKouiKbn &&
                                                                   (itemCdYohos != null && itemCdYohos.Contains(u.ItemCd))
                                                                   ).ToList();

            var query = from yoho in yohoMsts
                        join ten in listTenMst on yoho.ItemCd?.Trim() equals ten.ItemCd.Trim()
                        select new
                        {
                            Yoho = yoho,
                            ItemName = ten.Name,
                            ten.YohoKbn
                        };

            return query.OrderBy(u => u.Yoho.SortNo).AsEnumerable().Select(u => new YohoSetMstModel(u.ItemName, u.YohoKbn, u.Yoho?.SetId ?? 0, u.Yoho?.UserId ?? 0, u.Yoho?.ItemCd ?? string.Empty)).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
