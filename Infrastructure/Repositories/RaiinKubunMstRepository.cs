using Domain.Models.RaiinKubunMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Linq;

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
                            r.IsDeleted == 1,
                            new List<RaiinKbnKouiModel>(),
                            new List<RaiinKbnItemModel>(),
                            new List<RsvFrameMstModel>(),
                            new List<RsvGrpMstModel>(),
                            new List<RaiinKbnYayokuModel>()
                        ))
                    .ToList();

                result.Add(new RaiinKubunMstModel(
                        groupId,
                        raiinKubunMst.SortNo,
                        raiinKubunMst.GrpName,
                        raiinKubunMst.IsDeleted == 1,
                        detailList
                    ));
            }
            return result;
        }
        public List<RaiinKubunMstModel> LoadDataKubunSetting(int HpId)
        {
            List<RsvGrpMstModel> rsvGrpMstList = _tenantDataContext.RsvGrpMsts
                .Where(r => r.HpId == HpId && r.IsDeleted == 0)
                .Select(x => new RsvGrpMstModel(x.RsvGrpId, x.SortKey, x.RsvGrpName, x.IsDeleted))
                .ToList();

            List<RsvFrameMstModel> rsvFrameMstList = _tenantDataContext.RsvFrameMsts
                .Where(r => r.HpId == HpId && r.IsDeleted == 0)
                .Select(x => new RsvFrameMstModel(x.RsvGrpId, x.RsvFrameId, x.SortKey, x.RsvFrameName ?? String.Empty, x.TantoId, x.KaId, x.MakeRaiin, x.IsDeleted))
                .ToList();

            var raiinKubunMstList = _tenantDataContext.RaiinKbnMsts
               .Where(r => r.HpId == HpId && r.IsDeleted == 0).ToList();

            var groupIdlist = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            var raiinKubunDetailList = _tenantDataContext.RaiinKbnDetails
                                        .Where(r => groupIdlist.Contains(r.GrpCd) && (r.HpId == HpId && r.IsDeleted == 0))
                                        .ToList();
            var kbnCdList = raiinKubunDetailList.Select(r => r.KbnCd).ToList();

            var query = (from kbnDetail in _tenantDataContext.RaiinKbnDetails.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         join kou in _tenantDataContext.RaiinKbnKouis.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         on kbnDetail.KbnCd equals kou.KbnCd into kouis
                         from kbnKoui in kouis.DefaultIfEmpty()
                         join item in _tenantDataContext.RaiinKbItems.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         on kbnDetail.KbnCd equals item.KbnCd into items
                         from kbnItem in items.DefaultIfEmpty()
                         join yoyaku in _tenantDataContext.RaiinKbnYayokus.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         on kbnDetail.KbnCd equals yoyaku.KbnCd into yoyakus
                         from kbnYoyaku in yoyakus.DefaultIfEmpty()
                         select new
                         {
                             kbnKoui,
                             kbnYoyaku,
                             kbnItem
                         }).Distinct().ToList();
            var raiinKbnKouiList = query.Where(x => x.kbnKoui != null).Select(x => new RaiinKbnKouiModel(
                x.kbnKoui.GrpId,
                x.kbnKoui.KbnCd,
                x.kbnKoui.SeqNo,
                x.kbnKoui.KouiKbnId,
                x.kbnKoui.IsDeleted));

            var raiinKbnItemList = query.Where(x => x.kbnItem != null).Select(x => new RaiinKbnItemModel(
                x.kbnItem.GrpCd,
                x.kbnItem.KbnCd,
                x.kbnItem.SeqNo,
                x.kbnItem.ItemCd,
                x.kbnItem.IsExclude,
                x.kbnItem.IsDeleted
                ));

            var raiinKbnYayokuList = query.Where(x => x.kbnYoyaku != null).Select(x => new RaiinKbnYayokuModel(
                x.kbnYoyaku.KbnCd,
                x.kbnYoyaku.SeqNo,
                x.kbnYoyaku.YoyakuCd,
                x.kbnYoyaku.IsDeleted
                ));

            var raiinKubunMstModels = raiinKubunMstList.Select(x => new RaiinKubunMstModel(
                x.GrpCd,
                x.SortNo,
                x.GrpName,
                x.IsDeleted == 1,
                raiinKubunDetailList.Where(y => y.GrpCd == x.GrpCd)
                                    .Select(z => new RaiinKubunDetailModel(
                                        z.GrpCd,
                                        z.KbnCd,
                                        z.SortNo,
                                        z.KbnName,
                                        z.ColorCd ?? String.Empty,
                                        z.IsConfirmed == 1,
                                        z.IsAuto == 1,
                                        z.IsAutoDelete == 1,
                                        z.IsDeleted == 1,
                                        raiinKbnKouiList.Where(m => m.GrpId == z.GrpCd && m.KbnCd == z.KbnCd).Distinct().ToList(),
                                        raiinKbnItemList.Where(m => m.GrpCd == z.GrpCd && m.KbnCd == z.KbnCd).Distinct().ToList(),
                                        rsvFrameMstList,
                                        rsvGrpMstList,
                                        raiinKbnYayokuList.Where(m => m.GrpId == z.GrpCd && m.KbnCd == z.KbnCd).Distinct().ToList()
                                        )).Distinct().ToList()
                                        )).Distinct().ToList();
            return raiinKubunMstModels;
        }
    }
}
