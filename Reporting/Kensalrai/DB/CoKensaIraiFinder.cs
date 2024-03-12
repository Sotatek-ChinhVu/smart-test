using Domain.Constant;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Kensalrai.Model;
using KensaInfDetailModel = Reporting.Kensalrai.Model.KensaInfDetailModel;

namespace Reporting.Kensalrai.DB
{
    public class CoKensaIraiFinder : RepositoryBase, ICoKensaIraiFinder
    {
        public CoKensaIraiFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<CoRaiinInfModel> GetRaiinInf(int hpId, List<long> raiinNos)
        {
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
            p.HpId == hpId &&
            raiinNos.Contains(p.RaiinNo) &&
                p.IsDeleted == DeleteStatus.None
            );

            var kaMsts = NoTrackingDataContext.KaMsts.Where(p =>
            p.HpId == hpId &&
                p.IsDeleted == DeleteStatus.None
            );

            var userMsts = NoTrackingDataContext.UserMsts.Where(p =>
                p.HpId == hpId &&
                p.IsDeleted == DeleteStatus.None
            );

            var join = (
                from raiinInf in raiinInfs
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into joinKaMsts
                from joinKaMst in joinKaMsts.DefaultIfEmpty()
                join userMst in userMsts on
                    new { raiinInf.HpId, raiinInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into joinUserMsts
                from joinUserMst in joinUserMsts.DefaultIfEmpty()
                select new
                {
                    raiinInf,
                    joinKaMst,
                    joinUserMst
                }
                ).ToList();
            List<CoRaiinInfModel> results = new List<CoRaiinInfModel>();

            join?.ForEach(entity =>
            {
                results.Add(new CoRaiinInfModel(entity.raiinInf, entity.joinKaMst, entity.joinUserMst));
            }
                );
            return results;

        }
        /// <summary>
        /// 容器名を取得する
        /// </summary>
        /// <param name="containerCd">容器コード</param>
        /// <returns>容器名</returns>
        public string GetContainerName(int hpId, long containerCd)
        {
            var containerMsts = NoTrackingDataContext.ContainerMsts.Where(p =>
                p.HpId == hpId &&
                p.ContainerCd == containerCd
            );

            string result = "";

            if (containerMsts != null && containerMsts.Any())
            {
                result = containerMsts.FirstOrDefault()?.ContainerName ?? string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 身長体重を求める
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>身長, 体重</returns>
        public (double height, double weight) GetHeightWeight(int hpId, long ptId, int sinDate)
        {
            const string heightItemCd = "V0001";
            const string weightItemCd = "V0002";

            double height = 0;
            double weight = 0;

            var kensaInfDtls = NoTrackingDataContext.KensaInfDetails.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.IraiDate <= sinDate &&
                (p.KensaItemCd == heightItemCd || p.KensaItemCd == weightItemCd) &&
                p.IsDeleted == DeleteStatus.None
            ).ToList();

            if (kensaInfDtls != null && kensaInfDtls.Any())
            {
                height = _getResult(heightItemCd);
                weight = _getResult(weightItemCd);
            }

            return (height, weight);

            #region local method
            double _getResult(string itemCd)
            {
                double ret = 0;

                if (kensaInfDtls.Any(p => p.KensaItemCd == itemCd))
                {
                    ret =
                        CIUtil.StrToDoubleDef(
                            kensaInfDtls.Where(p => p.KensaItemCd == itemCd)
                                .OrderByDescending(p => p.IraiCd)
                                .FirstOrDefault()?.ResultVal ?? string.Empty, 0);
                }
                return ret;
            }
            #endregion
        }
        /// <summary>
        /// 検査センター名を取得する
        /// </summary>
        /// <param name="centerCd">センターコード</param>
        /// <returns>検査センター名</returns>
        public string GetCenterName(int hpId, string centerCd)
        {
            var kensaCenterMsts = NoTrackingDataContext.KensaCenterMsts.Where(p =>
                p.HpId == hpId &&
                p.CenterCd == centerCd
            );

            string result = "";

            if (kensaCenterMsts != null && kensaCenterMsts.Any())
            {
                result = kensaCenterMsts?.FirstOrDefault()?.CenterName ?? string.Empty;
            }
            return result;
        }

        public KensaMst GetKensaMst(int hpId, string itemCd)
        {
            var kensaMst = NoTrackingDataContext.KensaMsts.Where(p =>
                p.HpId == hpId &&
                p.KensaItemCd == itemCd
            );
            return kensaMst.FirstOrDefault() ?? new();
        }

        public RaiinInf GetRaiinInf(int hpId, long raiinno)
        {
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
                p.HpId == hpId &&
                p.RaiinNo == raiinno
            );
            return raiinInfs.FirstOrDefault() ?? new();
        }

        public PtInf GetPtInf(int hpId, long ptid)
        {
            var ptinfs = NoTrackingDataContext.PtInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptid
            );

            return ptinfs.FirstOrDefault() ?? new();
        }

        public List<KensaInfModel> GetKensaInfModelsPrint(int hpId, int startDate, int endDate, string centerCd)
        {
            var result = new List<KensaInfModel>();

            var kensaInfs = NoTrackingDataContext.KensaInfs.Where(
                                x => x.HpId == hpId &&
                                     x.IraiDate >= startDate && x.IraiDate <= endDate);
            var kensaInfDetails = NoTrackingDataContext.KensaInfDetails.Where(
                                x => x.HpId == hpId &&
                                     x.IraiDate >= startDate && x.IraiDate <= endDate);
            var ptInfs = NoTrackingDataContext.PtInfs.Where(
            x => x.HpId == hpId &&
                                     x.IsDelete == 0);
            var kensaCenterMsts = NoTrackingDataContext.KensaCenterMsts.Where(
                                x => x.HpId == hpId);
            var center = kensaCenterMsts.FirstOrDefault(item => item.CenterCd == centerCd);
            if (center == null) return result;
            var kensaMstEntities = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId &&
                                    (x.CenterCd == center.CenterCd || (center.PrimaryKbn == 1 && string.IsNullOrEmpty(x.CenterCd))));
            if (!string.IsNullOrEmpty(centerCd))
            {
                kensaInfs = kensaInfs.Where(x => x.CenterCd == centerCd);
                kensaCenterMsts = kensaCenterMsts.Where(x => x.CenterCd == centerCd);
            }

            var query = from kensaInf in kensaInfs
                        join ptInf in ptInfs on
                        new { kensaInf.HpId, kensaInf.PtId } equals
                        new { ptInf.HpId, ptInf.PtId }
                        join kensaCenterMst in kensaCenterMsts on
                        new { kensaInf.HpId, kensaInf.CenterCd } equals
                        new { kensaCenterMst.HpId, kensaCenterMst.CenterCd }
                        select new
                        {
                            kensaInf,
                            Name = ptInf.Name,
                            PtNum = ptInf.PtNum,
                            CenterName = kensaCenterMst.CenterName,
                            PrimaryKbn = kensaCenterMst.PrimaryKbn,
                            KensaInfDetails = (from kensaInfDetail in kensaInfDetails.AsEnumerable()
                                               join kensaMst in kensaMstEntities on kensaInfDetail.KensaItemCd equals kensaMst.KensaItemCd
                                               where kensaInf.HpId == kensaInfDetail.HpId &&
                                                     kensaInf.IraiDate == kensaInfDetail.IraiDate &&
                                                     kensaInf.IraiCd == kensaInfDetail.IraiCd &&
                                                     kensaInf.PtId == kensaInfDetail.PtId &&
                                                     kensaInf.RaiinNo == kensaInfDetail.RaiinNo &&
                                                     kensaInfDetail.IsDeleted == 0
                                               select kensaInfDetail)
                        };
            result = query.AsEnumerable()
                          .Select(x => new KensaInfModel(
                                        x.kensaInf,
                                        x.PtNum,
                                        x.Name,
                                        x.PrimaryKbn,
                                        x.CenterName,
                                        x.KensaInfDetails.Select(detail => new KensaInfDetailModel(detail)).ToList()
                                  ))
                          .OrderByDescending(x => x.IraiDate)
                          .ThenByDescending(x => x.UpdateDate)
                          .ToList();

            return result;
        }

        public List<KensaIraiModel> GetKensaIraiModelsForPrint(int hpId, List<KensaInfModel> kensaInfModels)
        {
            var result = new List<KensaIraiModel>();
            var ptInfEntities = NoTrackingDataContext.PtInfs.Where(x => x.HpId == hpId && x.IsDelete == 0);
            foreach (var kensaInf in kensaInfModels)
            {
                var odrInfEntities = NoTrackingDataContext.OdrInfs.Where(x => x.HpId == hpId &&
                                       x.IsDeleted == 0 &&
                                       x.PtId == kensaInf.PtId &&
                                       x.OdrKouiKbn >= 60 && x.OdrKouiKbn <= 69 &&
                                       x.InoutKbn == 1 &&
                                       x.RaiinNo == kensaInf.RaiinNo);
                var odrInfDetailEntities = NoTrackingDataContext.OdrInfDetails.Where(x => x.HpId == hpId &&
                                           !string.IsNullOrEmpty(x.ItemCd));
                var raiinInfEntities = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None);

                var tenMstEntities = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId && x.MasterSbt != "C" && x.SinKouiKbn >= 60 && x.SinKouiKbn <= 69 && x.IsDeleted == DeleteTypes.None);
                var kensaMstEntities = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId &&
                                        (x.CenterCd == kensaInf.CenterCd || (kensaInf.PrimaryKbn == 1 && string.IsNullOrEmpty(x.CenterCd))));
                var kensaCenterMstEntities = NoTrackingDataContext.KensaCenterMsts.Where(x => x.HpId == hpId);

                var todayOdrInfs = from odrInfEntity in odrInfEntities
                                   join ptInfEntity in ptInfEntities on
                                   new { odrInfEntity.HpId, odrInfEntity.PtId } equals
                                   new { ptInfEntity.HpId, ptInfEntity.PtId }
                                   join raiinInfEntity in raiinInfEntities on
                                   new { odrInfEntity.HpId, odrInfEntity.PtId, odrInfEntity.RaiinNo } equals
                                   new { raiinInfEntity.HpId, raiinInfEntity.PtId, raiinInfEntity.RaiinNo }
                                   select new
                                   {
                                       SikyuKbn = odrInfEntity.SikyuKbn,
                                       TosekiKbn = odrInfEntity.TosekiKbn,
                                       SortNo = odrInfEntity.SortNo,
                                       PtInf = ptInfEntity,
                                       RaiinInf = raiinInfEntity,
                                       OdrInfDetails = from odrInfDetailEntity in odrInfDetailEntities.AsEnumerable()
                                                       where odrInfDetailEntity.HpId == odrInfEntity.HpId &&
                                                             odrInfDetailEntity.PtId == odrInfEntity.PtId &&
                                                             odrInfDetailEntity.RaiinNo == odrInfEntity.RaiinNo &&
                                                             odrInfDetailEntity.RpNo == odrInfEntity.RpNo &&
                                                             odrInfDetailEntity.RpEdaNo == odrInfEntity.RpEdaNo
                                                       join tenMstEntity in tenMstEntities on
                                                       new { odrInfDetailEntity.HpId, odrInfDetailEntity.ItemCd } equals
                                                       new { tenMstEntity.HpId, tenMstEntity.ItemCd }
                                                       join kensaMstEntity in kensaMstEntities on
                                                       new { tenMstEntity.HpId, tenMstEntity.KensaItemCd, tenMstEntity.KensaItemSeqNo } equals
                                                       new { kensaMstEntity.HpId, kensaMstEntity.KensaItemCd, kensaMstEntity.KensaItemSeqNo }
                                                       where tenMstEntity.StartDate <= odrInfEntity.SinDate && tenMstEntity.EndDate >= odrInfEntity.SinDate
                                                       orderby new { odrInfDetailEntity.RpNo, odrInfDetailEntity.RpEdaNo, odrInfDetailEntity.RowNo }
                                                       select new
                                                       {
                                                           odrInfDetailEntity,
                                                           tenMstEntity,
                                                           KensaMst = kensaMstEntity,
                                                       }
                                   };
                var groupTodayOdrInfs = todayOdrInfs
                                       .AsEnumerable()
                                       .GroupBy(x => new
                                       {
                                           x.RaiinInf.RaiinNo,
                                           x.SikyuKbn,
                                           x.TosekiKbn
                                       });
                foreach (var groupTodayOdrInf in groupTodayOdrInfs)
                {
                    List<KensaIraiDetailModel> odrInfDetails = new List<KensaIraiDetailModel>();
                    var groupTodayOdrInfList = groupTodayOdrInf.ToList();
                    var firstTodayOdr = groupTodayOdrInfList.FirstOrDefault();
                    foreach (var todayOdr in groupTodayOdrInfList)
                    {
                        odrInfDetails.AddRange(todayOdr
                                                .OdrInfDetails
                                                .AsEnumerable()
                                                .Select((x) => new KensaIraiDetailModel(
                                                     true,
                                                     x.odrInfDetailEntity.RpNo,
                                                     x.odrInfDetailEntity.RpEdaNo,
                                                     x.odrInfDetailEntity.RowNo,
                                                     0,
                                                     x.tenMstEntity,
                                                     x.KensaMst))
                                                );
                    }
                    result.Add(new KensaIraiModel(
                                    kensaInf.IraiCd,
                                    firstTodayOdr.TosekiKbn,
                                    firstTodayOdr.SikyuKbn,
                                    firstTodayOdr.PtInf,
                                    firstTodayOdr.RaiinInf,
                                    odrInfDetails
                        ));
                }
            }
            // Filter irai done item
            result = result.Where(item => item.Details.Where(d => !string.IsNullOrEmpty(d.KensaItemCd)).Count() > 0).ToList();
            return result;
        }
    }
}
