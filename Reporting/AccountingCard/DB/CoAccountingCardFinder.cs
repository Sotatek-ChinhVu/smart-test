using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.AccountingCard.Model;

namespace Reporting.AccountingCard.DB
{
    public class CoAccountingCardFinder : RepositoryBase, ICoAccountingCardFinder
    {

        public CoAccountingCardFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        /// <summary>
        /// 患者情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate)
        {
            PtInf ptInf = NoTrackingDataContext.PtInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.IsDelete == DeleteStatus.None
            ).FirstOrDefault();

            if (ptInf == null)
            {
                return null;
            }
            return new CoPtInfModel(ptInf, sinDate);
        }
        /// <summary>
        /// 会計情報を取得する
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">検索開始日</param>
        /// <param name="endDate">検索終了日</param>
        /// <param name="raiinNos"></param>
        /// <returns></returns>
        ///         
        public List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int sinYm, int hokenId)
        {
            List<CoKaikeiInfModel> ret = new List<CoKaikeiInfModel>();

            var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
                p.HpId == hpId &&
                p.SinDate >= sinYm * 100 + 1 &&
            p.SinDate <= sinYm * 100 + 31 &&
            p.HokenId == hokenId &&
                p.PtId == ptId
            );

            var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
                p.HpId == hpId &&
            p.PtId == ptId &&
                p.IsDelete == DeleteStatus.None
            );

            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId //&&
                               //p.StartDate <= sinYm * 100 + 31 &&
                               //p.EndDate >= sinYm * 100 + 1 &&
                               //p.IsDeleted == DeleteStatus.None
            );

            var hokenMsts = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId);
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
                h => h.StartDate <= sinYm * 100 + 31 && h.PrefNo == 0 && new int[] { 0, 1, 3, 4, 8, 9 }.Contains(h.HokenSbtKbn)
            ).GroupBy(
                x => new { x.HpId, x.PrefNo, x.HokenNo, x.HokenEdaNo }
            ).Select(
                x => new
                {
                    x.Key.HpId,
                    x.Key.PrefNo,
                    x.Key.HokenNo,
                    x.Key.HokenEdaNo,
                    StartDate = x.Max(d => d.StartDate)
                }
            );

            //保険番号マスタの取得
            var houbetuMsts = (
                from hokenMst in hokenMsts
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                select new
                {
                    hokenMst
                }
            );


            var join = (
                from kaikeiInf in kaikeiInfs
                join ptInf in ptInfs on
                    new { kaikeiInf.HpId, kaikeiInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId } into ptInfJoins
                from ptInfJoin in ptInfJoins.DefaultIfEmpty()
                join ptHokenInf in ptHokenInfs on
                    new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.HokenId } equals
                    new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfJoins
                from ptHokenInfJoin in ptHokenInfJoins.DefaultIfEmpty()
                    //join hokenMst in hokenMsts on
                    //    new { ptHokenInfJoin.HpId, ptHokenInfJoin.HokenNo, ptHokenInfJoin.HokenEdaNo } equals
                    //    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo } into hokenMstJoins
                join hokenMst in houbetuMsts on
                    new { ptHokenInfJoin.HpId, ptHokenInfJoin.HokenNo, ptHokenInfJoin.HokenEdaNo } equals
                    new { hokenMst.hokenMst.HpId, hokenMst.hokenMst.HokenNo, hokenMst.hokenMst.HokenEdaNo } into hokenMstJoins
                from hokenMstJoin in hokenMstJoins.DefaultIfEmpty()
                select new
                {
                    kaikeiInf,
                    ptInf = ptInfJoin,
                    ptHokenInf = ptHokenInfJoin,
                    hokenMst = hokenMstJoin.hokenMst
                }
                ).ToList();

            var entities = join.AsEnumerable().Select(
                data =>
                    new CoKaikeiInfModel(
                        data.kaikeiInf,
                        FindKaikeiDetail(hpId, ptId, data.kaikeiInf.SinDate, data.kaikeiInf.RaiinNo),
                        data.ptInf,
                        data.ptHokenInf,
                        data.hokenMst,
                        FindPtKohi(
                            hpId, ptId, data.kaikeiInf.SinDate,
                            new HashSet<int> { data.kaikeiInf.Kohi1Id, data.kaikeiInf.Kohi2Id, data.kaikeiInf.Kohi3Id, data.kaikeiInf.Kohi4Id })
                    )
                )
                .ToList();
            List<CoKaikeiInfModel> results = new List<CoKaikeiInfModel>();

            entities?.ForEach(entity =>
            {

                results.Add(
                    new CoKaikeiInfModel(
                        entity.KaikeiInf,
                        entity.KaikeiDtls,
                        entity.PtInf,
                        entity.PtHokenInf,
                        entity.HokenMst,
                        entity.PtKohis
                    ));

            }
            );

            return results;
        }

        public List<KaikeiDetail> FindKaikeiDetail(int hpId, long ptId, int sinDate, long raiinNo)
        {
            return NoTrackingDataContext.KaikeiDetails.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo == raiinNo
            ).ToList();
        }

        public List<CoPtKohiModel> FindPtKohi(int hpId, long ptId, int sinDate, HashSet<int> kohiIds)
        {
            var hokenMsts = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId);
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
                h => h.StartDate <= sinDate
            ).GroupBy(
                x => new { x.HpId, x.PrefNo, x.HokenNo, x.HokenEdaNo }
            ).Select(
                x => new
                {
                    x.Key.HpId,
                    x.Key.PrefNo,
                    x.Key.HokenNo,
                    x.Key.HokenEdaNo,
                    StartDate = x.Max(d => d.StartDate)
                }
            );

            var kohiPriorities = NoTrackingDataContext.KohiPriorities.Where(k => k.HpId == hpId).AsQueryable();
            var ptKohis = NoTrackingDataContext.PtKohis.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                kohiIds.Contains(p.HokenId)
            );
            //保険番号マスタの取得
            var houbetuMsts = (
                from hokenMst in hokenMsts
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                select new
                {
                    hokenMst
                }
            );

            //公費の優先順位を取得
            var ptKohiQuery = (
                from ptKohi in ptKohis
                join houbetuMst in houbetuMsts on
                    new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                    new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo, houbetuMst.hokenMst.PrefNo }
                join kPriority in kohiPriorities on
                    new { houbetuMst.hokenMst.PrefNo, houbetuMst.hokenMst.Houbetu } equals
                    new { kPriority.PrefNo, kPriority.Houbetu } into kohiPriorityJoin
                from kohiPriority in kohiPriorityJoin.DefaultIfEmpty()
                where
                    ptKohi.HpId == hpId &&
                    ptKohi.PtId == ptId &&
                    ptKohi.IsDeleted == DeleteStatus.None
                select new
                {
                    ptKohi,
                    hokenMst = houbetuMst.hokenMst,
                    kohiPriority
                }
            ).ToList();

            var entities = ptKohiQuery.AsEnumerable().Select(
                data =>
                    new CoPtKohiModel(
                        data.ptKohi,
                        data.hokenMst,
                        data.kohiPriority
                    )
                )
                .ToList();
            List<CoPtKohiModel> results = new List<CoPtKohiModel>();

            entities?.ForEach(entity =>
            {

                results.Add(
                    new CoPtKohiModel(
                        entity.PtKohi,
                        entity.HokenMst,
                        entity.KohiPriority
                    ));

            }
            );

            return results;
        }
        /// <summary>
        /// 患者病名情報を取得する
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CoPtByomeiModel> FindPtByomei(int hpId, long ptId, int startDate, int endDate, int hokenId)
        {
            var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p =>
                p.HpId == hpId &&
                (ptId > 0 ? p.PtId == ptId : true) &&
                p.StartDate <= endDate &&
                ((p.TenkiKbn == TenkiKbnConst.Continued && p.TenkiDate == 0) || p.TenkiDate >= startDate) &&
                p.IsDeleted == DeleteStatus.None &&
                (p.HokenPid == hokenId || p.HokenPid == 0)
                )
                .OrderBy(p => p.StartDate)
                .ThenBy(p => p.SortNo)
                .ToList();

            List<CoPtByomeiModel> results = new List<CoPtByomeiModel>();

            ptByomeis?.ForEach(entity =>
            {
                results.Add(
                    new CoPtByomeiModel(
                        entity
                        ));
            }
            );
            return results;
        }
    }
}
