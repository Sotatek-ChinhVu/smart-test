using Domain.Constant;
using Domain.Models.KensaIrai;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class KensaIraiRepository : RepositoryBase, IKensaIraiRepository
{
    private IMessenger? _messenger;
    public KensaIraiRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public KensaCenterMstModel GetKensaCenterMst(int hpId, string centerCd)
    {
        var kensaCenterMstModel = NoTrackingDataContext.KensaCenterMsts.FirstOrDefault(item => item.HpId == hpId && item.CenterCd == centerCd);
        if (kensaCenterMstModel == null)
        {
            return new();
        }
        return new KensaCenterMstModel(kensaCenterMstModel.CenterCd ?? string.Empty,
                                       kensaCenterMstModel.CenterName ?? string.Empty,
                                       kensaCenterMstModel.PrimaryKbn);
    }

    public List<KensaInfModel> GetKensaInf(int hpId, long ptId, long raiinNo, string centerCd)
    {
        var kensaInfs = NoTrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                      && item.PtId == ptId
                                                                      && item.RaiinNo == raiinNo
                                                                      && item.InoutKbn == 1
                                                                      && item.CenterCd == centerCd
                                                                      && item.IsDeleted == DeleteStatus.None
                                                              ).ToList();

        var odrInfDtls = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                           && item.PtId == ptId
                                                                           && item.RaiinNo == raiinNo
                                                                           && !string.IsNullOrEmpty(item.ReqCd))
                                                            .ToList();

        List<string> reqcds = new();
        if (odrInfDtls != null && odrInfDtls.Any())
        {
            reqcds = odrInfDtls.GroupBy(p => p.ReqCd).Select(p => p.Key ?? string.Empty).ToList();
        }

        List<KensaInfModel> results = new();

        kensaInfs?.ForEach(entity =>
        {
            if (!reqcds.Contains(entity.IraiCd.ToString()))
            {
                results.Add(new KensaInfModel(
                    entity.PtId,
                    entity.IraiDate,
                    entity.RaiinNo,
                    entity.IraiCd,
                    entity.InoutKbn,
                    entity.Status,
                    entity.TosekiKbn,
                    entity.SikyuKbn,
                    entity.ResultCheck,
                    entity.CenterCd ?? string.Empty,
                    entity.Nyubi ?? string.Empty,
                    entity.Yoketu ?? string.Empty,
                    entity.Bilirubin ?? string.Empty,
                    false,
                    entity.CreateId));
            }
        });
        return results;
    }

    public List<KensaInfDetailModel> GetKensaInfDetail(int hpId, long ptId, long raiinNo, string centerCd)
    {
        var kensaInfs = NoTrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                      && item.PtId == ptId
                                                                      && item.RaiinNo == raiinNo
                                                                      && item.InoutKbn == 1
                                                                      && item.CenterCd == centerCd
                                                                      && item.IsDeleted == DeleteStatus.None
        );

        var kensaInfDtls = NoTrackingDataContext.KensaInfDetails.Where(item => item.HpId == hpId
                                                                               && item.PtId == ptId
                                                                               && item.RaiinNo == raiinNo
                                                                               && item.IsDeleted == DeleteStatus.None
        );

        var result = (
                from kensaInf in kensaInfs
                join kensaInfDtl in kensaInfDtls on
                    new { kensaInf.HpId, kensaInf.PtId, kensaInf.IraiCd } equals
                    new { kensaInfDtl.HpId, kensaInfDtl.PtId, kensaInfDtl.IraiCd }
                select new KensaInfDetailModel(kensaInfDtl.PtId,
                                               kensaInfDtl.IraiDate,
                                               kensaInfDtl.RaiinNo,
                                               kensaInfDtl.IraiCd,
                                               kensaInfDtl.SeqNo,
                                               kensaInfDtl.KensaItemCd ?? string.Empty,
                                               kensaInfDtl.ResultVal ?? string.Empty,
                                               kensaInfDtl.ResultType ?? string.Empty,
                                               kensaInfDtl.AbnormalKbn ?? string.Empty,
                                               kensaInfDtl.IsDeleted,
                                               kensaInfDtl.CmtCd1 ?? string.Empty,
                                               kensaInfDtl.CmtCd2 ?? string.Empty,
                                               new())
                    ).ToList();
        return result;
    }

    public bool SaveKensaInf(int hpId, int userId, List<KensaInfModel> kensaInfModels, List<KensaInfDetailModel> kensaInfDetailModels)
    {
        bool successed = false;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    SaveKensaInfAction(hpId, userId, kensaInfModels, kensaInfDetailModels);
                    TrackingDataContext.SaveChanges();
                    transaction.Commit();
                    successed = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });
        return successed;
    }

    public List<KensaIraiModel> GetKensaIraiModels(int hpId, long ptId, int startDate, int endDate, string kensaCenterMstCenterCd, int kensaCenterMstPrimaryKbn)
    {
        List<KensaIraiModel> result = new();
        var odrInfList = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                     && item.IsDeleted == 0
                                                                     && (ptId == 0 || item.PtId == ptId)
                                                                     && item.OdrKouiKbn >= 60
                                                                     && item.OdrKouiKbn <= 69
                                                                     && item.InoutKbn == 1
                                                                     && item.SinDate >= startDate
                                                                     && item.SinDate <= endDate)
                                                      .ToList();

        var ptIdList = odrInfList.Select(item => item.PtId).Distinct().ToList();
        var raiinNoList = odrInfList.Select(item => item.RaiinNo).Distinct().ToList();
        var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                   && item.IsDelete == 0
                                                                   && ptIdList.Contains(item.PtId))
                                                    .ToList();

        var odrInfDetailList = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                 && (ptId == 0 || item.PtId == ptId)
                                                                                 && !string.IsNullOrEmpty(item.ItemCd)
                                                                                 && string.IsNullOrEmpty(item.ReqCd)
                                                                                 && raiinNoList.Contains(item.RaiinNo))
                                                                  .ToList();
        var itemCdList = odrInfDetailList.Select(item => item.ItemCd).Distinct().ToList();

        var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                         && item.IsDeleted == DeleteTypes.None
                                                                         && (ptId == 0 || item.PtId == ptId)
                                                                         && raiinNoList.Contains(item.RaiinNo))
                                                          .ToList();

        var tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                     && item.MasterSbt != "C"
                                                                     && item.SinKouiKbn >= 60
                                                                     && item.SinKouiKbn <= 69
                                                                     && item.IsDeleted == DeleteTypes.None
                                                                     && itemCdList.Contains(item.ItemCd))
                                                       .ToList();

        var kensaItemCdList = tenMstList.Select(item => item.KensaItemCd).Distinct().ToList();
        var kensaItemSeqNoList = tenMstList.Select(item => item.KensaItemSeqNo).Distinct().ToList();

        var kensaMstList = NoTrackingDataContext.KensaMsts.Where(item => item.HpId == hpId
                                                                         && (item.CenterCd == kensaCenterMstCenterCd || (kensaCenterMstPrimaryKbn == 1 && string.IsNullOrEmpty(item.CenterCd)))
                                                                         && kensaItemSeqNoList.Contains(item.KensaItemSeqNo)
                                                                         && kensaItemCdList.Contains(item.KensaItemCd))
                                                          .ToList();

        if (kensaMstList.Any())
        {
            kensaMstList = kensaMstList.GroupBy(item => item.KensaItemCd)
                                           .Select(item => item.OrderBy(sort => sort.KensaItemSeqNo).First())
                                           .ToList();
        }

        var todayOdrInfList = (from odrInfEntity in odrInfList
                               join ptInfEntity in ptInfList on
                               new { odrInfEntity.HpId, odrInfEntity.PtId } equals
                               new { ptInfEntity.HpId, ptInfEntity.PtId }
                               join raiinInfEntity in raiinInfList on
                               new { odrInfEntity.HpId, odrInfEntity.PtId, odrInfEntity.RaiinNo } equals
                               new { raiinInfEntity.HpId, raiinInfEntity.PtId, raiinInfEntity.RaiinNo }
                               select new
                               {
                                   odrInfEntity.SikyuKbn,
                                   odrInfEntity.TosekiKbn,
                                   odrInfEntity.SortNo,
                                   PtInf = ptInfEntity,
                                   RaiinInf = raiinInfEntity,
                                   OdrInfDetails = from odrInfDetailEntity in odrInfDetailList
                                                   where odrInfDetailEntity.HpId == odrInfEntity.HpId &&
                                                         odrInfDetailEntity.PtId == odrInfEntity.PtId &&
                                                         odrInfDetailEntity.RaiinNo == odrInfEntity.RaiinNo &&
                                                         odrInfDetailEntity.RpNo == odrInfEntity.RpNo &&
                                                         odrInfDetailEntity.RpEdaNo == odrInfEntity.RpEdaNo
                                                   join tenMstEntity in tenMstList on
                                                   new { odrInfDetailEntity.HpId, odrInfDetailEntity.ItemCd } equals
                                                   new { tenMstEntity.HpId, tenMstEntity.ItemCd }
                                                   join kensaMstEntity in kensaMstList on
                                                   new { tenMstEntity.HpId, tenMstEntity.KensaItemCd, tenMstEntity.KensaItemSeqNo } equals
                                                   new { kensaMstEntity.HpId, kensaMstEntity.KensaItemCd, kensaMstEntity.KensaItemSeqNo } into tenMstKensaMstList
                                                   from tenMstKensaMst in tenMstKensaMstList.DefaultIfEmpty()
                                                   where tenMstEntity.StartDate <= odrInfEntity.SinDate && tenMstEntity.EndDate >= odrInfEntity.SinDate
                                                   select new
                                                   {
                                                       odrInfDetailEntity,
                                                       tenMstEntity,
                                                       KensaMst = tenMstKensaMst,
                                                   }
                               }).ToList();
        var groupTodayOdrInfs = todayOdrInfList
                                .GroupBy(x => new
                                {
                                    x.RaiinInf.RaiinNo,
                                    x.SikyuKbn,
                                    x.TosekiKbn
                                })
                                .ToList();
        foreach (var groupTodayOdrInf in groupTodayOdrInfs)
        {
            List<KensaIraiDetailModel> kensaIraiDetailList = new();
            var groupTodayOdrInfList = groupTodayOdrInf.ToList();
            var firstTodayOdr = groupTodayOdrInfList.FirstOrDefault();
            if (firstTodayOdr == null)
            {
                continue;
            }
            foreach (var todayOdr in groupTodayOdrInfList)
            {
                var todayOdrList = todayOdr.OdrInfDetails
                                           .Select((item) => new KensaIraiDetailModel(
                                                                 item.tenMstEntity?.KensaItemCd ?? string.Empty,
                                                                 item.tenMstEntity?.ItemCd ?? string.Empty,
                                                                 item.tenMstEntity?.Name ?? string.Empty,
                                                                 item.tenMstEntity?.KanaName1 ?? string.Empty,
                                                                 item.KensaMst?.CenterCd ?? string.Empty,
                                                                 item.KensaMst?.KensaItemCd ?? string.Empty,
                                                                 item.KensaMst?.CenterItemCd1 ?? string.Empty,
                                                                 item.KensaMst?.KensaKana ?? string.Empty,
                                                                 item.KensaMst?.KensaName ?? string.Empty,
                                                                 item.KensaMst?.ContainerCd ?? 0,
                                                                 item.odrInfDetailEntity.RpNo,
                                                                 item.odrInfDetailEntity.RpEdaNo,
                                                                 item.odrInfDetailEntity.RowNo,
                                                                 0))
                                          .ToList();
                kensaIraiDetailList.AddRange(todayOdrList);
            }
            result.Add(new KensaIraiModel(
                            firstTodayOdr.RaiinInf.SinDate,
                            firstTodayOdr.RaiinInf.RaiinNo,
                            0,
                            firstTodayOdr.PtInf.PtId,
                            firstTodayOdr.PtInf.PtNum.AsLong(),
                            firstTodayOdr.PtInf.Name ?? string.Empty,
                            firstTodayOdr.PtInf.KanaName ?? string.Empty,
                            firstTodayOdr.PtInf.Sex,
                            firstTodayOdr.PtInf.Birthday,
                            firstTodayOdr.TosekiKbn,
                            firstTodayOdr.SikyuKbn,
                            firstTodayOdr.RaiinInf.KaId,
                            kensaIraiDetailList
                ));
        }

        // Filter irai done item
        result = result.Where(item => item.KensaIraiDetails.Any(item => !string.IsNullOrEmpty(item.KensaItemCd)))
                       .OrderBy(item => item.SinDate)
                       .ThenBy(item => item.PtNum)
                       .ThenBy(item => item.SikyuKbn)
                       .ToList();
        return result;
    }

    public List<KensaIraiModel> GetKensaIraiModels(int hpId, List<KensaInfModel> kensaInfModelList)
    {
        string centerCd = kensaInfModelList.FirstOrDefault()?.CenterCd ?? string.Empty;
        int primaryKbn = kensaInfModelList.FirstOrDefault()?.PrimaryKbn ?? 0;

        var ptIdList = kensaInfModelList.Select(item => item.PtId).Distinct().ToList();
        var raiinNoList = kensaInfModelList.Select(item => item.RaiinNo).Distinct().ToList();

        var odrInfDBList = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                       && item.IsDeleted == 0
                                                                       && ptIdList.Contains(item.PtId)
                                                                       && item.OdrKouiKbn >= 60
                                                                       && item.OdrKouiKbn <= 69
                                                                       && item.InoutKbn == 1
                                                                       && raiinNoList.Contains(item.RaiinNo))
                                                         .ToList();

        var ptInfDBList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                     && item.IsDelete == 0
                                                                     && ptIdList.Contains(item.PtId))
                                                      .ToList();

        var odrInfDetailDBList = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                   && !string.IsNullOrEmpty(item.ItemCd)
                                                                                   && ptIdList.Contains(item.PtId)
                                                                                   && raiinNoList.Contains(item.RaiinNo))
                                                                    .ToList();
        var itemCdList = odrInfDetailDBList.Select(item => item.ItemCd).Distinct().ToList();

        var raiinInfDBList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                           && item.IsDeleted == DeleteTypes.None
                                                                           && ptIdList.Contains(item.PtId)
                                                                           && raiinNoList.Contains(item.RaiinNo))
                                                            .ToList();

        var tenMstDBList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                       && item.MasterSbt != "C"
                                                                       && item.SinKouiKbn >= 60
                                                                       && item.SinKouiKbn <= 69
                                                                       && item.IsDeleted == DeleteTypes.None
                                                                       && itemCdList.Contains(item.ItemCd))
                                                        .ToList();

        var kensaItemCdList = tenMstDBList.Select(item => item.KensaItemCd).Distinct().ToList();
        var kensaItemSeqNoList = tenMstDBList.Select(item => item.KensaItemSeqNo).Distinct().ToList();

        var kensaMstDBList = NoTrackingDataContext.KensaMsts.Where(item => item.HpId == hpId
                                                                           && kensaItemSeqNoList.Contains(item.KensaItemSeqNo)
                                                                           && kensaItemCdList.Contains(item.KensaItemCd))
                                                            .ToList();

        if (kensaMstDBList.Any())
        {
            kensaMstDBList = kensaMstDBList.GroupBy(item => item.KensaItemCd)
                                           .Select(item => item.OrderBy(sort => sort.KensaItemSeqNo).First())
                                           .ToList();
        }

        var kensaMstEntities = kensaMstDBList.Where(item => (item.CenterCd == centerCd || (primaryKbn == 1 && string.IsNullOrEmpty(item.CenterCd)))).ToList();

        var kensaInfDBList = NoTrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                           && raiinNoList.Contains(item.RaiinNo)
                                                                           && item.CenterCd == centerCd
                                                                           && item.IsDeleted == 0)
                                              .ToList();

        var kensaInfDetailDBList = NoTrackingDataContext.KensaInfDetails.Where(item => item.HpId == hpId
                                                                                       && raiinNoList.Contains(item.RaiinNo)
                                                                                       && item.IsDeleted == 0)
                                                          .ToList();

        #region Get old kensaInf list
        List<KensaIraiModel> oldKensaIraiList = new();
        foreach (var model in kensaInfModelList)
        {
            var kensaInf = kensaInfDBList.FirstOrDefault(item => item.IraiCd == model.IraiCd);
            if (kensaInf == null)
            {
                continue;
            }
            var raiinInf = raiinInfDBList.FirstOrDefault(item => item.RaiinNo == model.RaiinNo);
            if (raiinInf == null)
            {
                continue;
            }
            var ptInf = ptInfDBList.FirstOrDefault(item => item.PtId == model.PtId);
            if (ptInf == null)
            {
                continue;
            }
            List<KensaIraiDetailModel> kensaIraiDetailList = new();
            var kensaInfDetailItemList = kensaInfDetailDBList.Where(item => item.IraiCd == kensaInf.IraiCd).ToList();

            var odrInfItemList = odrInfDBList.Where(odrInf => model.RaiinNo == odrInf.RaiinNo
                                                              && odrInf.SikyuKbn == kensaInf.SikyuKbn
                                                              && odrInf.TosekiKbn == kensaInf.TosekiKbn)
                                             .ToList();
            if (!odrInfItemList.Any())
            {
                continue;
            }

            foreach (var detail in kensaInfDetailItemList)
            {
                var kensaMst = kensaMstEntities.Where(item => item.KensaItemCd == detail.KensaItemCd)
                                               .OrderBy(item => item.KensaItemSeqNo)
                                               .FirstOrDefault();
                if (kensaMst == null)
                {
                    continue;
                }
                var tenMstItem = tenMstDBList.FirstOrDefault(item => kensaMst.KensaItemCd == item.KensaItemCd);
                if (tenMstItem == null)
                {
                    continue;
                }
                var odrInfDetailItem = odrInfDetailDBList.FirstOrDefault(item => odrInfItemList.Any(odr => item.RaiinNo == odr.RaiinNo
                                                                                                           && item.RpNo == odr.RpNo
                                                                                                           && item.RpEdaNo == odr.RpEdaNo)
                                                                                 && item.ItemCd == tenMstItem.ItemCd
                                                                                 && item.ReqCd == detail.IraiCd.ToString());
                if (odrInfDetailItem == null)
                {
                    continue;
                }
                odrInfDetailDBList.Remove(odrInfDetailItem);
                var detailModel = new KensaIraiDetailModel(
                                      tenMstItem.KensaItemCd ?? string.Empty,
                                      tenMstItem.ItemCd ?? string.Empty,
                                      tenMstItem.Name ?? string.Empty,
                                      tenMstItem.KanaName1 ?? string.Empty,
                                      kensaMst?.CenterCd ?? string.Empty,
                                      kensaMst?.KensaItemCd ?? string.Empty,
                                      kensaMst?.CenterItemCd1 ?? string.Empty,
                                      kensaMst?.KensaKana ?? string.Empty,
                                      kensaMst?.KensaName ?? string.Empty,
                                      kensaMst?.ContainerCd ?? 0,
                                      odrInfDetailItem?.RpNo ?? 0,
                                      odrInfDetailItem?.RpEdaNo ?? 0,
                                      odrInfDetailItem?.RowNo ?? 0,
                                      0);
                kensaIraiDetailList.Add(detailModel);
            }

            var newDetailModelList = odrInfDetailDBList.Where(item => odrInfItemList.Any(odr => item.RaiinNo == odr.RaiinNo
                                                                                                && item.RpNo == odr.RpNo
                                                                                                && item.RpEdaNo == odr.RpEdaNo))
                                                       .ToList();

            foreach (var odrDetail in newDetailModelList)
            {
                var tenMstItem = tenMstDBList.FirstOrDefault(item => odrDetail.ItemCd == item.ItemCd);
                if (tenMstItem == null)
                {
                    continue;
                }

                var kensaMst = kensaMstEntities.Where(item => item.KensaItemCd == tenMstItem.KensaItemCd)
                                               .OrderBy(item => item.KensaItemSeqNo)
                                               .FirstOrDefault();
                if (kensaMst == null)
                {
                    continue;
                }
                var odrInfDetailItem = odrInfDetailDBList.FirstOrDefault(item => odrInfItemList.Any(odr => item.RaiinNo == odr.RaiinNo
                                                                                                           && item.RpNo == odr.RpNo
                                                                                                           && item.RpEdaNo == odr.RpEdaNo)
                                                                                 && item.ItemCd == tenMstItem.ItemCd
                                                                                 && string.IsNullOrEmpty(item.ReqCd));
                if (odrInfDetailItem == null)
                {
                    continue;
                }
                odrInfDetailDBList.Remove(odrInfDetailItem);
                var detailModel = new KensaIraiDetailModel(
                                      tenMstItem.KensaItemCd ?? string.Empty,
                                      tenMstItem.ItemCd ?? string.Empty,
                                      tenMstItem.Name ?? string.Empty,
                                      tenMstItem.KanaName1 ?? string.Empty,
                                      kensaMst?.CenterCd ?? string.Empty,
                                      kensaMst?.KensaItemCd ?? string.Empty,
                                      kensaMst?.CenterItemCd1 ?? string.Empty,
                                      kensaMst?.KensaKana ?? string.Empty,
                                      kensaMst?.KensaName ?? string.Empty,
                                      kensaMst?.ContainerCd ?? 0,
                                      odrInfDetailItem?.RpNo ?? 0,
                                      odrInfDetailItem?.RpEdaNo ?? 0,
                                      odrInfDetailItem?.RowNo ?? 0,
                                      0);
                kensaIraiDetailList.Add(detailModel);
            }

            oldKensaIraiList.Add(new KensaIraiModel(
                                     raiinInf.SinDate,
                                     kensaInf.RaiinNo,
                                     kensaInf.IraiCd,
                                     kensaInf.PtId,
                                     ptInf.PtNum.AsLong(),
                                     ptInf.Name ?? string.Empty,
                                     ptInf.KanaName ?? string.Empty,
                                     ptInf.Sex,
                                     ptInf.Birthday,
                                     kensaInf.TosekiKbn,
                                     kensaInf.SikyuKbn,
                                     raiinInf.KaId,
                                     kensaInf.UpdateDate,
                                     kensaIraiDetailList
                                ));
        }
        #endregion

        #region Get new kensaInf list
        List<KensaIraiModel> newKensaIraiList = new();
        var groupOdrInf = odrInfDBList.GroupBy(item => new
        {
            item.RaiinNo,
            item.SikyuKbn,
            item.TosekiKbn
        }).ToList();
        foreach (var item in groupOdrInf)
        {
            var odrInfItemList = item.ToList();
            foreach (var odrInf in odrInfItemList)
            {
                List<KensaIraiDetailModel> kensaIraiDetailList = new();
                var raiinInf = raiinInfDBList.FirstOrDefault(item => item.RaiinNo == odrInf.RaiinNo);
                if (raiinInf == null)
                {
                    continue;
                }
                var ptInf = ptInfDBList.FirstOrDefault(item => item.PtId == odrInf.PtId);
                if (ptInf == null)
                {
                    continue;
                }

                var newDetailModelList = odrInfDetailDBList.Where(item => odrInfItemList.Any(odr => item.RaiinNo == odr.RaiinNo
                                                                                                    && item.RpNo == odr.RpNo
                                                                                                    && item.RpEdaNo == odr.RpEdaNo))
                                                      .ToList();

                foreach (var odrDetail in newDetailModelList)
                {
                    var tenMstItem = tenMstDBList.FirstOrDefault(item => odrDetail.ItemCd == item.ItemCd);
                    if (tenMstItem == null)
                    {
                        continue;
                    }

                    var kensaMst = kensaMstEntities.Where(item => item.KensaItemCd == tenMstItem.KensaItemCd)
                                                   .OrderBy(item => item.KensaItemSeqNo)
                                                   .FirstOrDefault();
                    if (kensaMst == null)
                    {
                        continue;
                    }
                    var odrInfDetailItem = odrInfDetailDBList.FirstOrDefault(item => odrInfItemList.Any(odr => item.RaiinNo == odr.RaiinNo
                                                                                                               && item.RpNo == odr.RpNo
                                                                                                               && item.RpEdaNo == odr.RpEdaNo)
                                                                                     && item.ItemCd == tenMstItem.ItemCd
                                                                                     && string.IsNullOrEmpty(item.ReqCd));
                    if (odrInfDetailItem == null)
                    {
                        continue;
                    }
                    odrInfDetailDBList.Remove(odrInfDetailItem);
                    var detailModel = new KensaIraiDetailModel(
                                          tenMstItem.KensaItemCd ?? string.Empty,
                                          tenMstItem.ItemCd ?? string.Empty,
                                          tenMstItem.Name ?? string.Empty,
                                          tenMstItem.KanaName1 ?? string.Empty,
                                          kensaMst?.CenterCd ?? string.Empty,
                                          kensaMst?.KensaItemCd ?? string.Empty,
                                          kensaMst?.CenterItemCd1 ?? string.Empty,
                                          kensaMst?.KensaKana ?? string.Empty,
                                          kensaMst?.KensaName ?? string.Empty,
                                          kensaMst?.ContainerCd ?? 0,
                                          odrInfDetailItem?.RpNo ?? 0,
                                          odrInfDetailItem?.RpEdaNo ?? 0,
                                          odrInfDetailItem?.RowNo ?? 0,
                                          0);
                    kensaIraiDetailList.Add(detailModel);
                }
                if (kensaIraiDetailList.Any())
                {
                    newKensaIraiList.Add(new KensaIraiModel(
                                             raiinInf.SinDate,
                                             raiinInf.RaiinNo,
                                             0,
                                             ptInf.PtId,
                                             ptInf.PtNum.AsLong(),
                                             ptInf.Name ?? string.Empty,
                                             ptInf.KanaName ?? string.Empty,
                                             ptInf.Sex,
                                             ptInf.Birthday,
                                             odrInf.TosekiKbn,
                                             odrInf.SikyuKbn,
                                             raiinInf.KaId,
                                             odrInf.UpdateDate,
                                             kensaIraiDetailList));
                }
            }
        }
        #endregion

        oldKensaIraiList.AddRange(newKensaIraiList);
        oldKensaIraiList = oldKensaIraiList.OrderBy(item => item.SinDate)
                                           .ThenBy(item => item.PtNum.AsLong())
                                           .ThenBy(item => item.SikyuKbn)
                                           .ToList();
        return oldKensaIraiList;
    }

    public List<KensaIraiModel> CreateDataKensaIraiRenkei(int hpId, int userId, List<KensaIraiModel> kensaIraiList, string centerCd, int systemDate)
    {
        List<KensaInf> kensaInfs = new();
        List<(KensaInf kensaInf, List<KensaInfDetail> kensaInfDetailList, List<OdrInfDetail> odrInfDetailList)> modelRelationList = new();
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    foreach (var kensaIrai in kensaIraiList)
                    {
                        KensaInf kensaInf = new()
                        {
                            HpId = hpId,
                            PtId = kensaIrai.PtId,
                            RaiinNo = kensaIrai.RaiinNo,
                            IraiDate = systemDate,
                            InoutKbn = 1,
                            Status = 0,
                            TosekiKbn = kensaIrai.TosekiKbn,
                            SikyuKbn = kensaIrai.SikyuKbn >= 1 ? 1 : kensaIrai.SikyuKbn,
                            ResultCheck = 0,
                            CenterCd = centerCd,
                            Nyubi = string.Empty,
                            Yoketu = string.Empty,
                            Bilirubin = string.Empty,
                            IsDeleted = 0,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId,
                        };
                        kensaInfs.Add(kensaInf);

                        var ptIdList = kensaIraiList.Select(item => item.PtId).Distinct().ToList();
                        var raiinNoList = kensaIraiList.Select(item => item.RaiinNo).Distinct().ToList();
                        List<long> rpNoList = new();
                        List<long> rpEdaNoList = new();
                        List<int> rowNoList = new();

                        foreach (var detailList in from KensaIraiModel item in kensaIraiList
                                                   let detailList = item.KensaIraiDetails
                                                   select detailList)
                        {
                            rpNoList.AddRange(detailList.Select(detail => detail.RpNo).Distinct().ToList());
                            rpEdaNoList.AddRange(detailList.Select(detail => detail.RpEdaNo).Distinct().ToList());
                            rowNoList.AddRange(detailList.Select(detail => detail.RowNo).Distinct().ToList());
                        }
                        rpNoList = rpNoList.Distinct().ToList();
                        rpEdaNoList = rpEdaNoList.Distinct().ToList();
                        rowNoList = rowNoList.Distinct().ToList();

                        var odrInfDetailDBList = TrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                                 && ptIdList.Contains(item.PtId)
                                                                                                 && raiinNoList.Contains(item.RaiinNo)
                                                                                                 && rpNoList.Contains(item.RpNo)
                                                                                                 && rpEdaNoList.Contains(item.RpEdaNo)
                                                                                                 && rowNoList.Contains(item.RowNo))
                                                                                   .ToList();

                        List<OdrInfDetail> odrInfDetailList = new();
                        List<KensaInfDetail> kensaInfDetailList = new();
                        foreach (var kensaIraiDetail in kensaIrai.KensaIraiDetails)
                        {
                            kensaInfDetailList.Add(new KensaInfDetail()
                            {
                                HpId = hpId,
                                PtId = kensaIrai.PtId,
                                RaiinNo = kensaIrai.RaiinNo,
                                IraiDate = systemDate,
                                KensaItemCd = kensaIraiDetail.KensaItemCd,
                                ResultVal = string.Empty,
                                ResultType = string.Empty,
                                AbnormalKbn = string.Empty,
                                IsDeleted = 0,
                                CmtCd1 = string.Empty,
                                CmtCd2 = string.Empty,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                CreateId = userId,
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateId = userId,
                            });

                            if (!string.IsNullOrEmpty(kensaIraiDetail.KensaItemCd))
                            {
                                var odrInfDetail = odrInfDetailDBList.FirstOrDefault(item => item.HpId == hpId
                                                                                             && item.PtId == kensaIrai.PtId
                                                                                             && item.RaiinNo == kensaIrai.RaiinNo
                                                                                             && item.RpNo == kensaIraiDetail.RpNo
                                                                                             && item.RpEdaNo == kensaIraiDetail.RpEdaNo
                                                                                             && item.RowNo == kensaIraiDetail.RowNo);
                                if (odrInfDetail != null)
                                {
                                    odrInfDetailList.Add(odrInfDetail);
                                }
                            }
                        }
                        modelRelationList.Add((kensaInf, kensaInfDetailList, odrInfDetailList));
                    }

                    TrackingDataContext.KensaInfs.AddRange(kensaInfs);
                    TrackingDataContext.SaveChanges();
                    List<KensaInfDetail> kensaDetails = new();

                    foreach (var modelRelation in modelRelationList)
                    {
                        foreach (var detail in modelRelation.kensaInfDetailList)
                        {
                            detail.IraiCd = modelRelation.kensaInf.IraiCd;
                        }

                        kensaDetails.AddRange(modelRelation.kensaInfDetailList);

                        foreach (var odrDetail in modelRelation.odrInfDetailList)
                        {
                            odrDetail.JissiKbn = 1;
                            odrDetail.JissiDate = CIUtil.GetJapanDateTimeNow();
                            odrDetail.JissiId = userId;
                            odrDetail.ReqCd = modelRelation.kensaInf.IraiCd.AsString();
                        }
                    }
                    TrackingDataContext.KensaInfDetails.AddRange(kensaDetails);
                    TrackingDataContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });

        var raiinNoList = kensaIraiList.Select(item => item.RaiinNo).Distinct().ToList();
        var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                         && raiinNoList.Contains(item.RaiinNo)
                                                                         && item.IsDeleted == 0);
        foreach (var model in kensaIraiList)
        {
            var kensaInf = kensaInfs.FirstOrDefault(item => item.PtId == model.PtId
                                                            && item.RaiinNo == model.RaiinNo
                                                            && item.SikyuKbn == model.SikyuKbn
                                                            && item.TosekiKbn == model.TosekiKbn);
            if (kensaInf == null)
            {
                continue;
            }
            var raiinInf = raiinInfList.FirstOrDefault(item => item.RaiinNo == model.RaiinNo);
            model.UpdateIraiCd(kensaInf.IraiCd, raiinInf?.KaId ?? 0);
        }
        return kensaIraiList;
    }

    public List<KensaIraiModel> ReCreateDataKensaIraiRenkei(int hpId, int userId, List<KensaIraiModel> kensaIraiList, int systemDate)
    {
        List<(KensaInf kensaInf, List<KensaInfDetail> kensaInfDetailList, List<OdrInfDetail> odrInfDetailList)> modelRelationList = new();
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    var iraiCdList = kensaIraiList.Select(item => item.IraiCd).Distinct().ToList();
                    var kensaIraiDBList = TrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                                      && item.IsDeleted == 0
                                                                                      && iraiCdList.Contains(item.IraiCd))
                                                                       .ToList();

                    string centerCd = kensaIraiDBList.FirstOrDefault()?.CenterCd ?? string.Empty;
                    var ptIdList = kensaIraiList.Select(item => item.PtId).Distinct().ToList();
                    var raiinNoList = kensaIraiList.Select(item => item.RaiinNo).Distinct().ToList();

                    var odrInfDetailDBList = TrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                             && ptIdList.Contains(item.PtId)
                                                                                             && raiinNoList.Contains(item.RaiinNo))
                                                                              .ToList();

                    var kensaInfDetailDBList = TrackingDataContext.KensaInfDetails.Where(item => item.HpId == hpId
                                                                                                 && item.IsDeleted == 0
                                                                                                 && iraiCdList.Contains(item.IraiCd))
                                                                                  .ToList();

                    foreach (var kensaIrai in kensaIraiList)
                    {
                        if (!kensaIrai.KensaIraiDetails.Any())
                        {
                            continue;
                        }
                        var kensaInf = kensaIraiDBList.FirstOrDefault(item => item.IraiCd == kensaIrai.IraiCd);
                        if (kensaInf == null && kensaIrai.IraiCd == 0)
                        {
                            kensaInf = new KensaInf();
                            kensaInf.HpId = hpId;
                            kensaInf.IsDeleted = 0;
                            kensaInf.RaiinNo = kensaIrai.RaiinNo;
                            kensaInf.PtId = kensaIrai.PtId;
                            kensaInf.CenterCd = centerCd;
                            kensaInf.IraiCd = 0;
                            kensaInf.CreateDate = CIUtil.GetJapanDateTimeNow();
                            kensaInf.CreateId = userId;
                        }
                        if (kensaInf != null)
                        {
                            kensaInf.IraiDate = systemDate;
                            kensaInf.InoutKbn = 1;
                            kensaInf.Status = 0;
                            kensaInf.TosekiKbn = kensaIrai.TosekiKbn;
                            kensaInf.SikyuKbn = kensaIrai.SikyuKbn >= 1 ? 1 : kensaIrai.SikyuKbn;
                            kensaInf.ResultCheck = 0;
                            kensaInf.Nyubi = string.Empty;
                            kensaInf.Yoketu = string.Empty;
                            kensaInf.Bilirubin = string.Empty;
                            kensaInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            kensaInf.UpdateId = userId;

                            List<KensaInfDetail> newKensaInfDetailList = new();
                            List<OdrInfDetail> newOdrInfList = new();
                            foreach (var kensa in kensaIrai.KensaIraiDetails)
                            {
                                newKensaInfDetailList.Add(new KensaInfDetail()
                                {
                                    HpId = hpId,
                                    PtId = kensaIrai.PtId,
                                    RaiinNo = kensaIrai.RaiinNo,
                                    IraiDate = systemDate,
                                    KensaItemCd = kensa.KensaItemCd,
                                    ResultVal = string.Empty,
                                    ResultType = string.Empty,
                                    AbnormalKbn = string.Empty,
                                    IsDeleted = 0,
                                    CmtCd1 = string.Empty,
                                    CmtCd2 = string.Empty,
                                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                                    CreateId = userId,
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateId = userId,
                                });

                                if (!string.IsNullOrEmpty(kensa.KensaItemCd))
                                {
                                    var odrInfDetail = odrInfDetailDBList.FirstOrDefault(item => item.PtId == kensaIrai.PtId
                                                                                                 && item.RaiinNo == kensaIrai.RaiinNo
                                                                                                 && item.RpNo == kensa.RpNo
                                                                                                 && item.RpEdaNo == kensa.RpEdaNo
                                                                                                 && item.RowNo == kensa.RowNo);
                                    if (odrInfDetail != null)
                                    {
                                        newOdrInfList.Add(odrInfDetail);
                                    }
                                }
                            }
                            if (kensaInf.IraiCd == 0)
                            {
                                TrackingDataContext.KensaInfs.Add(kensaInf);
                                TrackingDataContext.SaveChanges();
                            }
                            modelRelationList.Add((kensaInf, newKensaInfDetailList, newOdrInfList));
                        }

                        var kensaInfDetailList = kensaInfDetailDBList.Where(item => item.IraiCd == kensaIrai.IraiCd);
                        foreach (var detail in kensaInfDetailList)
                        {
                            detail.IsDeleted = 1;
                            detail.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            detail.UpdateId = userId;
                        }

                        if (kensaInf != null)
                        {
                            var odrInfDetails = odrInfDetailDBList.Where(item => item.PtId == kensaInf.PtId
                                                                                 && item.RaiinNo == kensaInf.RaiinNo
                                                                                 && item.ReqCd == kensaInf.IraiCd.ToString())
                                                                  .ToList();
                            foreach (var odrInfDetail in odrInfDetails)
                            {
                                odrInfDetail.JissiKbn = 0;
                                odrInfDetail.JissiDate = null;
                                odrInfDetail.JissiId = 0;
                                odrInfDetail.JissiMachine = string.Empty;
                                odrInfDetail.ReqCd = string.Empty;
                            }
                        }
                    }

                    List<KensaInfDetail> kensaDetailList = new();
                    foreach (var modelRelation in modelRelationList)
                    {
                        foreach (var detail in modelRelation.kensaInfDetailList)
                        {
                            detail.IraiCd = modelRelation.kensaInf.IraiCd;
                        }

                        kensaDetailList.AddRange(modelRelation.kensaInfDetailList);

                        foreach (var odrDetail in modelRelation.odrInfDetailList)
                        {
                            odrDetail.JissiKbn = 1;
                            odrDetail.JissiDate = CIUtil.GetJapanDateTimeNow();
                            odrDetail.JissiId = userId;
                            odrDetail.ReqCd = modelRelation.kensaInf.IraiCd.AsString();
                        }
                    }
                    TrackingDataContext.KensaInfDetails.AddRange(kensaDetailList);
                    TrackingDataContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });

        var raiinNoList = kensaIraiList.Select(item => item.RaiinNo).Distinct().ToList();
        var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                         && raiinNoList.Contains(item.RaiinNo)
                                                                         && item.IsDeleted == 0);
        foreach (var model in kensaIraiList)
        {
            var mode = modelRelationList.FirstOrDefault(item => item.kensaInf.PtId == model.PtId
                                                                     && item.kensaInf.RaiinNo == model.RaiinNo
                                                                     && item.kensaInf.SikyuKbn == model.SikyuKbn
                                                                     && item.kensaInf.TosekiKbn == model.TosekiKbn);
            if (mode.kensaInf == null)
            {
                continue;
            }
            var raiinInf = raiinInfList.FirstOrDefault(item => item.RaiinNo == model.RaiinNo);
            model.UpdateIraiCd(mode.kensaInf.IraiCd, raiinInf?.KaId ?? 0);
        }
        return kensaIraiList;
    }

    public bool CheckExistCenterCd(int hpId, string centerCd)
    {
        return NoTrackingDataContext.KensaCenterMsts.Any(item => item.HpId == hpId && item.CenterCd == centerCd);
    }

    public bool CheckExistCenterCd(int hpId, List<string> centerCdList)
    {
        centerCdList = centerCdList.Distinct().ToList();
        return NoTrackingDataContext.KensaCenterMsts.Count(item => item.HpId == hpId && item.CenterCd != null && centerCdList.Contains(item.CenterCd)) != centerCdList.Count;
    }

    public bool CheckExistIraiCd(int hpId, List<long> iraiCdList)
    {
        iraiCdList = iraiCdList.Distinct().ToList();
        return NoTrackingDataContext.KensaInfDetails.Count(item => item.HpId == hpId && iraiCdList.Contains(item.IraiCd)) != iraiCdList.Count;
    }

    public List<KensaInfModel> GetKensaInfModels(int hpId, int startDate, int endDate, string centerCd = "")
    {
        var kensaInfList = NoTrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                         && item.IraiDate >= startDate
                                                                         && item.IraiDate <= endDate
                                                                         && item.InoutKbn != 0)
                                                          .ToList();

        var ptIdList = kensaInfList.Select(item => item.PtId).Distinct().ToList();
        var raiinNoList = kensaInfList.Select(item => item.RaiinNo).Distinct().ToList();
        var iraiDateList = kensaInfList.Select(item => item.IraiDate).Distinct().ToList();
        var iraiCdList = kensaInfList.Select(item => item.IraiCd).Distinct().ToList();
        var centerCdList = kensaInfList.Select(item => item.CenterCd).Distinct().ToList();

        var kensaInfDetailList = NoTrackingDataContext.KensaInfDetails.Where(item => item.HpId == hpId
                                                                                     && item.IraiDate >= startDate
                                                                                     && item.IraiDate <= endDate
                                                                                     && ptIdList.Contains(item.PtId)
                                                                                     && raiinNoList.Contains(item.RaiinNo)
                                                                                     && iraiDateList.Contains(item.IraiDate)
                                                                                     && iraiCdList.Contains(item.IraiCd)
                                                                                     && item.IsDeleted == 0)
                                                                      .ToList();
        var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                   && item.IsDelete == 0
                                                                   && ptIdList.Contains(item.PtId))
                                                    .ToList();
        var kensaCenterMstEntity = NoTrackingDataContext.KensaCenterMsts.Where(item => item.HpId == hpId
                                                                                       && centerCdList.Contains(item.CenterCd));

        if (!string.IsNullOrEmpty(centerCd))
        {
            kensaCenterMstEntity = kensaCenterMstEntity.Where(x => x.CenterCd == centerCd);
        }
        var kensaCenterMstList = kensaCenterMstEntity.ToList();

        var query = from kensaInf in kensaInfList
                    join ptInf in ptInfList on
                    new { kensaInf.HpId, kensaInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                    join kensaCenterMst in kensaCenterMstList on
                    new { kensaInf.HpId, kensaInf.CenterCd } equals
                    new { kensaCenterMst.HpId, kensaCenterMst.CenterCd } into KensaInfCenterMsts
                    from KensaInfCenterMst in KensaInfCenterMsts.DefaultIfEmpty()
                    select new
                    {
                        kensaInf,
                        ptInf.Name,
                        ptInf.PtNum,
                        CenterName = KensaInfCenterMst != null ? KensaInfCenterMst.CenterName : string.Empty,
                        PrimaryKbn = KensaInfCenterMst == null ? 0 : KensaInfCenterMst.PrimaryKbn,
                        KensaInfDetails = from kensaInfDetail in kensaInfDetailList
                                          where kensaInf.HpId == kensaInfDetail.HpId &&
                                                kensaInf.IraiDate == kensaInfDetail.IraiDate &&
                                                kensaInf.IraiCd == kensaInfDetail.IraiCd &&
                                                kensaInf.PtId == kensaInfDetail.PtId &&
                                                kensaInf.RaiinNo == kensaInfDetail.RaiinNo &&
                                                kensaInfDetail.IsDeleted == 0
                                          select kensaInfDetail
                    };
        var result = query.Select(item => new KensaInfModel(
                                          item.kensaInf.PtId,
                                          item.kensaInf.IraiDate,
                                          item.kensaInf.RaiinNo,
                                          item.kensaInf.IraiCd,
                                          item.kensaInf.InoutKbn,
                                          item.kensaInf.Status,
                                          item.kensaInf.TosekiKbn,
                                          item.kensaInf.SikyuKbn,
                                          item.kensaInf.ResultCheck,
                                          item.kensaInf.CenterCd ?? string.Empty,
                                          item.kensaInf.Nyubi ?? string.Empty,
                                          item.kensaInf.Yoketu ?? string.Empty,
                                          item.kensaInf.Bilirubin ?? string.Empty,
                                          item.kensaInf.IsDeleted == 1,
                                          item.kensaInf.CreateId,
                                          item.PrimaryKbn,
                                          item.PtNum.AsLong(),
                                          item.Name,
                                          item.CenterName,
                                          item.kensaInf.UpdateDate,
                                          item.kensaInf.CreateDate,
                                          item.KensaInfDetails.Select(detail => new KensaInfDetailModel(
                                                                                detail.PtId,
                                                                                detail.IraiDate,
                                                                                detail.RaiinNo,
                                                                                detail.IraiCd,
                                                                                detail.SeqNo,
                                                                                detail.KensaItemCd ?? string.Empty,
                                                                                detail.ResultVal ?? string.Empty,
                                                                                detail.ResultType ?? string.Empty,
                                                                                detail.AbnormalKbn ?? string.Empty,
                                                                                detail.IsDeleted,
                                                                                detail.CmtCd1 ?? string.Empty,
                                                                                detail.CmtCd2 ?? string.Empty,
                                                                                new())).ToList()))
                          .OrderByDescending(x => x.IraiDate)
                          .ThenByDescending(x => x.UpdateDate)
                          .ToList();
        return result;
    }

    public bool DeleteKensaInfModel(int hpId, int userId, List<KensaInfModel> kensaInfList)
    {
        var iraiCdList = kensaInfList.Select(item => item.IraiCd).Distinct().ToList();
        var iraiCdStringList = kensaInfList.Select(item => item.IraiCd.ToString()).Distinct().ToList();
        var raiinNoList = kensaInfList.Select(item => item.RaiinNo).Distinct().ToList();
        List<long> ptIdList = new();
        List<long> seqNoList = new();

        foreach (var item in kensaInfList.Select(item => item.KensaInfDetailModelList).ToList())
        {
            ptIdList.AddRange(item.Select(detail => detail.PtId).ToList());
            seqNoList.AddRange(item.Select(detail => detail.SeqNo).ToList());
        }
        ptIdList = ptIdList.Distinct().ToList();
        seqNoList = seqNoList.Distinct().ToList();

        var kensaInfDBList = TrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                         && iraiCdList.Contains(item.IraiCd))
                                                          .ToList();

        var kensaInfDetailDBList = TrackingDataContext.KensaInfDetails.Where(item => item.HpId == hpId
                                                                                     && iraiCdList.Contains(item.IraiCd)
                                                                                     && ptIdList.Contains(item.PtId)
                                                                                     && seqNoList.Contains(item.SeqNo))
                                                                      .ToList();
        var orderInfDetailDBList = TrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                                   && raiinNoList.Contains(item.RaiinNo)
                                                                                   && iraiCdStringList.Contains(item.ReqCd ?? string.Empty))
                                                                    .ToList();
        foreach (var kensaInf in kensaInfList)
        {
            var deleteKensaInf = kensaInfDBList.FirstOrDefault(item => item.IraiCd == kensaInf.IraiCd);
            if (deleteKensaInf != null)
            {
                deleteKensaInf.IsDeleted = 1;
                deleteKensaInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                deleteKensaInf.UpdateId = userId;
            }

            foreach (var detail in kensaInf.KensaInfDetailModelList)
            {
                var deleteKensaInfDetail = kensaInfDetailDBList.FirstOrDefault(item => item.IraiCd == detail.IraiCd
                                                                                       && item.PtId == detail.PtId
                                                                                       && item.SeqNo == detail.SeqNo);
                if (deleteKensaInfDetail != null)
                {
                    deleteKensaInfDetail.IsDeleted = 1;
                    deleteKensaInfDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    deleteKensaInfDetail.UpdateId = userId;
                }
            }
            var odrInfDetails = orderInfDetailDBList.Where(item => item.PtId == kensaInf.PtId
                                                                   && item.RaiinNo == kensaInf.RaiinNo
                                                                   && item.ReqCd == kensaInf.IraiCd.ToString());
            foreach (var odrInfDetail in odrInfDetails)
            {
                odrInfDetail.JissiKbn = 0;
                odrInfDetail.JissiDate = null;
                odrInfDetail.JissiId = 0;
                odrInfDetail.JissiMachine = string.Empty;
                odrInfDetail.ReqCd = string.Empty;
            }
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool CheckExistIraiCdList(int hpId, List<long> iraiCdList)
    {
        iraiCdList = iraiCdList.Distinct().ToList();
        var kensaInfCount = TrackingDataContext.KensaInfs.Count(item => item.HpId == hpId
                                                                        && iraiCdList.Contains(item.IraiCd));
        return iraiCdList.Count == kensaInfCount;
    }

    public List<long> GetIraiCdNotExistList(int hpId, List<long> iraiCdList)
    {
        iraiCdList = iraiCdList.Distinct().ToList();
        var kensaInfExist = TrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                           && iraiCdList.Contains(item.IraiCd))
                                                         .Select(item => item.IraiCd)
                                                         .Distinct()
                                                         .ToList();
        var kensaInfNotExist = iraiCdList.Where(item => !kensaInfExist.Contains(item)).ToList();
        return kensaInfNotExist;
    }

    public List<KensaIraiLogModel> GetKensaIraiLogModels(int hpId, int startDate, int endDate)
    {
        var kensaIraiLogEntities = NoTrackingDataContext.KensaIraiLogs.Where(item => item.HpId == hpId
                                                                                     && item.IraiDate >= startDate
                                                                                     && item.IraiDate <= endDate);
        var kencenterMstEntities = NoTrackingDataContext.KensaCenterMsts.Where(item => item.HpId == hpId);
        var query = from kensaIraiLog in kensaIraiLogEntities
                    join kensaCenterMst in kencenterMstEntities on
                    new { kensaIraiLog.HpId, kensaIraiLog.CenterCd } equals
                    new { kensaCenterMst.HpId, kensaCenterMst.CenterCd } into kensaInfList
                    from kensaInf in kensaInfList.DefaultIfEmpty()
                    select new
                    {
                        kensaIraiLog,
                        CenterName = kensaInf == null ? string.Empty : kensaInf.CenterName,
                    };
        var result = query.AsEnumerable()
                          .Select(item => new KensaIraiLogModel(
                                              item.kensaIraiLog.IraiDate,
                                              item.kensaIraiLog.CenterCd,
                                              item.CenterName,
                                              item.kensaIraiLog.FromDate,
                                              item.kensaIraiLog.ToDate,
                                              item.kensaIraiLog.IraiFile ?? string.Empty,
                                              item.kensaIraiLog.IraiList ?? new byte[] { },
                                              item.kensaIraiLog.CreateDate))
                          .ToList();
        return result;
    }

    public bool SaveKensaIraiLog(int hpId, int userId, KensaIraiLogModel model)
    {
        KensaIraiLog kensaIraiLog = new();
        kensaIraiLog.HpId = hpId;
        kensaIraiLog.CenterCd = model.CenterCd;
        kensaIraiLog.IraiDate = model.IraiDate;
        kensaIraiLog.FromDate = model.FromDate;
        kensaIraiLog.ToDate = model.ToDate;
        kensaIraiLog.IraiList = model.IraiList;
        kensaIraiLog.IraiFile = model.IraiFile;
        kensaIraiLog.CreateDate = CIUtil.GetJapanDateTimeNow();
        kensaIraiLog.CreateId = userId;
        kensaIraiLog.UpdateDate = CIUtil.GetJapanDateTimeNow();
        kensaIraiLog.UpdateId = userId;
        TrackingDataContext.Add(kensaIraiLog);
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<KensaInfMessageModel> SaveKensaIraiImport(int hpId, int userId, IMessenger messenger, List<KensaInfDetailModel> kensaInfDetailList)
    {
        _messenger = messenger;
        var iraiCdList = kensaInfDetailList.Select(item => item.IraiCd).Distinct().ToList();
        var kensaItemCdList = kensaInfDetailList.Select(item => item.KensaItemCd).Distinct().ToList();
        var kensaInfDBList = TrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                         && iraiCdList.Contains(item.IraiCd))
                                                          .ToList();
        var ptIdList = kensaInfDBList.Select(item => item.PtId).Distinct().ToList();
        var ptInfDBList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                     && ptIdList.Contains(item.PtId)
                                                                     && item.IsDelete == 0)
                                                      .ToList();

        var kensaInfDetailDBList = TrackingDataContext.KensaInfDetails.Where(item => item.HpId == hpId
                                                                                     && iraiCdList.Contains(item.IraiCd)
                                                                                     && item.KensaItemCd != null
                                                                                     && item.KensaItemCd != string.Empty
                                                                                     && item.IsDeleted == 0)
                                                                      .ToList();

        kensaItemCdList.AddRange(kensaInfDetailDBList.Select(item => item.KensaItemCd!));
        kensaItemCdList = kensaItemCdList.Distinct().ToList() ?? new();
        var kensaMstDBList = NoTrackingDataContext.KensaMsts.Where(item => item.HpId == hpId
                                                                           && (kensaItemCdList.Contains(item.KensaItemCd)
                                                                               || (item.OyaItemCd != null && kensaItemCdList.Contains(item.OyaItemCd)))
                                                                           && item.IsDelete == 0)
                                                            .ToList();
        if (kensaMstDBList.Any())
        {
            kensaMstDBList = kensaMstDBList.GroupBy(item => item.KensaItemCd)
                                           .Select(item => item.OrderBy(sort => sort.KensaItemSeqNo).First())
                                           .ToList();
        }

        List<KensaInfMessageModel> result = new();
        var iraiCdCount = iraiCdList.Count;
        string messageItem = string.Empty;
        int successCount = 0;
        bool doneProgress = true;


        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    foreach (var iraiCd in iraiCdList)
                    {
                        messageItem = string.Empty;
                        if (iraiCd == 0)
                        {
                            SendMessager(new KensaInfMessageStatus(true, false, string.Empty, "依頼キーが未設定です。"));
                            transaction.Rollback();
                            doneProgress = false;
                            break;
                        }
                        var kensaInfDetailModelList = kensaInfDetailList.Where(item => item.IraiCd == iraiCd).ToList();
                        var resultSave = SaveKensaInfAction(hpId, userId, iraiCd, kensaInfDetailModelList, ptInfDBList, kensaInfDBList, kensaInfDetailDBList, kensaMstDBList);
                        if (resultSave.rollBack)
                        {
                            transaction.Rollback();
                            doneProgress = false;
                            break;
                        }
                        if (resultSave.kensaInfMessageModel != null)
                        {
                            result.Add(resultSave.kensaInfMessageModel);
                            messageItem = JsonSerializer.Serialize(resultSave.kensaInfMessageModel);
                        }
                        successCount++;
                        if (successCount < iraiCdCount)
                        {
                            var status = new KensaInfMessageStatus(false, false, messageItem, string.Empty);
                            SendMessager(status);
                        }
                    }
                    if (doneProgress)
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    doneProgress = false;
                    throw;
                }
                finally
                {
                    if (doneProgress)
                    {
                        var status = new KensaInfMessageStatus(true, successCount, true, messageItem, string.Empty);
                        SendMessager(status);
                    }
                }
            });
        return result;
    }

    public bool SaveKensaResultLog(int hpId, int userId, string KekaFile)
    {
        var kensaResultLog = new KensaResultLog()
        {
            HpId = hpId,
            OpId = 0,
            ImpDate = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow()),
            KekaFile = KekaFile ?? string.Empty,
            CreateId = userId,
            CreateDate = CIUtil.GetJapanDateTimeNow()
        };
        TrackingDataContext.KensaResultLogs.Add(kensaResultLog);
        return TrackingDataContext.SaveChanges() > 0;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    #region private function
    private KensaInf ConvertToNewKensaInf(int hpId, int userId, KensaInfModel model)
    {
        KensaInf kensaInf = new();
        kensaInf.HpId = hpId;
        kensaInf.PtId = model.PtId;
        kensaInf.IraiDate = model.IraiDate;
        kensaInf.RaiinNo = model.RaiinNo;
        kensaInf.IraiCd = model.IraiCd;
        kensaInf.InoutKbn = model.InoutKbn;
        kensaInf.Status = model.Status;
        kensaInf.TosekiKbn = model.TosekiKbn;
        kensaInf.SikyuKbn = model.SikyuKbn;
        kensaInf.ResultCheck = model.ResultCheck;
        kensaInf.CenterCd = model.CenterCd;
        kensaInf.Nyubi = model.Nyubi;
        kensaInf.Yoketu = model.Yoketu;
        kensaInf.Bilirubin = model.Bilirubin;
        kensaInf.IsDeleted = model.IsDeleted ? 1 : 0;
        kensaInf.CreateDate = CIUtil.GetJapanDateTimeNow();
        kensaInf.CreateId = userId;
        kensaInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
        kensaInf.UpdateId = userId;
        return kensaInf;
    }

    private KensaInfDetail ConvertToNewKensaInfDetail(int hpId, int userId, KensaInfDetailModel model)
    {
        KensaInfDetail kensaInfDetail = new();
        kensaInfDetail.HpId = hpId;
        kensaInfDetail.SeqNo = 0;
        kensaInfDetail.PtId = model.PtId;
        kensaInfDetail.IraiCd = model.IraiCd;
        kensaInfDetail.RaiinNo = model.RaiinNo;
        kensaInfDetail.IraiDate = model.IraiDate;
        kensaInfDetail.KensaItemCd = model.KensaItemCd;
        kensaInfDetail.ResultVal = model.ResultVal;
        kensaInfDetail.ResultType = model.ResultType;
        kensaInfDetail.AbnormalKbn = model.AbnormalKbn;
        kensaInfDetail.IsDeleted = model.IsDeleted;
        kensaInfDetail.CmtCd1 = model.CmtCd1;
        kensaInfDetail.CmtCd2 = model.CmtCd2;
        kensaInfDetail.CreateDate = CIUtil.GetJapanDateTimeNow();
        kensaInfDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();
        kensaInfDetail.CreateId = userId;
        kensaInfDetail.UpdateId = userId;
        return kensaInfDetail;
    }

    private void SaveKensaInfAction(int hpId, int userId, List<KensaInfModel> kensaInfModels, List<KensaInfDetailModel> kensaInfDetailModels)
    {
        DateTime dt = CIUtil.GetJapanDateTimeNow();

        // 削除
        if (kensaInfDetailModels != null && kensaInfDetailModels.Any(p => p.IsDeleted == 1))
        {
            List<KensaInfDetailModel> delKensaDtls = kensaInfDetailModels.FindAll(p => p.IsDeleted == 1);
            var ptIdList = delKensaDtls.Select(item => item.PtId).Distinct().ToList();
            var seqNoList = delKensaDtls.Select(item => item.SeqNo).Distinct().ToList();
            var iraiCdList = delKensaDtls.Select(item => item.IraiCd).Distinct().ToList();
            var kensaInfDetailDeleteList = TrackingDataContext.KensaInfDetails.Where(item => item.HpId == hpId
                                                                                             && ptIdList.Contains(item.PtId)
                                                                                             && seqNoList.Contains(item.SeqNo)
                                                                                             && iraiCdList.Contains(item.IraiCd))
                                                                              .ToList();
            foreach (var item in kensaInfDetailDeleteList)
            {
                item.IsDeleted = 1;
                item.UpdateDate = dt;
                item.UpdateId = userId;
            }
            TrackingDataContext.KensaInfDetails.RemoveRange(kensaInfDetailDeleteList);
        }

        // 追加
        if (kensaInfModels.Any(p => p.IsAddNew))
        {
            List<KensaInfModel> addKensaInfs = kensaInfModels.FindAll(p => p.IsAddNew);
            foreach (KensaInfModel addKensaInf in addKensaInfs)
            {
                KensaInf newKensaInf = ConvertToNewKensaInf(hpId, userId, addKensaInf);
                newKensaInf.CreateDate = dt;
                newKensaInf.CreateId = addKensaInf.CreateId;
                newKensaInf.UpdateDate = dt;
                newKensaInf.UpdateId = userId;
                TrackingDataContext.KensaInfs.Add(newKensaInf);
                TrackingDataContext.SaveChanges();
                addKensaInf.ChangeIraiCd(newKensaInf.IraiCd);
            }

            // detailにiraicdを反映
            if (kensaInfDetailModels != null && kensaInfDetailModels.Any())
            {
                foreach (KensaInfModel addKensaInf in addKensaInfs)
                {
                    foreach (KensaInfDetailModel updKensaDtl in kensaInfDetailModels.FindAll(p => p.KeyNo == addKensaInf.KeyNo))
                    {
                        updKensaDtl.ChangeIraiCd(addKensaInf.IraiCd);
                    }
                }
            }
        }

        if (kensaInfDetailModels != null && kensaInfDetailModels.Any(p => p.IsAddNew))
        {
            List<KensaInfDetailModel> addKensaDtls = kensaInfDetailModels.FindAll(p => p.IsAddNew);
            foreach (KensaInfDetailModel addKensaDtl in addKensaDtls)
            {
                KensaInfDetail newKensaInfDetail = ConvertToNewKensaInfDetail(hpId, userId, addKensaDtl);
                newKensaInfDetail.CreateDate = dt;
                newKensaInfDetail.UpdateDate = dt;
                TrackingDataContext.KensaInfDetails.Add(newKensaInfDetail);
            }
        }

        if (kensaInfModels.Any(p => p.IsUpdate))
        {
            List<KensaInfModel> updKensaInfs = kensaInfModels.FindAll(p => p.IsUpdate);
            var ptIdList = updKensaInfs.Select(item => item.PtId).Distinct().ToList();
            var iraiCdList = updKensaInfs.Select(item => item.IraiCd).Distinct().ToList();

            var updKensaInfDB = TrackingDataContext.KensaInfs.Where(item => item.HpId == hpId
                                                                            && ptIdList.Contains(item.PtId)
                                                                            && iraiCdList.Contains(item.IraiCd))
                                                             .ToList();

            foreach (KensaInfModel model in updKensaInfs)
            {
                var kensaInf = updKensaInfDB.FirstOrDefault(item => item.IraiCd == model.IraiCd && item.PtId == model.PtId);
                if (kensaInf != null)
                {
                    kensaInf.IraiDate = model.IraiDate;
                    kensaInf.RaiinNo = model.RaiinNo;
                    kensaInf.IraiCd = model.IraiCd;
                    kensaInf.InoutKbn = model.InoutKbn;
                    kensaInf.Status = model.Status;
                    kensaInf.TosekiKbn = model.TosekiKbn;
                    kensaInf.SikyuKbn = model.SikyuKbn;
                    kensaInf.ResultCheck = model.ResultCheck;
                    kensaInf.CenterCd = model.CenterCd;
                    kensaInf.Nyubi = model.Nyubi;
                    kensaInf.Yoketu = model.Yoketu;
                    kensaInf.Bilirubin = model.Bilirubin;
                    kensaInf.IsDeleted = model.IsDeleted ? 1 : 0;
                    kensaInf.UpdateDate = dt;
                    kensaInf.UpdateId = userId;
                }
            }
        }
        TrackingDataContext.SaveChanges();
    }

    private (KensaInfMessageModel? kensaInfMessageModel, bool rollBack) SaveKensaInfAction(int hpId, int userId, long iraiCd, List<KensaInfDetailModel> kensaInfDetailModelList, List<PtInf> ptInfDBList, List<KensaInf> kensaInfDBList, List<KensaInfDetail> kensaInfDetailDBList, List<KensaMst> kensaMstDBList)
    {
        List<KensaInfDetailMessageModel> kensaInfDetailMessageList = new();
        kensaInfDetailModelList = kensaInfDetailModelList.OrderBy(item => item.Index).ToList();

        // update kensaInf
        var kensaInf = kensaInfDBList.FirstOrDefault(item => item.IraiCd == iraiCd);
        if (kensaInf == null)
        {
            // 分析物コードが未設定の場合は、読み飛ばす。 skip if iraiCd is not match
            return (null, false);
        }
        string iraiDate = CIUtil.SDateToShowSDate(kensaInf.IraiDate);
        var ptInf = ptInfDBList.FirstOrDefault(item => item.PtId == kensaInf.PtId);

        // update kensaInfDetail
        List<(int index, long seqNo)> indexUsedList = new();
        List<long> parentSeqNoUsedList = new();

        foreach (var detailModel in kensaInfDetailModelList)
        {
            bool isAddNew = false;
            if (string.IsNullOrEmpty(detailModel.KensaItemCd))
            {
                continue;
            }

            var kensaInfDetail = kensaInfDetailDBList.FirstOrDefault(item => item.IraiCd == iraiCd
                                                                             && item.KensaItemCd == detailModel.KensaItemCd
                                                                             && !indexUsedList.Select(item => item.index).Contains(detailModel.Index)
                                                                             && !indexUsedList.Select(index => index.seqNo).Contains(item.SeqNo));
            var kensaMst = kensaMstDBList.FirstOrDefault(item => item.KensaItemCd == detailModel.KensaItemCd);
            if (kensaMst == null)
            {
                continue;
            }

            #region check and return error message to FE
            if (string.IsNullOrEmpty(detailModel.Type) || detailModel.Type != "A1")
            {
                if (kensaInfDetailMessageList.Any())
                {
                    string itemSuccessed = JsonSerializer.Serialize(new KensaInfMessageModel(
                                                                    kensaInf.PtId,
                                                                    iraiCd,
                                                                    iraiDate,
                                                                    ptInf?.PtNum.AsLong() ?? 0,
                                                                    ptInf?.Name ?? string.Empty,
                                                                    kensaInfDetailMessageList));
                    SendMessager(new KensaInfMessageStatus(false, false, itemSuccessed, string.Empty));
                }

                string errorItem = JsonSerializer.Serialize(new KensaInfMessageModel(
                                                                kensaInf.PtId,
                                                                iraiCd,
                                                                iraiDate,
                                                                ptInf?.PtNum.AsLong() ?? 0,
                                                                ptInf?.Name ?? string.Empty,
                                                                new()
                                                                {
                                                                    new KensaInfDetailMessageModel(detailModel.KensaItemCd, kensaMst.KensaName ?? string.Empty)
                                                                }));
                SendMessager(new KensaInfMessageStatus(true, false, errorItem, string.IsNullOrEmpty(detailModel.Type) ? "レコード区分が未設定です。" : "レコード区分の値が不正です。"));
                return (null, true);
            }
            if (string.IsNullOrEmpty(detailModel.CenterCd))
            {
                if (kensaInfDetailMessageList.Any())
                {
                    string message = JsonSerializer.Serialize(new KensaInfMessageModel(
                                                                  kensaInf.PtId,
                                                                  iraiCd,
                                                                  iraiDate,
                                                                  ptInf?.PtNum.AsLong() ?? 0,
                                                                  ptInf?.Name ?? string.Empty,
                                                                  kensaInfDetailMessageList));
                    SendMessager(new KensaInfMessageStatus(false, false, message, string.Empty));
                }
                string errorItem = JsonSerializer.Serialize(new KensaInfMessageModel(
                                                                kensaInf.PtId,
                                                                iraiCd,
                                                                iraiDate,
                                                                ptInf?.PtNum.AsLong() ?? 0,
                                                                ptInf?.Name ?? string.Empty,
                                                                new()
                                                                {
                                                                    new KensaInfDetailMessageModel(detailModel.KensaItemCd, kensaMst.KensaName??string.Empty)
                                                                }));
                SendMessager(new KensaInfMessageStatus(true, false, errorItem, "センターコードが未設定です。"));
                return (null, true);
            }
            #endregion

            #region logic save
            if (kensaInfDetail == null)
            {
                kensaInfDetail = new KensaInfDetail();
                kensaInfDetail.HpId = hpId;
                kensaInfDetail.CreateDate = CIUtil.GetJapanDateTimeNow();
                kensaInfDetail.CreateId = userId;
                kensaInfDetail.IsDeleted = 0;
                kensaInfDetail.IraiCd = iraiCd;
                kensaInfDetail.PtId = kensaInf.PtId;
                kensaInfDetail.IraiDate = kensaInf.IraiDate;
                kensaInfDetail.RaiinNo = kensaInf.RaiinNo;
                kensaInfDetail.KensaItemCd = detailModel.KensaItemCd;
                kensaInfDetail.SeqNo = 0;
                isAddNew = true;
            }
            kensaInfDetail.UpdateDate = CIUtil.GetJapanDateTimeNow();
            kensaInfDetail.UpdateId = userId;
            kensaInfDetail.ResultVal = detailModel.ResultVal;
            kensaInfDetail.AbnormalKbn = detailModel.AbnormalKbn;
            kensaInfDetail.CmtCd1 = detailModel.CmtCd1;
            kensaInfDetail.CmtCd2 = detailModel.CmtCd2;
            kensaInfDetail.ResultType = detailModel.ResultType == "B" ? string.Empty : detailModel.ResultType;
            if (isAddNew)
            {
                TrackingDataContext.KensaInfDetails.Add(kensaInfDetail);
                TrackingDataContext.SaveChanges();
                kensaInfDetailDBList.Add(kensaInfDetail);
            }

            // get parent item in kensaDetailDbList
            var kensaDetailParent = kensaInfDetailDBList.Where(item => item.KensaItemCd == kensaMst.OyaItemCd
                                                                       && !parentSeqNoUsedList.Contains(item.SeqNo)
                                                                       && item.SeqNo != kensaInfDetail.SeqNo)
                                                        .OrderBy(item => item.SeqNo)
                                                        .FirstOrDefault();
            if (kensaDetailParent != null)
            {
                if (isAddNew)
                {
                    kensaInfDetail.SeqParentNo = kensaDetailParent.SeqNo;
                }
                parentSeqNoUsedList.Add(kensaDetailParent.SeqNo);
            }
            indexUsedList.Add(new(detailModel.Index, kensaInfDetail.SeqNo));
            kensaInfDetailMessageList.Add(new KensaInfDetailMessageModel(
                                              kensaInfDetail.KensaItemCd ?? string.Empty,
                                              kensaMst.KensaName ?? string.Empty));

            // update kensaInf
            kensaInf.CenterCd = detailModel.CenterCd;
            kensaInf.Nyubi = detailModel.Nyubi;
            kensaInf.Yoketu = detailModel.Yoketu;
            kensaInf.Bilirubin = detailModel.Bilirubin;
            kensaInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            kensaInf.UpdateId = userId;
            kensaInf.Status = 2;
            #endregion
        }

        TrackingDataContext.SaveChanges();
        var result = new KensaInfMessageModel(
                         kensaInf.PtId,
                         iraiCd,
                         iraiDate,
                         ptInf?.PtNum.AsLong() ?? 0,
                         ptInf?.Name ?? string.Empty,
                         kensaInfDetailMessageList);
        return (result, false);
    }

    private void SendMessager(KensaInfMessageStatus status)
    {
        _messenger!.Send(status);
    }
    #endregion
}
