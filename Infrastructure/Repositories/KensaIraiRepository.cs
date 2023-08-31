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
