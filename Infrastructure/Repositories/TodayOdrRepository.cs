using Domain.Models.Diseases;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
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

        private const string SUSPECT_FLAG = "の疑い";

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
            var raiinInf = _tenantTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);

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

        private string GetSanteiItemCd(int hpId, string itemCd, int sinDate)
        {
            var tenMst = _tenantNoTrackingDataContext.TenMsts.FirstOrDefault(p => p.HpId == hpId &&
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
            var tekiouByomeiMsts = _tenantNoTrackingDataContext.TekiouByomeiMsts.Where(p => p.HpId == hpId && itemCds.Contains(p.ItemCd) && p.IsInvalid == 0);
            var byomeiMsts = _tenantNoTrackingDataContext.ByomeiMsts.Where(p => p.HpId == hpId);

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

    }
}
