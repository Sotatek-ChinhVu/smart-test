using Domain.Constant;
using Domain.Models.RaiinKubunMst;
using Domain.Models.Reception;
using Entity.Tenant;
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
            var kbnCdList = raiinKubunDetailList.Select(r => r.KbnCd).ToList();

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

            var raiinKbnItemList = query.Where(x => x.kbnItem != null).Select(x => new RaiinKbnItemModel(
                x.kbnItem.HpId,
                x.kbnItem.GrpCd,
                x.kbnItem.KbnCd,
                x.kbnItem.SeqNo,
                x.kbnItem.ItemCd ?? string.Empty,
                x.kbnItem.IsExclude,
                x.kbnItem.IsDeleted,
                x.kbnItem.SortNo
                )).Distinct().GroupBy(x => new { x.HpId, x.GrpCd, x.KbnCd, x.SeqNo }).Select(x => x.First());

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
                raiinKubunDetailList.Where(y => y.GrpCd == x.GrpCd)
                                    .Select(z => new RaiinKubunDetailModel(
                                        z.HpId,
                                        z.GrpCd,
                                        z.KbnCd,
                                        z.SortNo,
                                        z.KbnName ?? string.Empty,
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

        public List<string> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels, int userId)
        {
            List<string> result = new List<string>();
            var currentKubunMstList = NoTrackingDataContext.RaiinKbnMsts.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunDetailList = NoTrackingDataContext.RaiinKbnDetails.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunKouiList = NoTrackingDataContext.RaiinKbnKouis.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunItemList = NoTrackingDataContext.RaiinKbItems.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunYoyakuList = NoTrackingDataContext.RaiinKbnYayokus.Where(x => x.IsDeleted == 0).ToList();

            int detailKbnCd = 0;
            if (currentKubunDetailList != null && currentKubunDetailList.Any())
            {
                detailKbnCd = currentKubunDetailList.Max(x => x.KbnCd);
            }
            int kouiKbnCd = 0;
            if (currentKubunKouiList != null && currentKubunKouiList.Any())
            {
                currentKubunKouiList.Max(x => x.KouiKbnId);
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

            result = ValidateRaiinKbnMst(raiinKubunMstModels, currentKubunMstList, currentKubunDetailList ?? new List<RaiinKbnDetail>(), currentKubunKouiList ?? new List<RaiinKbnKoui>(), currentKubunItemList ?? new List<RaiinKbItem>(), currentKubunYoyakuList ?? new List<RaiinKbnYayoku>());

            if (result.Any())
            {
                return result;
            }

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();

            var resultExecute = executionStrategy.Execute(
                () =>
                {
                    // execute your logic here
                    using (var transaction = TrackingDataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (raiinKubunMstModels != null && raiinKubunMstModels.Any())
                            {
                                var raiinKubunMstAddList = raiinKubunMstModels.Where(x => x.GroupId == 0).ToList();

                                if (raiinKubunMstAddList != null && raiinKubunMstAddList.Any())
                                {
                                    var currentGrpCd = currentKubunMstList.Max(x => x.GrpCd);
                                    raiinKubunMstAddList.ForEach(x =>
                                    {
                                        currentGrpCd++;

                                        x = new RaiinKubunMstModel(x.HpId, currentGrpCd, x.SortNo, x.GroupName, x.IsDeleted, x.RaiinKubunDetailModels);

                                        var resultIds = AddRaiinKubunDetail(currentGrpCd, x.RaiinKubunDetailModels, detailKbnCd, kouiKbnCd, itemSeqNo, yoyakuKbnCd, userId);
                                        detailKbnCd = resultIds.Item1;
                                        kouiKbnCd = resultIds.Item2;
                                        itemSeqNo = resultIds.Item3;
                                        yoyakuKbnCd = resultIds.Item4;
                                    });

                                    TrackingDataContext.RaiinKbnMsts.AddRange(raiinKubunMstAddList.Select(x => new RaiinKbnMst()
                                    {
                                        HpId = x.HpId,
                                        GrpCd = x.GroupId,
                                        SortNo = x.SortNo,
                                        GrpName = x.GroupName,
                                        IsDeleted = x.IsDeleted ? 1 : 0,
                                        CreateDate = DateTime.UtcNow,
                                        UpdateDate = DateTime.UtcNow,
                                        UpdateId = userId,
                                        CreateId = userId
                                    }).ToList());
                                    TrackingDataContext.SaveChanges();
                                }

                                var raiinKubunMstUpdateList = raiinKubunMstModels.Where(x => x.GroupId != 0).ToList();

                                if (raiinKubunMstUpdateList != null && raiinKubunMstUpdateList.Any())
                                {
                                    raiinKubunMstUpdateList.ForEach(x =>
                                    {
                                        if (x.RaiinKubunDetailModels.Any(x => x.KubunCd == 0))
                                        {
                                            var resultIds = AddRaiinKubunDetail(x.GroupId, x.RaiinKubunDetailModels, detailKbnCd, kouiKbnCd, itemSeqNo, yoyakuKbnCd, userId);
                                            detailKbnCd = resultIds.Item1;
                                            kouiKbnCd = resultIds.Item2;
                                            itemSeqNo = resultIds.Item3;
                                            yoyakuKbnCd = resultIds.Item4;
                                        }
                                        if (x.RaiinKubunDetailModels.Any(x => x.KubunCd != 0))
                                        {
                                            UpdateRaiinKubunDetail(x.GroupId, x.RaiinKubunDetailModels, currentKubunDetailList ?? new List<RaiinKbnDetail>(), currentKubunKouiList ?? new List<RaiinKbnKoui>(), currentKubunItemList ?? new List<RaiinKbItem>(), currentKubunYoyakuList ?? new List<RaiinKbnYayoku>(), kouiKbnCd, itemSeqNo, yoyakuKbnCd, userId);
                                        }
                                    });
                                    TrackingDataContext.UpdateRange(raiinKubunMstUpdateList.Select(x => new RaiinKbnMst()
                                    {
                                        HpId = x.HpId,
                                        GrpCd = x.GroupId,
                                        SortNo = x.SortNo,
                                        GrpName = x.GroupName,
                                        IsDeleted = x.IsDeleted ? 1 : 0,
                                        CreateDate = DateTime.SpecifyKind(DateTime.SpecifyKind(currentKubunMstList.FirstOrDefault(y => y.GrpCd == x.GroupId)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc), DateTimeKind.Utc),
                                        CreateId = currentKubunMstList.FirstOrDefault(y => y.GrpCd == x.GroupId)?.CreateId ?? 0,
                                        CreateMachine = currentKubunMstList.FirstOrDefault(y => y.GrpCd == x.GroupId)?.CreateMachine ?? string.Empty,
                                        UpdateDate = DateTime.UtcNow,
                                        UpdateId = userId
                                    }));
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
                    if (listColumnName.Select(i => i.Item2).Contains(item.Key) == false)
                        listColumnName.Add(new(item.Value.ToString(), item.Key));
                }
            }

            return listColumnName;
        }

        public List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> GetRaiinKouiKbns(int hpId)
        {
            var result = new List<(int, int, int, int)>();
            var raiinKouiKbns = NoTrackingDataContext.RaiinKbnKouis.Where(r => r.HpId == Session.HospitalID && r.IsDeleted == DeleteTypes.None);
            var kouiKbnMsts = NoTrackingDataContext.KouiKbnMsts.Where(k => k.HpId == Session.HospitalID);
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
                                    p.IsExclude,
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
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    CreateId = userId
                });
            }
            else
            {
                // Update
                raiinKbnInf.KbnCd = kbnCd;
                raiinKbnInf.UpdateDate = DateTime.UtcNow;
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
            var r = raiinKbnInfRespo.ToList();
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
                    var raiinKbnRsvs = raiinKbnYoyakus.Where(x => x.GrpId == detail.GrpCd && x.KbnCd == detail.KbnCd).FirstOrDefault();
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
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId,
                            CreateId = userId
                        });
                    }
                    else if (existingEntity.KbnCd != kbnInfDto.KbnCd)
                    {
                        // Update
                        existingEntity.KbnCd = kbnInfDto.KbnCd;
                        existingEntity.UpdateDate = DateTime.UtcNow;
                        existingEntity.UpdateId = userId;
                    }
                }
            }
        }

        #region RaiinKbn
        #region Add
        private (int, int, int, int) AddRaiinKubunDetail(int grpCd, List<RaiinKubunDetailModel> raiinKubunDetailModels, int currentKbnCd, int kouiKbnCd, int itemSeqNo, int yoyakuKbnCd, int userId)
        {
            if (raiinKubunDetailModels != null && raiinKubunDetailModels.Any())
            {
                raiinKubunDetailModels.ForEach(x =>
                {
                    currentKbnCd++;
                    x = new RaiinKubunDetailModel(x.HpId, grpCd, currentKbnCd, x.SortNo, x.KubunName, x.ColorCd, x.IsConfirmed, x.IsAuto, x.IsAutoDeleted, x.IsDeleted,
                        x.RaiinKbnKouiModels,
                        x.RaiinKbnItemModels,
                        new List<RsvFrameMstModel>(),
                        new List<RsvGrpMstModel>(),
                        x.RaiinKbnYayokuModels);

                    kouiKbnCd = AddRaiinKbnKoui(currentKbnCd, grpCd, x.RaiinKbnKouiModels, kouiKbnCd, userId);
                    itemSeqNo = AddRaiinKbItem(currentKbnCd, grpCd, x.RaiinKbnItemModels, itemSeqNo, userId);
                    yoyakuKbnCd = AddRaiinKbnYayoku(currentKbnCd, grpCd, x.RaiinKbnYayokuModels, yoyakuKbnCd, userId);
                });

                TrackingDataContext.RaiinKbnDetails.AddRange(raiinKubunDetailModels.Select(x => new RaiinKbnDetail()
                {
                    HpId = x.HpId,
                    GrpCd = grpCd,
                    KbnCd = x.KubunCd,
                    SortNo = x.SortNo,
                    KbnName = x.KubunName,
                    ColorCd = x.ColorCd,
                    IsConfirmed = x.IsConfirmed ? 1 : 0,
                    IsAuto = x.IsAuto ? 1 : 0,
                    IsAutoDelete = x.IsAutoDeleted ? 1 : 0,
                    IsDeleted = x.IsDeleted ? 1 : 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    CreateId = userId
                }));
            }
            TrackingDataContext.SaveChanges();
            return (currentKbnCd, kouiKbnCd, itemSeqNo, yoyakuKbnCd);
        }

        private int AddRaiinKbnKoui(int kbnCd, int grpCd, List<RaiinKbnKouiModel> raiinKbnKouiModels, int kouiKbnCd, int userId)
        {
            if (raiinKbnKouiModels != null && raiinKbnKouiModels.Any())
            {
                raiinKbnKouiModels.ForEach(x =>
                {
                    kouiKbnCd++;
                    x = new RaiinKbnKouiModel(x.HpId, x.GrpId, x.KbnCd, x.SeqNo, kouiKbnCd, x.IsDeleted);
                });

                TrackingDataContext.RaiinKbnKouis.AddRange(raiinKbnKouiModels.Select(x => new RaiinKbnKoui()
                {
                    HpId = x.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    KouiKbnId = x.KouiKbnId,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    IsDeleted = x.IsDeleted,
                    CreateDate = DateTime.UtcNow,
                    CreateId = userId
                }));
            }
            TrackingDataContext.SaveChanges();
            return kouiKbnCd;
        }

        private int AddRaiinKbItem(int kbnCd, int grpCd, List<RaiinKbnItemModel> raiinKbItemModels, int itemKbnCd, int userId)
        {
            if (raiinKbItemModels != null && raiinKbItemModels.Any())
            {
                raiinKbItemModels.ForEach(x =>
                {
                    itemKbnCd++;
                    x = new RaiinKbnItemModel(x.HpId, x.GrpCd, x.KbnCd, itemKbnCd, x.ItemCd, x.IsExclude, x.IsDeleted, x.SortNo);
                });

                TrackingDataContext.RaiinKbItems.AddRange(raiinKbItemModels.Select(x => new RaiinKbItem()
                {
                    HpId = x.HpId,
                    GrpCd = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    ItemCd = x.ItemCd,
                    IsExclude = x.IsExclude,
                    IsDeleted = x.IsDeleted,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    SortNo = x.SortNo,
                    CreateDate = DateTime.UtcNow,
                    CreateId = userId,
                }));
            }
            TrackingDataContext.SaveChanges();
            return itemKbnCd;
        }

        private int AddRaiinKbnYayoku(int kbnCd, int grpCd, List<RaiinKbnYayokuModel> raiinKbnYayokuModels, int yoyakuCd, int userId)
        {
            if (raiinKbnYayokuModels != null && raiinKbnYayokuModels.Any())
            {
                raiinKbnYayokuModels.ForEach(x =>
                {
                    yoyakuCd++;
                    x = new RaiinKbnYayokuModel(x.HpId, x.KbnCd, x.SeqNo, yoyakuCd, x.IsDeleted);
                });
                TrackingDataContext.RaiinKbnYayokus.AddRange(raiinKbnYayokuModels.Select(x => new RaiinKbnYayoku()
                {
                    HpId = x.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    YoyakuCd = x.YoyakuCd,
                    IsDeleted = x.IsDeleted,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    CreateId = userId
                }));
            }
            TrackingDataContext.SaveChanges();
            return yoyakuCd;
        }
        #endregion

        #region Update
        private void UpdateRaiinKubunDetail(int grpCd, List<RaiinKubunDetailModel> raiinKubunDetailModels, List<RaiinKbnDetail> currentRaiinKubunDetails, List<RaiinKbnKoui> raiinKbnKouis, List<RaiinKbItem> raiinKbItems, List<RaiinKbnYayoku> raiinKbnYayokus, int kouiId, int itemSeqNo, int yoyakuId, int userId)
        {
            if (raiinKubunDetailModels != null && raiinKubunDetailModels.Any())
            {
                raiinKubunDetailModels.ForEach(x =>
                {
                    var kouiModelAdd = x.RaiinKbnKouiModels.Where(x => x.KouiKbnId == 0).ToList();
                    if (kouiModelAdd != null && kouiModelAdd.Any())
                    {
                        kouiId = AddRaiinKbnKoui(x.KubunCd, x.GroupId, kouiModelAdd, kouiId, userId);
                    }

                    var kouiModelUpdate = x.RaiinKbnKouiModels.Where(x => x.KouiKbnId != 0).ToList();
                    if (kouiModelUpdate != null && kouiModelUpdate.Any())
                    {
                        UpdateRaiinKbnKoui(x.KubunCd, grpCd, kouiModelUpdate, raiinKbnKouis, userId);
                    }

                    var itemModelAdd = x.RaiinKbnItemModels.Where(x => x.SeqNo == 0).ToList();
                    if (itemModelAdd != null && itemModelAdd.Any())
                    {
                        itemSeqNo = AddRaiinKbItem(x.KubunCd, x.GroupId, itemModelAdd, itemSeqNo, userId);
                    }

                    var itemModelUpdate = x.RaiinKbnItemModels.Where(x => x.SeqNo != 0).ToList();
                    if (itemModelUpdate != null && itemModelUpdate.Any())
                    {
                        UpdateRaiinKbItem(x.KubunCd, x.GroupId, x.RaiinKbnItemModels, raiinKbItems, userId);
                    }

                    var yoyakuModelAdd = x.RaiinKbnYayokuModels.Where(x => x.YoyakuCd == 0).ToList();
                    if (yoyakuModelAdd != null && yoyakuModelAdd.Any())
                    {
                        yoyakuId = AddRaiinKbnYayoku(x.KubunCd, x.GroupId, x.RaiinKbnYayokuModels, yoyakuId, userId);
                    }

                    var yoyakuModelUpdate = x.RaiinKbnYayokuModels.Where(x => x.YoyakuCd != 0).ToList();
                    if (yoyakuModelUpdate != null && yoyakuModelUpdate.Any())
                    {
                        UpdateRaiinKbnYayoku(x.KubunCd, x.GroupId, yoyakuModelUpdate, raiinKbnYayokus, userId);
                    }
                });
                TrackingDataContext.RaiinKbnDetails.UpdateRange(raiinKubunDetailModels.Select(x => new RaiinKbnDetail()
                {
                    HpId = x.HpId,
                    GrpCd = grpCd,
                    KbnCd = x.KubunCd,
                    SortNo = x.SortNo,
                    KbnName = x.KubunName,
                    ColorCd = x.ColorCd,
                    IsConfirmed = x.IsConfirmed ? 1 : 0,
                    IsAuto = x.IsAuto ? 1 : 0,
                    IsAutoDelete = x.IsAutoDeleted ? 1 : 0,
                    IsDeleted = x.IsDeleted ? 1 : 0,
                    CreateDate = DateTime.SpecifyKind(currentRaiinKubunDetails.FirstOrDefault(y => y.GrpCd == x.GroupId && y.KbnCd == x.KubunCd)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc),
                    CreateId = currentRaiinKubunDetails.FirstOrDefault(y => y.GrpCd == x.GroupId && y.KbnCd == x.KubunCd)?.CreateId ?? 0,
                    CreateMachine = currentRaiinKubunDetails.FirstOrDefault(y => y.GrpCd == x.GroupId && y.KbnCd == x.KubunCd)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                }));
                TrackingDataContext.SaveChanges();
            }
        }

        private void UpdateRaiinKbnKoui(int kbnCd, int grpCd, List<RaiinKbnKouiModel> raiinKbnKouiModels, List<RaiinKbnKoui> raiinKbnKouis, int userId)
        {
            if (raiinKbnKouiModels != null && raiinKbnKouiModels.Any())
            {
                TrackingDataContext.RaiinKbnKouis.UpdateRange(raiinKbnKouiModels.Select(x => new RaiinKbnKoui()
                {
                    HpId = x.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    KouiKbnId = x.KouiKbnId,
                    IsDeleted = x.IsDeleted,
                    CreateDate = DateTime.SpecifyKind(raiinKbnKouis.FirstOrDefault(y => y.KouiKbnId == x.KouiKbnId)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc),
                    CreateId = raiinKbnKouis.FirstOrDefault(y => y.KouiKbnId == x.KouiKbnId)?.CreateId ?? 0,
                    CreateMachine = raiinKbnKouis.FirstOrDefault(y => y.KouiKbnId == x.KouiKbnId)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                }));
            }
            TrackingDataContext.SaveChanges();
        }

        private void UpdateRaiinKbItem(int kbnCd, int grpCd, List<RaiinKbnItemModel> raiinKbItemModels, List<RaiinKbItem> raiinKbItems, int userId)
        {
            if (raiinKbItemModels != null && raiinKbItemModels.Any())
            {
                TrackingDataContext.RaiinKbItems.UpdateRange(raiinKbItemModels.Select(x => new RaiinKbItem()
                {
                    HpId = x.HpId,
                    GrpCd = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    ItemCd = x.ItemCd,
                    IsExclude = x.IsExclude,
                    IsDeleted = x.IsDeleted,
                    SortNo = x.SortNo,
                    CreateDate = DateTime.SpecifyKind(raiinKbItems.FirstOrDefault(y => y.SeqNo == x.SeqNo)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc),
                    CreateId = raiinKbItems.FirstOrDefault(y => y.SeqNo == x.SeqNo)?.CreateId ?? 0,
                    CreateMachine = raiinKbItems.FirstOrDefault(y => y.SeqNo == x.SeqNo)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                }));
            }
            TrackingDataContext.SaveChanges();
        }

        private void UpdateRaiinKbnYayoku(int kbnCd, int grpCd, List<RaiinKbnYayokuModel> raiinKbnYayokuModels, List<RaiinKbnYayoku> raiinKbnYayokus, int userId)
        {
            if (raiinKbnYayokuModels != null && raiinKbnYayokuModels.Any())
            {
                var updateModel = raiinKbnYayokuModels.Select(x => new RaiinKbnYayoku()
                {
                    HpId = x.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    YoyakuCd = x.YoyakuCd,
                    IsDeleted = x.IsDeleted,
                    CreateDate = DateTime.SpecifyKind(raiinKbnYayokus.FirstOrDefault(y => y.YoyakuCd == x.YoyakuCd)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc),
                    CreateId = raiinKbnYayokus.FirstOrDefault(y => y.YoyakuCd == x.YoyakuCd)?.CreateId ?? 0,
                    CreateMachine = raiinKbnYayokus.FirstOrDefault(y => y.YoyakuCd == x.YoyakuCd)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                }).ToList();
                TrackingDataContext.RaiinKbnYayokus.UpdateRange(updateModel);
                TrackingDataContext.SaveChanges();
            }
        }
        #endregion

        #region Validate
        private List<string> ValidateRaiinKbnMst(List<RaiinKubunMstModel> raiinKubunMstModels, List<RaiinKbnMst> currentKubunMstList, List<RaiinKbnDetail> currentRaiinKubunDetails, List<RaiinKbnKoui> raiinKbnKouis, List<RaiinKbItem> raiinKbItems, List<RaiinKbnYayoku> raiinKbnYayokus)
        {
            List<string> result = new List<string>();

            if (raiinKubunMstModels.Any(x => string.IsNullOrEmpty(x.GroupName)))
            {
                result.Add(KubunSettingConstant.InvalidRaiinKbnMstGroupName);
                return result;
            }

            var currentRaiinKubunMsts = currentKubunMstList.Where(x => x.IsDeleted == 0).ToList();
            var currentSortNos = currentRaiinKubunMsts.Select(x => new Tuple<int, int>(x.GrpCd, x.SortNo)).ToList();
            var newSortNos = raiinKubunMstModels.Select(x => new Tuple<int, int>(x.GroupId, x.SortNo)).ToList();
            if (!ValidateSortNo(currentSortNos, newSortNos))
            {
                result.Add(KubunSettingConstant.InvalidRaiinKbnMstSortNo);
                return result;
            }

            var raiinKubunMstModel = raiinKubunMstModels.Where(x => x.HpId != 0 && x.GroupId != 0).ToList();
            var raiinKubunMstUpdate = from raiinKbnMst in raiinKubunMstModel
                                      join current in currentRaiinKubunMsts
                                      on new { raiinKbnMst.HpId, GrpCd = raiinKbnMst.GroupId }
                                      equals new { current.HpId, current.GrpCd }
                                      into msts
                                      from mst in msts.DefaultIfEmpty()
                                      select new { raiinKbnMst, mst };

            if (raiinKubunMstUpdate.Any(x => x.mst == null))
            {
                result.Add(KubunSettingConstant.RaiinKbnMstNotExisted);
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
                    var currentRaiinKubunDetailSortNos = currentDetails.Select(x => new Tuple<int, int>(x.KbnCd, x.SortNo)).ToList();
                    var newSortRaiinKubunDetailNos = raiinKubunDetailModels.Select(x => new Tuple<int, int>(x.KubunCd, x.SortNo)).ToList();

                    if (!ValidateSortNo(currentRaiinKubunDetailSortNos, newSortRaiinKubunDetailNos))
                    {
                        result.Add(KubunSettingConstant.InvalidRaiinKbnDetailSortNo);
                        return result;
                    }

                    raiinKubunDetailModels = raiinKubunDetailModels.Where(x => x.HpId != 0 && x.GroupId != 0 && x.KubunCd != 0).ToList();
                    var raiinKbnDetailUpdate = from raiinKbndetail in raiinKubunDetailModels
                                               join current in currentDetails
                                               on new { raiinKbndetail.HpId, GrpCd = raiinKbndetail.GroupId, KbnCd = raiinKbndetail.KubunCd }
                                               equals new { current.HpId, current.GrpCd, current.KbnCd }
                                               into details
                                               from detail in details.DefaultIfEmpty()
                                               select new { raiinKbndetail, detail };

                    if (raiinKbnDetailUpdate.Any(x => x.detail == null))
                    {
                        result.Add(KubunSettingConstant.RaiinKbnDetailNotExisted);
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

                            var currentRaiinKbnKouis = raiinKbnKouis.Where(x => x.IsDeleted == 0 && x.GrpId == raiinKubunDetail.GroupId && x.KbnCd == raiinKubunDetail.KubunCd).ToList();
                            var currentRaiinKbnKouiSortNos = currentRaiinKbnKouis.Select(x => new Tuple<int, int>(x.KouiKbnId, x.SeqNo)).ToList();
                            var newRaiinKbnKouiSortNos = raiinKbnKouiModels.Select(x => new Tuple<int, int>(x.KouiKbnId, x.SeqNo)).ToList();
                            if (!ValidateSortNo(currentRaiinKbnKouiSortNos, newRaiinKbnKouiSortNos))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnKouiSortNo);
                                return result;
                            }

                            raiinKbnKouiModels = raiinKbnKouiModels.Where(x => x.HpId != 0 && x.GrpId != 0 && x.KbnCd != 0 && x.KouiKbnId != 0).ToList();
                            var raiinKbnKouiUpdate = from raiinKbnKoui in raiinKbnKouiModels
                                                     join current in currentRaiinKbnKouis
                                                     on new { raiinKbnKoui.HpId, raiinKbnKoui.GrpId, raiinKbnKoui.KbnCd, raiinKbnKoui.KouiKbnId }
                                                     equals new { current.HpId, current.GrpId, current.KbnCd, current.KouiKbnId }
                                                     into Kouis
                                                     from koui in Kouis.DefaultIfEmpty()
                                                     select new { raiinKbnKoui, koui };

                            if (raiinKbnKouiUpdate.Any(x => x.koui == null))
                            {
                                result.Add(KubunSettingConstant.RaiinKbnKouiNotExisted);
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
                            var currentRaiinKbnItemSortNos = currentRaiinKbnItems.Select(x => new Tuple<int, int>((int)x.SeqNo, x.SortNo)).ToList();
                            var newRRaiinKbnItemSortNos = raiinKbItemModels.Select(x => new Tuple<int, int>((int)x.SeqNo, x.SortNo)).ToList();

                            if (!ValidateSortNo(currentRaiinKbnItemSortNos, newRRaiinKbnItemSortNos))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnItemSortNo);
                                return result;
                            }

                            raiinKbItemModels = raiinKbItemModels.Where(x => x.HpId != 0 && x.GrpCd != 0 && x.KbnCd != 0).ToList();
                            var raiinKbnItemUpdate = from raiinKbnItem in raiinKbItemModels
                                                     join current in raiinKbItems
                                                       on new { raiinKbnItem.HpId, raiinKbnItem.GrpCd, raiinKbnItem.KbnCd }
                                                       equals new { current.HpId, current.GrpCd, current.KbnCd }
                                                       into items
                                                     from item in items.DefaultIfEmpty()
                                                     select new { raiinKbnItem, item };

                            if (raiinKbnItemUpdate.Any(x => x.item == null))
                            {
                                result.Add(KubunSettingConstant.RaiinKbnItemNotExisted);
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
                            var currentRaiinKbnYayokuSortNos = currentRaiinKbnYayokus.Select(x => new Tuple<int, int>(x.YoyakuCd, (int)x.SeqNo)).ToList();
                            var newRaiinKbnYayokuSortNos = raiinKbnYayokuModels.Select(x => new Tuple<int, int>(x.YoyakuCd, (int)x.SeqNo)).ToList();

                            if (!ValidateSortNo(currentRaiinKbnYayokuSortNos, newRaiinKbnYayokuSortNos))
                            {
                                result.Add(KubunSettingConstant.InvalidRaiinKbnYayokuSortNo);
                                return result;
                            }

                            raiinKbnYayokuModels = raiinKbnYayokuModels.Where(x => x.HpId != 0 && x.GrpId != 0 && x.KbnCd != 0 && x.YoyakuCd != 0).ToList();
                            var raiinKbnYayokuUpdate = from raiinKbnYayoku in raiinKbnYayokuModels
                                                       join current in currentRaiinKbnYayokus
                                                       on new { raiinKbnYayoku.HpId, raiinKbnYayoku.GrpId, raiinKbnYayoku.KbnCd, raiinKbnYayoku.YoyakuCd }
                                                       equals new { current.HpId, current.GrpId, current.KbnCd, current.YoyakuCd }
                                                       into yayokus
                                                       from yoyaku in yayokus.DefaultIfEmpty()
                                                       select new { raiinKbnYayoku, yoyaku };

                            if (raiinKbnYayokuUpdate.Any(x => x.yoyaku == null))
                            {
                                result.Add(KubunSettingConstant.RaiinKbnYayokuNotExisted);
                                return result;
                            }
                        }
                    }
                }
            }
            return result;
        }
        private bool ValidateSortNo(List<Tuple<int, int>> currentValue, List<Tuple<int, int>> newValue)
        {
            if (newValue.Any(x => x.Item1 < 0 || x.Item2 < 0)) return false;
            var ids = newValue.Select(x => x.Item1).ToList();

            currentValue.ForEach(x =>
            {
                if (ids.Contains(x.Item1)) x = newValue.FirstOrDefault(y => y.Item1 == x.Item1) ?? new Tuple<int, int>(0, 0);
            });

            var value = currentValue.Union(newValue).Select(x => x.Item2).ToList();
            value.Sort();

            if (value.Zip(value.Skip(1), (curr, next) => curr < next).All(x => x))
                return true;

            return false;
        }
        #endregion
        #endregion

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
