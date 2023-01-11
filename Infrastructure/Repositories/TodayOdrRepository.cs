using Domain.Models.Diseases;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKubunMst;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Repositories
{
    public class TodayOdrRepository : RepositoryBase, ITodayOdrRepository
    {
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

        private const string SUSPECT_FLAG = "の疑い";

        public TodayOdrRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId)
        {

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();

            return executionStrategy.Execute(
                () =>
                {
                    using var transaction = TrackingDataContext.Database.BeginTransaction();
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

        public List<(string, string, List<CheckedDiseaseModel>)> GetCheckDiseases(int hpId, int sinDate, List<PtDiseaseModel> todayByomeis, List<OrdInfModel> todayOdrs)
        {
            var ptByomeis = todayByomeis.Where(p => p.IsDeleted == DeleteTypes.None && p.IsInMonth);
            List<OrdInfDetailModel> drugOrders = new List<OrdInfDetailModel>();
            int odrCount = 0;
            var checkedDiseases = new List<(string, string, List<CheckedDiseaseModel>)>();

            foreach (var order in todayOdrs)
            {
                foreach (var odrDetail in order.OrdInfDetails)
                {
                    string itemCd = odrDetail.ItemCd;
                    if (string.IsNullOrEmpty(itemCd) ||
                        itemCd == ItemCdConst.Con_TouyakuOrSiBunkatu ||
                        itemCd == ItemCdConst.Con_Refill) continue;

                    string santeiItemCd = GetSanteiItemCd(hpId, itemCd, sinDate);

                    var byomeisByOdr = GetTekiouByomeiByOrder(hpId, new List<string>() { itemCd, santeiItemCd });
                    if (byomeisByOdr.Count == 0) continue;

                    // No.6510 future byomei check
                    List<string> byomeiCds = byomeisByOdr.Select(p => p.ByomeiMst.ByomeiCd).ToList();
                    if (!drugOrders.Exists(p => p.ItemCd == odrDetail.ItemCd)
                        && !ptByomeis.Where(p => (p.HokenPid == 0 || p.HokenPid == order.HokenPid)
                        && p.StartDate <= sinDate && (!p.IsTenki || p.TenkiDate >= sinDate)
                        && (!odrDetail.IsDrug || !p.Byomei.AsString().Contains(SUSPECT_FLAG)))
                        .Any(p => byomeiCds.Contains(p.ByomeiCd)))
                    {
                        //set item name for grid mode
                        odrCount++;
                        List<CheckedDiseaseModel> byomeis = new();
                        foreach (var byomei in byomeisByOdr)
                        {
                            var byomeiModify = new CheckedDiseaseModel(byomei.SikkanCd, byomei.NanByoCd, byomei.Byomei, odrCount, byomei.PtDiseaseModel, byomei.ByomeiMst);
                            byomeis.Add(byomeiModify);
                        }
                        checkedDiseases.Add((odrDetail.ItemCd, odrDetail.ItemName, byomeis));
                    }
                }
            }

            return checkedDiseases;
        }



        private void SaveRaiinInf(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, int userId)
        {
            var raiinInf = TrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);

            if (raiinInf != null)
            {
                raiinInf.Status = 7; // temperaror with status 7
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
                TrackingDataContext.SaveChanges();
            }
        }

        private void SaveHeaderInf(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int userId)
        {

            var oldHeaderInfModel = TrackingDataContext.OdrInfs.FirstOrDefault(o => o.HpId == hpId && o.PtId == ptId && o.RaiinNo == raiinNo && o.SinDate == sinDate && o.OdrKouiKbn == 10);
            var oldoldSyosaiKihon = TrackingDataContext.OdrInfDetails.FirstOrDefault(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.ItemCd == ItemCdConst.SyosaiKihon);
            var oldJikanKihon = TrackingDataContext.OdrInfDetails.FirstOrDefault(odr => odr.HpId == hpId && odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.ItemCd == ItemCdConst.JikanKihon);

            if (oldHeaderInfModel != null)
            {
                if (oldHeaderInfModel.HokenPid == hokenPid &&
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

                    TrackingDataContext.OdrInfs.Add(newHeaderInf);
                    TrackingDataContext.OdrInfDetails.Add(odrSyosaiKionDetail);
                    TrackingDataContext.OdrInfDetails.Add(odrJikanDetail);
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

                TrackingDataContext.OdrInfs.Add(newHeaderInf);
                TrackingDataContext.OdrInfDetails.Add(odrSyosaiKionDetail);
                TrackingDataContext.OdrInfDetails.Add(odrJikanDetail);
            }

            TrackingDataContext.SaveChanges();
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
            var raiinListInfs = NoTrackingDataContext.RaiinListInfs.Where(item => item.HpId == hpId
                                                                && item.RaiinNo == raiinNo
                                                                && item.PtId == ptId
                                                                && item.SinDate == sinDate).ToList();

            // Get KouiKbnMst
            var kouiKbnMst = NoTrackingDataContext.KouiKbnMsts.ToList();

            // Get Raiin List
            var raiinListMstList = NoTrackingDataContext.RaiinListMsts.Where(item => item.IsDeleted == 0).ToList();
            var raiinListDetailList = NoTrackingDataContext.RaiinListDetails.Where(item => item.IsDeleted == 0).ToList();
            var raiinListKouiList = NoTrackingDataContext.RaiinListKouis.Where(item => item.IsDeleted == 0).ToList();
            var raiinListItemList = NoTrackingDataContext.RaiinListItems.Where(item => item.IsDeleted == 0).ToList();

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
                                TrackingDataContext.RaiinListInfs.Remove(raiinListInf);
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
                                TrackingDataContext.RaiinListInfs.Remove(raiinListInf);
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
                TrackingDataContext.RaiinListInfs.AddRange(raiinListInfList);
                TrackingDataContext.SaveChanges();
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
                    var ordInfo = TrackingDataContext.OdrInfs.FirstOrDefault(o => o.HpId == item.HpId && o.PtId == item.PtId && o.Id == item.Id && o.RaiinNo == item.RaiinNo && o.RpNo == item.RpNo && o.RpEdaNo == item.RpEdaNo);
                    if (ordInfo != null)
                    {
                        ordInfo.IsDeleted = DeleteTypes.Deleted;
                    }
                }
                else
                {
                    var ordInf = TrackingDataContext.OdrInfs.FirstOrDefault(o => o.HpId == item.HpId && o.PtId == item.PtId && o.Id == item.Id && o.RaiinNo == item.RaiinNo && o.RpNo == item.RpNo && o.RpEdaNo == item.RpEdaNo);

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
                        TrackingDataContext.OdrInfs.Add(ordInfEntity);
                        TrackingDataContext.OdrInfDetails.AddRange(ordInfDetailEntity);
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
                        TrackingDataContext.OdrInfs.Add(ordInfEntity);
                        TrackingDataContext.OdrInfDetails.AddRange(ordInfDetailEntity);
                    }
                }
            }

            TrackingDataContext.SaveChanges();
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
                var karteMst = TrackingDataContext.KarteInfs.FirstOrDefault(o => o.HpId == karte.HpId && o.PtId == karte.PtId && o.RaiinNo == karte.RaiinNo && karte.KarteKbn == o.KarteKbn);
                if (karteMst != null)
                {
                    karteMst.IsDeleted = DeleteTypes.Deleted;
                }
            }
            else
            {
                var karteMst = TrackingDataContext.KarteInfs.OrderByDescending(k => k.SeqNo).FirstOrDefault(o => o.HpId == karte.HpId && o.PtId == karte.PtId && o.RaiinNo == karte.RaiinNo && karte.KarteKbn == o.KarteKbn && karte.IsDeleted == DeleteTypes.None);

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

                        TrackingDataContext.KarteInfs.Add(karteEntity);
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

                        TrackingDataContext.KarteInfs.Add(karteEntity);
                    }
                }

                var karteImgs = TrackingDataContext.KarteImgInfs.Where(k => k.HpId == karte.HpId && k.PtId == karte.PtId && karte.RichText.Contains(k.FileName ?? string.Empty) && k.RaiinNo == 0);
                foreach (var img in karteImgs)
                {
                    img.RaiinNo = karte.RaiinNo;
                }
            }

            TrackingDataContext.SaveChanges();
        }

        private long GetMaxSeqNo(long ptId, int hpId, long raiinNo, int karteKbn)
        {
            var karteInf = NoTrackingDataContext.KarteInfs.Where(k => k.HpId == hpId && k.RaiinNo == raiinNo && k.KarteKbn == karteKbn && k.PtId == ptId).OrderByDescending(k => k.SeqNo).FirstOrDefault();

            return karteInf != null ? karteInf.SeqNo : 0;
        }

        private long GetMaxRpNo(int hpId, long ptId, long raiinNo, int sinDate)
        {
            var odrList = NoTrackingDataContext.OdrInfs
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
            var holidayMst = NoTrackingDataContext.HolidayMsts
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

            var sinRpInfs = NoTrackingDataContext.SinRpInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                checkSanteiKbn.Contains(o.SanteiKbn)
            ).AsQueryable();
            var count = sinRpInfs.Count();
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.RaiinNo != raiinNo);
            var count1 = sinKouiCounts.Count();
            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                itemCds.Contains(p.ItemCd ?? string.Empty) &&
                p.FmtKbn != 10  // 在がん医総のダミー項目を除く
                );
            var count2 = sinKouiDetails.Count();

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

            var entities = NoTrackingDataContext.DensiSanteiKaisus.Where((x) =>
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

        private string GetSanteiItemCd(int hpId, string itemCd, int sinDate)
        {
            var tenMst = NoTrackingDataContext.TenMsts.FirstOrDefault(p => p.HpId == hpId &&
                                                                                  p.ItemCd == itemCd &&
                                                                                  p.StartDate <= sinDate &&
                                                                                  p.EndDate >= sinDate);
            if (tenMst != null)
            {
                return tenMst.SanteiItemCd ?? string.Empty;
            }
            return string.Empty;
        }

        public List<CheckedDiseaseModel> GetTekiouByomeiByOrder(int hpId, List<string> itemCds)
        {
            var tekiouByomeiMsts = NoTrackingDataContext.TekiouByomeiMsts.Where(p => p.HpId == hpId && itemCds.Contains(p.ItemCd) && p.IsInvalid == 0);
            var byomeiMsts = NoTrackingDataContext.ByomeiMsts.Where(p => p.HpId == hpId);

            var query = from tekiByomei in tekiouByomeiMsts
                        join byomeiMst in byomeiMsts
                        on tekiByomei.ByomeiCd equals byomeiMst.ByomeiCd
                        select new
                        {
                            ByomeiMst = byomeiMst,
                            TekiByomei = tekiByomei
                        };
            var ptByomeiModels = new List<CheckedDiseaseModel>();
            foreach (var entity in query)
            {
                if (!ptByomeiModels.Any(p => p.ByomeiMst.ByomeiCd == entity.ByomeiMst.ByomeiCd))
                {
                    ptByomeiModels.Add(new CheckedDiseaseModel(entity.ByomeiMst.SikkanCd, entity.ByomeiMst.NanbyoCd, entity.ByomeiMst.Byomei ?? string.Empty, 0, new PtDiseaseModel(
                        entity.ByomeiMst.ByomeiCd, entity.ByomeiMst.Byomei ?? string.Empty, entity.ByomeiMst.SikkanCd
                        ), new ByomeiMstModel(entity.ByomeiMst.ByomeiCd, string.Empty, entity.ByomeiMst.Sbyomei ?? string.Empty, entity.ByomeiMst.KanaName1 ?? string.Empty, string.Empty, entity.ByomeiMst.NanbyoCd == NanbyoConst.Gairai ? "難病" : string.Empty, string.Empty, string.Empty, entity.ByomeiMst.IsAdopted == 1, entity.ByomeiMst.KanaName2 ?? string.Empty, entity.ByomeiMst.KanaName3 ?? string.Empty, entity.ByomeiMst.KanaName4 ?? string.Empty, entity.ByomeiMst.KanaName5 ?? string.Empty, entity.ByomeiMst.KanaName6 ?? string.Empty, entity.ByomeiMst.KanaName7 ?? string.Empty)));
                }
            }
            return ptByomeiModels;
        }

        public Dictionary<string, string> CheckNameChanged(List<OrdInfModel> odrInfModelList)
        {
            Dictionary<string, string> nameChanged = new Dictionary<string, string>();
            foreach (var odrInfModel in odrInfModelList)
            {
                CheckNameChanged(odrInfModel, ref nameChanged);
            }

            return nameChanged;
        }

        private void CheckNameChanged(OrdInfModel odrInfModel, ref Dictionary<string, string> nameChanged)
        {
            foreach (var detail in odrInfModel.OrdInfDetails)
            {
                if (string.IsNullOrEmpty(detail.ItemCd) || detail.IsDrugUsage || detail.IsNormalComment)
                {
                    continue;
                }
                string newName = CheckNameChanged(odrInfModel.HpId, odrInfModel.SinDate, detail);
                if (!string.IsNullOrEmpty(newName))
                {
                    string oldName;
                    if (detail.HasCmtName)
                    {
                        oldName = detail.CmtName;
                    }
                    else
                    {
                        oldName = detail.ItemName;
                    }
                    if (string.IsNullOrEmpty(oldName))
                    {
                        continue;
                    }
                    if (!nameChanged.ContainsKey(oldName))
                    {
                        nameChanged.Add(oldName, newName);
                    }
                }
            }
        }

        private string CheckNameChanged(int hpId, int sinDate, OrdInfDetailModel detail)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(detail.ItemCd) || detail.IsDrugUsage || detail.IsNormalComment)
            {
                return result;
            }

            var itemMst = NoTrackingDataContext.TenMsts.FirstOrDefault(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.ItemCd == detail.ItemCd);
            if (itemMst != null)
            {
                string oldName;

                if (detail.HasCmtName)
                {
                    oldName = detail.CmtName?.Trim() ?? string.Empty;
                }
                else
                {
                    oldName = detail.ItemName?.Trim() ?? string.Empty;
                }
                if (string.IsNullOrEmpty(oldName))
                {
                    return result;
                }
                if (oldName != itemMst.Name?.Trim())
                {
                    return itemMst.Name ?? string.Empty;
                }
            }

            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        /// <summary>
        /// 外来リハ初再診チェック
        /// </summary>
        public (int type, string itemName, int lastDaySanteiRiha, string rihaItemName) GetValidGairaiRiha(int hpId, int ptId, long raiinNo, int sinDate, int syosaiKbn, List<OrdInfModel> allOdrInf)
        {
            var checkGairaiRiha = NoTrackingDataContext.SystemConfs.FirstOrDefault(p =>
                  p.HpId == hpId && p.GrpCd == 2016 && p.GrpEdaNo == 0)?.Val ?? 0;

            if (checkGairaiRiha == 0)
            {
                return new(0, string.Empty, 0, string.Empty);
            }

            if (syosaiKbn != SyosaiConst.None && syosaiKbn != SyosaiConst.Jihi)
            {
                // 外来リハビリテーション診療料１
                int lastDaySanteiRiha1 = GetLastDaySantei(hpId, ptId, sinDate, raiinNo, ItemCdConst.IgakuGairaiRiha1);
                if (lastDaySanteiRiha1 != 0)
                {
                    int tgtDay = CIUtil.SDateInc(lastDaySanteiRiha1, 6);
                    if (lastDaySanteiRiha1 <= sinDate && tgtDay >= sinDate)
                    {
                        //前回算定日より7日以内の場合
                        string itemName = NoTrackingDataContext.TenMsts.FirstOrDefault(p =>
                                           p.HpId == hpId &&
                                           p.StartDate <= sinDate &&
                                           p.EndDate >= sinDate &&
                                           p.ItemCd == ItemCdConst.IgakuGairaiRiha1)?.Name ?? string.Empty;

                        return new(1, itemName, lastDaySanteiRiha1, string.Empty);
                    }
                }

            }

            if (syosaiKbn != SyosaiConst.None && syosaiKbn != SyosaiConst.Jihi)
            {
                // 外来リハビリテーション診療料２
                int lastDaySanteiRiha2 = GetLastDaySantei(hpId, ptId, sinDate, raiinNo, ItemCdConst.IgakuGairaiRiha2);
                if (lastDaySanteiRiha2 != 0)
                {
                    int tgtDay = CIUtil.SDateInc(lastDaySanteiRiha2, 13);
                    if (lastDaySanteiRiha2 <= sinDate && tgtDay >= sinDate)
                    {
                        //前回算定日より14日以内の場合
                        string itemName = NoTrackingDataContext.TenMsts.FirstOrDefault(p =>
                                           p.HpId == hpId &&
                                           p.StartDate <= sinDate &&
                                           p.EndDate >= sinDate &&
                                           p.ItemCd == ItemCdConst.IgakuGairaiRiha2)?.Name ?? string.Empty;
                        return new(2, itemName, lastDaySanteiRiha2, string.Empty);
                    }
                }
            }

            if (syosaiKbn != SyosaiConst.None && syosaiKbn != SyosaiConst.Jihi)
            {
                //外来リハビリテーション診療料がオーダーされているか？
                string rihaItemName = string.Empty;
                foreach (var odrInf in allOdrInf)
                {
                    foreach (var detail in odrInf.OrdInfDetails)
                    {
                        if (detail.ItemCd == ItemCdConst.IgakuGairaiRiha1 || detail.ItemCd == ItemCdConst.IgakuGairaiRiha2)
                        {
                            rihaItemName = detail.ItemName;
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(rihaItemName))
                {
                    return new(3, string.Empty, 0, string.Empty);

                }
            }

            return new(0, string.Empty, 0, string.Empty);
        }

        /// <summary>
        /// 予防注射再診チェック
        /// </summary>
        /// <returns></returns>
        public (double systemSetting, bool isExistYoboItemOnly) GetValidJihiYobo(int hpId, int syosaiKbn, int sinDate, List<OrdInfModel> allOrder)
        {
            var systemSetting = NoTrackingDataContext.SystemConfs.FirstOrDefault(p =>
                  p.HpId == hpId && p.GrpCd == 2016 && p.GrpEdaNo == 2)?.Val ?? 0;

            bool isExistYoboItemOnly = false;

            if (syosaiKbn != SyosaiConst.None && syosaiKbn != SyosaiConst.Jihi)
            {
                var itemCds = new List<string>();
                foreach (var item in allOrder)
                {
                    itemCds.AddRange(item.OrdInfDetails.Select(o => o.ItemCd));
                }
                var tenMstItems = NoTrackingDataContext.TenMsts.Where(p =>
               p.HpId == hpId &&
               p.StartDate <= sinDate &&
               p.EndDate >= sinDate &&
               itemCds.Contains(p.ItemCd));
                foreach (var odrInf in allOrder)
                {
                    isExistYoboItemOnly = true;
                    bool hasItemDetail = false;

                    foreach (var detail in odrInf.OrdInfDetails)
                    {
                        if (!string.IsNullOrEmpty(detail.ItemCd) && !detail.IsCommentMaster)
                        {
                            hasItemDetail = true;
                            var tenMstItem = tenMstItems.FirstOrDefault(t => t.ItemCd == detail.ItemCd) ?? new();
                            if (tenMstItem != null)
                            {
                                if (tenMstItem.JihiSbt == 0)
                                {
                                    isExistYoboItemOnly = false;
                                    break;
                                }
                                var jihiSbtItem = NoTrackingDataContext.JihiSbtMsts
                                                .FirstOrDefault(i => i.HpId == Session.HospitalID
                                                && i.IsDeleted == DeleteTypes.None
                                                && i.JihiSbt == tenMstItem.JihiSbt);
                                if (jihiSbtItem != null)
                                {
                                    if (jihiSbtItem.IsYobo != 1)
                                    {
                                        isExistYoboItemOnly = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    isExistYoboItemOnly = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (!hasItemDetail) // Contain comment only
                    {
                        isExistYoboItemOnly = false;
                    }
                    if (hasItemDetail && !isExistYoboItemOnly)
                    {
                        break;
                    }
                }
            }

            return (systemSetting, isExistYoboItemOnly);
        }

        private int GetLastDaySantei(int hpId, long ptId, int sinDate, long raiinNo, string itemCd)
        {
            int result = 0;
            var sinKouiCountDiffDayQuery = NoTrackingDataContext.SinKouiCounts.Where(s => s.HpId == hpId && s.PtId == ptId && (s.SinYm * 100 + s.SinDay) < sinDate);
            var sinKouiDetailQuery = NoTrackingDataContext.SinKouiDetails.Where(s => s.HpId == hpId && s.PtId == ptId && s.ItemCd == itemCd);
            var resultDiffDayQuery = from sinKouiCount in sinKouiCountDiffDayQuery
                                     join sinKouiDetail in sinKouiDetailQuery
                                     on new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SinYm }
                                     equals new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.RpNo, sinKouiDetail.SinYm }
                                     select new
                                     {
                                         SinKouiCount = sinKouiCount,
                                     };
            var resultCountList = resultDiffDayQuery.ToList();
            if (resultCountList.Count > 0)
            {
                //当日を含めない
                result = resultCountList.Max(d => d.SinKouiCount.SinYm * 100 + d.SinKouiCount.SinDay);
            }
            else
            {
                //当日を含める
                var sinKouiCountSameDayQuery = NoTrackingDataContext.SinKouiCounts
                    .Where(s => s.HpId == hpId && s.PtId == ptId && (s.SinYm * 100 + s.SinDay) <= sinDate && s.RaiinNo != raiinNo);
                var resultSameDayQuery = from sinKouiCount in sinKouiCountSameDayQuery
                                         join sinKouiDetail in sinKouiDetailQuery
                                         on new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RpNo, sinKouiCount.SinYm }
                                         equals new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.RpNo, sinKouiDetail.SinYm }
                                         select new
                                         {
                                             SinKouiCount = sinKouiCount,
                                         };
                resultCountList = resultSameDayQuery.ToList();
                if (resultCountList.Count > 0)
                {
                    result = resultCountList.Max(d => d.SinKouiCount.SinYm * 100 + d.SinKouiCount.SinDay);
                }
            }

            return result;
        }

        public List<RaiinKbnModel> InitDefaultByTodayOrder(List<RaiinKbnModel> raiinKbns, List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns, List<RaiinKbnItemModel> raiinKbnItemCds, List<OrdInfModel> todayOrds)
        {
            foreach (var raiinKbn in raiinKbns)
            {
                int settingRaiinKbnCd = 0;
                foreach (var kbnDetail in raiinKbn.RaiinKbnDetailModels)
                {
                    if (!kbnDetail.IsTodayOrderChecked) continue;

                    //grid raiinKbnItem
                    var kouiKbns = raiinKouiKbns.FindAll(k => k.grpId == kbnDetail.GrpCd && k.kbnCd == kbnDetail.KbnCd);

                    //checkbox group raiinKouiKbn
                    var kbnItems = raiinKbnItemCds.FindAll(p => p.GrpCd == kbnDetail.GrpCd && p.KbnCd == kbnDetail.KbnCd && !string.IsNullOrEmpty(p.ItemCd));
                    var includeItems = kbnItems.FindAll(p => !(p.IsExclude == 1));
                    var excludeItems = kbnItems.FindAll(p => p.IsExclude == 1);

                    bool existItem = false;
                    foreach (var todayOrd in todayOrds)
                    {
                        foreach (var todayOdrDetail in todayOrd.OrdInfDetails)
                        {
                            if (excludeItems.Exists(p => p.ItemCd == todayOdrDetail.ItemCd)) continue;

                            if (kouiKbns.Exists(p => p.kouiKbn1 == todayOrd.OdrKouiKbn || p.kouiKbn2 == todayOrd.OdrKouiKbn) ||
                                includeItems.Exists(p => p.ItemCd == todayOdrDetail.ItemCd))
                            {
                                existItem = true;
                                break;
                            }
                        }
                    }

                    if (existItem)
                    {
                        if (kbnDetail.IsAutoDelete == DeleteTypes.Deleted && raiinKbn.RaiinKbnInfModel.KbnCd == kbnDetail.KbnCd)
                        {
                            raiinKbn.RaiinKbnInfModel.ChangeKbnCd(0);
                        }
                    }

                    existItem = false;
                    foreach (var todayOrd in todayOrds)
                    {
                        foreach (var todayOdr in todayOrd.OrdInfDetails)
                        {
                            if (excludeItems.Exists(p => p.ItemCd == todayOdr.ItemCd)) continue;

                            if (kouiKbns.Exists(p => p.kouiKbn1 == todayOrd.OdrKouiKbn || p.kouiKbn2 == todayOrd.OdrKouiKbn) ||
                                includeItems.Exists(p => p.ItemCd == todayOdr.ItemCd))
                            {
                                existItem = true;
                                break;
                            }
                        }
                    }

                    if (existItem)
                    {
                        settingRaiinKbnCd = kbnDetail.KbnCd;
                    }
                }
                if (settingRaiinKbnCd != 0 && raiinKbn.RaiinKbnInfModel.KbnCd == 0)
                {
                    raiinKbn.RaiinKbnInfModel.ChangeKbnCd(settingRaiinKbnCd);
                }
            }

            return raiinKbns;
        }
    }
}
