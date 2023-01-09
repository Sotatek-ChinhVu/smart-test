using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Domain.Constant;

namespace EmrCalculateApi.Futan.DB.Finder
{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable IDE0075 // Simplify conditional expression
#pragma warning disable S1125
    public class FutancalcFinder
    {
        private List<KogakuLimitModel> _kogakuLimitModels = new List<KogakuLimitModel>();
        private readonly TenantDataContext _tenantDataContext;
        public FutancalcFinder(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        /// <summary>
        /// 患者情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public PtInfModel FindPtInf(int hpId, long ptId, int sinDate)
        {
            PtInf ptInf = _tenantDataContext.PtInfs.FindListNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.IsDelete == DeleteStatus.None
            ).FirstOrDefault();

            if (ptInf == null)
            {
                return null;
            }
            return new PtInfModel(ptInf, sinDate);
        }

        /// <summary>
        /// 保険パターン取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="hokenPid">保険パターンID</param>
        /// <returns></returns>
        public PtHokenPatternModel FindHokenPattern(int hpId, long ptId, int hokenPid)
        {
            PtHokenPattern ptHokenPattern = _tenantDataContext.PtHokenPatterns.FindListNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.HokenPid == hokenPid
            ).FirstOrDefault();

            if (ptHokenPattern == null)
            {
                return null;
            }
            return new PtHokenPatternModel(ptHokenPattern);
        }

        /// <summary>
        /// 主保険情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="hokenId">保険ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public PtHokenInfModel FindHokenInf(int hpId, long ptId, int hokenId, int sinDate)
        {
            if (hokenId == 0)
            {
                return null;
            }

            var ptHokenInfs = _tenantDataContext.PtHokenInfs.FindListQueryableNoTrack();

            for (int iCnt = 1; iCnt <= 2; iCnt++)
            {
                var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(h =>
                    h.PrefNo == 0 &&
                    (iCnt == 1 ? h.StartDate <= sinDate : true)
                );

                var joinQuery = (
                    from ptHokenInf in ptHokenInfs
                    join hokenMst in hokenMsts on
                        new { ptHokenInf.HpId, ptHokenInf.HokenNo, ptHokenInf.HokenEdaNo } equals
                        new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo }// into ptHokenMst
                    where
                            ptHokenInf.HpId == hpId &&
                            ptHokenInf.PtId == ptId &&
                            ptHokenInf.HokenId == hokenId
                    //ptHokenInf.IsDeleted == DeleteStatus.None
                    orderby
                        (iCnt == 1 ? hokenMst.StartDate : 0) descending, hokenMst.StartDate
                    select new
                    {
                        ptHokenInf,
                        hokenMst  //hokenMst = ptHokenMst.OrderByDescending(hm => hm.StartDate).FirstOrDefault()
                    }
                );

                var result = joinQuery.AsEnumerable().Select(
                    data =>
                        new PtHokenInfModel(
                            data.ptHokenInf,
                            data.hokenMst //data.hokenMst
                        )
                    )
                    .FirstOrDefault();

                if (result != null)
                {
                    return result;
                }
            }

            return null;

            ////PtHokenInf ptHokenInf = _tenantDataContext.PtHokenInfs.FindList(p =>
            ////    p.HpId == hpId &&
            ////    p.PtId == ptId &&
            ////    p.HokenId == hokenId &&
            ////    p.IsDeleted == 0
            ////).FirstOrDefault();

            ////if (ptHokenInf == null)
            ////{
            ////    return null;
            ////}
            ////return new PtHokenInfModel(ptHokenInf);
        }

        /// <summary>
        /// 公費情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="kohiId">公費ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="hokensyaNo">保険者番号</param>
        /// <returns></returns>
        public PtKohiModel FindKohiInf(int hpId, long ptId, int kohiId, int sinDate, string hokensyaNo)
        {
            var ptKohis = _tenantDataContext.PtKohis.FindListQueryableNoTrack();
            var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(h =>
                h.StartDate <= sinDate
            );
            var expHokNos = _tenantDataContext.ExceptHokensyas.FindListQueryableNoTrack(e =>
                e.HokensyaNo == hokensyaNo
            );
            var kohiPriorities = _tenantDataContext.KohiPriorities.FindListQueryableNoTrack();

            var joinQuery = (
                from ptKohi in ptKohis
                join hokenMst in hokenMsts on
                    new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo }
                join expHokNo in expHokNos on
                    new { hokenMst.HpId, hokenMst.PrefNo, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.StartDate } equals
                    new { expHokNo.HpId, expHokNo.PrefNo, expHokNo.HokenNo, expHokNo.HokenEdaNo, expHokNo.StartDate } into eJoin
                from ej in eJoin.DefaultIfEmpty()
                join kohiPriority in kohiPriorities on
                    new { hokenMst.PrefNo, hokenMst.Houbetu } equals
                    new { kohiPriority.PrefNo, kohiPriority.Houbetu } into pJoin
                from pj in pJoin.DefaultIfEmpty()
                where
                    ptKohi.HpId == hpId &&
                    ptKohi.PtId == ptId &&
                    ptKohi.HokenId == kohiId
                //ptKohi.IsDeleted == DeleteStatus.None
                orderby
                    hokenMst.StartDate descending
                select new
                {
                    ptKohi,
                    hokenMst,
                    expHokNo = !String.IsNullOrEmpty(ej.HokensyaNo),
                    priority = pj == null ? "99999" : pj.PriorityNo
                }
            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new PtKohiModel(
                        data.ptKohi,
                        data.hokenMst,  // ?? new HokenMst()
                        data.expHokNo,
                        data.priority
                    )
                )
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 調整額情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<PtSanteiConfModel> FindPtSanteiConf(int hpId, long ptId, int sinDate)
        {
            var ptSanteiConfs = _tenantDataContext.PtSanteiConfs.FindListNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                (p.KbnNo == 1 || p.KbnNo == 2) &&
                p.StartDate <= sinDate &&
                p.EndDate >= sinDate &&
                p.IsDeleted == DeleteStatus.None
            )
            .OrderBy(p => p.KbnNo)
            .ToList();

            return ptSanteiConfs.Select(p => new PtSanteiConfModel(p)).ToList();
        }


        /// <summary>
        /// 会計詳細情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="sortKey">計算順番</param>
        /// <param name="adjustDetail">高額療養費の合算調整額</param>
        /// <returns></returns>
        public List<KaikeiDetailModel> FindKaikeiDetail(
            int hpId, long ptId, int sinDate, long raiinNo, int hokenPid, string sortKey, bool adjustDetail
        )
        {
            int fromSinDate = sinDate / 100 * 100 + 1;

            var kaikeiDetails = _tenantDataContext.KaikeiDetails.FindListNoTrack(k =>
                k.HpId == hpId &&
                k.PtId == ptId &&
                k.SinDate >= fromSinDate && k.SinDate <= sinDate &&

                k.RaiinNo != raiinNo &&
                k.SortKey.CompareTo(sortKey) == -1 &&
                (adjustDetail ? k.AdjustPid > 0 : k.AdjustPid == 0)
            )
            .OrderBy(p => p.SinDate)
            .ThenBy(p => p.RaiinNo)
            .ThenBy(p => p.HokenPid)
            .ToList();

            return kaikeiDetails.Select(k => new KaikeiDetailModel(k)).ToList();
        }

        /// <summary>
        /// 会計詳細情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="sortKey">計算順番</param>
        /// <param name="adjustDetail">高額療養費の合算調整額</param>
        /// <returns></returns>
        public List<KaikeiDetailModel> FindTotalKaikeiDetail(
            int hpId, long ptId, int sinDate
        )
        {
            int fromSinDate = sinDate / 100 * 100 + 1;
            int toSinDate = sinDate / 100 * 100 + 31;

            var kaikeiDetails = _tenantDataContext.KaikeiDetails.FindListNoTrack(k =>
                k.HpId == hpId &&
                k.PtId == ptId &&
                k.SinDate >= fromSinDate && k.SinDate <= toSinDate
            )
            .OrderBy(p => p.SinDate)
            .ThenBy(p => p.RaiinNo)
            .ThenBy(p => p.HokenPid)
            .ToList();

            return kaikeiDetails.Select(k => new KaikeiDetailModel(k)).ToList();
        }

        /// <summary>
        /// 上限管理情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="sortKey">計算順番</param>
        /// <returns></returns>
        public List<LimitListInfModel> FindLimitListInf(
            int hpId, long ptId, int sinDate, long raiinNo, int hokenPid, string sortKey
        )
        {
            int fromSinDate = sinDate / 100 * 100 + 1;

            var limitListInfs = _tenantDataContext.LimitListInfs.FindListNoTrack(x =>
                x.HpId == hpId &&
                x.PtId == ptId &&
                x.SinDate >= fromSinDate && x.SinDate <= sinDate &&

                x.RaiinNo != raiinNo &&
                x.SortKey.CompareTo(sortKey) == -1 &&
                x.IsDeleted == DeleteStatus.None
            )
            .OrderBy(x => x.SortKey)
            .ToList();

            return limitListInfs.Select(x => new LimitListInfModel(x)).ToList();
        }

        /// <summary>
        /// 上限回数管理情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="sortKey">計算順番</param>
        /// <returns></returns>
        public List<LimitCntListInfModel> FindLimitCntListInf(int hpId, long ptId, int sinDate, string sortKey)
        {
            int fromSinDate = sinDate / 100 * 100 + 1;

            var limitCntListInfs = _tenantDataContext.LimitCntListInfs.FindListNoTrack(x =>
                x.HpId == hpId &&
                x.PtId == ptId &&
                x.SinDate >= fromSinDate && x.SinDate <= sinDate &&
                x.SortKey.CompareTo(sortKey) <= 0 &&
                x.IsDeleted == DeleteStatus.None
            )
            .OrderBy(x => x.SortKey)
            .ToList();

            return limitCntListInfs.Select(x => new LimitCntListInfModel(x)).ToList();
        }

        /// <summary>
        /// 高額療養費の限度額取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="AgeKbn">年齢区分</param>
        /// <param name="kogakuKbn">高額療養費区分</param>
        /// <param name="isTasukai">多数回該当</param>
        /// <returns></returns>
        public dynamic FindKogakuLimit(int hpId, int sinDate, int AgeKbn, int kogakuKbn, bool isTasukai)
        {
            if (_kogakuLimitModels == null)
            {
                var kogakuLimits = _tenantDataContext.KogakuLimits.FindListNoTrack(k =>
                    k.HpId == hpId
                ).ToList();

                _kogakuLimitModels = kogakuLimits.Select(k => new KogakuLimitModel(k)).ToList();
            }

            KogakuLimitModel kogakuLimit = _kogakuLimitModels.Where(k =>
                k.AgeKbn == AgeKbn &&
                k.StartDate <= sinDate && sinDate <= k.EndDate &&
                k.KogakuKbn == kogakuKbn
            ).OrderByDescending(k => k.StartDate).FirstOrDefault();

            return new
            {
                Limit = isTasukai && (kogakuLimit?.TasuLimit ?? 0) > 0 ? kogakuLimit?.TasuLimit ?? 0 : kogakuLimit?.BaseLimit ?? 0,
                Adjust = isTasukai ? 0 : kogakuLimit?.AdjustLimit ?? 0
            };
        }

        /// <summary>
        /// 収納請求情報の取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        public SyunoSeikyuModel FindSyunoSeikyu(int hpId, long ptId, int sinDate, long raiinNo)
        {
            SyunoSeikyu syunoSeikyu = _tenantDataContext.SyunoSeikyus.FindList(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo == raiinNo
            ).FirstOrDefault();

            if (syunoSeikyu == null)
            {
                return null;
            }
            return new SyunoSeikyuModel(syunoSeikyu);
        }

        /// <summary>
        /// 収納入金情報の取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        public int FindSyunoNyukin(int hpId, long raiinNo)
        {
            int nyukinGaku = _tenantDataContext.SyunoNyukin.FindList(p =>
                p.HpId == hpId &&
                p.RaiinNo == raiinNo &&
                p.IsDeleted == DeleteStatus.None
            ).Sum(p => p.NyukinGaku + p.AdjustFutan);

            return nyukinGaku;
        }
    }
}
