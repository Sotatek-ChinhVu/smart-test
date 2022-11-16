using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories
{
    public class TodayOdrRepository : ITodayOdrRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        private readonly int headerOdrKouiKbn = 10;
        private readonly string jikanItemCd = "@JIKAN";
        private readonly string shinItemCd = "@SHIN";
        private readonly string shinItemName = "診察料基本点数算定用";
        private readonly string jikanItemName = "時間外算定用";
        private readonly int jikanRow = 2;
        private readonly int shinRow = 1;
        private readonly int rpEdaNoDefault = 1;
        private readonly int rpNoDefault = 1;
        private readonly int daysCntDefalt = 1;

        public TodayOdrRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId)
        {

            var executionStrategy = _tenantTrackingDataContext.Database.CreateExecutionStrategy();

            return executionStrategy.Execute(
                () =>
                {
                    using var transaction = _tenantTrackingDataContext.Database.BeginTransaction();
                    try
                    {
                        if (odrInfs.Count > 0)
                        {
                            UpsertOdrInfs(hpId, ptId, raiinNo, sinDate, odrInfs, userId);
                        }

                        SaveRaiinInf(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime, userId);

                        UpsertKarteInfs(karteInfModel, userId);

                        SaveRaiinListInf(odrInfs, userId);

                        SaveHeaderInf(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, userId);

                        transaction.Commit();

                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();

                        return false;
                    }
                });

        }

        private void SaveRaiinInf(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, int userId)
        {
            var raiinInf = _tenantTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);

            if (raiinInf != null)
            {
                raiinInf.SyosaisinKbn = syosaiKbn;
                raiinInf.JikanKbn = jikanKbn;
                raiinInf.HokenPid = hokenPid;
                raiinInf.SanteiKbn = santeiKbn;
                raiinInf.TantoId = tantoId;
                raiinInf.KaId = kaId;
                raiinInf.UketukeTime = uketukeTime;
                raiinInf.SinEndTime = sinEndTime;
                raiinInf.SinStartTime = sinStartTime;
                raiinInf.UpdateId = userId;
                raiinInf.UpdateDate = DateTime.UtcNow;
                _tenantTrackingDataContext.SaveChanges();
            }
        }

        private void SaveHeaderInf(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int userId)
        {

            var oldHeaderInfModel = _tenantTrackingDataContext.OdrInfs.FirstOrDefault(o => o.HpId == hpId && o.PtId == ptId && o.RaiinNo == raiinNo && o.SinDate == sinDate && o.OdrKouiKbn == 10);
            var oldoldSyosaiKihon = _tenantTrackingDataContext.OdrInfDetails.FirstOrDefault(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.ItemCd == ItemCdConst.SyosaiKihon);
            var oldJikanKihon = _tenantTrackingDataContext.OdrInfDetails.FirstOrDefault(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.ItemCd == ItemCdConst.JikanKihon);

            if (oldHeaderInfModel != null)
            {
                if (oldHeaderInfModel?.HokenPid == hokenPid &&
                oldoldSyosaiKihon?.Suryo == syosaiKbn &&
                oldJikanKihon?.Suryo == jikanKbn &&
                oldHeaderInfModel.SanteiKbn == santeiKbn)
                {
                    if (oldHeaderInfModel.IsDeleted == DeleteTypes.Deleted)
                    {
                        oldHeaderInfModel.UpdateDate = DateTime.UtcNow;
                        oldHeaderInfModel.UpdateId = userId;
                    }
                    oldHeaderInfModel.IsDeleted = 0;
                }
                else
                {
                    // Be sure old header is deleted
                    oldHeaderInfModel.IsDeleted = DeleteTypes.Deleted;
                    oldHeaderInfModel.UpdateDate = DateTime.UtcNow;
                    oldHeaderInfModel.UpdateId = userId;

                    var newHeaderInf = new OdrInf
                    {
                        HpId = hpId,
                        RaiinNo = raiinNo,
                        RpNo = oldHeaderInfModel.RpNo,
                        RpEdaNo = oldHeaderInfModel.RpEdaNo + 1,
                        PtId = ptId,
                        SinDate = sinDate,
                        HokenPid = hokenPid,
                        OdrKouiKbn = headerOdrKouiKbn,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId,
                        CreateId = userId,
                        DaysCnt = daysCntDefalt
                    };

                    var odrSyosaiKionDetail = new OdrInfDetail
                    {
                        HpId = hpId,
                        RaiinNo = raiinNo,
                        RpNo = newHeaderInf.RpNo,
                        RpEdaNo = newHeaderInf.RpEdaNo,
                        RowNo = shinRow,
                        PtId = ptId,
                        SinDate = sinDate,
                        SinKouiKbn = headerOdrKouiKbn,
                        ItemCd = shinItemCd,
                        ItemName = shinItemName,
                        Suryo = syosaiKbn
                    };

                    var odrJikanDetail = new OdrInfDetail
                    {
                        HpId = hpId,
                        RaiinNo = raiinNo,
                        RpNo = newHeaderInf.RpNo,
                        RpEdaNo = newHeaderInf.RpEdaNo,
                        RowNo = jikanRow,
                        PtId = ptId,
                        SinDate = sinDate,
                        SinKouiKbn = headerOdrKouiKbn,
                        ItemCd = jikanItemCd,
                        ItemName = jikanItemName,
                        Suryo = jikanKbn
                    };

                    _tenantTrackingDataContext.OdrInfs.Add(newHeaderInf);
                    _tenantTrackingDataContext.OdrInfDetails.Add(odrSyosaiKionDetail);
                    _tenantTrackingDataContext.OdrInfDetails.Add(odrJikanDetail);
                }
            }
            else
            {
                var newHeaderInf = new OdrInf
                {
                    HpId = hpId,
                    RaiinNo = raiinNo,
                    RpNo = rpNoDefault,
                    RpEdaNo = rpEdaNoDefault,
                    PtId = ptId,
                    SinDate = sinDate,
                    HokenPid = hokenPid,
                    OdrKouiKbn = headerOdrKouiKbn,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    CreateId = userId,
                    DaysCnt = daysCntDefalt

                };

                var odrSyosaiKionDetail = new OdrInfDetail
                {
                    HpId = hpId,
                    RaiinNo = raiinNo,
                    RpNo = newHeaderInf.RpNo,
                    RpEdaNo = rpEdaNoDefault,
                    RowNo = shinRow,
                    PtId = ptId,
                    SinDate = sinDate,
                    SinKouiKbn = headerOdrKouiKbn,
                    ItemCd = shinItemCd,
                    ItemName = shinItemName,
                    Suryo = syosaiKbn
                };

                var odrJikanDetail = new OdrInfDetail
                {
                    HpId = hpId,
                    RaiinNo = raiinNo,
                    RpNo = newHeaderInf.RpNo,
                    RpEdaNo = rpEdaNoDefault,
                    RowNo = jikanRow,
                    PtId = ptId,
                    SinDate = sinDate,
                    SinKouiKbn = headerOdrKouiKbn,
                    ItemCd = jikanItemCd,
                    ItemName = jikanItemName,
                    Suryo = jikanKbn
                };

                _tenantTrackingDataContext.OdrInfs.Add(newHeaderInf);
                _tenantTrackingDataContext.OdrInfDetails.Add(odrSyosaiKionDetail);
                _tenantTrackingDataContext.OdrInfDetails.Add(odrJikanDetail);
            }

            _tenantTrackingDataContext.SaveChanges();
        }

        private void SaveRaiinListInf(List<OrdInfModel> ordInfs, int userId)
        {
            // Check input list todayOdrInfModels
            if (ordInfs.Count == 0)
            {
                return;
            }

            int hpId = ordInfs[0].HpId;
            long raiinNo = ordInfs[0].RaiinNo;
            long ptId = ordInfs[0].PtId;
            int sinDate = ordInfs[0].SinDate;

            // Get Raiin List Inf
            var raiinListInfs = _tenantNoTrackingDataContext.RaiinListInfs.Where(item => item.HpId == hpId
                                                                && item.RaiinNo == raiinNo
                                                                && item.PtId == ptId
                                                                && item.SinDate == sinDate).ToList();

            // Get KouiKbnMst
            var kouiKbnMst = _tenantNoTrackingDataContext.KouiKbnMsts.ToList();

            // Get Raiin List
            var raiinListMstList = _tenantNoTrackingDataContext.RaiinListMsts.Where(item => item.IsDeleted == 0).ToList();
            var raiinListDetailList = _tenantNoTrackingDataContext.RaiinListDetails.Where(item => item.IsDeleted == 0).ToList();
            var raiinListKouiList = _tenantNoTrackingDataContext.RaiinListKouis.Where(item => item.IsDeleted == 0).ToList();
            var raiinListItemList = _tenantNoTrackingDataContext.RaiinListItems.Where(item => item.IsDeleted == 0).ToList();

            // Filter GrpId
            // Get all raiin list master contain item and koui
            List<string> itemList = new();
            List<int> kouiList = new();
            foreach (var odr in ordInfs)
            {
                foreach (var odrInfDetail in odr.OrdInfDetails)
                {
                    if (string.IsNullOrEmpty(odrInfDetail.ItemCd) || odrInfDetail.SinKouiKbn == 0) continue;
                    if (!itemList.Contains(odrInfDetail.ItemCd) && !string.IsNullOrEmpty(odrInfDetail.ItemCd))
                    {
                        itemList.Add(odrInfDetail.ItemCd);
                    }
                    if (!kouiList.Contains(odrInfDetail.SinKouiKbn))
                    {
                        kouiList.Add(odrInfDetail.SinKouiKbn);
                    }
                }
            }
            var kouiIdList = kouiKbnMst.FindAll(item => kouiList.Contains(item.KouiKbn1) || kouiList.Contains(item.KouiKbn2)).Select(item => item.KouiKbnId);
            var grpIdByItemList = raiinListItemList.FindAll(item => itemList.Contains(item.ItemCd)).Select(item => item.GrpId).Distinct().ToList();
            var grpIdByKouiKbnList = raiinListKouiList.FindAll(item => kouiIdList.Contains(item.KouiKbnId)).Select(item => item.GrpId).Distinct().ToList();
            grpIdByItemList.AddRange(grpIdByKouiKbnList);
            raiinListMstList = raiinListMstList.FindAll(item => grpIdByItemList.Distinct().Contains(item.GrpId));

            // Define Added RaiinListInf
            List<RaiinListInf> raiinListInfList = new();
            bool IsDeleteExecute = false;
            // Process to raiin list inf
            foreach (var mst in raiinListMstList)
            {
                var detailList = raiinListDetailList.FindAll(item => item.GrpId == mst.GrpId);
                foreach (var detail in detailList)
                {
                    HashSet<int> deleteKouiSet = new();
                    HashSet<int> currentKouiSet = new();
                    HashSet<string> deleteItemCdSet = new();
                    HashSet<string> currentItemCdSet = new();
                    var raiinListKouis = raiinListKouiList.FindAll(item => item.GrpId == detail.GrpId && item.KbnCd == detail.KbnCd);
                    var raiinListItems = raiinListItemList.FindAll(item => item.GrpId == detail.GrpId && item.KbnCd == detail.KbnCd);
                    var raiinListItemExclude = raiinListItemList.FindAll(item => item.GrpId == detail.GrpId && item.KbnCd == detail.KbnCd && item.IsExclude == 1).Select(item => item.ItemCd).ToList();
                    foreach (var odr in ordInfs)
                    {
                        foreach (var odrInfDetail in odr.OrdInfDetails)
                        {
                            if (string.IsNullOrEmpty(odrInfDetail.ItemCd) || odrInfDetail.SinKouiKbn == 0) continue;
                            if (odr.IsDeleted != 0 || raiinListItemExclude.Contains(odrInfDetail.ItemCd))
                            {
                                deleteKouiSet.Add(odrInfDetail.SinKouiKbn);
                                deleteItemCdSet.Add(odrInfDetail.ItemCd);
                                continue;
                            }
                            currentKouiSet.Add(odrInfDetail.SinKouiKbn);
                            currentItemCdSet.Add(odrInfDetail.ItemCd);
                        }
                    }

                    // Delete with SinKouiKbn
                    foreach (int koui in deleteKouiSet.ToArray())
                    {
                        if (currentKouiSet.Contains(koui))
                        {
                            continue;
                        }
                        var kouiMst = kouiKbnMst?.Find(item => item.KouiKbn1 == koui || item.KouiKbn2 == koui) ?? new KouiKbnMst();
                        if (kouiMst == null) continue;

                        // Get List RaiinListKoui contains koui 
                        List<RaiinListKoui> kouiItemList = raiinListKouis.FindAll(item => item.KouiKbnId == kouiMst.KouiKbnId);
                        foreach (RaiinListKoui kouiItem in kouiItemList)
                        {
                            var raiinListInf = raiinListInfs?.Find(item => item.GrpId == kouiItem.GrpId
                                                                                && item.KbnCd == kouiItem.KbnCd
                                                                                && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN);
                            if (raiinListInf != null)
                            {
                                _tenantTrackingDataContext.RaiinListInfs.Remove(raiinListInf);
                                raiinListInfs?.Remove(raiinListInf);
                                IsDeleteExecute = true;
                            }
                        }
                    }

                    // Delete with ItemCd
                    foreach (string itemCd in deleteItemCdSet.ToArray())
                    {
                        if (currentItemCdSet.Contains(itemCd))
                        {
                            continue;
                        }
                        List<RaiinListItem> itemCdList = raiinListItems.FindAll(item => item.ItemCd == itemCd);
                        foreach (RaiinListItem raiinListItem in itemCdList)
                        {
                            var raiinListInf = raiinListInfs?.Find(item => item.GrpId == raiinListItem.GrpId
                                                                           && item.KbnCd == raiinListItem.KbnCd
                                                                           && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN);
                            if (raiinListInf != null)
                            {
                                _tenantTrackingDataContext.RaiinListInfs.Remove(raiinListInf);
                                raiinListInfs?.Remove(raiinListInf);
                                IsDeleteExecute = true;
                            }
                        }
                    }


                    // Add or Update with ItemCd
                    foreach (string itemCd in currentItemCdSet.ToArray())
                    {
                        List<RaiinListItem> itemCdList = raiinListItems.FindAll(item => item.ItemCd == itemCd);
                        foreach (RaiinListItem raiinListItem in itemCdList)
                        {
                            var raiinListInf = raiinListInfs?.Find(item => item.GrpId == raiinListItem.GrpId && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN) ?? new RaiinListInf();
                            if (raiinListInf == null)
                            {
                                // Check contains with grpId
                                if (raiinListInfList.Find(item => item.RaiinNo == raiinNo
                                                                 && item.SinDate == sinDate
                                                                 && item.GrpId == raiinListItem.GrpId
                                                                 && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN) == null)
                                {
                                    // create new 
                                    raiinListInf = new RaiinListInf()
                                    {
                                        HpId = hpId,
                                        PtId = ptId,
                                        RaiinNo = raiinNo,
                                        SinDate = sinDate,
                                        GrpId = raiinListItem.GrpId,
                                        KbnCd = raiinListItem.KbnCd,
                                        UpdateDate = DateTime.Now,
                                        UpdateId = userId,
                                        RaiinListKbn = RaiinListKbnConstants.ITEM_KBN
                                    };
                                    raiinListInfList.Add(raiinListInf);
                                }
                            }
                            else
                            {
                                // update
                                var originSortNo = detailList.Find(item => item.KbnCd == raiinListInf.KbnCd)?.SortNo;
                                var newSortNo = detailList.Find(item => item.KbnCd == raiinListItem.KbnCd)?.SortNo;
                                if (originSortNo == null || originSortNo > newSortNo)
                                {
                                    raiinListInf.KbnCd = raiinListItem.KbnCd;
                                    raiinListInf.UpdateDate = DateTime.Now;
                                    raiinListInf.UpdateId = userId;
                                }
                            }
                        }
                    }

                    // Add or Update with SinKouiKbn
                    foreach (int koui in currentKouiSet.ToArray())
                    {
                        var kouiMst = kouiKbnMst?.Find(item => item.KouiKbn1 == koui || item.KouiKbn2 == koui) ?? new KouiKbnMst();
                        if (kouiMst == null) continue;

                        List<RaiinListKoui> kouiItemList = raiinListKouis.FindAll(item => item.KouiKbnId == kouiMst.KouiKbnId);
                        foreach (RaiinListKoui kouiItem in kouiItemList)
                        {
                            var raiinListInf = raiinListInfs?.Find(item => item.GrpId == kouiItem.GrpId && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN) ?? new RaiinListInf();
                            if (raiinListInf == null)
                            {
                                // Check contains with grpId
                                if (raiinListInfList.Find(item => item.RaiinNo == raiinNo
                                                                 && item.SinDate == sinDate
                                                                 && item.GrpId == kouiItem.GrpId
                                                                 && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN) == null)
                                {
                                    // create new 
                                    raiinListInf = new RaiinListInf()
                                    {
                                        HpId = hpId,
                                        PtId = ptId,
                                        RaiinNo = raiinNo,
                                        SinDate = sinDate,
                                        GrpId = kouiItem.GrpId,
                                        KbnCd = kouiItem.KbnCd,
                                        UpdateDate = DateTime.Now,
                                        UpdateId = userId,
                                        RaiinListKbn = RaiinListKbnConstants.KOUI_KBN
                                    };
                                    raiinListInfList.Add(raiinListInf);
                                }
                            }
                            else
                            {
                                // update
                                var originSortNo = detailList.Find(item => item.KbnCd == raiinListInf.KbnCd)?.SortNo;
                                var newSortNo = detailList.Find(item => item.KbnCd == kouiItem.KbnCd)?.SortNo;
                                if (originSortNo == null || originSortNo > newSortNo)
                                {
                                    raiinListInf.KbnCd = kouiItem.KbnCd;
                                    raiinListInf.UpdateDate = DateTime.Now;
                                    raiinListInf.UpdateId = userId;
                                }
                            }
                        }
                    }
                }
            }
            if (raiinListInfList.Count > 0 || IsDeleteExecute)
            {
                _tenantTrackingDataContext.RaiinListInfs.AddRange(raiinListInfList);
                _tenantTrackingDataContext.SaveChanges();
            }
        }

        private void UpsertOdrInfs(int hpId, long ptId, long raiinNo, int sinDate, List<OrdInfModel> ordInfs, int userId)
        {
            var rpNoMax = GetMaxRpNo(hpId, ptId, raiinNo, sinDate);
            rpNoMax = rpNoMax < 2 ? 1 : rpNoMax;
            foreach (var item in ordInfs)
            {
                if (item.IsDeleted == DeleteTypes.Deleted)
                {
                    var ordInfo = _tenantTrackingDataContext.OdrInfs.FirstOrDefault(o => o.HpId == item.HpId && o.PtId == item.PtId && o.Id == item.Id && o.RaiinNo == item.RaiinNo && o.RpNo == item.RpNo && o.RpEdaNo == item.RpEdaNo);
                    if (ordInfo != null)
                    {
                        ordInfo.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var ordInf = _tenantTrackingDataContext.OdrInfs.FirstOrDefault(o => o.HpId == item.HpId && o.PtId == item.PtId && o.Id == item.Id && o.RaiinNo == item.RaiinNo && o.RpNo == item.RpNo && o.RpEdaNo == item.RpEdaNo);

                    if (ordInf == null)
                    {
                        rpNoMax++;
                        var ordInfEntity = new OdrInf
                        {
                            HpId = item.HpId,
                            PtId = item.PtId,
                            SinDate = item.SinDate,
                            RaiinNo = item.RaiinNo,
                            RpNo = rpNoMax,
                            RpEdaNo = 1,
                            Id = 0,
                            HokenPid = item.HokenPid,
                            OdrKouiKbn = item.OdrKouiKbn,
                            RpName = item.RpName,
                            InoutKbn = item.InoutKbn,
                            SikyuKbn = item.SikyuKbn,
                            SyohoSbt = item.SyohoSbt,
                            SanteiKbn = item.SanteiKbn,
                            TosekiKbn = item.TosekiKbn,
                            DaysCnt = item.DaysCnt,
                            SortNo = item.SortNo,
                            IsDeleted = item.IsDeleted,
                            CreateDate = DateTime.UtcNow,
                            CreateId = userId,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId
                        };

                        var ordInfDetailEntity = item?.OrdInfDetails.Select(
                                od => new OdrInfDetail
                                {
                                    HpId = od.HpId,
                                    PtId = od.PtId,
                                    SinDate = od.SinDate,
                                    RaiinNo = od.RaiinNo,
                                    RpNo = ordInfEntity.RpNo,
                                    RpEdaNo = 1,
                                    RowNo = od.RowNo,
                                    SinKouiKbn = od.SinKouiKbn,
                                    ItemCd = od.ItemCd,
                                    ItemName = od.ItemName,
                                    Suryo = od.Suryo,
                                    UnitName = od.UnitName,
                                    UnitSBT = od.UnitSbt,
                                    TermVal = od.TermVal,
                                    KohatuKbn = od.KohatuKbn,
                                    SyohoKbn = od.SyohoKbn,
                                    SyohoLimitKbn = od.SyohoLimitKbn,
                                    DrugKbn = od.DrugKbn,
                                    YohoKbn = od.YohoKbn,
                                    Kokuji1 = od.Kokuji1,
                                    Kokiji2 = od.Kokuji2,
                                    IsNodspRece = od.IsNodspRece,
                                    IpnCd = od.IpnCd,
                                    IpnName = od.IpnName,
                                    JissiKbn = od.JissiKbn,
                                    JissiDate = od.JissiDate,
                                    JissiId = od.JissiId,
                                    JissiMachine = od.JissiMachine,
                                    ReqCd = od.ReqCd,
                                    Bunkatu = od.Bunkatu,
                                    CmtName = od.CmtName,
                                    CmtOpt = od.CmtOpt,
                                    FontColor = od.FontColor,
                                    CommentNewline = od.CommentNewline
                                }
                            ) ?? new List<OdrInfDetail>();
                        _tenantTrackingDataContext.OdrInfs.Add(ordInfEntity);
                        _tenantTrackingDataContext.OdrInfDetails.AddRange(ordInfDetailEntity);
                    }
                    else
                    {
                        ordInf.IsDeleted = DeleteTypes.Deleted;
                        var ordInfEntity = new OdrInf
                        {
                            HpId = item.HpId,
                            PtId = item.PtId,
                            SinDate = item.SinDate,
                            RaiinNo = item.RaiinNo,
                            RpNo = item.RpNo,
                            RpEdaNo = item.RpEdaNo + 1,
                            Id = item.Id,
                            HokenPid = item.HokenPid,
                            OdrKouiKbn = item.OdrKouiKbn,
                            RpName = item.RpName,
                            InoutKbn = item.InoutKbn,
                            SikyuKbn = item.SikyuKbn,
                            SyohoSbt = item.SyohoSbt,
                            SanteiKbn = item.SanteiKbn,
                            TosekiKbn = item.TosekiKbn,
                            DaysCnt = item.DaysCnt,
                            SortNo = item.SortNo,
                            IsDeleted = item.IsDeleted,
                            CreateDate = DateTime.UtcNow,
                            CreateId = userId,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId
                        };

                        var ordInfDetailEntity = item?.OrdInfDetails.Select(
                                od => new OdrInfDetail
                                {
                                    HpId = od.HpId,
                                    PtId = od.PtId,
                                    SinDate = od.SinDate,
                                    RaiinNo = od.RaiinNo,
                                    RpNo = od.RpNo,
                                    RpEdaNo = ordInfEntity.RpEdaNo,
                                    RowNo = od.RowNo,
                                    SinKouiKbn = od.SinKouiKbn,
                                    ItemCd = od.ItemCd,
                                    ItemName = od.ItemName,
                                    Suryo = od.Suryo,
                                    UnitName = od.UnitName,
                                    UnitSBT = od.UnitSbt,
                                    TermVal = od.TermVal,
                                    KohatuKbn = od.KohatuKbn,
                                    SyohoKbn = od.SyohoKbn,
                                    SyohoLimitKbn = od.SyohoLimitKbn,
                                    DrugKbn = od.DrugKbn,
                                    YohoKbn = od.YohoKbn,
                                    Kokuji1 = od.Kokuji1,
                                    Kokiji2 = od.Kokuji2,
                                    IsNodspRece = od.IsNodspRece,
                                    IpnCd = od.IpnCd,
                                    IpnName = od.IpnName,
                                    JissiKbn = od.JissiKbn,
                                    JissiDate = od.JissiDate,
                                    JissiId = od.JissiId,
                                    JissiMachine = od.JissiMachine,
                                    ReqCd = od.ReqCd,
                                    Bunkatu = od.Bunkatu,
                                    CmtName = od.CmtName,
                                    CmtOpt = od.CmtOpt,
                                    FontColor = od.FontColor,
                                    CommentNewline = od.CommentNewline
                                }
                            ) ?? new List<OdrInfDetail>();
                        _tenantTrackingDataContext.OdrInfs.Add(ordInfEntity);
                        _tenantTrackingDataContext.OdrInfDetails.AddRange(ordInfDetailEntity);
                    }
                }
            }

            _tenantTrackingDataContext.SaveChanges();
        }

        private void UpsertKarteInfs(KarteInfModel karte, int userId)
        {
            int hpId = karte.HpId;
            long ptId = karte.PtId;
            long raiinNo = karte.RaiinNo;
            int karteKbn = karte.KarteKbn;

            var seqNo = GetMaxSeqNo(ptId, hpId, raiinNo, karteKbn) + 1;

            if (karte.IsDeleted == DeleteTypes.Deleted)
            {
                var karteMst = _tenantTrackingDataContext.KarteInfs.FirstOrDefault(o => o.HpId == karte.HpId && o.PtId == karte.PtId && o.RaiinNo == karte.RaiinNo && karte.KarteKbn == o.KarteKbn);
                if (karteMst != null)
                {
                    karteMst.IsDeleted = DeleteTypes.Deleted;
                }
            }
            else
            {
                var karteMst = _tenantTrackingDataContext.KarteInfs.OrderByDescending(k => k.SeqNo).FirstOrDefault(o => o.HpId == karte.HpId && o.PtId == karte.PtId && o.RaiinNo == karte.RaiinNo && karte.KarteKbn == o.KarteKbn && karte.IsDeleted == DeleteTypes.None);

                if (karteMst == null)
                {
                    if (!string.IsNullOrEmpty(karte.Text) && !string.IsNullOrEmpty(karte.RichText))
                    {
                        var karteEntity = new KarteInf
                        {
                            HpId = karte.HpId,
                            PtId = karte.PtId,
                            SinDate = karte.SinDate,
                            RaiinNo = karte.RaiinNo,
                            KarteKbn = karte.KarteKbn,
                            SeqNo = seqNo,
                            Text = karte.Text,
                            RichText = Encoding.UTF8.GetBytes(karte.RichText),
                            IsDeleted = karte.IsDeleted,
                            CreateDate = DateTime.UtcNow,
                            CreateId = userId,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId
                        };

                        _tenantTrackingDataContext.KarteInfs.Add(karteEntity);
                    }
                }
                else
                {
                    if (karte.Text != karteMst.Text && Encoding.UTF8.GetBytes(karte.RichText) != karteMst.RichText)
                    {
                        karteMst.IsDeleted = DeleteTypes.Deleted;

                        var karteEntity = new KarteInf
                        {
                            HpId = karte.HpId,
                            PtId = karte.PtId,
                            SinDate = karte.SinDate,
                            RaiinNo = karte.RaiinNo,
                            KarteKbn = karte.KarteKbn,
                            SeqNo = seqNo,
                            Text = karte.Text,
                            RichText = Encoding.UTF8.GetBytes(karte.RichText),
                            IsDeleted = karte.IsDeleted,
                            CreateDate = DateTime.UtcNow,
                            CreateId = userId,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = userId
                        };

                        _tenantTrackingDataContext.KarteInfs.Add(karteEntity);
                    }
                }

                var karteImgs = _tenantTrackingDataContext.KarteImgInfs.Where(k => k.HpId == karte.HpId && k.PtId == karte.PtId && karte.RichText.Contains(k.FileName) && k.RaiinNo == 0);
                foreach (var img in karteImgs)
                {
                    img.RaiinNo = karte.RaiinNo;
                }
            }

            _tenantTrackingDataContext.SaveChanges();
        }

        private long GetMaxSeqNo(long ptId, int hpId, long raiinNo, int karteKbn)
        {
            var karteInf = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.HpId == hpId && k.RaiinNo == raiinNo && k.KarteKbn == karteKbn && k.PtId == ptId).OrderByDescending(k => k.SeqNo).FirstOrDefault();

            return karteInf != null ? karteInf.SeqNo : 0;
        }

        private long GetMaxRpNo(int hpId, long ptId, long raiinNo, int sinDate)
        {
            var odrList = _tenantNoTrackingDataContext.OdrInfs
            .Where(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate);

            if (odrList.Any())
            {
                return odrList.Max(odr => odr.RpNo);
            }

            return 0;
        }

        ///<summary>
        ///指定の月数後の日付を取得する
        ///休日の場合は、その前の休日以外の日付
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数後の休日以外の日付</returns>
        public int MonthsAfterExcludeHoliday(int hpId, int baseDate, int term)
        {
            int retDate = CIUtil.MonthsAfter(baseDate, term);

            DateTime? dt;
            DateTime dt1;

            dt = CIUtil.SDateToDateTime(retDate);
            if (dt != null)
            {
                dt1 = (DateTime)dt;
                int i = 1;

                while (IsHoliday(hpId, CIUtil.DateTimeToInt(dt1)) == true)
                {
                    // 休日の場合、1日前に移動
                    dt1 = dt1.AddDays(-1);
                    i++;
                    if (i > 31)
                    {
                        break;
                    }
                }
                retDate = CIUtil.DateTimeToInt(dt1);
            }
            return retDate;
        }

        private bool IsHoliday(int hpId, int datetime)
        {
            var holidayMst = _tenantNoTrackingDataContext.HolidayMsts
                .Where(p =>
                    p.HpId == hpId &&
                    p.SinDate == datetime &&
                    p.HolidayKbn > 0 &&
                    p.KyusinKbn > 0 &&
                    p.IsDeleted != DeleteTypes.Deleted)
                .FirstOrDefault();
            return holidayMst != null;
        }

        /// <summary>
        /// 指定の期間に指定の項目が何回算定されているかカウントする
        /// ※複数項目用
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">カウント開始日</param>
        /// <param name="endDate">カウント終了日</param>
        /// <param name="sinDate">診療日（除外する日）</param>
        /// <param name="itemCds">カウントする項目のリスト</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定回数</returns>
        public double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;

            List<int> checkHokenKbn = new List<int>();

            if (hokenKbns != null)
            {
                checkHokenKbn = hokenKbns;
            }

            List<int> checkSanteiKbn = new List<int>();

            if (santeiKbns != null)
            {
                checkSanteiKbn = santeiKbns;
            }

            var sinRpInfs = _tenantNoTrackingDataContext.SinRpInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantNoTrackingDataContext.SinKouiCounts.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.RaiinNo != raiinNo);
            var sinKouiDetails = _tenantNoTrackingDataContext.SinKouiDetails.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                itemCds.Contains(p.ItemCd ?? string.Empty) &&
                p.FmtKbn != 10  // 在がん医総のダミー項目を除く
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd ?? string.Empty) &&
                    sinKouiCount.SinDate >= startDate &&
                    sinKouiCount.SinDate <= endDate &&
                    sinKouiCount.RaiinNo != raiinNo
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(a.sinKouiDetail.ItemCd ?? string.Empty) ? 1 : a.sinKouiDetail.Suryo)) }
            );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault()?.sum ?? 0;
            }
            else
            {
                return 0;
            }
        }

        public List<DensiSanteiKaisuModel> FindDensiSanteiKaisuList(int hpId, List<string> itemCds, int minSinDate, int maxSinDate)
        {
            List<int> unitCds = new List<int> { 53, 121, 131, 138, 141, 142, 143, 144, 145, 146, 147, 148, 997, 998, 999 };

            var entities = _tenantNoTrackingDataContext.DensiSanteiKaisus.Where((x) =>
                    x.HpId == hpId &&
                    itemCds.Contains(x.ItemCd) &&
                    x.StartDate <= minSinDate &&
                    x.EndDate >= maxSinDate &&
                    x.IsInvalid == 0 &&
                    unitCds.Contains(x.UnitCd)
                ).ToList();

            List<DensiSanteiKaisuModel> results = new List<DensiSanteiKaisuModel>();
            entities?.ForEach(entity =>
            {
                results.Add(new DensiSanteiKaisuModel(entity.Id, entity.HpId, entity.ItemCd, entity.UnitCd, entity.MaxCount, entity.SpJyoken, entity.StartDate, entity.EndDate, entity.SeqNo, entity.UserSetting, entity.TargetKbn, entity.TermCount, entity.TermSbt, entity.IsInvalid, entity.ItemGrpCd));
            });

            return results;
        }
    }
}
