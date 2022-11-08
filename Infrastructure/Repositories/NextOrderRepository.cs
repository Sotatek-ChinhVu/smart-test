using Domain.Models.NextOrder;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class NextOrderRepository : INextOrderRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        public NextOrderRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<NextOrderModel> GetList(int hpId, long ptId, int rsvkrtKbn, bool isDeleted)
        {
            var allRsvkrtMst = _tenantNoTrackingDataContext.RsvkrtMsts.Where(rsv => rsv.HpId == hpId && rsv.PtId == ptId && rsv.RsvkrtKbn == rsvkrtKbn && (isDeleted || rsv.IsDeleted == 0))?.AsEnumerable();

            return allRsvkrtMst?.Select(rsv => ConvertToModel(rsv)).ToList() ?? new List<NextOrderModel>();
        }

        private static NextOrderModel ConvertToModel(RsvkrtMst rsvkrtMst)
        {
            return new NextOrderModel(
                        rsvkrtMst.HpId,
                        rsvkrtMst.PtId,
                        rsvkrtMst.RsvkrtNo,
                        rsvkrtMst.RsvkrtKbn,
                        rsvkrtMst.RsvDate,
                        rsvkrtMst.RsvName ?? string.Empty,
                        rsvkrtMst.IsDeleted,
                        rsvkrtMst.SortNo
                   );
        }
    }
}
