using Domain.Models.Santei;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SanteiInfRepository : RepositoryBase, ISanteiInfRepository
{
    public SanteiInfRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

    public List<SanteiInfModel> GetListSanteiInf(int hpId, long ptId, int sinDate)
    {
        // Santei Inf
        var santeiInfQuery = NoTrackingDataContext.SanteiInfs.Where(item => item.HpId == hpId
                                                                && (item.PtId == ptId || item.PtId == 0));

        // Item Cd List from SanteiInf
        var itemCdList = santeiInfQuery.Select(item => item.ItemCd).Distinct().ToList();

        var tenMsts = NoTrackingDataContext.TenMsts.Where(e => e.HpId == hpId
                                                            && itemCdList.Contains(e.ItemCd)
                                                            && e.StartDate <= sinDate
                                                            && e.EndDate >= sinDate)
                                                   .Select(item => new { item.ItemCd, item.Name });

        // Query Santei inf code
        var kensaTenMst = NoTrackingDataContext.TenMsts.Where(e => e.HpId == hpId
                                                                && e.StartDate <= sinDate
                                                                && e.EndDate >= sinDate)
                                                       .Select(item => new { item.SanteiItemCd, item.ItemCd });

        var tenMstList = from santeiInf in santeiInfQuery
                         join tenMst in kensaTenMst on santeiInf.ItemCd
                                                   equals tenMst.SanteiItemCd into tenMstLeft
                         from tenMst in tenMstLeft.DefaultIfEmpty()
                         select new
                         {
                             SanteCd = santeiInf.ItemCd,
                             ItemCd = tenMst.ItemCd ?? santeiInf.ItemCd
                         };

        // Get Last order 前回日
        var odrInfDetailQuery = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                        && item.PtId == ptId
                                                                                        && item.SinDate <= sinDate)
                                                                                        .Select(item => new
                                                                                        {
                                                                                            item.HpId,
                                                                                            item.SinDate,
                                                                                            item.PtId,
                                                                                            item.RaiinNo,
                                                                                            item.RpEdaNo,
                                                                                            item.RpNo,
                                                                                            item.ItemCd,
                                                                                            item.Suryo
                                                                                        });

        var odrInfQuery = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.SinDate <= sinDate
                                                                                && item.IsDeleted == 0)
                                                                                .Select(item => new
                                                                                {
                                                                                    item.HpId,
                                                                                    item.SinDate,
                                                                                    item.PtId,
                                                                                    item.RaiinNo,
                                                                                    item.RpEdaNo,
                                                                                    item.RpNo
                                                                                });


        var odrInfLastOdrRawDataList = (from odrInfDetail in odrInfDetailQuery
                                        join odrInf in odrInfQuery on new { odrInfDetail.HpId, odrInfDetail.SinDate, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpEdaNo, odrInfDetail.RpNo }
                                                                   equals new { odrInf.HpId, odrInf.SinDate, odrInf.PtId, odrInf.RaiinNo, odrInf.RpEdaNo, odrInf.RpNo }
                                        join tenMst in tenMstList on new { odrInfDetail.ItemCd }
                                                                  equals new { tenMst.ItemCd }
                                        select new
                                        {
                                            odrInfDetail,
                                            odrInf,
                                            tenMst
                                        }
                                 ).ToList();



        var odrInfLastList = (from item in odrInfLastOdrRawDataList
                              group item.odrInfDetail by item.tenMst.SanteCd into g
                              select new
                              {
                                  ItemCd = g.Key,
                                  SinDate = g.Where(o => o.SinDate < sinDate).Select(x => x.SinDate).OrderByDescending(x => x).FirstOrDefault()
                              }).ToList();

        Dictionary<string, int> dicLastOrderDate = new();
        foreach (var lastOdr in odrInfLastList)
        {
            dicLastOrderDate.Add(lastOdr.ItemCd, lastOdr.SinDate);
        }

        // Count and sum 回数 and 数量

        // Santei inf detail query
        var santeiInfDetailQuery = NoTrackingDataContext.SanteiInfDetails.Where(item => item.HpId == hpId
                                                                                     && item.PtId == ptId
                                                                                     && item.IsDeleted == 0
                                                                                     && item.EndDate >= sinDate)
                                                                         .Select(item => new { item.ItemCd, item.KisanDate, item.KisanSbt })
                                                                         .OrderBy(item => item.ItemCd)
                                                                         .ToList();

        // Get 起算日. Get min KISAN_DATE (Check YUUKOU_DATE >= SINDATE)
        int kisanSbt = 0;

        // logic get last kisanDate
        List<KisanDateStruct> kisanDateList = santeiInfDetailQuery.Where(item => !string.IsNullOrEmpty(item.ItemCd))
                                                                  .OrderByDescending(item => item.KisanDate)
                                                                  .GroupBy(item => item.ItemCd)
                                                                  .Select(item => item.First())
                                                                  .Select(item => new KisanDateStruct(item.ItemCd!, item.KisanDate, item.KisanSbt))
                                                                  .ToList();

        // logic Type 初回算定 => If 前回日 already exists, 起算日 will not be displayed
        foreach (var kisanDateStructItem in kisanDateList)
        {
            if (dicLastOrderDate.ContainsKey(kisanDateStructItem.ItemCd) && kisanDateStructItem.KisanSbt == 1)
            {
                kisanDateStructItem.SetKisanDate(dicLastOrderDate[kisanDateStructItem.ItemCd], kisanSbt);
            }
        }

        var odrInfDetailJoinList = (from odrInfDetail in odrInfDetailQuery
                                    join odrInf in odrInfQuery on new { odrInfDetail.HpId, odrInfDetail.SinDate, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpEdaNo, odrInfDetail.RpNo }
                                                               equals new { odrInf.HpId, odrInf.SinDate, odrInf.PtId, odrInf.RaiinNo, odrInf.RpEdaNo, odrInf.RpNo }
                                    join tenMst in tenMstList on new { odrInfDetail.ItemCd }
                                                               equals new { tenMst.ItemCd }
                                    select new
                                    {
                                        odrInfDetail,
                                        odrInf,
                                        tenMst
                                    })
                                    .ToList();

        var listOdrInfDetailCounts = (from joinItem in odrInfDetailJoinList
                                      join kisan in kisanDateList on new { joinItem.odrInfDetail.ItemCd }
                                                                 equals new { kisan.ItemCd } into kisanLeft
                                      from kisan in kisanLeft.DefaultIfEmpty()
                                      where kisan.KisanDate == 0 || joinItem.odrInfDetail.SinDate >= kisan.KisanDate
                                      group joinItem.odrInfDetail by joinItem.tenMst.SanteCd into g
                                      select new SanteiInfModel(
                                                 g.Key,
                                                 g.Count(),
                                                 g.Sum(o => o.Suryo),
                                                 0,
                                                 0)
                                      ).ToList();

        // Current Month Count query
        int currentMonthStDate = 100 * (sinDate / 100) + 1;
        int currentMonthEndDate = currentMonthStDate + 30;
        var currentMonthOdrInfDetailQuery = from odrInfDetail in odrInfDetailQuery
                                            join odrInf in odrInfQuery on new { odrInfDetail.HpId, odrInfDetail.SinDate, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpEdaNo, odrInfDetail.RpNo }
                                                                       equals new { odrInf.HpId, odrInf.SinDate, odrInf.PtId, odrInf.RaiinNo, odrInf.RpEdaNo, odrInf.RpNo }
                                            join tenMst in tenMstList on new { odrInfDetail.ItemCd }
                                                                       equals new { tenMst.ItemCd }
                                            where odrInfDetail.SinDate <= currentMonthEndDate
                                               && odrInfDetail.SinDate >= currentMonthStDate
                                            group odrInfDetail by tenMst.SanteCd into g
                                            select new
                                            {
                                                ItemCd = g.Key,
                                                Count = g.Count(),
                                                Sum = g.Sum(o => o.Suryo)
                                            };
        var listCurrentMonthOdrInfDetails = currentMonthOdrInfDetailQuery.Select(item => new SanteiInfModel(
                                                                                                             item.ItemCd,
                                                                                                             0,
                                                                                                             0,
                                                                                                             item.Count,
                                                                                                             item.Sum
                                                                        )).ToList();

        // If have 起算日 then have 回数 and 数量 
        // Main query
        var query = from santeiInf in santeiInfQuery
                    join tenMst in tenMsts on new { santeiInf.ItemCd }
                                               equals new { tenMst.ItemCd }
                    select new
                    {
                        SanteiInf = santeiInf,
                        TenMst = tenMst
                    };

        var result = query.OrderBy(item => item.SanteiInf.PtId)
                    .ThenBy(item => item.SanteiInf.SeqNo)
                    .ToList();

        var listSanteiInfDetails = GetListSanteiInfDetails(hpId, ptId);

        return result.Select(item => ConvertToSanteiInfModel(
                                                    item.SanteiInf,
                                                    item.TenMst.Name,
                                                    dicLastOrderDate,
                                                    listOdrInfDetailCounts,
                                                    listCurrentMonthOdrInfDetails,
                                                    listSanteiInfDetails))
                     .ToList();
    }

    private class KisanDateStruct
    {
        public string ItemCd { get; private set; }

        public int KisanDate { get; private set; }

        public int KisanSbt { get; private set; }

        public KisanDateStruct SetKisanDate(int kisanDate, int kisanSbt)
        {
            KisanDate = kisanDate;
            KisanSbt = kisanSbt;
            return this;
        }

        public KisanDateStruct(string itemCd, int kisanDate, int kisanSbt)
        {
            ItemCd = itemCd;
            KisanSbt = kisanSbt;
            KisanDate = kisanDate;
        }
    }

    public List<SanteiInfDetailModel> GetListSanteiInfDetails(int hpId, long ptId)
    {
        return NoTrackingDataContext.SanteiInfDetails.Where(item => item.HpId == hpId
                                                                 && item.IsDeleted == 0
                                                                 && (item.PtId == ptId))
                                                     .Select(item => new SanteiInfDetailModel(
                                                                         item.Id,
                                                                         item.PtId,
                                                                         item.ItemCd ?? string.Empty,
                                                                         item.EndDate,
                                                                         item.KisanSbt,
                                                                         item.KisanDate,
                                                                         item.Byomei ?? string.Empty,
                                                                         item.HosokuComment ?? string.Empty,
                                                                         item.Comment ?? string.Empty
                                                     )).ToList();
    }

    public List<SanteiInfModel> GetOnlyListSanteiInf(int hpId, long ptId)
    {
        var listSanteiInfs = NoTrackingDataContext.SanteiInfs.Where(item => item.HpId == hpId
                                                                            && (item.PtId == ptId || item.PtId == 0))
                                                             .ToList();
        return listSanteiInfs.Select(item => ConvertToSanteiInfModel(item))
                             .ToList();
    }

    public bool SaveSantei(int hpId, int userId, long ptId, List<SanteiInfModel> listSanteiInfModels)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        return executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    if (SaveListSanteiInfAction(hpId, userId, ptId, listSanteiInfModels))
                    {
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    transaction.Rollback();
                    return false;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });
    }

    public bool CheckExistItemCd(int hpId, List<string> listItemCds)
    {
        var tenMsts = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                  && listItemCds.Contains(item.ItemCd)
                                                    ).Select(item => item.ItemCd)
                                                     .ToList();
        if (listItemCds.Any() && tenMsts.Any())
        {
            foreach (var item in listItemCds)
            {
                if (!tenMsts.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public List<SanteiInfModel> GetCalculationInfo(int hpId, long ptId, int sinDate)
    {
        List<int> listAletTermIsValid = new List<int>() { 2, 3, 4, 5, 6 };

        // Query Santei inf code
        var santeiInfs = NoTrackingDataContext.SanteiInfs.Where(u => u.HpId == hpId &&
                                                                                     (u.PtId == ptId || u.PtId == 0) &&
                                                                                     u.AlertDays > 0 &&
                                                                                     listAletTermIsValid.Contains(u.AlertTerm)).GroupBy(u => u.ItemCd).Select(x => x.OrderBy(t => t.PtId).FirstOrDefault() ?? new()).ToList();
        var itemCdOfSanteiInfs = santeiInfs.Select(s => s.ItemCd).Distinct().ToList();
        var santeiInfDetails = NoTrackingDataContext.SanteiInfDetails.Where(u => u.HpId == hpId &&
                                                                                                 u.PtId == ptId &&
                                                                                                 u.KisanDate > 0 &&
                                                                                                 u.EndDate >= sinDate &&
                                                                                                 u.IsDeleted == 0 && itemCdOfSanteiInfs.Contains(u.ItemCd));
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(u => u.HpId == hpId &&
                                                               u.PtId == ptId &&
                                                               u.SinDate < sinDate &&
                                                               u.IsDeleted == 0).ToList();
        var raiinNos = odrInfs.Select(o => o.RaiinNo).Distinct().ToList();
        var sinDates = odrInfs.Select(o => o.SinDate).Distinct().ToList();
        var rpNos = odrInfs.Select(o => o.RpNo).Distinct().ToList();
        var rpEdaNos = odrInfs.Select(o => o.RpEdaNo).Distinct().ToList();

        var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(u => u.HpId == hpId &&
                                                                                           u.PtId == ptId && raiinNos.Contains(u.RaiinNo) && sinDates.Contains(u.SinDate) && rpNos.Contains(u.RpNo) && rpEdaNos.Contains(u.RpEdaNo)).ToList();
        var itemCds = odrInfDetails.Select(od => od.ItemCd).Distinct().ToList();

        var tenMsts = NoTrackingDataContext.TenMsts.Where(u => u.HpId == hpId &&
                                                                        u.StartDate <= sinDate &&
                                                                        u.EndDate >= sinDate && itemCds.Contains(u.ItemCd));

        var kensaTenMst = NoTrackingDataContext.TenMsts.Where(e => e.HpId == hpId
                                                                                && e.StartDate <= sinDate
                                                                                && e.EndDate >= sinDate && itemCds.Contains(e.ItemCd));

        var tenMstList = from santeiInf in santeiInfs
                         join tenMst in kensaTenMst on santeiInf.ItemCd
                                                equals tenMst.SanteiItemCd into tenMstLeft
                         from tenMst in tenMstLeft.DefaultIfEmpty()
                         select new
                         {
                             SanteiCd = santeiInf.ItemCd,
                             ItemCd = tenMst?.ItemCd ?? santeiInf.ItemCd
                         };

        var listOdrInfs = from odrInfItem in odrInfs
                          join odrInfDetailItem in odrInfDetails on new { odrInfItem.RaiinNo, odrInfItem.RpEdaNo, odrInfItem.RpNo } equals
                                                                     new { odrInfDetailItem.RaiinNo, odrInfDetailItem.RpEdaNo, odrInfDetailItem.RpNo }
                          join tenMstItem in tenMstList on odrInfDetailItem.ItemCd equals tenMstItem.ItemCd
                          select new
                          {
                              tenMstItem.SanteiCd,
                              OdrInf = odrInfItem,
                              OdrInfDetail = odrInfDetailItem,
                          };

        //Get last oder day by ItemCd
        var listOrdInfomation = listOdrInfs.AsEnumerable().OrderByDescending(u => u.OdrInf.SinDate).GroupBy(o => o.SanteiCd).Select(g => g.First()).ToList(); //select distinct by ItemCd
        var listOrdDetailInfomation = listOrdInfomation.Select(o => new { o.OdrInfDetail, o.SanteiCd }).ToList(); // only select OdrDetailInfo 

        var santeiQuery = from santeiInfItem in santeiInfs
                          join santeiInfDetailItem in santeiInfDetails on santeiInfItem.ItemCd equals santeiInfDetailItem.ItemCd into listSanteiDetail
                          join tenMstItem in tenMsts on santeiInfItem.ItemCd equals tenMstItem.ItemCd
                          select new
                          {
                              SanteiInf = santeiInfItem,
                              SnteiInfDetail = listSanteiDetail.OrderByDescending(u => u.KisanDate).FirstOrDefault(),
                              TenMst = tenMstItem
                          };

        var result = santeiQuery.Select(u => new SanteiInfModel(u.SanteiInf.Id,
             u.SanteiInf.PtId, u.SanteiInf.ItemCd ?? string.Empty, u.SanteiInf.SeqNo, u.SanteiInf.AlertDays, u.SanteiInf.AlertTerm, u.TenMst.Name ?? string.Empty, LastOdrDate(u.SnteiInfDetail, listOrdDetailInfomation.FirstOrDefault(o => o.SanteiCd == u.SanteiInf.ItemCd
                                                                                                                                                                                                                                             && o.OdrInfDetail.SinDate < sinDate)?.OdrInfDetail), u.SnteiInfDetail?.KisanSbt ?? 0, 0, 0, 0, sinDate, new() { new SanteiInfDetailModel(u.SnteiInfDetail?.Id ?? 0, u.SnteiInfDetail?.ItemCd ?? string.Empty, u.SnteiInfDetail?.KisanSbt ?? 0, u.SnteiInfDetail?.KisanDate ?? 0, u.SnteiInfDetail?.EndDate ?? 0) })).OrderBy(t => t.ItemCd).ToList();

        return result;
    }

    private int LastOdrDate(SanteiInfDetail santeiInfDetail, OdrInfDetail? odrInfDetail)
    {
        if (santeiInfDetail != null && santeiInfDetail.KisanSbt != 1)
        {
            return santeiInfDetail.KisanDate;
        }
        else if (odrInfDetail != null)
        {
            return odrInfDetail.SinDate;
        }
        return 0;
    }

    public List<SanteiInfDetailModel> GetListAutoSanteiMst(int hpId)
    {
        var autoSanteiMsts = NoTrackingDataContext.AutoSanteiMsts.Where(u => u.HpId == hpId).OrderBy(u => u.StartDate).ToList();
        return autoSanteiMsts.Select(x => new SanteiInfDetailModel(
                                                x.Id,
                                                x.ItemCd,
                                                x.StartDate,
                                                x.EndDate
                              )).ToList();
    }

    public bool SaveAutoSanteiMst(int hpId, int userId, List<SanteiInfDetailModel> santeiMst)
    {
        var addedGenModels = new List<SanteiInfDetailModel>();
        var updatedGenModels = new List<SanteiInfDetailModel>();
        var deletedGenModels = new List<SanteiInfDetailModel>();

        foreach (var modelVal in santeiMst)
        {
            if (modelVal.AutoSanteiMstModelStatus == ModelStatus.Added && !modelVal.CheckDefaultValue())
            {
                addedGenModels.Add(modelVal);
            }
            if (modelVal.AutoSanteiMstModelStatus == ModelStatus.Modified)
            {
                updatedGenModels.Add(modelVal);
            }
            if (modelVal.AutoSanteiMstModelStatus == ModelStatus.Deleted)
            {
                deletedGenModels.Add(modelVal);
            }
        }

        if (!addedGenModels.Any() && !updatedGenModels.Any() && !deletedGenModels.Any()) return true;

        if (deletedGenModels.Any())
        {
            var modelsToDelete = TrackingDataContext.AutoSanteiMsts.AsEnumerable().Where(x => deletedGenModels.Any(d => d.Id == x.Id && x.HpId == hpId && d.ItemCd == x.ItemCd)).ToList();
            TrackingDataContext.AutoSanteiMsts.RemoveRange(modelsToDelete);
        }

        if (updatedGenModels.Any())
        {

            foreach (var model in updatedGenModels)
            {
                var santeis = TrackingDataContext.AutoSanteiMsts.FirstOrDefault(x => x.HpId == hpId && x.ItemCd == model.ItemCd && x.Id == model.Id);

                if (santeis != null)
                {
                    santeis.StartDate = model.StartDate;
                    santeis.EndDate = model.EndDate;
                    santeis.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    santeis.UpdateId = userId;
                }
            }
        }

        if (addedGenModels.Any())
        {
            foreach (var model in addedGenModels)
            {
                TrackingDataContext.AutoSanteiMsts.Add(new AutoSanteiMst()
                {
                    HpId = hpId,
                    ItemCd = model.ItemCd,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId
                });
            }
        }

        return TrackingDataContext.SaveChanges() > 0;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    #region private function
    private SanteiInfModel ConvertToSanteiInfModel(SanteiInf santeiInf, string itemName, Dictionary<string, int> dicLastOrderDate, List<SanteiInfModel> listOdrInfDetailCounts, List<SanteiInfModel> listCurrentMonthOdrInfDetails, List<SanteiInfDetailModel> listSanteiInfDetails)
    {
        int lastOdrDate = 0;
        int santeiItemCount = 0;
        double santeiItemSum = 0;
        int currentMonthSanteiItemCount = 0;
        double currentMonthSanteiItemSum = 0;

        // Get last order sindate
        foreach (var item in dicLastOrderDate)
        {
            if (item.Key == santeiInf.ItemCd)
            {
                lastOdrDate = item.Value;
                break;
            }
        }

        // Count and sum item
        var odrDetail = listOdrInfDetailCounts.FirstOrDefault(item => item.ItemCd == santeiInf.ItemCd);
        if (odrDetail != null)
        {
            santeiItemCount = odrDetail.SanteiItemCount;
            santeiItemSum = odrDetail.SanteiItemSum;
        }

        // Count and sum item by month
        var currentMonthOdrDetail = listCurrentMonthOdrInfDetails.FirstOrDefault(item => item.ItemCd == santeiInf.ItemCd);
        if (currentMonthOdrDetail != null)
        {
            currentMonthSanteiItemCount = currentMonthOdrDetail.CurrentMonthSanteiItemCount;
            currentMonthSanteiItemSum = currentMonthOdrDetail.CurrentMonthSanteiItemSum;
        }

        return new SanteiInfModel(
                                     santeiInf.Id,
                                     santeiInf.PtId,
                                     santeiInf.ItemCd ?? string.Empty,
                                     santeiInf.SeqNo,
                                     santeiInf.AlertDays,
                                     santeiInf.AlertTerm,
                                     itemName,
                                     lastOdrDate,
                                     santeiItemCount,
                                     santeiItemSum,
                                     currentMonthSanteiItemCount,
                                     currentMonthSanteiItemSum,
                                     0,
                                     listSanteiInfDetails.Where(item => item.ItemCd == santeiInf.ItemCd).ToList()
                                 );
    }

    private SanteiInfModel ConvertToSanteiInfModel(SanteiInf santeiInf)
    {
        return new SanteiInfModel(
                                     santeiInf.Id,
                                     santeiInf.PtId,
                                     santeiInf.ItemCd ?? string.Empty,
                                     santeiInf.AlertDays,
                                     santeiInf.AlertTerm
                                 );
    }

    private bool SaveListSanteiInfAction(int hpId, int userId, long ptId, List<SanteiInfModel> listSanteiInfModels)
    {
        var listSanteiInfId = listSanteiInfModels.Where(item => item.Id > 0).Select(item => item.Id).ToList();
        var listSanteiInfDb = TrackingDataContext.SanteiInfs.Where(item => item.HpId == hpId && listSanteiInfId.Contains(item.Id)).ToList();
        List<SanteiInfDetailModel> listSanteiInfDetailUpdates = new();
        foreach (var model in listSanteiInfModels)
        {
            listSanteiInfDetailUpdates.AddRange(model.SanteiInfDetailList);
            if (model.Id <= 0 && !model.IsDeleted)
            {
                TrackingDataContext.SanteiInfs.Add(ConvertToNewSanteiInfEntity(hpId, userId, model));
            }
            else if (model.Id > 0)
            {
                var santeiInf = listSanteiInfDb.FirstOrDefault(item => item.Id == model.Id);
                if (santeiInf != null)
                {
                    if (model.IsDeleted)
                    {
                        var listSantaiInfDetailDeletes = NoTrackingDataContext.SanteiInfDetails.Where(item => item.HpId == hpId
                                                                                                              && item.IsDeleted == 0
                                                                                                              && item.ItemCd == model.ItemCd
                                                                                                              && item.PtId == santeiInf.PtId)
                                                                                               .Select(item => new SanteiInfDetailModel(
                                                                                                                                    item.Id,
                                                                                                                                    item.PtId,
                                                                                                                                    item.ItemCd ?? string.Empty,
                                                                                                                                    item.EndDate,
                                                                                                                                    item.KisanSbt,
                                                                                                                                    item.KisanDate,
                                                                                                                                    item.Byomei ?? string.Empty,
                                                                                                                                    item.HosokuComment ?? string.Empty,
                                                                                                                                    item.Comment ?? string.Empty,
                                                                                                                                    true
                                                                                               )).ToList();
                        listSanteiInfDetailUpdates.AddRange(listSantaiInfDetailDeletes);
                        TrackingDataContext.SanteiInfs.Remove(santeiInf);
                    }
                    else
                    {
                        santeiInf.SeqNo = model.SeqNo;
                        santeiInf.PtId = model.PtId;
                        santeiInf.AlertTerm = model.AlertTerm;
                        santeiInf.AlertDays = model.AlertDays;
                        santeiInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        santeiInf.UpdateId = userId;
                    }
                }
            }
        }
        return SaveListSanteiInfDetail(hpId, userId, ptId, listSanteiInfDetailUpdates);
    }

    public bool SaveListSanteiInfDetail(int hpId, int userId, long ptId, List<SanteiInfDetailModel> listSanteiInfDetailModels)
    {
        var listSanteiInfDetailItemCd = listSanteiInfDetailModels.Select(item => item.ItemCd).Distinct().ToList();
        var listSanteiInfDetailDb = TrackingDataContext.SanteiInfDetails.Where(item => item.HpId == hpId && item.ItemCd != null
                                                                                       && listSanteiInfDetailItemCd.Contains(item.ItemCd)
                                                                                       && item.IsDeleted == 0)
                                                                        .ToList();

        foreach (var model in listSanteiInfDetailModels)
        {
            if (model.Id <= 0 && !model.IsDeleted)
            {
                TrackingDataContext.SanteiInfDetails.Add(ConvertToNewSanteiInfDetailEntity(hpId, userId, ptId, model));
            }
            else if (model.Id > 0)
            {
                var santeiInfDetail = listSanteiInfDetailDb.FirstOrDefault(item => item.Id == model.Id);
                if (santeiInfDetail != null)
                {
                    santeiInfDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    santeiInfDetail.UpdateId = userId;
                    if (model.IsDeleted)
                    {
                        santeiInfDetail.IsDeleted = 1;
                    }
                    else
                    {
                        santeiInfDetail.PtId = ptId;
                        santeiInfDetail.KisanSbt = model.KisanSbt;
                        santeiInfDetail.KisanDate = model.KisanDate;
                        santeiInfDetail.Byomei = model.Byomei;
                        santeiInfDetail.HosokuComment = model.HosokuComment;
                        santeiInfDetail.EndDate = model.EndDate;
                        santeiInfDetail.Comment = model.Comment;
                    }
                }
            }
        }
        TrackingDataContext.SaveChanges();
        return true;
    }

    private SanteiInf ConvertToNewSanteiInfEntity(int hpId, int userId, SanteiInfModel model)
    {
        return new SanteiInf()
        {
            Id = 0,
            PtId = model.PtId,
            HpId = hpId,
            ItemCd = model.ItemCd,
            SeqNo = model.SeqNo,
            AlertDays = model.AlertDays,
            AlertTerm = model.AlertTerm,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateId = userId
        };
    }

    private SanteiInfDetail ConvertToNewSanteiInfDetailEntity(int hpId, int userId, long ptId, SanteiInfDetailModel model)
    {
        return new SanteiInfDetail()
        {
            Id = 0,
            PtId = ptId,
            HpId = hpId,
            ItemCd = model.ItemCd,
            EndDate = model.EndDate,
            KisanSbt = model.KisanSbt,
            KisanDate = model.KisanDate,
            Byomei = model.Byomei,
            HosokuComment = model.HosokuComment,
            Comment = model.Comment,
            IsDeleted = 0,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateId = userId
        };
    }
    #endregion
}
