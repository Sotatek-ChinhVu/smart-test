using Domain.Models.Diseases;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKubunMst;
using Domain.Models.SystemConf;
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
        private readonly ISystemConfRepository _systemConf;

        public TodayOdrRepository(ITenantProvider tenantProvider, ISystemConfRepository systemConf) : base(tenantProvider)
        {
            _systemConf = systemConf;
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
                        if (karteInfModel.PtId > 0 && karteInfModel.HpId > 0 && karteInfModel.RaiinNo > 0 && karteInfModel.SinDate > 0)
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
                raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        oldHeaderInfModel.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        oldHeaderInfModel.UpdateId = userId;
                    }
                    oldHeaderInfModel.IsDeleted = 0;
                }
                else
                {
                    // Be sure old header is deleted
                    oldHeaderInfModel.IsDeleted = DeleteTypes.Deleted;
                    oldHeaderInfModel.UpdateDate = CIUtil.GetJapanDateTimeNow();
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
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.RaiinNo != raiinNo);
            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p =>
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

        public List<(int, int, List<Tuple<string, string, long>>)> GetAutoAddOrders(int hpId, long ptId, int sinDate, List<Tuple<int, int, string>> addingOdrList, List<Tuple<int, int, string, double>> currentOdrList)
        {
            List<OrdInfModel> autoAddOdr = new();
            var itemCds = new List<string>();
            var autoItems = new List<(int, int, List<Tuple<string, string, long>>)>();
            var itemCdAutos = new List<string>();

            foreach (var itemCd in addingOdrList.Select(o => o.Item3))
            {
                itemCds.Add(itemCd);
            }

            var allSanteiGrpDetail = NoTrackingDataContext.SanteiGrpDetails
                                    .Where(s => itemCds.Contains(s.ItemCd)).ToList();
            foreach (var addingOrd in addingOdrList)
            {
                if (string.IsNullOrEmpty(addingOrd.Item3))
                {
                    continue;
                }

                var santeiGrpDetails = allSanteiGrpDetail.Where(s => s.ItemCd == addingOrd.Item3).ToList();
                var santeiGrpCds = santeiGrpDetails.Select(s => s.SanteiGrpCd);

                if (santeiGrpDetails.Count == 0)
                {
                    continue;
                }

                var santeiAutoOrders = NoTrackingDataContext.SanteiAutoOrders.Where(e =>
                                         e.HpId == hpId &&
                                         santeiGrpCds.Contains(e.SanteiGrpCd) &&
                                         e.StartDate <= sinDate &&
                                         e.EndDate >= sinDate).ToList();
                var santeiAutoOrderDetails = NoTrackingDataContext.SanteiAutoOrderDetails.Where(s => santeiGrpCds.Contains(s.SanteiGrpCd)).ToList();

                foreach (var santeiGrpDetail in santeiGrpDetails)
                {
                    var santeiAutoOrder = santeiAutoOrders.FirstOrDefault(s => s.SanteiGrpCd == santeiGrpDetail.SanteiGrpCd && s.HpId == santeiGrpDetail.HpId);
                    if (santeiAutoOrder == null)
                    {
                        continue;
                    }

                    if (santeiAutoOrder.TermCnt == 1 && santeiAutoOrder.TermSbt == 4 && (santeiAutoOrder.CntType == 2 || santeiAutoOrder.CntType == 3))
                    {
                        var santeiAutoOdrDetailList = santeiAutoOrderDetails.Where(s => s.SanteiGrpCd == santeiAutoOrder.SanteiGrpCd && s.SeqNo == santeiAutoOrder.SeqNo).ToList();
                        List<string> autoOdrDetailItemCdList = santeiAutoOdrDetailList.Select(s => s.ItemCd).Distinct().ToList();

                        if (santeiAutoOdrDetailList.Count == 0)
                        {
                            continue;
                        }

                        double santeiCntInMonth = 0;
                        foreach (var itemCd in autoOdrDetailItemCdList)
                        {
                            santeiCntInMonth += GetOdrCountInMonth(ptId, sinDate, itemCd);
                        }

                        double countInCurrentOdr = 0;

                        if (santeiAutoOrder.CntType == 2)
                        {
                            foreach (var item in currentOdrList)
                            {
                                if (autoOdrDetailItemCdList.Contains(item.Item3))
                                {
                                    countInCurrentOdr += (item.Item4 <= 0 || ItemCdConst.ZaitakuTokushu.Contains(item.Item3)) ? 1 : item.Item4;
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in currentOdrList)
                            {
                                if (autoOdrDetailItemCdList.Contains(item.Item3))
                                {
                                    countInCurrentOdr++;
                                }
                            }
                        }

                        double totalSanteiCount = santeiCntInMonth + countInCurrentOdr;

                        if (totalSanteiCount >= santeiAutoOrder.MaxCnt)
                        {
                            continue;
                        }

                        double countInAutoAdd = autoAddOdr.Count();
                        if (totalSanteiCount + countInAutoAdd >= santeiAutoOrder.MaxCnt)
                        {
                            continue;
                        }

                        var autoAddItem = AutoAddItem(hpId, sinDate, santeiAutoOdrDetailList);
                        autoItems.Add(new(addingOrd.Item1, addingOrd.Item2, autoAddItem));
                    }
                }
            }

            return autoItems;
        }

        public List<OrdInfModel> AutoAddOrders(int hpId, int userId, int sinDate, List<Tuple<int, int, string, int, int>> addingOdrList, List<Tuple<int, int, string, long>> autoAddItems)
        {
            List<OrdInfModel> autoAddOdr = new();
            var itemCds = new List<string>();

            foreach (var autoAddItem in autoAddItems)
            {
                itemCds.Add(autoAddItem.Item3);
            }
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(t => t.HpId == hpId).ToList();
            var ipnKasanExcludes = NoTrackingDataContext.ipnKasanExcludes.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate)).ToList();
            var ipnKasanExcludeItems = NoTrackingDataContext.ipnKasanExcludeItems.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate)).ToList();
            var listYohoSets = NoTrackingDataContext.YohoSetMsts.Where(y => y.HpId == hpId && y.IsDeleted == 0 && y.UserId == userId).ToList();
            var itemCdYohos = listYohoSets?.Select(od => od.ItemCd ?? string.Empty);

            var tenMstYohos = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.IsNosearch == 0 && t.StartDate <= sinDate && t.EndDate >= sinDate && (itemCdYohos != null && itemCdYohos.Contains(t.ItemCd))).ToList();

            var checkKensaIrai = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 0);
            var kensaIrai = checkKensaIrai?.Val ?? 0;
            var checkKensaIraiCondition = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 1);
            var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;


            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => autoAddItems.Select(i => i.Item3).Contains(t.ItemCd)).ToList();
            var santeiAutoOdrDetailList = NoTrackingDataContext.SanteiAutoOrderDetails.Where(s => autoAddItems.Select(a => a.Item4).Contains(s.Id)).ToList();

            foreach (var addingOdr in addingOdrList)
            {
                var autoAddItem = autoAddItems.FirstOrDefault(i => i.Item1 == addingOdr.Item1 && i.Item2 == addingOdr.Item2);
                if (autoAddItem == null)
                {
                    continue;
                }
                var targetItem = tenMsts.FirstOrDefault(t => t.ItemCd == autoAddItem?.Item3);
                OdrInf odrInf = new OdrInf();
                odrInf.OdrKouiKbn = targetItem?.SinKouiKbn ?? 0;
                odrInf.SinDate = sinDate;
                odrInf.RpName = addingOdr.Item3;
                odrInf.InoutKbn = addingOdr.Item4;
                odrInf.DaysCnt = 1;

                var santeiAutoOdrDetail = santeiAutoOdrDetailList.FirstOrDefault(s => (autoAddItem != null && s.Id == autoAddItem.Item4));
                OdrInfDetail odrDetail = new OdrInfDetail();
                odrDetail.SinKouiKbn = targetItem?.SinKouiKbn ?? 0;
                odrDetail.SinDate = sinDate;
                odrDetail.Suryo = santeiAutoOdrDetail?.Suryo ?? 0;
                odrDetail.ItemCd = autoAddItem?.Item3 ?? string.Empty;
                odrDetail.ItemName = targetItem?.Name ?? string.Empty;

                if (!string.IsNullOrEmpty(targetItem?.OdrUnitName))
                {
                    odrDetail.UnitSBT = 1;
                    odrDetail.UnitName = targetItem.OdrUnitName;
                    odrDetail.TermVal = targetItem.OdrTermVal;
                }
                else if (!string.IsNullOrEmpty(targetItem?.CnvUnitName))
                {
                    odrDetail.UnitSBT = 2;
                    odrDetail.UnitName = targetItem.CnvUnitName;
                    odrDetail.TermVal = targetItem.CnvTermVal;
                }
                else
                {
                    odrDetail.UnitSBT = 0;
                    odrDetail.UnitName = string.Empty;
                    odrDetail.TermVal = 0;
                }

                odrDetail.KohatuKbn = targetItem?.KohatuKbn ?? 0;
                odrDetail.YohoKbn = targetItem?.YohoKbn ?? 0;
                odrDetail.DrugKbn = targetItem?.DrugKbn ?? 0;

                var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrDetail.ItemCd);
                var ten = tenMst?.Ten ?? 0;
                if (tenMst != null && string.IsNullOrEmpty(odrDetail.IpnCd)) odrDetail.IpnCd = tenMst.IpnNameCd;

                var kensaMst = tenMst == null ? null : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);

                var alternationIndex = addingOdr.Item2 % 2;

                var isGetPriceInYakka = IsGetPriceInYakka(tenMst, ipnKasanExcludes, ipnKasanExcludeItems);

                int kensaGaichu = GetKensaGaichu(odrDetail, tenMst, addingOdr.Item4, addingOdr.Item5, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);

                var newOdr = ConvertToModel(odrInf, odrDetail, tenMst ?? new TenMst(), isGetPriceInYakka, alternationIndex, kensaGaichu, addingOdr.Item4, GetListYohoSetMstModelByUserID(listYohoSets ?? new List<YohoSetMst>(), tenMstYohos?.Where(t => t.SinKouiKbn == odrDetail.SinKouiKbn)?.ToList() ?? new List<TenMst>()));
                autoAddOdr.Add(newOdr);
            }

            return autoAddOdr;
        }

        private static bool IsGetPriceInYakka(TenMst? tenMst, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems)
        {
            if (tenMst == null) return false;

            var ipnKasanExclude = ipnKasanExcludes.FirstOrDefault(u => u.IpnNameCd == tenMst.IpnNameCd);

            var ipnKasanExcludeItem = ipnKasanExcludeItems.FirstOrDefault(u => u.ItemCd == tenMst.ItemCd);

            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        private static int GetKensaGaichu(OdrInfDetail? odrInfDetail, TenMst? tenMst, int inOutKbn, int odrKouiKbn, KensaMst? kensaMst, int kensaIraiCondition, int kensaIrai)
        {
            if (string.IsNullOrEmpty(odrInfDetail?.ItemCd) &&
                   string.IsNullOrEmpty(odrInfDetail?.ItemName?.Trim()) &&
                   odrInfDetail?.SinKouiKbn == 0)
            {
                return KensaGaichuTextConst.NONE;
            }

            if (odrInfDetail?.SinKouiKbn == 61 || odrInfDetail?.SinKouiKbn == 64)
            {
                bool kensaCondition;
                if (kensaIraiCondition == 0)
                {
                    kensaCondition = (odrInfDetail.SinKouiKbn == 61 || odrInfDetail.SinKouiKbn == 64) && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9";
                }
                else
                {
                    kensaCondition = odrInfDetail.SinKouiKbn == 61 && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9" && (tenMst == null ? 0 : tenMst.HandanGrpKbn) != 6;
                }

                if (kensaCondition && inOutKbn == 1)
                {
                    int kensaSetting = kensaIrai;
                    if (kensaMst == null)
                    {
                        if (kensaSetting > 0)
                        {
                            return KensaGaichuTextConst.GAICHU_NONE;
                        }
                    }
                    else if (string.IsNullOrEmpty(kensaMst.CenterItemCd1)
                        && string.IsNullOrEmpty(kensaMst.CenterItemCd2) && kensaSetting > 1)
                    {
                        return KensaGaichuTextConst.GAICHU_NOT_SET;
                    }
                }
            }

            if (!string.IsNullOrEmpty(odrInfDetail?.ItemName) && string.IsNullOrEmpty(odrInfDetail.ItemCd))
            {
                if (inOutKbn == 1 && (odrKouiKbn >= 20 && odrKouiKbn <= 23) || odrKouiKbn == 28)
                {
                    if (odrInfDetail.IsNodspRece == 0)
                    {
                        return KensaGaichuTextConst.IS_DISPLAY_RECE_ON;
                    }
                }
                else
                {
                    if (odrInfDetail.IsNodspRece == 1)
                    {
                        return KensaGaichuTextConst.IS_DISPLAY_RECE_OFF;
                    }
                }
            }
            return KensaGaichuTextConst.NONE;
        }

        private static List<YohoSetMstModel> GetListYohoSetMstModelByUserID(List<YohoSetMst> listYohoSetMst, List<TenMst> listTenMst)
        {
            var query = from yoho in listYohoSetMst
                        join ten in listTenMst on yoho.ItemCd?.Trim() equals ten.ItemCd.Trim()
                        select new
                        {
                            Yoho = yoho,
                            ItemName = ten.Name,
                            ten.YohoKbn
                        };

            return query.OrderBy(u => u.Yoho.SortNo).AsEnumerable().Select(u => new YohoSetMstModel(u.ItemName, u.YohoKbn, u.Yoho?.SetId ?? 0, u.Yoho?.UserId ?? 0, u.Yoho?.ItemCd ?? string.Empty)).ToList();
        }

        private double GetOdrCountInMonth(long ptId, int sinDate, string itemCd)
        {
            int firstDayOfSinDate = sinDate / 100 * 100 + 1;
            DateTime firstDaySinDateDateTime = CIUtil.IntToDate(firstDayOfSinDate);
            DateTime lastDayOfPrevMonthDateTime = firstDaySinDateDateTime.AddDays(-1);
            int lastDayOfPrevMonth = CIUtil.DateTimeToInt(lastDayOfPrevMonthDateTime);

            var odrInfQuery = NoTrackingDataContext.OdrInfs
              .Where(odr => odr.PtId == ptId && odr.SinDate > lastDayOfPrevMonth && odr.SinDate <= sinDate && odr.OdrKouiKbn != 10 && odr.IsDeleted == 0);
            var odrInfDetailQuery = NoTrackingDataContext.OdrInfDetails
              .Where(odrDetail => odrDetail.PtId == ptId
              && odrDetail.SinDate > lastDayOfPrevMonth
              && odrDetail.SinDate <= sinDate
              && odrDetail.ItemCd == itemCd);

            var odrJoinDetail = from odrInf in odrInfQuery.AsEnumerable()
                                join odrDetail in odrInfDetailQuery
                                on new { odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                                equals new { odrDetail.PtId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
                                into ListDetail
                                select new
                                {
                                    OdrInf = odrInf,
                                    OdrDetail = ListDetail
                                };
            var allDetailList = odrJoinDetail.AsEnumerable().Select(d => d.OdrDetail).ToList();
            var allDetail = new List<OdrInfDetail>();
            foreach (var detailList in allDetailList)
            {
                allDetail.AddRange(detailList);
            }
            return allDetail.Sum(d => (d.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(d.ItemCd ?? string.Empty)) ? 1 : d.Suryo);
        }

        private List<Tuple<string, string, long>> AutoAddItem(int hpId, int sinDate, List<SanteiAutoOrderDetail> santeiAutoOrderDetails)
        {
            List<Tuple<string, string, long>> autoItemList = new();
            var autoItems = santeiAutoOrderDetails.Select(s => s.ItemCd);

            var tenMsts = NoTrackingDataContext.TenMsts.Where(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   autoItems.Contains(p.ItemCd)).ToList();

            foreach (var santeiAutoOrderDetail in santeiAutoOrderDetails)
            {
                var tenItem = tenMsts.FirstOrDefault(t => t.ItemCd == santeiAutoOrderDetail.ItemCd);
                autoItemList.Add(new Tuple<string, string, long>(santeiAutoOrderDetail.ItemCd, tenItem?.Name ?? string.Empty, santeiAutoOrderDetail.Id));
            }

            return autoItemList;
        }

        private static OrdInfModel ConvertToModel(OdrInf ordInf, OdrInfDetail odrInfDetail, TenMst tenMst, bool isGetPriceInYakka, int alternationIndex, int kensaGaichu, int inOutKbn, List<YohoSetMstModel> yohoSets)
        {
            var ordDetail = new OrdInfDetailModel(
                                odrInfDetail.HpId,
                                odrInfDetail.RaiinNo,
                                odrInfDetail.RpNo,
                                odrInfDetail.RpEdaNo,
                                odrInfDetail.RowNo,
                                odrInfDetail.PtId,
                                odrInfDetail.SinDate,
                                odrInfDetail.SinKouiKbn,
                                odrInfDetail.ItemCd ?? string.Empty,
                                odrInfDetail.ItemName ?? string.Empty,
                                odrInfDetail.Suryo,
                                odrInfDetail.UnitName ?? string.Empty,
                                odrInfDetail.UnitSBT,
                                odrInfDetail.TermVal,
                                odrInfDetail.KohatuKbn,
                                odrInfDetail.SyohoKbn,
                                odrInfDetail.SyohoLimitKbn,
                                odrInfDetail.DrugKbn,
                                odrInfDetail.YohoKbn,
                                odrInfDetail.Kokuji1 ?? string.Empty,
                                odrInfDetail.Kokiji2 ?? string.Empty,
                                odrInfDetail.IsNodspRece,
                                odrInfDetail.IpnCd ?? string.Empty,
                                odrInfDetail.IpnName ?? string.Empty,
                                odrInfDetail.JissiKbn,
                                odrInfDetail.JissiDate ?? DateTime.MinValue,
                                odrInfDetail.JissiId,
                                odrInfDetail.JissiMachine ?? string.Empty,
                                odrInfDetail.ReqCd ?? string.Empty,
                                odrInfDetail.Bunkatu ?? string.Empty,
                                odrInfDetail.CmtName ?? string.Empty,
                                odrInfDetail.CmtOpt ?? string.Empty,
                                odrInfDetail.FontColor ?? string.Empty,
                                odrInfDetail.CommentNewline,
                                tenMst?.MasterSbt ?? string.Empty,
                                inOutKbn,
                                0,
                                isGetPriceInYakka,
                                0,
                                0,
                                tenMst?.Ten ?? 0,
                                0,
                                alternationIndex,
                                kensaGaichu,
                                tenMst?.OdrTermVal ?? 0,
                                tenMst?.CnvTermVal ?? 0,
                                tenMst?.YjCd ?? string.Empty,
                                yohoSets ?? new List<YohoSetMstModel>(),
                                0,
                                0,
                                tenMst?.CnvUnitName ?? string.Empty,
                                tenMst?.OdrUnitName ?? string.Empty,
                                string.Empty,
                                string.Empty
                    );

            return new OrdInfModel(ordInf.HpId,
                        ordInf.RaiinNo,
                        ordInf.RpNo,
                        ordInf.RpEdaNo,
                        ordInf.PtId,
                        ordInf.SinDate,
                        ordInf.HokenPid,
                        ordInf.OdrKouiKbn,
                        ordInf.RpName ?? string.Empty,
                        ordInf.InoutKbn,
                        ordInf.SikyuKbn,
                        ordInf.SyohoSbt,
                        ordInf.SanteiKbn,
                        ordInf.TosekiKbn,
                        ordInf.DaysCnt,
                        ordInf.SortNo,
                        ordInf.IsDeleted,
                        ordInf.Id,
                        new List<OrdInfDetailModel>() { ordDetail },
                        ordInf.CreateDate,
                        ordInf.CreateId,
                        "",
                        ordInf.UpdateDate,
                        ordInf.UpdateId,
                        ""
                   );

            ;
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
            foreach (var detail in odrInfModel.OrdInfDetails.Where(o => !o.ItemCd.StartsWith("CO")))
            {
                if (string.IsNullOrEmpty(detail.ItemCd) || detail.IsDrugUsage || detail.IsNormalComment)
                {
                    continue;
                }
                string newName = CheckNameChanged(odrInfModel.HpId, odrInfModel.SinDate, detail);
                if (!string.IsNullOrEmpty(newName))
                {
                    string oldName;
                    if (detail.HasCmtName && !detail.Is831Cmt)
                    {
                        oldName = detail.CmtName.Trim();
                    }
                    else
                    {
                        if (!detail.Is831Cmt)
                        {
                            oldName = detail.ItemName.Trim();
                        }
                        else
                        {
                            var tempOldName = detail.ItemName.Trim().Split("；").FirstOrDefault();
                            oldName = tempOldName != null ? tempOldName + "；" : string.Empty;
                        }
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

                if (detail.HasCmtName && !detail.Is831Cmt)
                {
                    oldName = detail.CmtName.Trim() ?? string.Empty;
                }
                else
                {
                    if (detail.Is831Cmt)
                    {
                        var tempOldName = detail.ItemName.Trim().Split("；").FirstOrDefault();
                        oldName = tempOldName != null ? tempOldName + "；" : string.Empty;
                    }
                    else
                    {
                        oldName = detail.ItemName?.Trim() ?? string.Empty;
                    }
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
                                                .FirstOrDefault(i => i.HpId == hpId
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

        public Dictionary<string, bool> ConvertInputItemToTodayOdr(int hpId, int sinDate, Dictionary<string, string> detailInfs)
        {
            var ipnKasanExcludeQuery = NoTrackingDataContext.IpnKasanMsts.Where(u => u.HpId == hpId && u.StartDate <= sinDate && u.EndDate >= sinDate);

            var ipnKasanExcludeItemQuery = NoTrackingDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.StartDate <= sinDate && u.EndDate >= sinDate);

            var query = from detail in detailInfs
                        join ipnkasan in ipnKasanExcludeQuery
                            on detail.Value equals ipnkasan.IpnNameCd into ipnKasanList
                        join ipnKasanItem in ipnKasanExcludeItemQuery
                            on detail.Key equals ipnKasanItem.ItemCd into ipnKasanItemList
                        select new
                        {
                            ItemCd = detail.Key,
                            IsGetYaka = ipnKasanList.FirstOrDefault() == null && ipnKasanItemList.FirstOrDefault() == null
                        };

            return query.AsEnumerable().ToDictionary(u => u.ItemCd, u => u.IsGetYaka);
        }

        public List<OrdInfModel> FromNextOrderToTodayOrder(int hpId, int sinDate, long raiinNo, int userId, List<RsvkrtOrderInfModel> rsvkrtOdrInfModels)
        {
            List<OrdInfModel> ordInfs;
            List<string> itemCds = new();
            List<string> ipNameCds = new();
            foreach (var ordInfDetail in rsvkrtOdrInfModels.Select(item => item.OrdInfDetails).ToList())
            {
                itemCds.AddRange(ordInfDetail.Select(od => od.ItemCd));
                ipNameCds.AddRange(ordInfDetail.Select(od => od.IpnCd));
            }
            itemCds = itemCds.Distinct().ToList();
            ipNameCds = itemCds.Distinct().ToList();
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.StartDate <= sinDate && t.EndDate >= sinDate && itemCds.Contains(t.ItemCd)).ToList();
            var kensaItemCds = tenMsts.Select(t => t.KensaItemCd).ToList();
            var kensaItemSeqNos = tenMsts.Select(t => t.KensaItemSeqNo).ToList();
            var ipns = NoTrackingDataContext.IpnNameMsts.Where(ipn =>
                   ipn.HpId == hpId &&
                   ipn.StartDate <= sinDate &&
                   ipn.EndDate >= sinDate &&
                   ipNameCds.Contains(ipn.IpnNameCd)).ToList();
            var kensMsts = NoTrackingDataContext.KensaMsts.Where(e =>
                e.HpId == hpId &&
                kensaItemCds.Contains(e.KensaItemCd) &&
                kensaItemSeqNos.Contains(e.KensaItemSeqNo))
                .ToList();
            var ipnMinYakkas = NoTrackingDataContext.IpnMinYakkaMsts.Where(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   ipNameCds.Contains(p.IpnNameCd)).ToList();

            ordInfs = ConvertToDetailModel(hpId, raiinNo, sinDate, userId, rsvkrtOdrInfModels, ipns, tenMsts, kensMsts, ipnMinYakkas);

            return ordInfs;
        }

        public List<OrdInfModel> ConvertToDetailModel(int hpId, long raiinNo, int sinDate, int userId, List<RsvkrtOrderInfModel> rsvkrtOdrInfModels, List<IpnNameMst> ipns, List<TenMst> tenMsts, List<KensaMst> kensMsts, List<IpnMinYakkaMst> ipnMinYakkas)
        {
            int autoSetKohatu = (int)_systemConf.GetSettingValue(2020, 2, hpId);
            int autoSetSenpatu = (int)_systemConf.GetSettingValue(2021, 2, hpId);
            int autoSetSyohoKbnKohatuDrug = (int)_systemConf.GetSettingValue(2020, 0, hpId);
            int autoSetSyohoLimitKohatuDrug = (int)_systemConf.GetSettingValue(2020, 1, hpId);
            int autoSetSyohoKbnSenpatuDrug = (int)_systemConf.GetSettingValue(2021, 0, hpId);
            int autoSetSyohoLimitSenpatuDrug = (int)_systemConf.GetSettingValue(2021, 1, hpId);

            List<OrdInfModel> ordInfs = new();
            foreach (var rsvkrtOdrInfModel in rsvkrtOdrInfModels)
            {
                List<OrdInfDetailModel> odrInfDetails = new();
                int rowNo = 0;
                foreach (var odrDetail in rsvkrtOdrInfModel.OrdInfDetails)
                {
                    int sinKouiKbn = odrDetail.SinKouiKbn;
                    string itemCd = odrDetail.ItemCd;
                    string itemName = odrDetail.ItemName;
                    double suryo = odrDetail.Suryo;
                    string unitName = odrDetail.UnitName;
                    int unitSBT = odrDetail.UnitSbt;
                    double termVal = odrDetail.TermVal;
                    string bunkatu = odrDetail.Bunkatu;

                    int kohatuKbn = odrDetail.KohatuKbn;
                    int drugKbn = odrDetail.DrugKbn;
                    int yohoKbn = odrDetail.YohoKbn;
                    int isNodspRece = odrDetail.IsNodspRece;
                    string ipnName = "";
                    TenMst? tenMst = new();
                    if (!string.IsNullOrEmpty(itemCd))
                    {
                        tenMst = tenMsts.FirstOrDefault(od => od.ItemCd == itemCd);
                    }
                    string ipnCd = tenMst == null ? "" : tenMst.IpnNameCd ?? string.Empty;
                    if (!string.IsNullOrEmpty(ipnCd))
                    {
                        ipnName = ipns.FirstOrDefault(od => od.IpnNameCd == ipnCd)?.IpnName ?? string.Empty;
                    }
                    else
                    {
                        ipnName = string.Empty;
                    }
                    string kokuji1 = tenMst == null ? "" : tenMst.Kokuji1 ?? string.Empty;
                    string kokuji2 = tenMst == null ? "" : tenMst.Kokuji2 ?? string.Empty;

                    string cmtName = odrDetail.CmtName;
                    string cmtOpt = odrDetail.CmtOpt;
                    string fontColor = odrDetail.FontColor;
                    int commentNewline = odrDetail.CommentNewline;
                    kohatuKbn = tenMst == null ? kohatuKbn : tenMst.KohatuKbn;
                    var syosai = CaculateSyosai(sinKouiKbn, autoSetKohatu, autoSetSyohoKbnKohatuDrug, autoSetSyohoLimitKohatuDrug, autoSetSyohoLimitSenpatuDrug, autoSetSyohoKbnSenpatuDrug, autoSetSenpatu, drugKbn, kohatuKbn, itemName, ipnName, odrDetail, tenMst ?? new());

                    // Correct TermVal
                    termVal = CorrectTermVal(unitSBT, tenMst ?? new(), termVal);
                    var kensMst = tenMst == null ? null : kensMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo);
                    var ipnMinYakka = tenMst == null ? null : ipnMinYakkas.FirstOrDefault(k => k.IpnNameCd == tenMst.IpnNameCd);
                    var isGetPriceInYakka = CheckIsGetYakkaPrice(hpId, tenMst ?? new(), sinDate);
                    double ten = tenMst == null ? 0 : tenMst.Ten;
                    var masterSbt = tenMst == null ? "" : tenMst.MasterSbt;
                    var cmtCol1 = tenMst == null ? 0 : tenMst.CmtCol1;

                    int currenRowNo = ++rowNo;
                    var odrInfDetail = new OrdInfDetailModel(
                           odrDetail.HpId, raiinNo, 0, 0, currenRowNo, odrDetail.PtId, sinDate, sinKouiKbn, itemCd, itemName, suryo, unitName, unitSBT, termVal, kohatuKbn, syosai.Item1, syosai.Item2, drugKbn, yohoKbn, kokuji1, kokuji2, isNodspRece, ipnCd, ipnName, 0, DateTime.MinValue, 0, string.Empty, string.Empty, bunkatu, cmtName, cmtOpt, fontColor, commentNewline, masterSbt ?? string.Empty, 0, ipnMinYakka?.Yakka ?? 0, isGetPriceInYakka, 0, cmtCol1, ten, 0, 0, 0, 0, 0, string.Empty, new(), 0, 0, string.Empty, string.Empty, kensMst?.CenterItemCd1 ?? string.Empty, kensMst?.CenterItemCd2 ?? string.Empty
                        );
                    odrInfDetails.Add(odrInfDetail);
                }
                OrdInfModel odrInf = new OrdInfModel(hpId, raiinNo, 0, 0, rsvkrtOdrInfModel.PtId, sinDate, rsvkrtOdrInfModel.HokenPid, rsvkrtOdrInfModel.OdrKouiKbn, rsvkrtOdrInfModel.RpName, rsvkrtOdrInfModel.InoutKbn, rsvkrtOdrInfModel.SikyuKbn, rsvkrtOdrInfModel.SyohoSbt, rsvkrtOdrInfModel.SanteiKbn, rsvkrtOdrInfModel.TosekiKbn, rsvkrtOdrInfModel.DaysCnt, rsvkrtOdrInfModel.SortNo, rsvkrtOdrInfModel.IsDeleted, 0, odrInfDetails, DateTime.MinValue, userId, string.Empty, DateTime.MinValue, userId, string.Empty);
                ordInfs.Add(odrInf);
            }
            return ordInfs;
        }

        public (int, int) CaculateSyosai(int sinKouiKbn, int autoSetKohatu, int autoSetSyohoKbnKohatuDrug, int autoSetSyohoLimitKohatuDrug, int autoSetSyohoLimitSenpatuDrug, int autoSetSyohoKbnSenpatuDrug, int autoSetSenpatu, int drugKbn, int kohatuKbn, string itemName, string ipnName, RsvKrtOrderInfDetailModel odrDetail, TenMst tenMst)
        {
            int syohoKbn = 0;
            int syohoLimitKbn = 0;
            if ((odrDetail.SinKouiKbn == 20 && odrDetail.DrugKbn > 0) || (odrDetail.IsInDrugOdr && odrDetail.IsInjection))
            {
                bool isChangeKouhatu = tenMst != null && odrDetail.KohatuKbn != tenMst.KohatuKbn;
                if (isChangeKouhatu && tenMst != null)
                {
                    switch (tenMst.KohatuKbn)
                    {
                        case 0:
                            // 先発品
                            syohoKbn = 0;
                            syohoLimitKbn = 0;
                            break;
                        case 1:
                            // 後発品
                            syohoKbn = autoSetSyohoKbnKohatuDrug + 1;
                            syohoLimitKbn = autoSetSyohoLimitKohatuDrug;
                            break;
                        case 2:
                            // 後発品のある先発品
                            syohoKbn = autoSetSyohoKbnSenpatuDrug + 1;
                            syohoLimitKbn = autoSetSyohoLimitSenpatuDrug;
                            break;
                    }
                    if (odrDetail.SyohoKbn == 3 && string.IsNullOrEmpty(ipnName))
                    {
                        // 一般名マスタに登録がない
                        syohoKbn = 2;
                    }
                }
            }

            if ((sinKouiKbn == 20 && drugKbn > 0) || (odrDetail.IsInDrugOdr && odrDetail.IsInjection))
            {
                switch (kohatuKbn)
                {
                    case 0:
                        // 先発品
                        syohoKbn = 0;
                        syohoLimitKbn = 0;
                        break;
                    case 1:
                        // 後発品
                        if (autoSetKohatu == 0)
                        {
                            //マスタ設定に準じる
                            syohoKbn = autoSetSyohoKbnKohatuDrug + 1;
                            syohoLimitKbn = autoSetSyohoLimitKohatuDrug;
                        }
                        else
                        {
                            //各セットの設定に準じる
                            syohoKbn = odrDetail.SyohoKbn;
                            syohoLimitKbn = odrDetail.SyohoLimitKbn;
                        }
                        if (syohoKbn == 0 && autoSetSyohoKbnKohatuDrug == 2 && !string.IsNullOrEmpty(ipnName))
                        {
                            syohoKbn = autoSetSyohoKbnKohatuDrug + 1;
                        }
                        break;
                    case 2:
                        // 後発品のある先発品
                        if (autoSetSenpatu == 0)
                        {
                            //マスタ設定に準じる
                            syohoKbn = autoSetSyohoKbnSenpatuDrug + 1;
                            syohoLimitKbn = autoSetSyohoLimitSenpatuDrug;
                        }
                        else
                        {
                            //各セットの設定に準じる
                            syohoKbn = odrDetail.SyohoKbn;
                            syohoLimitKbn = odrDetail.SyohoLimitKbn;
                        }
                        if (syohoKbn == 0 && autoSetSyohoKbnSenpatuDrug == 2 && !string.IsNullOrEmpty(ipnName))
                        {
                            syohoKbn = autoSetSyohoKbnSenpatuDrug + 1;
                        }
                        break;
                }

                if (tenMst != null && syohoKbn == 3 && string.IsNullOrEmpty(ipnName))
                {
                    // 一般名マスタに登録がない
                    syohoKbn = 2;
                }
            }
            return (syohoKbn, syohoLimitKbn);
        }

        public static double CorrectTermVal(int unitSbt, TenMst tenMst, double originTermVal)
        {
            if (tenMst == null || (string.IsNullOrEmpty(tenMst.ItemCd) && tenMst.StartDate == 0 && tenMst.HpId == 0)) return 0;
            double termVal = originTermVal;
            if (unitSbt == UnitSbtConst.BASIC)
            {
                termVal = tenMst.OdrTermVal;
            }
            else if (unitSbt == UnitSbtConst.CONVERT)
            {
                termVal = tenMst.CnvTermVal;
            }
            return termVal;
        }

        private bool CheckIsGetYakkaPrice(int hpId, TenMst tenMst, int sinDate)
        {
            if (tenMst == null) return false;
            var ipnKasanExclude = NoTrackingDataContext.ipnKasanExcludes.Where(u => u.HpId == hpId && u.IpnNameCd == tenMst.IpnNameCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();

            var ipnKasanExcludeItem = NoTrackingDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.ItemCd == tenMst.ItemCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();
            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        public List<(int type, string message, int odrInfPosition, int odrInfDetailPosition, TenItemModel tenItemMst, double suryo)> AutoCheckOrder(int hpId, int sinDate, long ptId, List<OrdInfModel> odrInfs)
        {
            var currentListOrder = odrInfs.Where(o => o.Id > 0).ToList();
            var addingOdrList = odrInfs.Where(o => o.Id == 0).ToList();
            List<(int type, string message, int positionOdr, int odrInfDetailPosition, TenItemModel temItemMst, double suryo)> result = new();
            int odrInfIndex = 0, odrInfDetailIndex = 0;
            foreach (var checkingOdr in addingOdrList)
            {
                var odrInfDetails = checkingOdr.OrdInfDetails.Where(d => !d.IsEmpty).ToList();
                foreach (var detail in odrInfDetails)
                {
                    if (string.IsNullOrEmpty(detail.ItemCd))
                    {
                        continue;
                    }

                    var santeiGrpDetailList = FindSanteiGrpDetailList(detail.ItemCd);
                    if (santeiGrpDetailList.Count == 0)
                    {
                        continue;
                    }

                    foreach (var santeiGrpDetail in santeiGrpDetailList)
                    {
                        var santeiCntCheck = FindSanteiCntCheck(hpId, santeiGrpDetail.SanteiGrpCd, sinDate);
                        if (santeiCntCheck == null)
                        {
                            continue;
                        }
                        // Now, check TermCnt = 1 and TermSbt = 4 and CntType = 2 only. In other case, just ignore
                        if (santeiCntCheck.TermCnt == 1 && santeiCntCheck.TermSbt == 4 && (santeiCntCheck.CntType == 2 || santeiCntCheck.CntType == 3))
                        {
                            double santeiCntInMonth = GetOdrCountInMonth(ptId, sinDate, detail.ItemCd);
                            double countInCurrentOdr = 0;

                            if (santeiCntCheck.CntType == 2)
                            {
                                foreach (var item in currentListOrder)
                                {
                                    foreach (var itemDetail in item.OrdInfDetails)
                                    {
                                        if (itemDetail.RpNo != detail.RpNo && itemDetail.ItemCd == detail.ItemCd)
                                        {
                                            countInCurrentOdr += (itemDetail.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(itemDetail.ItemCd)) ? 1 : itemDetail.Suryo;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in currentListOrder)
                                {
                                    foreach (var itemDetail in item.OrdInfDetails)
                                    {
                                        if (itemDetail.RpNo != detail.RpNo && itemDetail.ItemCd == detail.ItemCd)
                                        {
                                            countInCurrentOdr++;
                                        }
                                    }
                                }
                            }

                            double totalSanteiCount = santeiCntInMonth + countInCurrentOdr;

                            if (totalSanteiCount >= santeiCntCheck.MaxCnt)
                            {
                                var targetItem = FindTenMst(hpId, santeiCntCheck.TargetCd ?? string.Empty, sinDate);
                                if (targetItem == null)
                                {
                                    continue;
                                }

                                StringBuilder stringBuilder = new StringBuilder("");
                                stringBuilder.Append("'");
                                stringBuilder.Append(detail.DisplayItemName);
                                stringBuilder.Append("'");
                                stringBuilder.Append("が");
                                stringBuilder.Append(Environment.NewLine);
                                stringBuilder.Append("1ヶ月 ");
                                stringBuilder.Append(santeiCntCheck.MaxCnt);
                                stringBuilder.Append("単位を超えています。");
                                stringBuilder.Append(Environment.NewLine);
                                stringBuilder.Append("'");
                                stringBuilder.Append(targetItem.Name);
                                stringBuilder.Append("'に置き換えますか？");

                                string msg = stringBuilder.ToString();

                                result.Add(new(1, msg, odrInfIndex, odrInfDetailIndex, targetItem, 0));
                            }
                            else if (totalSanteiCount + detail.Suryo > santeiCntCheck.MaxCnt)
                            {
                                StringBuilder stringBuilder = new StringBuilder("");
                                stringBuilder.Append("'");
                                stringBuilder.Append(detail.DisplayItemName);
                                stringBuilder.Append("'");
                                stringBuilder.Append("が");
                                stringBuilder.Append(Environment.NewLine);
                                stringBuilder.Append("1ヶ月 ");
                                stringBuilder.Append(santeiCntCheck.MaxCnt);
                                stringBuilder.Append("単位を超えます。");
                                stringBuilder.Append(Environment.NewLine);
                                stringBuilder.Append("数量を'");
                                stringBuilder.Append(santeiCntCheck.MaxCnt - totalSanteiCount);
                                stringBuilder.Append("'に変更しますか？");

                                string msg = stringBuilder.ToString();
                                var suryo = santeiCntCheck.MaxCnt - totalSanteiCount;
                                result.Add(new(2, msg, odrInfIndex, odrInfDetailIndex, new(), suryo));
                            }
                        }
                    }

                    odrInfDetailIndex++;
                }
                odrInfIndex++;
            }
            return result;
        }

        public List<OrdInfModel> ChangeAfterAutoCheckOrder(int hpId, int sinDate, int userId, long raiinNo, long ptId, List<OrdInfModel> odrInfs, List<Tuple<int, string, int, int, TenItemModel, double>> targetItems)
        {
            List<OrdInfModel> result = new();
            var currentListOrder = odrInfs.Where(o => o.Id > 0).ToList();
            var addingOdrList = odrInfs.Where(o => o.Id == 0).ToList();
            int odrInfIndex = 0, odrInfDetailIndex = 0;
            List<string> ipnNameCds = new List<string>();
            foreach (var ordInfDetails in odrInfs.Select(o => o.OrdInfDetails))
            {
                ipnNameCds.AddRange(ordInfDetails.Select(od => od.IpnCd));
            }
            ipnNameCds = ipnNameCds.Distinct().ToList();
            var ipnNameMsts = NoTrackingDataContext.IpnNameMsts.Where(i =>
                   i.HpId == hpId &&
                   i.StartDate <= sinDate &&
                   i.EndDate >= sinDate &&
                   ipnNameCds.Contains(i.IpnNameCd)).Select(i => new Tuple<string, string>(i.IpnNameCd, i.IpnName ?? string.Empty));
            var autoSetSyohoKbnKohatuDrug = _systemConf.GetSettingValue(2020, 0, hpId);
            var autoSetSyohoLimitKohatuDrug = _systemConf.GetSettingValue(2020, 1, hpId);
            var autoSetSyohoKbnSenpatuDrug = _systemConf.GetSettingValue(2021, 0, hpId);
            var autoSetSyohoLimitSenpatuDrug = _systemConf.GetSettingValue(2021, 1, hpId);

            foreach (var checkingOdr in addingOdrList)
            {
                var checkGroupOrder = currentListOrder.FirstOrDefault(odrInf => odrInf.HokenPid == checkingOdr.HokenPid
                                                     && odrInf.GroupKoui.Value == checkingOdr.GroupKoui.Value
                                                     && odrInf.InoutKbn == checkingOdr?.InoutKbn
                                                     && odrInf.SyohoSbt == checkingOdr?.SyohoSbt
                                                     && odrInf.SikyuKbn == checkingOdr.SikyuKbn
                                                     && odrInf.TosekiKbn == checkingOdr.TosekiKbn
                                                     && odrInf.SanteiKbn == checkingOdr.SanteiKbn);
                var odrInfDetails = checkingOdr.OrdInfDetails.Where(d => !d.IsEmpty).ToList();
                foreach (var detail in odrInfDetails)
                {
                    var targetItem = targetItems.FirstOrDefault(t => t.Item3 == odrInfIndex && t.Item4 == odrInfDetailIndex);

                    if (targetItem == null) return new();

                    if (targetItem.Item1 == 1)
                    {
                        var tenItemMst = targetItem.Item5;
                        var grpKouiDetail = CIUtil.GetGroupKoui(detail.SinKouiKbn);
                        var grpKouiTarget = CIUtil.GetGroupKoui(tenItemMst.SinKouiKbn);
                        var itemShugiList = odrInfDetails.Where(d => d.IsShugi).ToList();
                        string itemCd = tenItemMst.ItemCd;
                        string itemName = tenItemMst.Name;
                        int sinKouiKbn = tenItemMst.SinKouiKbn;
                        int kohatuKbn = tenItemMst.KohatuKbn;
                        int drugKbn = tenItemMst.DrugKbn;
                        string unitNameBefore = detail.UnitName;
                        int unitSBT = 0;
                        string unitName = "";
                        double termVal = 0;
                        double suryo = 0;
                        int yohoKbn = tenItemMst.YohoKbn;
                        string ipnCd = tenItemMst.IpnNameCd;
                        string ipnName = "";
                        string kokuji1 = tenItemMst.Kokuji1;
                        string kokuji2 = tenItemMst.Kokuji2;
                        int syohoKbn = 0;
                        int syohoLimitKbn = 0;

                        if (grpKouiDetail == grpKouiTarget || itemShugiList.Count == 1)
                        {
                            if (!string.IsNullOrEmpty(tenItemMst.OdrUnitName))
                            {
                                unitSBT = 1;
                                unitName = tenItemMst.OdrUnitName;
                                termVal = tenItemMst.OdrTermVal;
                            }
                            else if (!string.IsNullOrEmpty(tenItemMst.CnvUnitName))
                            {
                                unitSBT = 2;
                                unitName = tenItemMst.CnvUnitName;
                                termVal = tenItemMst.CnvTermVal;
                            }
                            else
                            {
                                unitSBT = 0;
                                unitName = string.Empty;
                                termVal = 0;
                                suryo = 0;
                            }
                            if (!string.IsNullOrEmpty(detail.UnitName) && detail.UnitName != unitNameBefore)
                            {
                                suryo = 1;
                            }

                            if (!string.IsNullOrEmpty(detail.IpnCd))
                            {
                                ipnName = ipnNameMsts.FirstOrDefault(i => i.Item1 == tenItemMst.IpnNameCd)?.Item2 ?? string.Empty;
                            }
                            else
                            {
                                ipnName = string.Empty;
                            }

                            if (detail.SinKouiKbn == 20 && detail.DrugKbn > 0)
                            {
                                switch (detail.KohatuKbn)
                                {
                                    case 0:
                                        // 先発品
                                        syohoKbn = 0;
                                        syohoLimitKbn = 0;
                                        break;
                                    case 1:
                                        // 後発品
                                        syohoKbn = (int)autoSetSyohoKbnKohatuDrug + 1;
                                        syohoLimitKbn = (int)autoSetSyohoLimitKohatuDrug;
                                        break;
                                    case 2:
                                        // 後発品のある先発品
                                        syohoKbn = (int)autoSetSyohoKbnSenpatuDrug + 1;
                                        syohoLimitKbn = (int)autoSetSyohoLimitSenpatuDrug;
                                        break;
                                }
                                if (detail.SyohoKbn == 3 && string.IsNullOrEmpty(detail.IpnName))
                                {
                                    // 一般名マスタに登録がない
                                    syohoKbn = 2;
                                }
                            }

                            if (itemShugiList.Count == 1)
                            {
                                checkingOdr.ChangeOdrKouiKbn(detail.SinKouiKbn);
                                if (checkGroupOrder != null)
                                {
                                    checkingOdr.Delete();
                                    result.Add(checkingOdr);
                                }
                            }
                            detail.ChangeOrdInfDetail(itemCd, itemName, sinKouiKbn, kohatuKbn, drugKbn, unitSBT, unitName, termVal, suryo, yohoKbn, ipnCd, ipnName, kokuji1, kokuji2, syohoKbn, syohoLimitKbn);
                            result.Add(checkingOdr);
                        }
                        else
                        {
                            // Difference group koui
                            sinKouiKbn = tenItemMst.SinKouiKbn;
                            itemCd = tenItemMst.ItemCd;
                            itemName = tenItemMst.Name;
                            unitNameBefore = detail.UnitName;
                            if (!string.IsNullOrEmpty(tenItemMst.OdrUnitName))
                            {
                                unitSBT = 1;
                                unitName = tenItemMst.OdrUnitName;
                                termVal = tenItemMst.OdrTermVal;
                            }
                            else if (!string.IsNullOrEmpty(tenItemMst.CnvUnitName))
                            {
                                unitSBT = 2;
                                unitName = tenItemMst.CnvUnitName;
                                termVal = tenItemMst.CnvTermVal;
                            }
                            else
                            {
                                unitSBT = 0;
                                unitName = string.Empty;
                                termVal = 0;
                                suryo = 0;
                            }
                            if (!string.IsNullOrEmpty(detail.UnitName) && detail.UnitName != unitNameBefore)
                            {
                                suryo = 1;
                            }

                            kohatuKbn = tenItemMst.KohatuKbn;
                            yohoKbn = tenItemMst.YohoKbn;
                            drugKbn = tenItemMst.DrugKbn;
                            ipnCd = tenItemMst.IpnNameCd;
                            if (!string.IsNullOrEmpty(detail.IpnCd))
                            {
                                ipnName = ipnNameMsts.FirstOrDefault(t => t.Item1 == tenItemMst.IpnNameCd)?.Item2 ?? string.Empty;
                            }
                            else
                            {
                                ipnName = string.Empty;
                            }

                            kokuji1 = tenItemMst.Kokuji1;
                            kokuji2 = tenItemMst.Kokuji2;

                            if (detail.SinKouiKbn == 20 && detail.DrugKbn > 0)
                            {
                                switch (detail.KohatuKbn)
                                {
                                    case 0:
                                        // 先発品
                                        syohoKbn = 0;
                                        syohoLimitKbn = 0;
                                        break;
                                    case 1:
                                        // 後発品
                                        syohoKbn = (int)autoSetSyohoKbnKohatuDrug + 1;
                                        syohoLimitKbn = (int)autoSetSyohoLimitKohatuDrug;
                                        break;
                                    case 2:
                                        // 後発品のある先発品
                                        syohoKbn = (int)autoSetSyohoKbnSenpatuDrug + 1;
                                        syohoLimitKbn = (int)autoSetSyohoLimitSenpatuDrug;
                                        break;
                                }
                                if (detail.SyohoKbn == 3 && string.IsNullOrEmpty(detail.IpnName))
                                {
                                    // 一般名マスタに登録がない
                                    syohoKbn = 2;
                                }
                            }

                            List<OrdInfDetailModel> odrInfDetail = new();
                            var odrInfDetailModel = new OrdInfDetailModel(
                                hpId,
                                raiinNo,
                                0,
                                0,
                                1,
                                ptId,
                                sinDate,
                                sinKouiKbn,
                                itemCd,
                                itemName,
                                suryo,
                                unitName,
                                unitSBT,
                                termVal,
                                kohatuKbn,
                                syohoKbn,
                                syohoLimitKbn,
                                drugKbn,
                                yohoKbn,
                                kokuji1,
                                kokuji2,
                                0,
                                ipnCd,
                                ipnName,
                                0,
                                DateTime.MinValue,
                                0,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                0,
                                string.Empty,
                                0,
                                0,
                                false,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                string.Empty,
                                new(),
                                0,
                                0,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty
                            );
                            odrInfDetail.Add(odrInfDetailModel);

                            OrdInfModel odrInf = new OrdInfModel(
                                    hpId,
                                    raiinNo,
                                    0,
                                    0,
                                    ptId,
                                    sinDate,
                                    0,
                                    tenItemMst.SinKouiKbn,
                                    string.Empty,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0,
                                    1,
                                    0,
                                    0,
                                    0,
                                    odrInfDetail,
                                    DateTime.MinValue,
                                    userId,
                                    string.Empty,
                                    DateTime.MinValue,
                                    userId,
                                    string.Empty
                                );

                            result.Add(odrInf);
                            checkingOdr.Delete();
                        }
                    }
                    else
                    {
                        detail.ChangeSuryo(targetItem.Item6);
                        result.Add(checkingOdr);
                    }
                    odrInfDetailIndex++;
                }

                odrInfIndex++;
            }

            return result;
        }

        private List<SanteiGrpDetail> FindSanteiGrpDetailList(string itemCd)
        {
            var entities = NoTrackingDataContext.SanteiGrpDetails
                                    .Where(s => s.ItemCd == itemCd);
            return entities.ToList();
        }

        private SanteiCntCheck FindSanteiCntCheck(int hpId, int santeiGrpCd, int sinDate)
        {
            var entity = NoTrackingDataContext.SanteiCntChecks.Where(e =>
                 e.HpId == hpId &&
                 e.SanteiGrpCd == santeiGrpCd &&
                 e.StartDate <= sinDate &&
                 e.EndDate >= sinDate)
                 .FirstOrDefault();
            return entity ?? new SanteiCntCheck();
        }

        public TenItemModel FindTenMst(int hpId, string itemCd, int sinDate)
        {
            var entity = NoTrackingDataContext.TenMsts.FirstOrDefault(p =>
                   p.HpId == hpId &&
                   p.StartDate <= sinDate &&
                   p.EndDate >= sinDate &&
                   p.ItemCd == itemCd &&
                   p.IsDeleted == DeleteTypes.None);

            return entity != null ? new TenItemModel(
                   entity.HpId,
                   entity.ItemCd,
                   entity.RousaiKbn,
                   entity.KanaName1 ?? string.Empty,
                   entity.Name ?? string.Empty,
                   entity.KohatuKbn,
                   entity.MadokuKbn,
                   entity.KouseisinKbn,
                   entity.OdrUnitName ?? string.Empty,
                   entity.EndDate,
                   entity.DrugKbn,
                   entity.MasterSbt ?? string.Empty,
                   entity.BuiKbn,
                   entity.IsAdopted,
                   entity.Ten,
                   entity.TenId,
                   string.Empty,
                   string.Empty,
                   entity.CmtCol1,
                   entity.IpnNameCd ?? string.Empty,
                   entity.SinKouiKbn,
                   entity.YjCd ?? string.Empty,
                   entity.CnvUnitName ?? string.Empty,
                   entity.StartDate,
                   entity.YohoKbn,
                   entity.CmtColKeta1,
                   entity.CmtColKeta2,
                   entity.CmtColKeta3,
                   entity.CmtColKeta4,
                   entity.CmtCol2,
                   entity.CmtCol3,
                   entity.CmtCol4,
                   entity.IpnNameCd ?? string.Empty,
                   entity.MinAge ?? string.Empty,
                   entity.MaxAge ?? string.Empty,
                   entity.SanteiItemCd ?? string.Empty,
                   entity.OdrTermVal,
                   entity.CnvTermVal,
                   entity.DefaultVal,
                   entity.Kokuji1 ?? string.Empty,
                   entity.Kokuji2 ?? string.Empty
                ) : new();

        }

        public bool IsHolidayForDefaultTime(int hpId, int sinDate)
        {
            var holidayMst = NoTrackingDataContext.HolidayMsts.Where(t => t.HpId == hpId && t.SinDate == sinDate && t.IsDeleted != 1).FirstOrDefault();
            return holidayMst != null && holidayMst.HolidayKbn != 0;
        }
    }
}
