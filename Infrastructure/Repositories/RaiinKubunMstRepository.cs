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
            List<RaiinKbnMst> raiinKubunMstList = _tenantDataContext.RaiinKbnMsts
                .Where(r => isDeleted || r.IsDeleted == 0)
                .OrderBy(r => r.SortNo)
                .ToList();

            List<int> groupIdList = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            List<RaiinKbnDetail> raiinKubunDetailList = _tenantDataContext.RaiinKbnDetails
                .Where(r => groupIdList.Contains(r.GrpCd) && (isDeleted || r.IsDeleted == 0))
                .ToList();

            List<RaiinKubunMstModel> result = new();

            foreach (var raiinKubunMst in raiinKubunMstList)
            {
                int groupId = raiinKubunMst.GrpCd;

                List<RaiinKubunDetailModel> detailList = raiinKubunDetailList
                    .Where(r => r.GrpCd == groupId)
                    .Select(r => new RaiinKubunDetailModel(
                            r.GrpCd,
                            r.KbnCd,
                            r.SortNo,
                            r.KbnName,
                            r.ColorCd ?? string.Empty,
                            r.IsConfirmed == 1,
                            r.IsAuto == 1,
                            r.IsAutoDelete == 1,
                            r.IsDeleted == 1
                        ))
                    .ToList();

                result.Add(new RaiinKubunMstModel(
                        groupId,
                        raiinKubunMst.SortNo,
                        raiinKubunMst.GrpName,
                        raiinKubunMst.IsDeleted == 1,
                        detailList,
                        new List<RaiinKbnKouiModel>(),
                        new List<RaiinKbnItemModel>(),
                        new List<RsvFrameMstModel>(),
                        new List<RsvGrpMstModel>(),
                        new List<RaiinKbnYayokuModel>()
                    ));
            }
            return result;
        }
        public List<RaiinKubunMstModel> LoadDataKubunSetting(int HpId)
        {
            var raiinKubunMstList = _tenantDataContext.RaiinKbnMsts
               .Where(r => r.HpId == HpId && r.IsDeleted == 0).ToList();

            List<int> groupIdlist = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            List<RaiinKubunDetailModel> raiinKubunDetailList = _tenantDataContext.RaiinKbnDetails
                .Where(r => groupIdlist.Contains(r.GrpCd) && (r.HpId == HpId && r.IsDeleted == 0))
                .Select(x => new RaiinKubunDetailModel(x.GrpCd, x.KbnCd, x.SortNo, x.KbnName, x.ColorCd ?? string.Empty, x.IsConfirmed == 1, x.IsAuto == 1, x.IsAutoDelete == 1, x.IsDeleted == 1))
                .ToList();

            List<RaiinKbnKouiModel> raiinKbnKouiList = _tenantDataContext.RaiinKbnKouis
                .Where(r => groupIdlist.Contains(r.GrpId) && (r.HpId == HpId && r.IsDeleted == 0))
                .Select(x => new RaiinKbnKouiModel(x.GrpId, x.KbnCd, x.SeqNo, x.KouiKbnId, x.IsDeleted))
                .ToList();

            List<RaiinKbnItemModel> raiinKbnItemList = _tenantDataContext.RaiinKbItems
                .Where(r => groupIdlist.Contains(r.GrpCd) && (r.HpId == HpId && r.IsDeleted == 0))
                .Select(x => new RaiinKbnItemModel(x.GrpCd, x.KbnCd, x.SeqNo, x.ItemCd, x.IsExclude, x.IsDeleted))
                .ToList();

            List<RsvGrpMstModel> rsvGrpMstList = _tenantDataContext.RsvGrpMsts
                .Where(r => r.HpId == HpId && r.IsDeleted == 0)
                .Select(x => new RsvGrpMstModel(x.RsvGrpId, x.SortKey, x.RsvGrpName, x.IsDeleted))
                .ToList();

            List<RsvFrameMstModel> rsvFrameMstList = _tenantDataContext.RsvFrameMsts
                .Where(r => r.HpId == HpId && r.IsDeleted == 0)
                .Select(x => new RsvFrameMstModel(x.RsvGrpId, x.RsvFrameId, x.SortKey, x.RsvFrameName ?? String.Empty, x.TantoId, x.KaId, x.MakeRaiin, x.IsDeleted))
                .ToList();

            List<RaiinKbnYayokuModel> raiinKbnYayokuList = _tenantDataContext.RaiinKbnYayokus
                .Where(r => groupIdlist.Contains(r.GrpId) && (r.HpId == HpId && r.IsDeleted == 0))
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
