using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using EmrCalculateApi.Interface;
using Domain.Constant;
using Infrastructure.Interfaces;

namespace EmrCalculateApi.Ika.DB.Finder
{
    public class RaiinInfFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public RaiinInfFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        //来院情報取得
        /// <summary>
        /// 来院情報取得に診療科マスタを結合したデータを取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>
        /// 指定の患者の指定の診療日の来院情報
        /// SIN_START_TIME順にソート
        /// </returns>
        public List<RaiinInfModel> FindRaiinInfData(int hpId, long ptId, int sinDate)
        {
            var kaMsts = _tenantDataContext.KaMsts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                //p.Status >= 5 &&
                p.IsDeleted == DeleteTypes.None);
            var ptInfs = _tenantDataContext.PtInfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.IsDelete == DeleteTypes.None);

            var joinQuery = (
                from raiinInf in raiinInfs
                join ptInf in ptInfs on
                    new { raiinInf.HpId, raiinInf.PtId} equals
                    new { ptInf.HpId, ptInf.PtId}
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into kaJoin
                from ka in kaJoin.DefaultIfEmpty()
                where
                    raiinInf.HpId == hpId &&
                    raiinInf.PtId == ptId &&
                    raiinInf.SinDate == sinDate &&
                    raiinInf.IsDeleted == DeleteTypes.None
                //orderby
                //    raiinInf.HpId, raiinInf.PtId, 
                //    //raiinInf.SinDate, ("000000" + raiinInf.SinStartTime ?? "").Substring((raiinInf.SinStartTime ?? "").Length, 6),
                //    raiinInf.OyaRaiinNo, raiinInf.RaiinNo
                select new
                {
                    SinStartTimeOrder = raiinInf.SinStartTime == null ? "000000": raiinInf.SinStartTime.Substring(2, 6),
                    raiinInf,
                    kaMst = ka
                }
            );

            var finalJoinQuery = joinQuery.OrderBy(j => j.SinStartTimeOrder).Select(j => new Tuple<RaiinInf, KaMst>(j.raiinInf, j.kaMst));

            var entities = finalJoinQuery.AsEnumerable().Select(
                data =>
                    new RaiinInfModel(data.Item1, data.Item2)
                )
                .ToList();

            List<RaiinInfModel> results = new List<RaiinInfModel>();

            entities?.ForEach(entity => {
                results.Add(new RaiinInfModel(entity.RaiinInf, entity.KaMst));
            });

            return results;
        }

        //来院情報取得
        /// <summary>
        /// 診療月に属する当該患者の全来院日を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>
        /// 指定の患者の指定の診療日に属する月の来院日情報
        /// </returns>
        public List<RaiinDaysModel> FindRaiinInfDays(int hpId, long ptId, int sinDate)
        {
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate >= sinDate / 100 * 100 + 1 &&
                p.SinDate <= sinDate / 100 * 100 + 31 &&
                p.Status >= RaiinState.Calculate &&
                p.IsDeleted == DeleteTypes.None).AsEnumerable();

            var joinQuery = (
                from raiinInf in raiinInfs
                where
                    raiinInf.HpId == hpId &&
                    raiinInf.PtId == ptId &&
                    raiinInf.SinDate >= sinDate / 100 * 100 + 1 &&
                    raiinInf.SinDate <= sinDate / 100 * 100 + 31 &&
                    raiinInf.IsDeleted == DeleteTypes.None
                group raiinInf by
                    new { HpId = raiinInf.HpId, PtId = raiinInf.PtId, SinDate = raiinInf.SinDate } into A
                orderby
                    A.Key.HpId, A.Key.PtId, A.Key.SinDate
                select new
                {
                    A
                }
            );

            //var entities =
            return
                joinQuery.Select(
                    data =>
                        new RaiinDaysModel(data.A.Key.HpId, data.A.Key.PtId, data.A.Key.SinDate)
                    )
                    .ToList();

            //List<RaiinDaysModel> results = new List<RaiinDaysModel>();

            //entities?.ForEach(entity => {
            //    results.Add(new RaiinDaysModel(entity.HpId, entity.PtId, entity.SinDate));
            //});

            //return results;
        }

        //来院情報取得
        /// <summary>
        /// 請求年月に属する当該患者の全来院日を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="seikyuYm">請求年月</param>
        /// <param name="ptIds">患者ID</param>
        /// <returns>
        /// 指定の請求年月に属する月の来院日情報
        /// </returns>
        public List<RaiinDaysModel> FindRaiinInfDaysInMonth(int hpId, int seikyuYm, List<long> ptIds)
        {
            int fromSinDate = seikyuYm * 100 + 1;
            int toSinDate = seikyuYm * 100 + 99;

            var receSeikyus = _tenantDataContext.ReceSeikyus.FindListQueryableNoTrack(r => r.IsDeleted == DeleteStatus.None);

            var maxReceSeikyus = _tenantDataContext.ReceSeikyus.FindListQueryableNoTrack(
                r => r.IsDeleted == DeleteStatus.None
            ).GroupBy(
                r => new { r.HpId, r.SinYm, r.PtId, r.HokenId }
            ).Select(
                r => new
                {
                    r.Key.HpId,
                    r.Key.SinYm,
                    r.Key.PtId,
                    r.Key.HokenId,
                    SeikyuYm = r.Max(x => x.SeikyuYm)
                }
            );

            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack();
            if (ptIds?.Count >= 1)
            {
                raiinInfs = raiinInfs.Where(r => ptIds.Contains(r.PtId));
            }

            var joinQuery = (
                from raiinInf in raiinInfs.AsEnumerable()
                join rs in receSeikyus on
                    new { raiinInf.HpId, raiinInf.PtId, SinYm = (int)Math.Floor((double)raiinInf.SinDate / 100) } equals
                    new { rs.HpId, rs.PtId, rs.SinYm } into rsJoin
                from receSeikyu in rsJoin.DefaultIfEmpty()
                where
                            raiinInf.HpId == hpId &&
                            raiinInf.Status >= RaiinState.Calculate &&
                            raiinInf.IsDeleted == DeleteTypes.None &&
                            (
                                //当月分
                                (raiinInf.SinDate >= fromSinDate && raiinInf.SinDate <= toSinDate) ||
                                //月遅れ・返戻分
                                (
                                    (
                                        from rs1 in maxReceSeikyus
                                        where
                                            rs1.HpId == hpId &&
                                            rs1.SeikyuYm == seikyuYm
                                        select rs1
                                    ).Any(
                                        r =>
                                            r.HpId == raiinInf.HpId &&
                                            r.PtId == raiinInf.PtId &&
                                            r.SinYm == raiinInf.SinDate / 100
                                    )
                                )
                            )
                            //&&
                            //(
                            //    //当月の月遅れ・返戻分を除く
                            //    !(
                            //        from rs2 in receSeikyus
                            //        where
                            //            rs2.HpId == hpId &&
                            //            rs2.SeikyuYm != seikyuYm
                            //        select rs2
                            //    ).Any(
                            //        r =>
                            //            r.HpId == raiinInf.HpId &&
                            //            r.PtId == raiinInf.PtId &&
                            //            r.SinYm == raiinInf.SinDate / 100
                            //    )
                            //)

                group raiinInf by
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate } into A
                orderby
                    A.Key.HpId, A.Key.PtId, A.Key.SinDate
                select new
                {
                    A
                }

            );

            return
                joinQuery.AsEnumerable().Select(
                    data =>
                        new RaiinDaysModel(data.A.Key.HpId, data.A.Key.PtId, data.A.Key.SinDate)
                ).ToList();
        }
    }
}
