using Domain.Models.MonshinInf;
using Domain.Models.Santei;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SanteiRepository : ISanteiRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;
        private readonly TenantDataContext _tenantDataContextNoTracking;

        public SanteiRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<SanteiOrderDetailModel> CheckAutoAddOrderItem(int hpId, string itemCd, int sinDate)
        {
            var santeiGrpCds = _tenantDataContextNoTracking.SanteiGrpDetails.Where(s => s.HpId == hpId && s.ItemCd == itemCd).Select(s => s.SanteiGrpCd);
            var santeiAutoOrders = _tenantDataContextNoTracking.SanteiAutoOrders.Where(s => santeiGrpCds.Contains(s.SanteiGrpCd) && s.HpId == hpId && s.StartDate <= sinDate && s.EndDate >= sinDate && s.SpCondition == 0).Select(s => new Tuple<int, int> (s.AddType, s.SanteiGrpCd));
            var allSanteiAutoOrderDetails = new List<SanteiOrderDetailModel>();

            foreach (var item in santeiAutoOrders)
            {
                var santeiAutoOrderDetails = _tenantDataContextNoTracking.SanteiAutoOrderDetails.Where(s => item.Item2 == s.SanteiGrpCd && s.HpId == hpId && s.ItemCd == itemCd).Select(s => ConvertSanteiOrderDetailToModel(item.Item1, s));
                allSanteiAutoOrderDetails.AddRange(santeiAutoOrderDetails);
            }
         
            return allSanteiAutoOrderDetails;
        }

        private SanteiOrderDetailModel ConvertSanteiOrderDetailToModel(int addType, SanteiAutoOrderDetail santeiAutoOrderDetail)
        {
            return new SanteiOrderDetailModel(
                    santeiAutoOrderDetail.Id,
                    santeiAutoOrderDetail.HpId,
                    santeiAutoOrderDetail.SanteiGrpCd,
                    santeiAutoOrderDetail.SeqNo,
                    santeiAutoOrderDetail.ItemCd,
                    santeiAutoOrderDetail.Suryo,
                    addType
                );
        }

    }
}
