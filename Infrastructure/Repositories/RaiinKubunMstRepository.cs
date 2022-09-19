using Domain.Models.RaiinKubunMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class RaiinKubunMstRepository : IRaiinKubunMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public RaiinKubunMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<RaiinKubunMstModel> GetList(bool isDeleted)
        {
            var raiinKubunMstList = _tenantDataContext.RaiinKbnMsts
               .Where(r => isDeleted || r.IsDeleted == 0);
               
            var query = from groupId in raiinKubunMstList
                        join detail in _tenantDataContext.RaiinKbnDetails
                         .Where(r => isDeleted || r.IsDeleted == 0) on groupId.GrpCd equals detail.GrpCd
                        join _tenantDataContext.RaiinKbnDetails
                         .Where(r => isDeleted || r.IsDeleted == 0) on groupId.GrpCd equals detail.GrpCd
            //List<int> groupIdList = raiinKubunMstList.Select(r => r.GrpCd).ToList();

                        //List<RaiinKubunDetailModel> raiinKubunDetailList = _tenantDataContext.RaiinKbnDetails
                        //    .Where(r => groupIdList.Contains(r.GrpCd) && (isDeleted || r.IsDeleted == 0))
                        //    .Select(x => new RaiinKubunDetailModel(x.GrpCd, x.KbnCd, x.SortNo, x.KbnName, x.ColorCd ?? String.Empty, x.IsConfirmed == 1, x.IsAuto == 1, x.IsAutoDelete == 1, x.IsDeleted == 1))
                        //    .ToList();

            List<RaiinKbnKouiModel> raiinKbnKouiList = _tenantDataContext.RaiinKbnKouis
                .Where(r => groupIdList.Contains(r.GrpId) && (isDeleted || r.IsDeleted == 0))
                .Select(x => new RaiinKbnKouiModel(x.GrpId, x.KbnCd, x.SeqNo, x.KouiKbnId, x.IsDeleted))
                .ToList();

            List<RaiinKbnItemModel> raiinKbnItemList = _tenantDataContext.RaiinKbItems
                .Where(r => groupIdList.Contains(r.GrpCd) && (isDeleted || r.IsDeleted == 0))
                .Select(x => new RaiinKbnItemModel(x.GrpCd, x.KbnCd, x.SeqNo, x.ItemCd, x.IsExclude, x.IsDeleted))
                .ToList();

            List<RsvGrpMstModel> rsvGrpMstList = _tenantDataContext.RsvGrpMsts
                .Where(r => isDeleted || r.IsDeleted == 0)
                .Select(x => new RsvGrpMstModel(x.RsvGrpId, x.SortKey, x.RsvGrpName, x.IsDeleted))
                .ToList();

            List<RsvFrameMstModel> rsvFrameMstList = _tenantDataContext.RsvFrameMsts
                .Where(r => isDeleted || r.IsDeleted == 0)
                .Select(x => new RsvFrameMstModel(x.RsvGrpId, x.RsvFrameId, x.SortKey, x.RsvFrameName ?? String.Empty, x.TantoId, x.KaId, x.MakeRaiin, x.IsDeleted))
                .ToList();

            List<RaiinKbnYayokuModel> raiinKbnYayokuList = _tenantDataContext.RaiinKbnYayokus
                .Where(r => groupIdList.Contains(r.GrpId) && (isDeleted || r.IsDeleted == 0))
                .Select(x => new RaiinKbnYayokuModel(x.KbnCd, x.SeqNo, x.YoyakuCd, x.IsDeleted))
                .ToList();

            return raiinKubunMstList
                        .Select(x => new RaiinKubunMstModel
                        (x.GrpCd,
                        x.SortNo,
                        x.GrpName,
                        x.IsDeleted == 1,
                        raiinKubunDetailList,
                        raiinKbnKouiList,
                        raiinKbnItemList,
                        rsvFrameMstList,
                        rsvGrpMstList,
                        raiinKbnYayokuList))
                .ToList();
        }

    }
}
