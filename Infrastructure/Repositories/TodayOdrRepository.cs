using Domain.Models.InsuranceInfor;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
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
        private List<string> _syosinls =
            new List<string>
            {
                ItemCdConst.Syosin,
                ItemCdConst.SyosinCorona,
                ItemCdConst.SyosinJouhou,
                ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi
            };

        public TodayOdrRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel)
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
                            SaveRaiinInf(hpId, ptId, raiinNo, sinDate, syosaiKbn, jikanKbn, hokenPid, santeiKbn, tantoId, kaId, uketukeTime, sinStartTime, sinEndTime);
                            UpsertOdrInfs(hpId, ptId, raiinNo, sinDate, odrInfs);
                        }

                        UpsertKarteInfs(karteInfModel);

                        SaveRaiinListInf(odrInfs);

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

        private void SaveRaiinInf(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime)
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
                raiinInf.UpdateId = TempIdentity.UserId;
                raiinInf.UpdateDate = DateTime.UtcNow;
                raiinInf.UpdateMachine = TempIdentity.ComputerName;
                _tenantTrackingDataContext.SaveChanges();
            }
        }


        private void SaveRaiinListInf(List<OrdInfModel> ordInfs)
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
                                                                                && item.RaiinListKbn == RaiinListKbnConstants.KOUI_KBN) ?? new RaiinListInf();
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
                                                                           && item.RaiinListKbn == RaiinListKbnConstants.ITEM_KBN) ?? new RaiinListInf();
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
                                        UpdateId = TempIdentity.UserId,
                                        UpdateMachine = TempIdentity.ComputerName,
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
                                    raiinListInf.UpdateId = TempIdentity.UserId;
                                    raiinListInf.UpdateMachine = TempIdentity.ComputerName;
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
                                        UpdateId = TempIdentity.UserId,
                                        UpdateMachine = TempIdentity.ComputerName,
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
                                    raiinListInf.UpdateId = TempIdentity.UserId;
                                    raiinListInf.UpdateMachine = TempIdentity.ComputerName;
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

        private void UpsertOdrInfs(int hpId, long ptId, long raiinNo, int sinDate, List<OrdInfModel> ordInfs)
        {
            var rpNoMax = GetMaxRpNo(hpId, ptId, raiinNo, sinDate);
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
                        var ordInfEntity = new OdrInf
                        {
                            HpId = item.HpId,
                            PtId = item.PtId,
                            SinDate = item.SinDate,
                            RaiinNo = item.RaiinNo,
                            RpNo = rpNoMax++,
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
                            CreateId = TempIdentity.UserId,
                            CreateMachine = TempIdentity.ComputerName,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = TempIdentity.UserId,
                            UpdateMachine = TempIdentity.ComputerName
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
                            CreateId = TempIdentity.UserId,
                            CreateMachine = TempIdentity.ComputerName,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = TempIdentity.UserId,
                            UpdateMachine = TempIdentity.ComputerName
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

        private void UpsertKarteInfs(KarteInfModel karte)
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
                var karteMst = _tenantTrackingDataContext.KarteInfs.FirstOrDefault(o => o.HpId == karte.HpId && o.PtId == karte.PtId && o.RaiinNo == karte.RaiinNo && karte.KarteKbn == o.KarteKbn);

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
                            CreateId = TempIdentity.UserId,
                            CreateMachine = TempIdentity.ComputerName,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = TempIdentity.UserId,
                            UpdateMachine = TempIdentity.ComputerName
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
                            CreateId = TempIdentity.UserId,
                            CreateMachine = TempIdentity.ComputerName,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = TempIdentity.UserId,
                            UpdateMachine = TempIdentity.ComputerName
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

        /// <summary>
        /// 年齢チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> AgeLimitCheck(int sinDate, int iBirthDay, int checkAge, TenItemModel tenMstItem, List<OrdInfDetailModel> allOdrInfDetail)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();

            #region sub function
            int iYear = 0;
            int iMonth = 0;
            int iDay = 0;
            bool needCheckMaxAge, needCheckMinAge;
            CIUtil.SDateToDecodeAge(iBirthDay, sinDate, ref iYear, ref iMonth, ref iDay);

            // Total day from birthday to sindate
            int iDays = 0;
            if (iBirthDay < sinDate)
            {
                iDays = CIUtil.DaysBetween(CIUtil.StrToDate(CIUtil.SDateToShowSDate(iBirthDay)), CIUtil.StrToDate(CIUtil.SDateToShowSDate(sinDate)));
            }

            // tenMstAgeCheck = TenMst.MinAge or TenMst.MaxAge
            bool _CheckInBirthMonth(int tenMstAgeCheck)
            {
                return (iYear > tenMstAgeCheck) ||
                       ((iYear == tenMstAgeCheck) && ((iBirthDay % 10000 / 100) < (sinDate % 10000 / 100)));
            }

            // tenMstAgeCheck = TenMst.MinAge or TenMst.MaxAge
            bool _CheckAge(string tenMstAgeCheck)
            {
                bool subResult = false;

                if (tenMstAgeCheck == "AA")
                {
                    // 生後２８日
                    subResult = (iDays >= 28);
                }
                else if (tenMstAgeCheck == "B3")
                {
                    //３歳に達した日の翌月の１日
                    subResult = _CheckInBirthMonth(3);
                }
                else if (tenMstAgeCheck == "B6")
                {
                    //６歳に達した日の翌月の１日
                    subResult = _CheckInBirthMonth(6);
                }
                else if (tenMstAgeCheck == "BF")
                {
                    //１５歳に達した日の翌月の１日（現状入院項目のみ）
                    subResult = _CheckInBirthMonth(15);
                }
                else if (tenMstAgeCheck == "BK")
                {
                    //２０歳に達した日の翌月の１日（現状入院項目のみ）
                    subResult = _CheckInBirthMonth(20);
                }
                else if (tenMstAgeCheck == "AE")
                {
                    //生後９０日
                    subResult = (iDays >= 90);
                }
                else if (tenMstAgeCheck == "MG")
                {
                    //未就学
                    subResult = CIUtil.IsStudent(iBirthDay, sinDate);
                }
                else
                {
                    subResult = iYear >= CIUtil.StrToIntDef(tenMstAgeCheck, 0);
                }
                return subResult;
            }

            // tenMstAgeCheck = TenMst.MinAge or TenMst.MaxAge
            string FormatDisplayMessage(string tenMstAgeCheck, CheckAgeType checkAgeType)
            {
                string formatedCheckKbn = string.Empty;

                if (tenMstAgeCheck == "AA")
                {
                    // 生後２８日
                    formatedCheckKbn = "生後２８日";
                }
                else if (tenMstAgeCheck == "B3")
                {
                    //３歳に達した日の翌月の１日
                    formatedCheckKbn = "３歳に達した日の翌月の１日";
                }
                else if (tenMstAgeCheck == "B6")
                {
                    //６歳に達した日の翌月の１日
                    formatedCheckKbn = "６歳に達した日の翌月の１日";
                }
                else if (tenMstAgeCheck == "BF")
                {
                    //１５歳に達した日の翌月の１日（現状入院項目のみ）
                    formatedCheckKbn = "１５歳に達した日の翌月の１日";
                }
                else if (tenMstAgeCheck == "BK")
                {
                    //２０歳に達した日の翌月の１日（現状入院項目のみ）
                    formatedCheckKbn = "２０歳に達した日の翌月の１日";
                }
                else if (tenMstAgeCheck == "AE")
                {
                    //生後９０日
                    formatedCheckKbn = "生後９０日";
                }
                else if (tenMstAgeCheck == "MG")
                {
                    //未就学
                    formatedCheckKbn = "未就学";
                }
                else
                {
                    formatedCheckKbn = CIUtil.StrToIntDef(tenMstAgeCheck, 0) + "歳";
                    if (checkAgeType == CheckAgeType.MaxAge)
                    {
                        formatedCheckKbn += "未満";
                    }
                    else if (checkAgeType == CheckAgeType.MinAge)
                    {
                        formatedCheckKbn += "以上";
                    }
                }
                return formatedCheckKbn;
            }
            #endregion

            if (checkAge == 0)
            {
                return checkSpecialItemList;
            }

            List<string> checkedItem = new List<string>();
            foreach (var detail in allOdrInfDetail)
            {
                if (checkedItem.Contains(detail.ItemCd))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(detail.ItemCd))
                {
                    continue;
                }
                if (tenMstItem == null)
                {
                    continue;
                }

                needCheckMaxAge = !string.IsNullOrEmpty(tenMstItem.MaxAge) && tenMstItem.MaxAge != "00" && tenMstItem.MaxAge != "0";
                needCheckMinAge = !string.IsNullOrEmpty(tenMstItem.MinAge) && tenMstItem.MinAge != "00" && tenMstItem.MinAge != "0";
                string msg = string.Empty;
                if (needCheckMinAge && needCheckMaxAge && _CheckAge(tenMstItem.MaxAge))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge)}、{FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge)}）\"は上限年齢以上のため、算定できません。";
                }
                else if (needCheckMinAge && needCheckMaxAge && !_CheckAge(tenMstItem.MinAge))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge)}、{FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge)}）\"は下限年齢未満のため、算定できません。";
                }
                if (needCheckMaxAge && _CheckAge(tenMstItem.MaxAge))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MaxAge, CheckAgeType.MaxAge)}）\"は上限年齢以上のため、算定できません。";
                }
                else if (needCheckMinAge && !_CheckAge(tenMstItem.MinAge))
                {
                    msg = $"\"{tenMstItem.Name}（{FormatDisplayMessage(tenMstItem.MinAge, CheckAgeType.MinAge)}）\"は下限年齢未満のため、算定できません。";
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.AgeLimit, string.Empty, msg, detail.ItemCd);


                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        /// <summary>
        /// 有効期限チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> ExpiredCheck(int sinDate, List<TenItemModel> tenMstItemList, List<OrdInfDetailModel> allOdrInfDetail)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();

            #region sub function
            string FormatDisplayMessage(string itemName, int dateCheck, bool isCheckStartDate)
            {
                string dateString = CIUtil.SDateToShowSDate(dateCheck);

                if (isCheckStartDate)
                {
                    return $"\"{itemName}\"は{dateString}から使用可能です。";
                }
                else
                {
                    return $"\"{itemName}\"は{dateString}まで使用可能です。";
                }
            }
            #endregion

            List<string> checkedItem = new List<string>();
            foreach (var detail in allOdrInfDetail)
            {
                if (checkedItem.Contains(detail.ItemCd))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(detail.ItemCd))
                {
                    continue;
                }
                if (tenMstItemList.Count == 0)
                {
                    continue;
                }
                int minStartDate = tenMstItemList.Min(item => item.StartDate);

                if (minStartDate > sinDate)
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.Expiration, string.Empty, FormatDisplayMessage(detail.DisplayItemName, minStartDate, true), detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                int maxEndDate = tenMstItemList.Max(item => item.EndDate);

                if (maxEndDate < sinDate)
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.Expiration, string.Empty, FormatDisplayMessage(detail.DisplayItemName, maxEndDate, false), detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        /// <summary>
        /// 算定回数チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> CalculationCountCheck(int hpId, int sinDate,long raiinNo, long ptId,  int hokenKbn, int hokensyuHandling, int syosinDate, TenItemModel santeiTenMst, List<DensiSanteiKaisuModel> densiSanteiKaisuModels, TenItemModel tenMst ,List<OrdInfDetailModel> allOdrInfDetail, List<ItemGrpMstModel> itemGrpMsts, List<(long rpno, long edano, int hokenId)> hokenIds)
        {
            #region Sub function
            int WeeksBefore(int baseDate, int term)
            {
                return CIUtil.WeeksBefore(baseDate, term);
            }

            int MonthsBefore(int baseDate, int term)
            {
                return CIUtil.MonthsBefore(baseDate, term);
            }

            int YearsBefore(int baseDate, int term)
            {
                return CIUtil.YearsBefore(baseDate, term);
            }

            int DaysBefore(int baseDate, int term)
            {
                return CIUtil.DaysBefore(baseDate, term);
            }

            int MonthsAfter(int baseDate, int term)
            {
                return CIUtil.MonthsAfter(baseDate, term);
            }
            int GetPtHokenKbn(long rpno, long edano)
            {
                int? ret = 0;
                if (hokenIds.Any(p => p.rpno == rpno && p.edano == edano))
                {
                    ret = hokenKbn;
                }

                return ret == null ? 0 : ret.Value;
            }

            int GetHokenKbn(int odrHokenKbn)
            {
                int hokenKbn = 0;

                if (new int[] { 0 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 4;
                }
                else if (new int[] { 1, 2 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 0;
                }
                else if (new int[] { 11, 12 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 1;
                }
                else if (new int[] { 13 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 2;
                }
                else if (new int[] { 14 }.Contains(odrHokenKbn))
                {
                    hokenKbn = 3;
                }

                return hokenKbn;
            }
            /// <summary>
            /// チェック用保険区分を返す
            /// 健保、労災、自賠の場合、オプションにより、同一扱いにするか別扱いにするか決定
            /// 自費の場合、健保と自費を対象にする
            /// </summary>
            /// <param name="hokenKbn">
            /// 0-健保、1-労災、2-アフターケア、3-自賠、4-自費
            /// </param>
            /// <returns></returns>
            List<int> GetCheckHokenKbns(int odrHokenKbn)
            {
                List<int> results = new List<int>();

                int hokenKbn = GetHokenKbn(odrHokenKbn);

                if (hokensyuHandling == 0)
                {
                    // 同一に考える
                    if (hokenKbn <= 3)
                    {
                        results.AddRange(new List<int> { 0, 1, 2, 3 });
                    }
                    else
                    {
                        results.Add(hokenKbn);
                    }
                }
                else if (hokensyuHandling == 1)
                {
                    // すべて同一に考える
                    results.AddRange(new List<int> { 0, 1, 2, 3, 4 });
                }
                else
                {
                    // 別に考える
                    results.Add(hokenKbn);
                }

                if (hokenKbn == 4)
                {
                    results.Add(0);
                }

                return results;
            }

            List<int> GetCheckSanteiKbns(int odrHokenKbn)
            {
                List<int> results = new List<int> { 0 };

                int hokenKbn = GetHokenKbn(odrHokenKbn);

                if (hokensyuHandling == 0)
                {
                    // 同一に考える
                    if (hokenKbn == 4)
                    {
                        //results.Add(2);
                    }
                }
                else if (hokensyuHandling == 1)
                {
                    // すべて同一に考える
                    results.Add(2);
                }
                else
                {
                    // 別に考える
                }

                return results;
            }
            #endregion

            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();
            int endDate = sinDate;
            // MAX_COUNT>1の場合は注意扱いする単位のコード

            List<string> checkedItem = new List<string>();
            foreach (var odrDetail in allOdrInfDetail)
            {
                if (string.IsNullOrEmpty(odrDetail.ItemCd))
                {
                    continue;
                }
                if (checkedItem.Contains(odrDetail.ItemCd))
                {
                    continue;
                }

                string santeiItemCd = odrDetail.ItemCd;
                string itemName = odrDetail.DisplayItemName;
                if (tenMst != null)
                {
                    if (!string.IsNullOrEmpty(tenMst.ItemCd)
                        && !string.IsNullOrEmpty(tenMst.SanteiItemCd)
                        && tenMst.ItemCd != tenMst.SanteiItemCd
                        && tenMst.SanteiItemCd != ItemCdConst.NoSantei
                        && !tenMst.ItemCd.StartsWith("Z"))
                    {
                        santeiItemCd = tenMst.SanteiItemCd;
                        if (santeiTenMst != null)
                        {
                            itemName = santeiTenMst.Name;
                        }
                    }
                }

                double suryo = 0;
                var allSameDetail = allOdrInfDetail.Where(d => d.ItemCd == odrDetail.ItemCd);
                foreach (var item in allSameDetail)
                {
                    suryo += (item.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(odrDetail.ItemCd)) ? 1 : item.Suryo;
                }

                //if (odrDetail.ItemCd != santeiItemCd)
                //{
                //    foreach (var odrInfDetail in allOdrInfDetail)
                //    {
                //        if (odrInfDetail.ItemCd == santeiItemCd)
                //        {
                //            suryo += (odrInfDetail.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(odrInfDetail.ItemCd)) ? 1 : odrInfDetail.Suryo;
                //        }
                //    }
                //}

                densiSanteiKaisuModels = densiSanteiKaisuModels.Where(d => d.ItemCd == santeiItemCd).ToList();
                // チェック期間と表記を取得する
                foreach (var densiSanteiKaisu in densiSanteiKaisuModels)
                {
                    string sTerm = string.Empty;
                    int startDate = 0;

                    List<int> checkHokenKbnTmp = new List<int>();
                    checkHokenKbnTmp.AddRange(GetCheckHokenKbns(GetPtHokenKbn(odrDetail.RpNo, odrDetail.RpEdaNo)));

                    if (densiSanteiKaisu.TargetKbn == 1)
                    {
                        // 健保のみ対象の場合はすべて対象
                    }
                    else if (densiSanteiKaisu.TargetKbn == 2)
                    {
                        // 労災のみ対象の場合、健保は抜く
                        checkHokenKbnTmp.RemoveAll(p => new int[] { 0 }.Contains(p));
                    }

                    List<int> checkSanteiKbnTmp = new List<int>();
                    checkSanteiKbnTmp.AddRange(GetCheckSanteiKbns(GetPtHokenKbn(odrDetail.RpNo, odrDetail.RpEdaNo)));

                    switch (densiSanteiKaisu.UnitCd)
                    {
                        case 53:    //患者あたり
                            sTerm = "患者あたり";
                            break;
                        case 121:   //1日
                            startDate = sinDate;
                            sTerm = "日";
                            break;
                        case 131:   //1月
                            startDate = sinDate / 100 * 100 + 1;
                            sTerm = "月";
                            break;
                        case 138:   //1週
                            startDate = WeeksBefore(sinDate, 1);
                            sTerm = "週";
                            break;
                        case 141:   //一連
                            startDate = -1;
                            sTerm = "一連";
                            break;
                        case 142:   //2週
                            startDate = WeeksBefore(sinDate, 2);
                            sTerm = "2週";
                            break;
                        case 143:   //2月
                            startDate = MonthsBefore(sinDate, 1);
                            sTerm = "2月";
                            break;
                        case 144:   //3月
                            startDate = MonthsBefore(sinDate, 2);
                            sTerm = "3月";
                            break;
                        case 145:   //4月
                            startDate = MonthsBefore(sinDate, 3);
                            sTerm = "4月";
                            break;
                        case 146:   //6月
                            startDate = MonthsBefore(sinDate, 5);
                            sTerm = "6月";
                            break;
                        case 147:   //12月
                            startDate = MonthsBefore(sinDate, 11);
                            sTerm = "12月";
                            break;
                        case 148:   //5年
                            startDate = YearsBefore(sinDate, 5);
                            sTerm = "5年";
                            break;
                        case 997:   //初診から1カ月（休日除く）
                            if (allOdrInfDetail.Where(d => d != odrDetail).Count(p => _syosinls.Contains(p.ItemCd)) > 0)
                            {
                                // 初診関連項目を算定している場合、算定不可
                                endDate = 99999999;
                            }
                            else
                            {
                                // 直近の初診日から１か月後を取得する（休日除く）
                                endDate = syosinDate;
                                endDate = MonthsAfterExcludeHoliday(hpId, endDate, 1);
                            }
                            break;
                        case 998:   //初診から1カ月
                            if (allOdrInfDetail.Where(d => d != odrDetail).Count(p => _syosinls.Contains(p.ItemCd)) > 0)
                            {
                                // 初診関連項目を算定している場合、算定不可
                                endDate = 99999999;
                            }
                            else
                            {
                                // 直近の初診日から１か月後を取得する（休日除く）
                                endDate = syosinDate;
                                endDate = MonthsAfter(endDate, 1);
                            }
                            break;
                        case 999:   //カスタム
                            if (densiSanteiKaisu.TermSbt == 2)
                            {
                                //日
                                startDate = DaysBefore(sinDate, densiSanteiKaisu.TermCount);
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "日";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "日";
                                }
                            }
                            else if (densiSanteiKaisu.TermSbt == 3)
                            {
                                //週
                                startDate = WeeksBefore(sinDate, densiSanteiKaisu.TermCount);
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "週";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "週";
                                }
                            }
                            else if (densiSanteiKaisu.TermSbt == 4)
                            {
                                //月
                                startDate = MonthsBefore(sinDate, densiSanteiKaisu.TermCount);
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "月";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "月";
                                }
                            }
                            else if (densiSanteiKaisu.TermSbt == 5)
                            {
                                //年
                                startDate = (sinDate / 10000 - (densiSanteiKaisu.TermCount - 1)) * 10000 + 101;
                                if (densiSanteiKaisu.TermCount == 1)
                                {
                                    sTerm = "年間";
                                }
                                else
                                {
                                    sTerm = densiSanteiKaisu.TermCount + "年間";
                                }
                            }
                            break;
                        default:
                            startDate = -1;
                            break;
                    }

                    if (densiSanteiKaisu.UnitCd == 997 || densiSanteiKaisu.UnitCd == 998)
                    {
                        //初診から1カ月
                        if (endDate > sinDate)
                        {
                            string conditionMsg = string.Empty;
                            //算定不可
                            if (densiSanteiKaisu.SpJyoken == 1)
                            {
                                conditionMsg = "算定できない可能性があります。";
                            }
                            else
                            {
                                conditionMsg = "算定できません。";
                            }

                            string errMsg = string.Format("'{0}' は、初診から1カ月以内のため、" + conditionMsg, odrDetail.ItemName);
                            CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.CalculationCount, string.Empty, errMsg, odrDetail.ItemCd);

                            checkSpecialItemList.Add(checkSpecialItem);
                        }
                    }
                    else
                    {
                        double count = 0;
                        if (startDate >= 0)
                        {
                            List<string> itemCds = new List<string>();

                            if (densiSanteiKaisu.ItemGrpCd > 0)
                            {
                                // 項目グループの設定がある場合
                                itemGrpMsts = itemGrpMsts.Where(i => i.ItemGrpCd == densiSanteiKaisu.ItemGrpCd).ToList();
                            }

                            if (itemGrpMsts != null && itemGrpMsts.Any())
                            {
                                // 項目グループの設定がある場合
                                itemCds.AddRange(itemGrpMsts.Select(x => x.ItemCd));
                            }
                            else
                            {
                                itemCds.Add(odrDetail.ItemCd);
                            }

                            count = SanteiCount(hpId, ptId, startDate, endDate, sinDate, raiinNo, itemCds, checkSanteiKbnTmp, checkHokenKbnTmp);
                        }
                        if (densiSanteiKaisu.MaxCount <= count // 上限値を超えるかチェックする
                        || densiSanteiKaisu.MaxCount < count + suryo) // 今回分を足すと超えてしまう場合は注意（MaxCount = count + konkaiSuryoはセーフ）
                        {
                            string errMsg = $"\"{itemName}\" {sTerm}{count + suryo}回算定({densiSanteiKaisu.MaxCount}回まで)";
                            CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.CalculationCount, string.Empty, errMsg, odrDetail.ItemCd);

                            checkSpecialItemList.Add(checkSpecialItem);
                        }
                    }
                }
                checkedItem.Add(odrDetail.ItemCd);
            }

            return checkSpecialItemList;
        }

        /// <summary>
        /// 重複チェック
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> DuplicateCheck(int sinDate, List<TenItemModel> tenMstItems, List<OrdInfDetailModel> allOdrInfDetail)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();
            List<string> checkedItem = new List<string>();
            foreach (var detail in allOdrInfDetail)
            {
                // ｺﾒﾝﾄや用法,特材、分割処方は対象外
                if (string.IsNullOrEmpty(detail.ItemCd) // ｺﾒﾝﾄ
                    || detail.ItemCd.StartsWith("Y") // 用法
                    || detail.ItemCd.StartsWith("Z") // 特材
                    || detail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu // 分割処方
                    || detail.ItemCd == ItemCdConst.Con_Refill) //リフィル
                {
                    continue;
                }

                if (checkedItem.Contains(detail.ItemCd))
                {
                    continue;
                }

                var tenMstItem = tenMstItems.FirstOrDefault(t => t.ItemCd == detail.ItemCd);
                if (tenMstItem == null)
                {
                    continue;
                }

                if (tenMstItem.MasterSbt == "C" || tenMstItem.BuiKbn > 0)
                {
                    continue;
                }

                var itemCount = allOdrInfDetail.Where(d => d.ItemCd == detail.ItemCd).Count();

                if (itemCount > 1)
                {
                    CheckSpecialItemModel checkSpecialItem = new CheckSpecialItemModel(CheckSpecialType.Duplicate, string.Empty, $"\"{detail.DisplayItemName}\"", detail.ItemCd);

                    checkSpecialItemList.Add(checkSpecialItem);
                }

                checkedItem.Add(detail.ItemCd);
            }

            return checkSpecialItemList;
        }

        ///<summary>
        ///指定の月数後の日付を取得する
        ///休日の場合は、その前の休日以外の日付
        ///</summary>
        ///<param name="baseDate">基準日</param>
        ///<param name="term">月数</param>
        ///<returns>基準日の指定月数後の休日以外の日付</returns>
        private int MonthsAfterExcludeHoliday(int hpId, int baseDate, int term)
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


        /// <summary>
        /// 項目コメント
        /// </summary>
        /// <param name="allOdrInfDetail"></param>
        /// <returns></returns>
        private List<CheckSpecialItemModel> ItemCommentCheck(Dictionary<string, string> items, List<ItemCmtModel> allCmtCheckMst, List<KarteInfModel> karteInfs)
        {
            List<CheckSpecialItemModel> checkSpecialItemList = new List<CheckSpecialItemModel>();
            foreach (var item in items)
            {
                if (checkSpecialItemList.Any(p => p.ItemCd == item.Key)) continue;

                if (IsShowCommentCheckMst(item.Key, allCmtCheckMst, karteInfs))
                {
                    checkSpecialItemList.Add(new CheckSpecialItemModel(CheckSpecialType.ItemComment, string.Empty, $"\"{item.Value}\"に対するコメントがありません。", item.Key));
                }
            }
            return checkSpecialItemList;
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
        private double SanteiCount(
            int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo,
            List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns)
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
                itemCds.Contains(p.ItemCd) &&
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
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    sinKouiCount.SinDate >= startDate &&
                    sinKouiCount.SinDate <= endDate &&
                    sinKouiCount.RaiinNo != raiinNo
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 || ItemCdConst.ZaitakuTokushu.Contains(a.sinKouiDetail.ItemCd) ? 1 : a.sinKouiDetail.Suryo)) }
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

        private bool IsShowCommentCheckMst(string itemCd, List<ItemCmtModel> allCmtCheckMst, List<KarteInfModel> karteInfs)
        {
            var itemCmtModels = allCmtCheckMst.FindAll(p => p.ItemCd == itemCd).OrderBy(p => p.SortNo).ToList();
            if (itemCmtModels.Count == 0) return false;

            foreach (var itemCmtModel in itemCmtModels)
            {
                if (karteInfs.Exists(p => p.KarteKbn == itemCmtModel.KarteKbn && p.Text.AsString().Contains(itemCmtModel.Comment)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
