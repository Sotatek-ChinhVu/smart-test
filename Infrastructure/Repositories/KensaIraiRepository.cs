using Domain.Constant;
using Domain.Models.KensaIrai;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

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

    public bool SaveKensaInf(int hpId, int userId, List<KensaInfModel> KensaInfModels, List<KensaInfDetailModel> KensaInfDetailModels)
    {
        DateTime dt = CIUtil.GetJapanDateTimeNow();

        // 削除
        if (KensaInfDetailModels != null && KensaInfDetailModels.Any(p => p.IsDeleted == 1))
        {
            List<KensaInfDetailModel> delKensaDtls = KensaInfDetailModels.FindAll(p => p.IsDeleted == 1);
            var ptIdList = delKensaDtls.Select(item =>item.PtId).Distinct().ToList();
            var seqNoList = delKensaDtls.Select(item =>item.SeqNo).Distinct().ToList();
            var iraiCdList = delKensaDtls.Select(item =>item.IraiCd).Distinct().ToList();
            var kensaInfDetailDeleteList = TrackingDataContext.KensaInfDetails.Where(item => item.HpId==hpId
            && ptIdList.Contains(item.PtId)
            )
            dbService.KensaInfDetailRepository.RemoveRange(delKensaDtls.Select(p => p.KensaInfDetail));
        }

        // 追加
        if (KensaInfModels.Any(p => p.IsAddNew))
        {
            List<KensaInfModel> addKensaInfs = KensaInfModels.FindAll(p => p.IsAddNew);
            foreach (KensaInfModel addKensaInf in addKensaInfs)
            {
                addKensaInf.CreateDate = dt;
                if (addKensaInf.CreateId == 0)
                {
                    addKensaInf.CreateId = userId;
                }
                addKensaInf.CreateMachine = machine;
                addKensaInf.UpdateDate = dt;
                addKensaInf.UpdateId = userId;
                addKensaInf.UpdateMachine = machine;
            }

            dbService.KensaInfRepository.AddRange(addKensaInfs.Select(p => p.KensaInf));
            dbService.SaveChanged();

            // detailにiraicdを反映
            foreach (KensaInfModel addKensaInf in addKensaInfs)
            {
                foreach (KensaInfDetailModel updKensaDtl in KensaInfDetailModels.FindAll(p => p.KeyNo == addKensaInf.KeyNo))
                {
                    updKensaDtl.IraiCd = addKensaInf.IraiCd;
                }
            }
        }

        if (KensaInfDetailModels.Any(p => p.IsAddNew))
        {
            List<KensaInfDetailModel> addKensaDtls = KensaInfDetailModels.FindAll(p => p.IsAddNew);
            foreach (KensaInfDetailModel addKensaDtl in addKensaDtls)
            {
                addKensaDtl.CreateDate = dt;
                addKensaDtl.CreateId = userId;
                addKensaDtl.CreateMachine = machine;
                addKensaDtl.UpdateDate = dt;
                addKensaDtl.UpdateId = userId;
                addKensaDtl.UpdateMachine = machine;
            }

            dbService.KensaInfDetailRepository.AddRange(addKensaDtls.Select(p => p.KensaInfDetail));
        }

        if (KensaInfModels.Any(p => p.IsUpdate))
        {
            List<KensaInfModel> updKensaInfs = KensaInfModels.FindAll(p => p.IsUpdate);
            foreach (KensaInfModel updKensaInf in updKensaInfs)
            {
                updKensaInf.UpdateDate = dt;
                updKensaInf.UpdateId = userId;
                updKensaInf.UpdateMachine = machine;
            }
        }
        dbService.SaveChanged();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }


    #region private function
        private KensaInfDetail ConvertToKensaInfDetail(int hpId, int userId,KensaInfDetailModel model)
    {
        return new KensaInfDetail
    }


    #endregion
}
