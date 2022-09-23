using Domain.Models.RaiinKubunMst;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public class RaiinKubunMstRepository : IRaiinKubunMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContextNoTracking;
        private readonly TenantDataContext _tenantDataContextTracking;

        public RaiinKubunMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<RaiinKubunMstModel> GetList(bool isDeleted)
        {
            List<RaiinKbnMst> raiinKubunMstList = _tenantDataContextNoTracking.RaiinKbnMsts
                .Where(r => isDeleted || r.IsDeleted == 0)
                .OrderBy(r => r.SortNo)
                .ToList();

            List<int> groupIdList = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            List<RaiinKbnDetail> raiinKubunDetailList = _tenantDataContextNoTracking.RaiinKbnDetails
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
                        1,
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
            List<RsvGrpMstModel> rsvGrpMstList = _tenantDataContextNoTracking.RsvGrpMsts
                .Where(r => r.HpId == HpId && r.IsDeleted == 0)
                .Select(x => new RsvGrpMstModel(x.RsvGrpId, x.SortKey, x.RsvGrpName, x.IsDeleted))
                .ToList();

            List<RsvFrameMstModel> rsvFrameMstList = _tenantDataContextNoTracking.RsvFrameMsts
                .Where(r => r.HpId == HpId && r.IsDeleted == 0)
                .Select(x => new RsvFrameMstModel(x.RsvGrpId, x.RsvFrameId, x.SortKey, x.RsvFrameName ?? String.Empty, x.TantoId, x.KaId, x.MakeRaiin, x.IsDeleted))
                .ToList();

            var raiinKubunMstList = _tenantDataContextNoTracking.RaiinKbnMsts
               .Where(r => r.HpId == HpId && r.IsDeleted == 0).ToList();

            var groupIdlist = raiinKubunMstList.Select(r => r.GrpCd).ToList();

            var raiinKubunDetailList = _tenantDataContextNoTracking.RaiinKbnDetails
                                        .Where(r => groupIdlist.Contains(r.GrpCd) && (r.HpId == HpId && r.IsDeleted == 0))
                                        .ToList();
            var kbnCdList = raiinKubunDetailList.Select(r => r.KbnCd).ToList();

            var query = (from kbnDetail in _tenantDataContextNoTracking.RaiinKbnDetails.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         join kou in _tenantDataContextNoTracking.RaiinKbnKouis.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         on kbnDetail.KbnCd equals kou.KbnCd into kouis
                         from kbnKoui in kouis.DefaultIfEmpty()
                         join item in _tenantDataContextNoTracking.RaiinKbItems.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         on kbnDetail.KbnCd equals item.KbnCd into items
                         from kbnItem in items.DefaultIfEmpty()
                         join yoyaku in _tenantDataContextNoTracking.RaiinKbnYayokus.Where(r => r.HpId == HpId && r.IsDeleted == 0).AsQueryable()
                         on kbnDetail.KbnCd equals yoyaku.KbnCd into yoyakus
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
                x.kbnKoui.IsDeleted));

            var raiinKbnItemList = query.Where(x => x.kbnItem != null).Select(x => new RaiinKbnItemModel(
                x.kbnItem.HpId,
                x.kbnItem.GrpCd,
                x.kbnItem.KbnCd,
                x.kbnItem.SeqNo,
                x.kbnItem.ItemCd,
                x.kbnItem.IsExclude,
                x.kbnItem.IsDeleted,
                x.kbnItem.SortNo
                ));

            var raiinKbnYayokuList = query.Where(x => x.kbnYoyaku != null).Select(x => new RaiinKbnYayokuModel(
                x.kbnYoyaku.HpId,
                x.kbnYoyaku.KbnCd,
                x.kbnYoyaku.SeqNo,
                x.kbnYoyaku.YoyakuCd,
                x.kbnYoyaku.IsDeleted
                ));

            var raiinKubunMstModels = raiinKubunMstList.Select(x => new RaiinKubunMstModel(
                x.HpId,
                x.GrpCd,
                x.SortNo,
                x.GrpName,
                x.IsDeleted == 1,
                raiinKubunDetailList.Where(y => y.GrpCd == x.GrpCd)
                                    .Select(z => new RaiinKubunDetailModel(
                                        z.HpId,
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

        public List<(bool, string)> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels)
        {
            List<(bool, string)> result = new List<(bool, string)>();
            var currentKubunMstList = _tenantDataContextNoTracking.RaiinKbnMsts.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunDetailList = _tenantDataContextNoTracking.RaiinKbnDetails.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunKouiList = _tenantDataContextNoTracking.RaiinKbnKouis.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunItemList = _tenantDataContextNoTracking.RaiinKbItems.Where(x => x.IsDeleted == 0).ToList();
            var currentKubunYoyakuList = _tenantDataContextNoTracking.RaiinKbnYayokus.Where(x => x.IsDeleted == 0).ToList();

            int detailKbnCd = currentKubunDetailList.Max(x => x.KbnCd);
            int kouiKbnCd = currentKubunKouiList.Max(x => x.KouiKbnId);
            int itemSeqNo = currentKubunItemList.Max(x => (int)x.SeqNo);
            int yoyakuKbnCd = currentKubunYoyakuList.Max(x => x.YoyakuCd);

            result = ValidateRaiinKbnMst(raiinKubunMstModels, currentKubunMstList, currentKubunDetailList, currentKubunKouiList, currentKubunItemList, currentKubunYoyakuList);

            if (result.Any(x => !x.Item1))
            {
                return result;
            }

            var executionStrategy = _tenantDataContextTracking.Database.CreateExecutionStrategy();

            var resultExecute = executionStrategy.Execute(
                () =>
                {
                    // execute your logic here
                    using (var transaction = _tenantDataContextTracking.Database.BeginTransaction())
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

                                        var resultIds = AddRaiinKubunDetail(currentGrpCd, x.RaiinKubunDetailModels, detailKbnCd, kouiKbnCd, itemSeqNo, yoyakuKbnCd);
                                        detailKbnCd = resultIds.Item1;
                                        kouiKbnCd = resultIds.Item2;
                                        itemSeqNo = resultIds.Item3;
                                        yoyakuKbnCd = resultIds.Item4;
                                    });

                                    _tenantDataContextTracking.RaiinKbnMsts.AddRange(raiinKubunMstAddList.Select(x => new RaiinKbnMst()
                                    {
                                        HpId = x.HpId,
                                        GrpCd = x.GroupId,
                                        SortNo = x.SortNo,
                                        GrpName = x.GroupName,
                                        IsDeleted = x.IsDeleted ? 1 : 0,
                                        CreateDate = DateTime.UtcNow,
                                        CreateId = TempIdentity.UserId,
                                        CreateMachine = TempIdentity.ComputerName
                                    }).ToList());
                                    _tenantDataContextTracking.SaveChanges();
                                }

                                var raiinKubunMstUpdateList = raiinKubunMstModels.Where(x => x.GroupId != 0).ToList();

                                if (raiinKubunMstUpdateList != null && raiinKubunMstUpdateList.Any())
                                {
                                    raiinKubunMstUpdateList.ForEach(x =>
                                    {
                                        if (x.RaiinKubunDetailModels.Any(x => x.KubunCd == 0))
                                        {
                                            var resultIds = AddRaiinKubunDetail(x.GroupId, x.RaiinKubunDetailModels, detailKbnCd, kouiKbnCd, itemSeqNo, yoyakuKbnCd);
                                            detailKbnCd = resultIds.Item1;
                                            kouiKbnCd = resultIds.Item2;
                                            itemSeqNo = resultIds.Item3;
                                            yoyakuKbnCd = resultIds.Item4;
                                        }
                                        if (x.RaiinKubunDetailModels.Any(x => x.KubunCd != 0))
                                        {
                                            UpdateRaiinKubunDetail(x.GroupId, x.RaiinKubunDetailModels, currentKubunDetailList, currentKubunKouiList, currentKubunItemList, currentKubunYoyakuList, kouiKbnCd, itemSeqNo, yoyakuKbnCd);
                                        }
                                    });
                                    _tenantDataContextTracking.UpdateRange(raiinKubunMstUpdateList.Select(x => new RaiinKbnMst()
                                    {
                                        HpId = x.HpId,
                                        GrpCd = x.GroupId,
                                        SortNo = x.SortNo,
                                        GrpName = x.GroupName,
                                        IsDeleted = x.IsDeleted ? 1 : 0,
                                        CreateDate = DateTime.SpecifyKind(DateTime.SpecifyKind(currentKubunMstList.FirstOrDefault(y => y.GrpCd == x.GroupId)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc), DateTimeKind.Utc)  ,
                                        CreateId = currentKubunMstList.FirstOrDefault(y => y.GrpCd == x.GroupId)?.CreateId ?? 0,
                                        CreateMachine = currentKubunMstList.FirstOrDefault(y => y.GrpCd == x.GroupId)?.CreateMachine ?? string.Empty,
                                        UpdateDate = DateTime.UtcNow,
                                        UpdateId = TempIdentity.UserId,
                                        UpdateMachine = TempIdentity.ComputerName,
                                    }));
                                }
                            }
                            _tenantDataContextTracking.SaveChanges();
                            transaction.Commit();
                            result.Add(new(true, KubunSettingConstant.Successed));
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            result.Add(new(false, KubunSettingConstant.Failed));
                            return false;
                        }
                    }
                });
            return result;
        }

        #region RaiinKbn
        #region Add
        private (int, int, int, int) AddRaiinKubunDetail(int grpCd, List<RaiinKubunDetailModel> raiinKubunDetailModels, int currentKbnCd, int kouiKbnCd, int itemSeqNo, int yoyakuKbnCd)
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

                    kouiKbnCd = AddRaiinKbnKoui(currentKbnCd, grpCd, x.RaiinKbnKouiModels, kouiKbnCd);
                    itemSeqNo = AddRaiinKbItem(currentKbnCd, grpCd, x.RaiinKbnItemModels, itemSeqNo);
                    yoyakuKbnCd = AddRaiinKbnYayoku(currentKbnCd, grpCd, x.RaiinKbnYayokuModels, yoyakuKbnCd);
                });

                _tenantDataContextTracking.RaiinKbnDetails.AddRange(raiinKubunDetailModels.Select(x => new RaiinKbnDetail()
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
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName,
                }));
            }
            _tenantDataContextTracking.SaveChanges();
            return (currentKbnCd, kouiKbnCd, itemSeqNo, yoyakuKbnCd);
        }

        private int AddRaiinKbnKoui(int kbnCd, int grpCd, List<RaiinKbnKouiModel> raiinKbnKouiModels, int kouiKbnCd)
        {
            if (raiinKbnKouiModels != null && raiinKbnKouiModels.Any())
            {
                raiinKbnKouiModels.ForEach(x =>
                {
                    kouiKbnCd++;
                    x = new RaiinKbnKouiModel(x.HpId, x.GrpId, x.KbnCd, x.SeqNo, kouiKbnCd, x.IsDeleted);
                });

                _tenantDataContextTracking.RaiinKbnKouis.AddRange(raiinKbnKouiModels.Select(x => new RaiinKbnKoui()
                {
                    HpId = x.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    KouiKbnId = x.KouiKbnId,
                    IsDeleted = x.IsDeleted,
                    CreateDate = DateTime.UtcNow,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName,
                }));
            }
            _tenantDataContextTracking.SaveChanges();
            return kouiKbnCd;
        }

        private int AddRaiinKbItem(int kbnCd, int grpCd, List<RaiinKbnItemModel> raiinKbItemModels, int itemKbnCd)
        {
            if (raiinKbItemModels != null && raiinKbItemModels.Any())
            {
                raiinKbItemModels.ForEach(x =>
                {
                    itemKbnCd++;
                    x = new RaiinKbnItemModel(x.HpId, x.GrpCd, x.KbnCd, itemKbnCd, x.ItemCd, x.IsExclude, x.IsDeleted, x.SortNo);
                });

                _tenantDataContextTracking.RaiinKbItems.AddRange(raiinKbItemModels.Select(x => new RaiinKbItem()
                {
                    HpId = x.HpId,
                    GrpCd = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    ItemCd = x.ItemCd,
                    IsExclude = x.IsExclude,
                    IsDeleted = x.IsDeleted,
                    SortNo = x.SortNo,
                    CreateDate = DateTime.UtcNow,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName,
                }));
            }
            _tenantDataContextTracking.SaveChanges();
            return itemKbnCd;
        }

        private int AddRaiinKbnYayoku(int kbnCd, int grpCd, List<RaiinKbnYayokuModel> raiinKbnYayokuModels, int yoyakuCd)
        {
            if (raiinKbnYayokuModels != null && raiinKbnYayokuModels.Any())
            {
                raiinKbnYayokuModels.ForEach(x =>
                {
                    yoyakuCd++;
                    x = new RaiinKbnYayokuModel(x.HpId, x.KbnCd, x.SeqNo, yoyakuCd, x.IsDeleted);
                });
                _tenantDataContextTracking.RaiinKbnYayokus.AddRange(raiinKbnYayokuModels.Select(x => new RaiinKbnYayoku()
                {
                    HpId = x.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    YoyakuCd = x.YoyakuCd,
                    IsDeleted = x.IsDeleted,
                    CreateDate = DateTime.UtcNow,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName,
                }));
            }
            _tenantDataContextTracking.SaveChanges();
            return yoyakuCd;
        }
        #endregion

        #region Update
        private void UpdateRaiinKubunDetail(int grpCd, List<RaiinKubunDetailModel> raiinKubunDetailModels, List<RaiinKbnDetail> currentRaiinKubunDetails, List<RaiinKbnKoui> raiinKbnKouis, List<RaiinKbItem> raiinKbItems, List<RaiinKbnYayoku> raiinKbnYayokus, int kouiId, int itemSeqNo, int yoyakuId)
        {
            if (raiinKubunDetailModels != null && raiinKubunDetailModels.Any())
            {
                raiinKubunDetailModels.ForEach(x =>
                {
                    if (x.RaiinKbnKouiModels.Any(x => x.KouiKbnId == 0))
                    {
                        kouiId = AddRaiinKbnKoui(x.KubunCd, x.GroupId, x.RaiinKbnKouiModels, kouiId);
                    }
                    if (x.RaiinKbnKouiModels.Any(x => x.KouiKbnId != 0))
                    {
                        UpdateRaiinKbnKoui(x.KubunCd, grpCd, x.RaiinKbnKouiModels, raiinKbnKouis);
                    }
                    if (x.RaiinKbnItemModels.Any(x => x.SeqNo == 0))
                    {
                        itemSeqNo = AddRaiinKbItem(x.KubunCd, x.GroupId, x.RaiinKbnItemModels, itemSeqNo);
                    }
                    if (x.RaiinKbnItemModels.Any(x => x.SeqNo != 0))
                    {
                        UpdateRaiinKbItem(x.KubunCd, x.GroupId, x.RaiinKbnItemModels, raiinKbItems);
                    }
                    if (x.RaiinKbnYayokuModels.Any(x => x.YoyakuCd == 0))
                    {
                        yoyakuId = AddRaiinKbnYayoku(x.KubunCd, x.GroupId, x.RaiinKbnYayokuModels, yoyakuId);
                    }
                    if (x.RaiinKbnYayokuModels.Any(x => x.YoyakuCd != 0))
                    {
                        UpdateRaiinKbnYayoku(x.KubunCd, x.GroupId, x.RaiinKbnYayokuModels, raiinKbnYayokus);
                    }
                });
                _tenantDataContextTracking.RaiinKbnDetails.UpdateRange(raiinKubunDetailModels.Select(x => new RaiinKbnDetail()
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
                    CreateDate = DateTime.SpecifyKind(currentRaiinKubunDetails.FirstOrDefault(y => y.GrpCd == x.GroupId && y.KbnCd == x.KubunCd)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc) ,
                    CreateId = currentRaiinKubunDetails.FirstOrDefault(y => y.GrpCd == x.GroupId && y.KbnCd == x.KubunCd)?.CreateId ?? 0,
                    CreateMachine = currentRaiinKubunDetails.FirstOrDefault(y => y.GrpCd == x.GroupId && y.KbnCd == x.KubunCd)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = TempIdentity.UserId,
                    UpdateMachine = TempIdentity.ComputerName,
                }));
                _tenantDataContextTracking.SaveChanges();
            }
        }

        private void UpdateRaiinKbnKoui(int kbnCd, int grpCd, List<RaiinKbnKouiModel> raiinKbnKouiModels, List<RaiinKbnKoui> raiinKbnKouis)
        {
            if (raiinKbnKouiModels != null && raiinKbnKouiModels.Any())
            {
                _tenantDataContextTracking.RaiinKbnKouis.UpdateRange(raiinKbnKouiModels.Select(x => new RaiinKbnKoui()
                {
                    HpId = x.HpId,
                    GrpId = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    KouiKbnId = x.KouiKbnId,
                    IsDeleted = x.IsDeleted,
                    CreateDate = DateTime.SpecifyKind(raiinKbnKouis.FirstOrDefault(y => y.KouiKbnId == x.KouiKbnId)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc) ,
                    CreateId = raiinKbnKouis.FirstOrDefault(y => y.KouiKbnId == x.KouiKbnId)?.CreateId ?? 0,
                    CreateMachine = raiinKbnKouis.FirstOrDefault(y => y.KouiKbnId == x.KouiKbnId)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = TempIdentity.UserId,
                    UpdateMachine = TempIdentity.ComputerName,
                }));
            }
            _tenantDataContextTracking.SaveChanges();
        }

        private void UpdateRaiinKbItem(int kbnCd, int grpCd, List<RaiinKbnItemModel> raiinKbItemModels, List<RaiinKbItem> raiinKbItems)
        {
            if (raiinKbItemModels != null && raiinKbItemModels.Any())
            {
                _tenantDataContextTracking.RaiinKbItems.UpdateRange(raiinKbItemModels.Select(x => new RaiinKbItem()
                {
                    HpId = x.HpId,
                    GrpCd = grpCd,
                    KbnCd = kbnCd,
                    SeqNo = x.SeqNo,
                    ItemCd = x.ItemCd,
                    IsExclude = x.IsExclude,
                    IsDeleted = x.IsDeleted,
                    SortNo = x.SortNo,
                    CreateDate = DateTime.SpecifyKind(raiinKbItems.FirstOrDefault(y => y.SeqNo == x.SeqNo)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc) ,
                    CreateId = raiinKbItems.FirstOrDefault(y => y.SeqNo == x.SeqNo)?.CreateId ?? 0,
                    CreateMachine = raiinKbItems.FirstOrDefault(y => y.SeqNo == x.SeqNo)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = TempIdentity.UserId,
                    UpdateMachine = TempIdentity.ComputerName,
                }));
            }
            _tenantDataContextTracking.SaveChanges();
        }

        private void UpdateRaiinKbnYayoku(int kbnCd, int grpCd, List<RaiinKbnYayokuModel> raiinKbnYayokuModels, List<RaiinKbnYayoku> raiinKbnYayokus)
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
                    CreateDate = DateTime.SpecifyKind(raiinKbnYayokus.FirstOrDefault(y => y.YoyakuCd == x.YoyakuCd)?.CreateDate ?? DateTime.MinValue, DateTimeKind.Utc) ,
                    CreateId = raiinKbnYayokus.FirstOrDefault(y => y.YoyakuCd == x.YoyakuCd)?.CreateId ?? 0,
                    CreateMachine = raiinKbnYayokus.FirstOrDefault(y => y.YoyakuCd == x.YoyakuCd)?.CreateMachine ?? string.Empty,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = TempIdentity.UserId,
                    UpdateMachine = TempIdentity.ComputerName,
                }).ToList();
                _tenantDataContextTracking.RaiinKbnYayokus.UpdateRange(updateModel);
                _tenantDataContextTracking.SaveChanges();
            }
        }
        #endregion

        #region Validate
        private List<(bool, string)> ValidateRaiinKbnMst(List<RaiinKubunMstModel> raiinKubunMstModels, List<RaiinKbnMst> currentKubunMstList, List<RaiinKbnDetail> currentRaiinKubunDetails, List<RaiinKbnKoui> raiinKbnKouis, List<RaiinKbItem> raiinKbItems, List<RaiinKbnYayoku> raiinKbnYayokus)
        {
            List<(bool, string)> result = new List<(bool, string)>();

            if (raiinKubunMstModels.Any(x => string.IsNullOrEmpty(x.GroupName)))
            {
                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnMstGroupName));
                return result;
            }

            var currentRaiinKubunMsts = currentKubunMstList.Where(x => x.IsDeleted == 0).ToList();
            var currentSortNos = currentRaiinKubunMsts.Select(x => new Tuple<int, int>(x.GrpCd, x.SortNo)).ToList();
            var newSortNos = raiinKubunMstModels.Select(x => new Tuple<int, int>(x.GroupId, x.SortNo)).ToList();
            if (!ValidateSortNo(currentSortNos, newSortNos))
            {
                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnMstSortNo));
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
                result.Add(new(false, KubunSettingConstant.RaiinKbnMstNotExisted));
                return result;
            }

            foreach (var raiinKubunMst in raiinKubunMstModels)
            {
                if (raiinKubunMst.RaiinKubunDetailModels != null && raiinKubunMst.RaiinKubunDetailModels.Any())
                {
                    var raiinKubunDetailModels = raiinKubunMst.RaiinKubunDetailModels;
                    if (raiinKubunDetailModels.Any(x => string.IsNullOrEmpty(x.KubunName)))
                    {
                        result.Add(new(false, KubunSettingConstant.InvalidKubunName));
                        return result;
                    }

                    if (raiinKubunDetailModels.Any(x => x.GroupId != raiinKubunMst.GroupId))
                    {
                        result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnDetailGroupId));
                        return result;
                    }

                    var currentDetails = currentRaiinKubunDetails.Where(x => x.IsDeleted == 0 && x.GrpCd == raiinKubunMst.GroupId).ToList();
                    var currentRaiinKubunDetailSortNos = currentDetails.Select(x => new Tuple<int, int>(x.KbnCd, x.SortNo)).ToList();
                    var newSortRaiinKubunDetailNos = raiinKubunDetailModels.Select(x => new Tuple<int, int>(x.KubunCd, x.SortNo)).ToList();

                    if (!ValidateSortNo(currentRaiinKubunDetailSortNos, newSortRaiinKubunDetailNos))
                    {
                        result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnDetailSortNo));
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
                        result.Add(new(false, KubunSettingConstant.RaiinKbnDetailNotExisted));
                        return result;
                    }

                    foreach (var raiinKubunDetail in raiinKubunMst.RaiinKubunDetailModels)
                    {
                        if (raiinKubunDetail.RaiinKbnKouiModels != null && raiinKubunDetail.RaiinKbnKouiModels.Any())
                        {
                            var raiinKbnKouiModels = raiinKubunDetail.RaiinKbnKouiModels;

                            if (raiinKbnKouiModels.Any(x => x.KbnCd != raiinKubunDetail.KubunCd))
                            {
                                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnKouiKbnCd));
                                return result;
                            }

                            var currentRaiinKbnKouis = raiinKbnKouis.Where(x => x.IsDeleted == 0 && x.GrpId == raiinKubunDetail.GroupId && x.KbnCd == raiinKubunDetail.KubunCd).ToList();
                            var currentRaiinKbnKouiSortNos = currentRaiinKbnKouis.Select(x => new Tuple<int, int>(x.KouiKbnId, x.SeqNo)).ToList();
                            var newRaiinKbnKouiSortNos = raiinKbnKouiModels.Select(x => new Tuple<int, int>(x.KouiKbnId, x.SeqNo)).ToList();
                            if (!ValidateSortNo(currentRaiinKbnKouiSortNos, newRaiinKbnKouiSortNos))
                            {
                                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnKouiSortNo));
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
                                result.Add(new(false, KubunSettingConstant.RaiinKbnKouiNotExisted));
                                return result;
                            }
                        }
                        if (raiinKubunDetail.RaiinKbnItemModels != null && raiinKubunDetail.RaiinKbnItemModels.Any())
                        {
                            if (raiinKubunDetail.RaiinKbnItemModels.Any(x => x.KbnCd != raiinKubunDetail.KubunCd))
                            {
                                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnItemKbnCd));
                                return result;
                            }
                            var raiinKbItemModels = raiinKubunDetail.RaiinKbnItemModels;
                            if (raiinKbItemModels.Any(x => string.IsNullOrEmpty(x.ItemCd)))
                            {
                                result.Add(new(false, KubunSettingConstant.InvalidItemCD));
                                return result;
                            }

                            var currentRaiinKbnItems = raiinKbItems.Where(x => x.IsDeleted == 0 && x.GrpCd == raiinKubunDetail.GroupId && x.KbnCd == raiinKubunDetail.KubunCd).ToList();
                            var currentRaiinKbnItemSortNos = currentRaiinKbnItems.Select(x => new Tuple<int, int>((int)x.SeqNo, x.SortNo)).ToList();
                            var newRRaiinKbnItemSortNos = raiinKbItemModels.Select(x => new Tuple<int, int>((int)x.SeqNo, x.SortNo)).ToList();

                            if (!ValidateSortNo(currentRaiinKbnItemSortNos, newRRaiinKbnItemSortNos))
                            {
                                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnItemSortNo));
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
                                result.Add(new(false, KubunSettingConstant.RaiinKbnItemNotExisted));
                                return result;
                            }
                        }
                        if (raiinKubunDetail.RaiinKbnYayokuModels != null && raiinKubunDetail.RaiinKbnYayokuModels.Any())
                        {
                            var raiinKbnYayokuModels = raiinKubunDetail.RaiinKbnYayokuModels;
                            if (raiinKbnYayokuModels.Any(x => x.KbnCd != raiinKubunDetail.KubunCd))
                            {
                                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnYoyakuKbnCd));
                                return result;
                            }
                            var currentRaiinKbnYayokus = raiinKbnYayokus.Where(x => x.IsDeleted == 0 && x.GrpId == raiinKubunDetail.GroupId && x.KbnCd == raiinKubunDetail.KubunCd).ToList();
                            var currentRaiinKbnYayokuSortNos = currentRaiinKbnYayokus.Select(x => new Tuple<int, int>(x.YoyakuCd, (int)x.SeqNo)).ToList();
                            var newRaiinKbnYayokuSortNos = raiinKbnYayokuModels.Select(x => new Tuple<int, int>(x.YoyakuCd, (int)x.SeqNo)).ToList();

                            if (!ValidateSortNo(currentRaiinKbnYayokuSortNos, newRaiinKbnYayokuSortNos))
                            {
                                result.Add(new(false, KubunSettingConstant.InvalidRaiinKbnYayokuSortNo));
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
                                result.Add(new(false, KubunSettingConstant.RaiinKbnYayokuNotExisted));
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
    }
}
