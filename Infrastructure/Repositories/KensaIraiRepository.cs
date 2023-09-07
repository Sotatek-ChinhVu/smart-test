using Domain.Constant;
using Domain.Models.KensaIrai;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class KensaIraiRepository : RepositoryBase, IKensaIraiRepository
{
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
                catch
                {
                    transaction.Rollback();
                }
            });
        return successed;
    }

    public List<KensaIraiModel> GetKensaIraiModels(int hpId, long ptId, int startDate, int endDate, string kensaCenterMstCenterCd, int kensaCenterMstPrimaryKbn)
    {
        List<KensaIraiModel> result = new();
        var ptInfList = TrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                 && item.IsDelete == 0
                                                                 && (ptId == 0 || item.PtId == ptId));
        var odrInfList = TrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                   && item.IsDeleted == 0
                                                                   && (ptId == 0 || item.PtId == ptId)
                                                                   && item.OdrKouiKbn >= 60
                                                                   && item.OdrKouiKbn <= 69
                                                                   && item.InoutKbn == 1
                                                                   && item.SinDate >= startDate
                                                                   && item.SinDate <= endDate);

        var odrInfDetailList = TrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                               && (ptId == 0 || item.PtId == ptId)
                                                                               && !string.IsNullOrEmpty(item.ItemCd)
                                                                               && string.IsNullOrEmpty(item.ReqCd));

        var raiinInfList = TrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                       && item.IsDeleted == DeleteTypes.None
                                                                       && (ptId == 0 || item.PtId == ptId));

        var tenMstList = TrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                   && item.MasterSbt != "C"
                                                                   && item.SinKouiKbn >= 60
                                                                   && item.SinKouiKbn <= 69
                                                                   && item.IsDeleted == DeleteTypes.None);

        var kensaMstList = TrackingDataContext.KensaMsts.Where(item => item.HpId == hpId
                                                                       && (item.CenterCd == kensaCenterMstCenterCd || (kensaCenterMstPrimaryKbn == 1 && string.IsNullOrEmpty(item.CenterCd))));

        var todayOdrInfList = from odrInfEntity in odrInfList
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
                              };
        var groupTodayOdrInfs = todayOdrInfList
                                    .AsEnumerable()
                                    .GroupBy(x => new
                                    {
                                        x.RaiinInf.RaiinNo,
                                        x.SikyuKbn,
                                        x.TosekiKbn
                                    });
        //foreach (var groupTodayOdrInf in groupTodayOdrInfs)
        //{
        //    List<KensaIraiDetailModel> odrInfDetails = new List<KensaIraiDetailModel>();
        //    var groupTodayOdrInfList = groupTodayOdrInf.ToList();
        //    var firstTodayOdr = groupTodayOdrInfList.First();
        //    foreach (var todayOdr in groupTodayOdrInfList)
        //    {
        //        odrInfDetails.AddRange(todayOdr
        //                                .OdrInfDetails
        //                                .AsEnumerable()
        //                                .Select((x) => new KensaIraiDetailModel(
        //                                     true,
        //                                     x.odrInfDetailEntity.RpNo,
        //                                     x.odrInfDetailEntity.RpEdaNo,
        //                                     x.odrInfDetailEntity.RowNo,
        //                                     0,
        //                                     x.tenMstEntity,
        //                                     x.KensaMst))
        //                                );
        //    }
        //    result.Add(new KensaIraiModel(
        //                    0,
        //                    firstTodayOdr.TosekiKbn,
        //                    firstTodayOdr.SikyuKbn,
        //                    firstTodayOdr.PtInf,
        //                    firstTodayOdr.RaiinInf,
        //                    odrInfDetails
        //        ));
        //}
        //// Filter irai done item
        //result = result.Where(item => item.Details.Where(d => !string.IsNullOrEmpty(d.KensaItemCd)).Count() > 0).ToList();
        return result;
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
        kensaInfDetail.PtId = model.PtId;
        kensaInfDetail.IraiCd = model.IraiCd;
        kensaInfDetail.RaiinNo = model.RaiinNo;
        kensaInfDetail.IraiDate = model.IraiDate;
        kensaInfDetail.SeqNo = model.SeqNo;
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


    #endregion
}
