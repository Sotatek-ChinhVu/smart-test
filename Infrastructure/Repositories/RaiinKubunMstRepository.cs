using Domain.Constant;
using Domain.Models.RaiinKubunMst;
using Domain.Models.Reception;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Infrastructure.Repositories
{
    public class RaiinKubunMstRepository : RepositoryBase, IRaiinKubunMstRepository
    {
        public RaiinKubunMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<RaiinKubunMstModel> GetList(bool isDeleted)
        {
            List<RaiinKbnMst> raiinKubunMstList = NoTrackingDataContext.RaiinKbnMsts
                .Where(r => isDeleted || r.IsDeleted == 0)
                .OrderBy(r => r.SortNo)
                .ToList();

            List<int> groupIdList = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            List<RaiinKbnDetail> raiinKubunDetailList = NoTrackingDataContext.RaiinKbnDetails
                .Where(r => groupIdList.Contains(r.GrpCd) && (isDeleted || r.IsDeleted == 0))
                .ToList();

            List<RaiinKubunMstModel> result = new();

            foreach (var raiinKubunMst in raiinKubunMstList)
            {
                int groupId = raiinKubunMst.GrpCd;

                List<RaiinKubunDetailModel> detailList = raiinKubunDetailList
                    .Where(r => r.GrpCd == groupId)
                    .Select(r => new RaiinKubunDetailModel(
                            r.HpId,
                            r.GrpCd,
                            r.KbnCd,
                            r.SortNo,
                            r.KbnName ?? string.Empty,
                            r.ColorCd ?? string.Empty,
                            r.IsConfirmed == 1,
                            r.IsAuto,
                            r.IsAutoDelete,
                            r.IsDeleted == 1,
                            new List<RaiinKbnKouiModel>(),
                            new List<RaiinKbnItemModel>(),
                            new List<RsvFrameMstModel>(),
                            new List<RsvGrpMstModel>(),
                            new List<RaiinKbnYayokuModel>()
                        ))
                    .ToList();

                result.Add(new RaiinKubunMstModel(
                        1,
                        groupId,
                        raiinKubunMst.SortNo,
                        raiinKubunMst.GrpName ?? string.Empty,
                        raiinKubunMst.IsDeleted == 1,
                        detailList
                    ));
            }
            return result;
        }

        public List<RaiinKubunMstModel> LoadDataKubunSetting(int hpId, int userId)
        {
            List<RsvGrpMstModel> rsvGrpMstList = NoTrackingDataContext.RsvGrpMsts
                .Where(r => r.HpId == hpId && r.IsDeleted == 0)
                .Select(x => new RsvGrpMstModel(x.RsvGrpId, x.SortKey, x.RsvGrpName ?? string.Empty, x.IsDeleted))
                .ToList();

            List<RsvFrameMstModel> rsvFrameMstList = NoTrackingDataContext.RsvFrameMsts
                .Where(r => r.HpId == hpId && r.IsDeleted == 0)
                .Select(x => new RsvFrameMstModel(x.RsvGrpId, x.RsvFrameId, x.SortKey, x.RsvFrameName ?? String.Empty, x.TantoId, x.KaId, x.MakeRaiin, x.IsDeleted))
                .ToList();

            var raiinKubunMstList = NoTrackingDataContext.RaiinKbnMsts
               .Where(r => r.HpId == hpId && r.IsDeleted == 0).ToList();

            var groupIdlist = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            var raiinKubunDetailList = NoTrackingDataContext.RaiinKbnDetails
                                        .Where(r => groupIdlist.Contains(r.GrpCd) && (r.HpId == hpId && r.IsDeleted == 0))
                                        .ToList();

            var query = (from kbnDetail in NoTrackingDataContext.RaiinKbnDetails.Where(r => r.HpId == hpId && r.IsDeleted == 0).AsQueryable()
                         join kou in NoTrackingDataContext.RaiinKbnKouis.Where(r => r.HpId == hpId && r.IsDeleted == 0).AsQueryable()
                         on new { kbnDetail.KbnCd, kbnDetail.GrpCd } equals new { kou.KbnCd, GrpCd = kou.GrpId } into kouis
                         from kbnKoui in kouis.DefaultIfEmpty()
                         join item in NoTrackingDataContext.RaiinKbItems.Where(r => r.HpId == hpId && r.IsDeleted == 0).AsQueryable()
                         on new { kbnDetail.KbnCd, kbnDetail.GrpCd } equals new { item.KbnCd, item.GrpCd } into items
                         from kbnItem in items.DefaultIfEmpty()
                         join yoyaku in NoTrackingDataContext.RaiinKbnYayokus.Where(r => r.HpId == hpId && r.IsDeleted == 0).AsQueryable()
                         on new { kbnDetail.KbnCd, kbnDetail.GrpCd } equals new { yoyaku.KbnCd, GrpCd = yoyaku.GrpId } into yoyakus
                         from kbnYoyaku in yoyakus.DefaultIfEmpty()
                         select new
                         {
                             kbnKoui,
                             kbnYoyaku,
                             kbnItem
                         }).Distinct().ToList();
            var raiinKbnKouiList = query.Where(x => x.kbnKoui != null).Select(x => new RaiinKbnKouiModel(
                x.kbnKoui.HpId,
                x.kbnKoui.GrpId,
                x.kbnKoui.KbnCd,
                x.kbnKoui.SeqNo,
                x.kbnKoui.KouiKbnId,
                x.kbnKoui.IsDeleted)).GroupBy(x => new { x.HpId, x.GrpId, x.KbnCd, x.SeqNo }).Select(x => x.First());


            var itemCdList = query.Where(x => x.kbnItem != null).Select(item => item.kbnItem.ItemCd).Distinct().ToList();
            var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.IsDeleted == 0 && itemCdList.Contains(item.ItemCd)).ToList();

            var raiinKbnItemList = query.Where(x => x.kbnItem != null).Select(x => new RaiinKbnItemModel(
                x.kbnItem.HpId,
                x.kbnItem.GrpCd,
                x.kbnItem.KbnCd,
                x.kbnItem.SeqNo,
                x.kbnItem.ItemCd ?? string.Empty,
                x.kbnItem.IsExclude,
                x.kbnItem.IsDeleted == 1,
                x.kbnItem.SortNo,
                tenMstList.FirstOrDefault(item => item.ItemCd == x.kbnItem.ItemCd)?.Name ?? string.Empty
                )).Distinct()
                .GroupBy(x => new { x.HpId, x.GrpCd, x.KbnCd, x.SeqNo })
                .Select(x => x.First())
                .ToList();

            var raiinKbnYayokuList = query.Where(x => x.kbnYoyaku != null).Select(x => new RaiinKbnYayokuModel(
                x.kbnYoyaku.HpId,
                x.kbnYoyaku.KbnCd,
                x.kbnYoyaku.SeqNo,
                x.kbnYoyaku.YoyakuCd,
                x.kbnYoyaku.IsDeleted
                )).Distinct().GroupBy(x => new { x.HpId, x.GrpId, x.KbnCd, x.SeqNo }).Select(x => x.First());

            var raiinKubunMstModels = raiinKubunMstList.Select(x => new RaiinKubunMstModel(
                x.HpId,
                x.GrpCd,
                x.SortNo,
                x.GrpName ?? string.Empty,
                x.IsDeleted == 1,
                GetMaxKbnCd(hpId, x.GrpCd),
                raiinKubunDetailList.Where(y => y.GrpCd == x.GrpCd)
                                    .Select(z => new RaiinKubunDetailModel(
                                        z.HpId,
                                        z.GrpCd,
                                        z.KbnCd,
                                        z.SortNo,
                                        z.KbnName ?? string.Empty,
                                        z.ColorCd?.Length > 0 ? "#" + z.ColorCd : string.Empty,
                                        z.IsConfirmed == 1,
                                        z.IsAuto,
                                        z.IsAutoDelete,
                                        z.IsDeleted == 1,
                                        raiinKbnKouiList.Where(m => m.GrpId == z.GrpCd && m.KbnCd == z.KbnCd).Distinct().ToList(),
                                        raiinKbnItemList.Where(m => m.GrpCd == z.GrpCd && m.KbnCd == z.KbnCd).Distinct().OrderBy(item => item.SortNo).ToList(),
                                        rsvFrameMstList,
                                        rsvGrpMstList,
                                        raiinKbnYayokuList.Where(m => m.GrpId == z.GrpCd && m.KbnCd == z.KbnCd).Distinct().ToList()
                                        )).Distinct().OrderBy(item => item.SortNo).ToList()
                                        )).Distinct().OrderBy(item => item.SortNo).ToList();
            return raiinKubunMstModels;
        }

        public List<string> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels, int userId, int hpId)
        {
            List<string> result = new List<string>();
            var currentKubunMstList = TrackingDataContext.RaiinKbnMsts.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunDetailList = TrackingDataContext.RaiinKbnDetails.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunKouiList = TrackingDataContext.RaiinKbnKouis.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunItemList = TrackingDataContext.RaiinKbItems.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunYoyakuList = TrackingDataContext.RaiinKbnYayokus.Where(x => x.IsDeleted == 0).ToList();

            int detailKbnCd = 0;
            if (currentKubunDetailList != null && currentKubunDetailList.Any())
            {
                detailKbnCd = currentKubunDetailList.Max(x => x.KbnCd);
            }
            int kouiKbnCd = 0;
            if (currentKubunKouiList != null && currentKubunKouiList.Any())
            {
                currentKubunKouiList?.Max(x => x.KouiKbnId);
            }
            int itemSeqNo = 0;
            if (currentKubunItemList != null && currentKubunItemList.Any())
            {
                itemSeqNo = currentKubunItemList.Max(x => (int)x.SeqNo);
            }
            int yoyakuKbnCd = 0;
            if (currentKubunYoyakuList != null && currentKubunYoyakuList.Any())
            {
                yoyakuKbnCd = currentKubunYoyakuList.Max(x => x.YoyakuCd);
            }

            result = ValidateRaiinKbnMst(raiinKubunMstModels, currentKubunDetailList ?? new List<RaiinKbnDetail>(), currentKubunKouiList ?? new List<RaiinKbnKoui>(), currentKubunItemList ?? new List<RaiinKbItem>(), currentKubunYoyakuList ?? new List<RaiinKbnYayoku>());

            if (result.Any())
            {
                return result;
            }

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();

            executionStrategy.Execute(
                () =>
                {
                    // execute your logic here
                    using (var transaction = TrackingDataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (raiinKubunMstModels != null && raiinKubunMstModels.Any())
                            {
                                var raiinKubunMstUpdateList = raiinKubunMstModels.Where(x => x.GroupId != 0).ToList();

                                if (raiinKubunMstUpdateList != null && raiinKubunMstUpdateList.Any())
                                {
                                    raiinKubunMstUpdateList.ForEach(x =>
                                    {
                                        if (x.RaiinKubunDetailModels.Any(x => x.KubunCd != 0))
                                        {
                                            UpdateRaiinKubunDetail(x.GroupId, x.RaiinKubunDetailModels, currentKubunDetailList ?? new List<RaiinKbnDetail>(), currentKubunKouiList ?? new List<RaiinKbnKoui>(), currentKubunItemList ?? new List<RaiinKbItem>(), currentKubunYoyakuList ?? new List<RaiinKbnYayoku>(), kouiKbnCd, itemSeqNo, yoyakuKbnCd, userId);
                                        }
                                    });
                                    foreach (var model in raiinKubunMstUpdateList)
                                    {
                                        var raiinKubunMst = currentKubunMstList.FirstOrDefault(item => item.HpId == model.HpId && item.GrpCd == model.GroupId);
                                        if (raiinKubunMst != null)
                                        {
                                            raiinKubunMst.SortNo = model.SortNo;
                                            raiinKubunMst.GrpName = model.GroupName;
                                            raiinKubunMst.IsDeleted = model.IsDeleted ? 1 : 0;
                                            raiinKubunMst.UpdateId = userId;
                                            raiinKubunMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                            continue;
                                        }
                                        TrackingDataContext.RaiinKbnMsts.Add(new RaiinKbnMst()
                                        {
                                            HpId = model.HpId,
                                            GrpCd = model.GroupId,
                                            SortNo = model.SortNo,
                                            GrpName = model.GroupName,
                                            IsDeleted = model.IsDeleted ? 1 : 0,
                                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                                            CreateId = userId,
                                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                            UpdateId = userId
                                        });
                                    }
                                    TrackingDataContext.SaveChanges();
                                }
                            }
                            TrackingDataContext.SaveChanges();
                            transaction.Commit();
                            result.Add(KubunSettingConstant.Successed);
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            result.Add(KubunSettingConstant.Failed);
                            return false;
                        }
                    }
                });
            return result;
        }

        public List<(string, string)> GetListColumnName(int hpId)
        {
            var listRaiinKbnMst = NoTrackingDataContext.RaiinKbnMsts
              .Where(item => item.HpId == hpId && item.IsDeleted == 0)
              .OrderBy(item => item.SortNo)
              .ToDictionary(item => item.GrpName ?? string.Empty, item => item.GrpCd);

            var listColumnName = new List<(string, string)>()
            {
                new("uketukeNo","順番"), new("sameVisit","同一来院"), new("status","状態"), new("ptNum","患者番号"),
                new("kanaName","カナ氏名"), new("name", "氏名"), new("sex", "性"), new("birthday","生年月日"), new("age", "年齢"),
                new("nameDuplicateState", "読"),new("yoyakuTime", "予約時間"), new ("reservationName", "予約名"), new("uketukeSbtId", "受付種別"),
                new("uketukeTime", "受付時間"), new("sinStartTime", "診察開始"), new("sinEndTime", "診察終了"), new("kaikeiTime", "精算時間"),
                new("raiinCmt", "来院コメント"), new ("ptComment", "患者コメント"), new ("hokenPatternName", "保険"), new ("tantoId", "担当医"),
                new ("kaId", "診療科"), new("lastVisitDate", "前回来院"), new ("sname", "主治医"), new ("raiinRemark",
                "備考"), new("confirmationState", "資格確認状況"), new ("confirmationResult", "資格確認結果")
            };



            if (listRaiinKbnMst != null && listRaiinKbnMst.Count > 0)
            {
                foreach (var item in listRaiinKbnMst)
                {
                    if (!listColumnName.Select(i => i.Item2).Contains(item.Key))
                    {
                        listColumnName.Add(new(item.Value.ToString(), item.Key));
                    }
                }
            }

            return listColumnName;
        }

        public List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> GetRaiinKouiKbns(int hpId)
        {
            var result = new List<(int, int, int, int)>();
            var raiinKouiKbns = NoTrackingDataContext.RaiinKbnKouis.Where(r => r.HpId == hpId && r.IsDeleted == DeleteTypes.None);
            var kouiKbnMsts = NoTrackingDataContext.KouiKbnMsts.Where(k => k.HpId == hpId);
            var query = from raiinKouiKbn in raiinKouiKbns
                        join kouiKbnMst in kouiKbnMsts
                        on raiinKouiKbn.KouiKbnId equals kouiKbnMst.KouiKbnId
                        select new
                        {
                            RaiinKouiKbn = raiinKouiKbn,
                            KouiKbnMst = kouiKbnMst
                        };
            foreach (var entity in query)
            {
                result.Add(new(entity.RaiinKouiKbn.GrpId, entity.RaiinKouiKbn.KbnCd, entity.KouiKbnMst.KouiKbn1, entity.KouiKbnMst.KouiKbn2));
            }
            return result;
        }

        public List<RaiinKbnItemModel> GetRaiinKbnItems(int hpId)
        {
            return NoTrackingDataContext.RaiinKbItems
                            .Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None)
                            .AsEnumerable().Select(p => new RaiinKbnItemModel(
                                    p.HpId,
                                    p.GrpCd,
                                    p.KbnCd,
                                    p.SeqNo,
                                    p.ItemCd ?? string.Empty,
                                    p.IsExclude,
                                    p.IsExclude == 1,
                                    p.SortNo
                                )).ToList();
        }

        public void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int grpId, int kbnCd, int userId)
        {
            // Use Index (HpId, PtId, SinDate, RaiinNo, GrpId, IsDelete) to find the record faster
            var raiinKbnInf = TrackingDataContext.RaiinKbnInfs.FirstOrDefault(r =>
                r.HpId == hpId
                && r.PtId == ptId
                && r.SinDate == sinDate
                && r.RaiinNo == raiinNo
                && r.GrpId == grpId
                && r.IsDelete == DeleteTypes.None);
            if (raiinKbnInf is null)
            {
                // Insert
                TrackingDataContext.RaiinKbnInfs.Add(new RaiinKbnInf
                {
                    HpId = hpId,
                    PtId = ptId,
                    SinDate = sinDate,
                    RaiinNo = raiinNo,
                    GrpId = grpId,
                    KbnCd = kbnCd,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateId = userId
                });
            }
            else
            {
                // Update
                raiinKbnInf.KbnCd = kbnCd;
                raiinKbnInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiinKbnInf.UpdateId = userId;
            }

            TrackingDataContext.SaveChanges();
        }

        public bool SoftDelete(int hpId, long ptId, int sinDate, long raiinNo, int grpId)
        {
            var raiinKbnInf = TrackingDataContext.RaiinKbnInfs.FirstOrDefault(r =>
                r.HpId == hpId
                && r.PtId == ptId
                && r.SinDate == sinDate
                && r.RaiinNo == raiinNo
                && r.GrpId == grpId
                && r.IsDelete == DeleteTypes.None);
            if (raiinKbnInf is null)
            {
                return false;
            }

            raiinKbnInf.IsDelete = DeleteTypes.Deleted;
            TrackingDataContext.SaveChanges();
            return true;
        }

        public List<RaiinKbnModel> GetRaiinKbns(int hpId, long ptId, long raiinNo, int sinDate)
        {
            var raiinKbnMstRespo = NoTrackingDataContext.RaiinKbnMsts.Where(p => p.IsDeleted == 0 && p.HpId == hpId);
            var raiinKbnDetailRespo = NoTrackingDataContext.RaiinKbnDetails.Where(p => p.IsDeleted == 0 && p.HpId == hpId);
            var raiinKbnInfRespo = NoTrackingDataContext.RaiinKbnInfs.Where(p => p.IsDelete == 0 && p.HpId == hpId && p.RaiinNo == raiinNo && p.PtId == ptId && p.SinDate == sinDate);
            var result = (from kbnMst in raiinKbnMstRespo.AsEnumerable()
                          join kbnDetail in raiinKbnDetailRespo on
                          new { kbnMst.HpId, kbnMst.GrpCd } equals
                          new { kbnDetail.HpId, kbnDetail.GrpCd } into details
                          join kbnInf in raiinKbnInfRespo on
                          new { kbnMst.HpId, kbnMst.GrpCd } equals
                          new { kbnInf.HpId, GrpCd = kbnInf.GrpId } into infs
                          from inf in infs.OrderByDescending(p => p.SeqNo).Take(1).DefaultIfEmpty()
                          where
                          kbnMst.IsDeleted == 0 &&
                          kbnMst.HpId == hpId
                          select new
                          {
                              KbnMst = kbnMst,
                              KbnDetails = details.OrderBy(p => p.SortNo),
                              KbnInf = inf
                          })
                          ?.OrderBy(p => p.KbnMst.SortNo)
                          ?.Select(obj => new RaiinKbnModel(obj.KbnMst.HpId, obj.KbnMst.GrpCd, obj.KbnMst.SortNo, obj.KbnMst?.GrpName ?? string.Empty, obj.KbnMst?.IsDeleted ?? 0,
                                                                 new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, obj.KbnMst?.GrpCd ?? 0, obj.KbnInf?.SeqNo ?? 0, obj.KbnInf?.KbnCd ?? 0, obj.KbnInf?.IsDelete ?? 0), obj.KbnDetails?.Select(p => new RaiinKbnDetailModel(p.HpId, p.GrpCd, p.KbnCd, p.SortNo, p.KbnName ?? string.Empty, p.ColorCd ?? string.Empty, p.IsConfirmed, p.IsAuto, p.IsAutoDelete, p.IsDeleted)).ToList() ?? new()))?.ToList() ?? new();
            return result;
        }

        public List<RaiinKbnModel> InitDefaultByRsv(int hpId, int frameID, List<RaiinKbnModel> raiinKbns)
        {
            var raiinKbnYoyakus = NoTrackingDataContext.RaiinKbnYayokus
                    .Where(x => x.IsDeleted == 0 && x.YoyakuCd == frameID && x.HpId == hpId)
                    .ToList();
            foreach (var raiinKbnMst in raiinKbns)
            {
                if (raiinKbnMst.RaiinKbnInfModel.KbnCd != 0) continue;

                foreach (var detail in raiinKbnMst.RaiinKbnDetailModels)
                {
                    var raiinKbnRsvs = raiinKbnYoyakus.FirstOrDefault(x => x.GrpId == detail.GrpCd && x.KbnCd == detail.KbnCd);
                    if (raiinKbnRsvs != null)
                    {
                        raiinKbnMst.RaiinKbnInfModel.ChangeKbnCd(detail.KbnCd);
                        break;
                    }
                }
            }

            return raiinKbns;
        }

        public IEnumerable<RaiinKbnModel> GetPatientRaiinKubuns(int hpId, long ptId, int raiinNo, int sinDate)
        {
            var raiinKbnMst = NoTrackingDataContext.RaiinKbnMsts.Where(x => x.IsDeleted == DeleteStatus.None && x.HpId == hpId).ToList();

            var raiinKbnInf = NoTrackingDataContext.RaiinKbnInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.RaiinNo == raiinNo && x.SinDate == sinDate && x.IsDelete == DeleteStatus.None).ToList();

            var joinQuery = from rkbInf in raiinKbnInf
                            join rknMst in raiinKbnMst on rkbInf.GrpId equals rknMst.GrpCd
                            select new
                            {
                                RkbInf = rkbInf,
                                RknMst = rknMst
                            };
            var dataListItem = joinQuery.AsEnumerable().Select(x => new RaiinKbnModel(
                                                         x.RknMst.HpId,
                                                         x.RknMst.GrpCd,
                                                         x.RknMst.SortNo,
                                                         x.RknMst.GrpName ?? string.Empty,
                                                         x.RknMst.IsDeleted,
                                                         new RaiinKbnInfModel(
                                                             x.RkbInf.HpId,
                                                             x.RkbInf.PtId,
                                                             x.RkbInf.SinDate,
                                                             x.RkbInf.RaiinNo,
                                                             x.RkbInf.GrpId,
                                                             x.RkbInf.SeqNo,
                                                             x.RkbInf.KbnCd,
                                                             x.RkbInf.IsDelete
                                                         ),
                                                         new()
                                                         ));
            return dataListItem;
        }

        public bool SaveRaiinKbnInfs(int hpId, long ptId, int sinDate, long raiinNo, int userId, IEnumerable<RaiinKbnInfDto> kbnInfDtos)
        {
            var raiinInf = TrackingDataContext.RaiinInfs
                 .FirstOrDefault(r => r.HpId == hpId
                     && r.PtId == ptId
                     && r.SinDate == sinDate
                     && r.RaiinNo == raiinNo
                     && r.IsDeleted == DeleteTypes.None);
            if (raiinInf is null)
            {
                return false;
            }

            SaveRaiinKbnInfs(hpId, userId, raiinInf, kbnInfDtos);
            TrackingDataContext.SaveChanges();

            return true;
        }

        private void SaveRaiinKbnInfs(int hpId, int userId, RaiinInf raiinInf, IEnumerable<RaiinKbnInfDto> kbnInfDtos)
        {
            var existingEntities = TrackingDataContext.RaiinKbnInfs
                .Where(x => x.HpId == hpId
                    && x.PtId == raiinInf.PtId
                    && x.SinDate == raiinInf.SinDate
                    && x.RaiinNo == raiinInf.RaiinNo
                    && x.IsDelete == DeleteTypes.None)
                .ToList();

            foreach (var kbnInfDto in kbnInfDtos)
            {
                var existingEntity = existingEntities.Find(x => x.GrpId == kbnInfDto.GrpId);
                if (kbnInfDto.KbnCd == CommonConstants.KbnCdDeleteFlag)
                {
                    if (existingEntity is not null)
                    {
                        // Soft-delete
                        existingEntity.IsDelete = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    if (existingEntity is null)
                    {
                        // Insert
                        TrackingDataContext.RaiinKbnInfs.Add(new RaiinKbnInf
                        {
                            HpId = hpId,
                            PtId = raiinInf.PtId,
                            SinDate = raiinInf.SinDate,
                            RaiinNo = raiinInf.RaiinNo,
                            GrpId = kbnInfDto.GrpId,
                            KbnCd = kbnInfDto.KbnCd,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId,
                            CreateId = userId
                        });
                    }
                    else if (existingEntity.KbnCd != kbnInfDto.KbnCd)
                    {
                        // Update
                        existingEntity.KbnCd = kbnInfDto.KbnCd;
                        existingEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        existingEntity.UpdateId = userId;
                    }
                }
            }
        }
        #region RaiinKbn

        #region Update
        private void UpdateRaiinKubunDetail(int grpCd, List<RaiinKubunDetailModel> raiinKubunDetailModels, List<RaiinKbnDetail> currentRaiinKubunDetails, List<RaiinKbnKoui> raiinKbnKouis, List<RaiinKbItem> raiinKbItems, List<RaiinKbnYayoku> raiinKbnYayokus, int kouiId, int itemSeqNo, int yoyakuId, int userId)
        {
            if (raiinKubunDetailModels != null && raiinKubunDetailModels.Any())
            {
                foreach (var model in raiinKubunDetailModels)
                {
                    var raiinKubun = currentRaiinKubunDetails.FirstOrDefault(kubun => kubun.GrpCd == model.GroupId && kubun.KbnCd == model.KubunCd);
                    if (raiinKubun != null)
                    {
                        raiinKubun.SortNo = model.SortNo;
                        raiinKubun.KbnName = model.KubunName;
                        raiinKubun.ColorCd = model.ColorCd != null && model.ColorCd.Contains("#") ? model.ColorCd.Replace("#", string.Empty) : model.ColorCd;
                        raiinKubun.IsConfirmed = model.IsConfirmed ? 1 : 0;
                        raiinKubun.IsAuto = model.IsAuto;
                        raiinKubun.IsAutoDelete = model.IsAutoDeleted;
                        raiinKubun.IsDeleted = model.IsDeleted ? 1 : 0;
                        raiinKubun.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        raiinKubun.UpdateId = userId;
                        continue;
                    }
                    TrackingDataContext.RaiinKbnDetails.Add(new RaiinKbnDetail()
                    {
                        HpId = model.HpId,
                        GrpCd = grpCd,
                        KbnCd = model.KubunCd,
                        SortNo = model.SortNo,
                        KbnName = model.KubunName,
                        ColorCd = model.ColorCd != null && model.ColorCd.Contains("#") ? model.ColorCd.Replace("#", string.Empty) : model.ColorCd,
                        IsConfirmed = model.IsConfirmed ? 1 : 0,
                        IsAuto = model.IsAuto,
                        IsAutoDelete = model.IsAutoDeleted,
                        IsDeleted = model.IsDeleted ? 1 : 0,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                    });
                }
                TrackingDataContext.SaveChanges();

                raiinKubunDetailModels.ForEach(x =>
                {
                    UpdateRaiinKbnKoui(x.KubunCd, grpCd, x.RaiinKbnKouiModels, raiinKbnKouis, userId);
                    UpdateRaiinKbItem(x.KubunCd, x.GroupId, x.RaiinKbnItemModels, raiinKbItems, userId);
                    UpdateRaiinKbnYayoku(x.KubunCd, x.GroupId, x.RaiinKbnYayokuModels, raiinKbnYayokus, userId);
                });
            }
        }

        private void UpdateRaiinKbnKoui(int kbnCd, int grpCd, List<RaiinKbnKouiModel> raiinKbnKouiModels, List<RaiinKbnKoui> raiinKbnKouis, int userId)
        {
            foreach (var model in raiinKbnKouiModels)
            {
                var raiinKbnKoui = raiinKbnKouis.FirstOrDefault(item => item.GrpId == model.GrpId && item.SeqNo == model.SeqNo && item.KbnCd == model.KbnCd);
                if (raiinKbnKoui != null)
                {
                    raiinKbnKoui.KouiKbnId = model.KouiKbnId;
                    raiinKbnKoui.IsDeleted = model.IsDeleted;
                    raiinKbnKoui.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    raiinKbnKoui.UpdateId = userId;
                    continue;
                }
                TrackingDataContext.RaiinKbnKouis.Add(new RaiinKbnKoui()
                {
                    HpId = model.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = 0,
                    KouiKbnId = model.KouiKbnId,
                    IsDeleted = model.IsDeleted,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId
                });
            }
            TrackingDataContext.SaveChanges();
        }

        private void UpdateRaiinKbItem(int kbnCd, int grpCd, List<RaiinKbnItemModel> raiinKbItemModels, List<RaiinKbItem> raiinKbItems, int userId)
        {
            foreach (var model in raiinKbItemModels)
            {
                var raiinKbnItem = raiinKbItems.FirstOrDefault(item => item.GrpCd == model.GrpCd && item.SeqNo == model.SeqNo && item.KbnCd == model.KbnCd);
                if (raiinKbnItem != null)
                {
                    raiinKbnItem.ItemCd = model.ItemCd;
                    raiinKbnItem.IsExclude = model.IsExclude;
                    raiinKbnItem.IsDeleted = model.IsDeleted ? 1 : 0;
                    raiinKbnItem.SortNo = model.SortNo;
                    raiinKbnItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    raiinKbnItem.UpdateId = userId;
                    continue;
                }
                TrackingDataContext.RaiinKbItems.Add(new RaiinKbItem()
                {
                    HpId = model.HpId,
                    GrpCd = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = 0,
                    ItemCd = model.ItemCd,
                    IsExclude = model.IsExclude,
                    IsDeleted = model.IsDeleted ? 1 : 0,
                    SortNo = model.SortNo,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId
                });
            }
            TrackingDataContext.SaveChanges();
        }

        private void UpdateRaiinKbnYayoku(int kbnCd, int grpCd, List<RaiinKbnYayokuModel> raiinKbnYayokuModels, List<RaiinKbnYayoku> raiinKbnYayokus, int userId)
        {
            foreach (var model in raiinKbnYayokuModels)
            {
                var raiinKbnYayoku = raiinKbnYayokus.FirstOrDefault(item => item.GrpId == model.GrpId && item.SeqNo == model.SeqNo && item.KbnCd == model.KbnCd);
                if (raiinKbnYayoku != null)
                {
                    raiinKbnYayoku.YoyakuCd = model.YoyakuCd;
                    raiinKbnYayoku.IsDeleted = model.IsDeleted;
                    raiinKbnYayoku.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    raiinKbnYayoku.UpdateId = userId;
                    continue;
                }
                TrackingDataContext.RaiinKbnYayokus.Add(new RaiinKbnYayoku()
                {
                    HpId = model.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = 0,
                    YoyakuCd = model.YoyakuCd,
                    IsDeleted = model.IsDeleted,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId
                });
            }
            TrackingDataContext.SaveChanges();
        }
        #endregion

        #region Validate
        private List<string> ValidateRaiinKbnMst(List<RaiinKubunMstModel> raiinKubunMstModels, List<RaiinKbnDetail> currentRaiinKubunDetails, List<RaiinKbnKoui> raiinKbnKouis, List<RaiinKbItem> raiinKbItems, List<RaiinKbnYayoku> raiinKbnYayokus)
        {
            List<string> result = new List<string>();

            if (raiinKubunMstModels.Any(x => string.IsNullOrEmpty(x.GroupName)))
            {
                result.Add(KubunSettingConstant.InvalidRaiinKbnMstGroupName);
                return result;
            }

            var newSortNos = raiinKubunMstModels.Select(x => new Tuple<int, int>(x.GroupId, x.SortNo)).ToList();
            if (!ValidateSortNo(newSortNos))
            {
                result.Add(KubunSettingConstant.InvalidRaiinKbnMstSortNo);
                return result;
            }

            foreach (var raiinKubunMst in raiinKubunMstModels)
            {
                if (raiinKubunMst.RaiinKubunDetailModels != null && raiinKubunMst.RaiinKubunDetailModels.Any())
                {
                    var raiinKubunDetailModels = raiinKubunMst.RaiinKubunDetailModels;
                    if (raiinKubunDetailModels.Any(x => string.IsNullOrEmpty(x.KubunName)))
                    {
                        result.Add(KubunSettingConstant.InvalidKubunName);
                        return result;
                    }

                    if (raiinKubunDetailModels.Any(x => x.GroupId != raiinKubunMst.GroupId))
                    {
                        result.Add(KubunSettingConstant.InvalidRaiinKbnDetailGroupId);
                        return result;
                    }

                    var currentDetails = currentRaiinKubunDetails.Where(x => x.IsDeleted == 0 && x.GrpCd == raiinKubunMst.GroupId).ToList();
                    var newSortRaiinKubunDetailNos = raiinKubunDetailModels.Select(x => new Tuple<int, int>(x.KubunCd, x.SortNo)).ToList();

                    if (!ValidateSortNo(newSortRaiinKubunDetailNos))
                    {
                        result.Add(KubunSettingConstant.InvalidRaiinKbnDetailSortNo);
                        return result;
                    }

                    foreach (var raiinKubunDetail in raiinKubunMst.RaiinKubunDetailModels)
                    {
                        if (raiinKubunDetail.RaiinKbnKouiModels != null && raiinKubunDetail.RaiinKbnKouiModels.Any())
                        {
                            var raiinKbnKouiModels = raiinKubunDetail.RaiinKbnKouiModels;

                            if (raiinKbnKouiModels.Any(x => x.KbnCd != raiinKubunDetail.KubunCd))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnKouiKbnCd);
                                return result;
                            }
                        }
                        if (raiinKubunDetail.RaiinKbnItemModels != null && raiinKubunDetail.RaiinKbnItemModels.Any())
                        {
                            if (raiinKubunDetail.RaiinKbnItemModels.Any(x => x.KbnCd != raiinKubunDetail.KubunCd))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnItemKbnCd);
                                return result;
                            }
                            var raiinKbItemModels = raiinKubunDetail.RaiinKbnItemModels;
                            if (raiinKbItemModels.Any(x => string.IsNullOrEmpty(x.ItemCd)))
                            {
                                result.Add(KubunSettingConstant.InvalidItemCD);
                                return result;
                            }

                            var currentRaiinKbnItems = raiinKbItems.Where(x => x.IsDeleted == 0 && x.GrpCd == raiinKubunDetail.GroupId && x.KbnCd == raiinKubunDetail.KubunCd).ToList();
                            var newRRaiinKbnItemSortNos = raiinKbItemModels.Select(x => new Tuple<int, int>((int)x.SeqNo, x.SortNo)).ToList();

                            if (!ValidateSortNo(newRRaiinKbnItemSortNos))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnItemSortNo);
                                return result;
                            }
                        }
                        if (raiinKubunDetail.RaiinKbnYayokuModels != null && raiinKubunDetail.RaiinKbnYayokuModels.Any())
                        {
                            var raiinKbnYayokuModels = raiinKubunDetail.RaiinKbnYayokuModels;
                            if (raiinKbnYayokuModels.Any(x => x.KbnCd != raiinKubunDetail.KubunCd))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnYoyakuKbnCd);
                                return result;
                            }
                            var currentRaiinKbnYayokus = raiinKbnYayokus.Where(x => x.IsDeleted == 0 && x.GrpId == raiinKubunDetail.GroupId && x.KbnCd == raiinKubunDetail.KubunCd).ToList();
                            var newRaiinKbnYayokuSortNos = raiinKbnYayokuModels.Select(x => new Tuple<int, int>(x.YoyakuCd, (int)x.SeqNo)).ToList();

                            if (!ValidateSortNo(newRaiinKbnYayokuSortNos))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnYayokuSortNo);
                                return result;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool ValidateSortNo(List<Tuple<int, int>> newValue)
        {
            if (newValue.Any(x => x.Item1 < 0 || x.Item2 < 0)) { return false; }
            return true;
        }
        #endregion
        #endregion

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public int GetMaxGrpId(int hpId)
        {
            return NoTrackingDataContext.RaiinKbnMsts.Where(item => item.HpId == hpId)?.Max(item => item.GrpCd) ?? 0;
        }

        private int GetMaxKbnCd(int hpId, int grpId)
        {
            var raiinKbnDetailKbnCdList = NoTrackingDataContext.RaiinKbnDetails.Where(item => item.HpId == hpId && item.GrpCd == grpId).Select(item => item.KbnCd).ToList();
            return raiinKbnDetailKbnCdList.Any() ? raiinKbnDetailKbnCdList.Max() : 0;
        }
    }
}
