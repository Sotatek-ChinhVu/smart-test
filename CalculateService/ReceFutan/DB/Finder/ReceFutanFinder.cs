using Domain.Constant;
using CalculateService.Extensions;
using CalculateService.ReceFutan.Models;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;

namespace CalculateService.ReceFutan.DB.Finder
{
    class ReceFutanFinder
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        private readonly TenantDataContext _tenantDataContext;
        public ReceFutanFinder(TenantDataContext tenantDataContext,
            List<TokkiMstModel>? tokkiMstModels = null, List<KogakuLimitModel>? kogakuLimits = null)
        {
            _tenantDataContext = tenantDataContext;
            _tokkiMstModels = tokkiMstModels;
            _kogakuLimitModels = kogakuLimits;
        }

        private List<TokkiMstModel>? _tokkiMstModels;
        private List<KogakuLimitModel>? _kogakuLimitModels;

        /// <summary>
        /// レセプト対象の会計情報の取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptIds">患者ID</param>
        /// <param name="seikyuYm">請求年月</param>
        /// <returns></returns>
        public List<KaikeiDetailModel> FindKaikeiDetail(int hpId, List<long> ptIds, int seikyuYm)
        {
            int fromSinDate = seikyuYm * 100 + 1;
            int toSinDate = seikyuYm * 100 + 99;
            bool havePtIds = ptIds.Any();

            var maxReceSeikyus = _tenantDataContext.ReceSeikyus.Where(item => item.IsDeleted == DeleteStatus.None
                                                                              && item.HpId == hpId
                                                                              && item.SeikyuYm == seikyuYm
            ).GroupBy(
                r => new { r.SinYm, r.PtId, r.HokenId }
            ).Select(
                r => new
                {
                    r.Key.SinYm,
                    r.Key.PtId,
                    r.Key.HokenId,
                    SeikyuYm = r.Max(x => x.SeikyuYm)
                }
            ).ToList();
            var ptIdMaxReceSeikyuList = maxReceSeikyus.Select(item => item.PtId).Distinct().ToList();
            var sinYmMaxReceSeikyuList = maxReceSeikyus.Select(item => item.SinYm).Distinct().ToList();
            var hokenIdMaxReceSeikyuList = maxReceSeikyus.Select(item => item.HokenId).Distinct().ToList();

            var receSeikyus = _tenantDataContext.ReceSeikyus.Where(item => item.HpId == hpId
                                                                           && item.IsDeleted == DeleteStatus.None)
                                                            .ToList();

            var kaikeiDetails = _tenantDataContext.KaikeiDetails.Where(item => item.HpId == hpId
                                                                               && (!havePtIds || ptIds.Contains(item.PtId))
                                                                               && ((item.SinDate >= fromSinDate && item.SinDate <= toSinDate)
                                                                                    || (ptIdMaxReceSeikyuList.Contains(item.PtId)
                                                                                        && sinYmMaxReceSeikyuList.Contains(item.SinDate / 100)
                                                                                        && hokenIdMaxReceSeikyuList.Contains(item.HokenId)))
                                                                 ).ToList();

            var raiinNoList = kaikeiDetails.Select(item => item.RaiinNo).Distinct().ToList();

            var raiinInfs = _tenantDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                       && raiinNoList.Contains(item.RaiinNo))
                                                        .ToList();

            var joinQuery = (
                from kaikeiDetail in kaikeiDetails
                join rs in receSeikyus on
                    new { kaikeiDetail.PtId, SinYm = kaikeiDetail.SinDate / 100, kaikeiDetail.HokenId } equals
                    new { rs.PtId, rs.SinYm, rs.HokenId } into rsJoin
                from receSeikyu in rsJoin.DefaultIfEmpty()
                join raiinInf in raiinInfs on
                    new { kaikeiDetail.RaiinNo } equals
                    new { raiinInf.RaiinNo }
                where
                            //kaikeiDetail.HokenKbn != HokenKbn.Jihi &&  //自費を除く
                            (
                                //当月分
                                (kaikeiDetail.SinDate >= fromSinDate && kaikeiDetail.SinDate <= toSinDate) ||
                                //月遅れ・返戻分
                                (
                                    (
                                        from rs1 in maxReceSeikyus
                                        select rs1
                                    ).Any(
                                        r =>
                                            r.PtId == kaikeiDetail.PtId &&
                                            r.SinYm == kaikeiDetail.SinDate / 100 &&
                                            r.HokenId == kaikeiDetail.HokenId
                                    )
                                )
                            ) &&
                            (
                                //当月の月遅れ・返戻分を除く
                                !(
                                    from rs2 in receSeikyus
                                    where
                                        rs2.SeikyuYm != seikyuYm
                                    select rs2
                                ).Any(
                                    r =>
                                        r.PtId == kaikeiDetail.PtId &&
                                        r.SinYm == kaikeiDetail.SinDate / 100 &&
                                        r.HokenId == kaikeiDetail.HokenId
                                )
                            )

                orderby
                    kaikeiDetail.HpId, kaikeiDetail.PtId, kaikeiDetail.SortKey
                select new
                {
                    kaikeiDetail,
                    SeikyuKbn = receSeikyu == null ? 0 : receSeikyu.SeikyuKbn,
                    raiinInf.KaId,
                    raiinInf.TantoId
                }

            );

            //患者指定
            if (ptIds?.Count >= 1)
            {
                joinQuery = joinQuery.Where(
                    q => ptIds.Contains(q.kaikeiDetail.PtId)
                );
            }

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new KaikeiDetailModel(
                        data.kaikeiDetail,
                        data.SeikyuKbn,
                        data.KaId,
                        data.TantoId
                    )
                )
                .ToList();

            #region '同一月内に社保生保と生保単独が混在して１枚のレセにまとめる場合の、返戻月遅れ分に関する特殊処理'            

            #region '当月の返戻月遅れ分の内、結合されるID分を除外する'
            var excludeQuery = (
                from receSeikyu in receSeikyus
                where
                    receSeikyu.HpId == hpId &&
                    receSeikyu.SeikyuYm != seikyuYm &&
                    receSeikyu.SinYm == seikyuYm
                select new
                {
                    receSeikyu.PtId,
                    receSeikyu.SinYm,
                    receSeikyu.HokenId,
                    receSeikyu.SeikyuKbn
                }
            );

            //患者指定
            if (ptIds?.Count >= 1)
            {
                excludeQuery = excludeQuery.Where(
                    p => ptIds.Contains(p.PtId)
                );
            }
            var excludeReces = excludeQuery.ToList();

            foreach (var excludeRece in excludeReces)
            {
                var wrkKaikeis = (
                    from kaikeiDetail in kaikeiDetails
                    where
                        kaikeiDetail.HpId == hpId &&
                        kaikeiDetail.PtId == excludeRece.PtId &&
                        kaikeiDetail.SinDate >= excludeRece.SinYm * 100 + 1 &&
                        kaikeiDetail.SinDate <= excludeRece.SinYm * 100 + 99 &&
                        kaikeiDetail.HokenKbn == HokenKbn.Syaho
                    select new
                    {
                        kaikeiDetail.HokenId,
                        kaikeiDetail.ReceSbt
                    }
                ).ToList();

                var regRece = wrkKaikeis.Find(k => k.HokenId == excludeRece.HokenId);
                if (regRece != null)
                {
                    bool isNoHoken = regRece.ReceSbt.Substring(1, 1) == "2";

                    var wrkKaikei = isNoHoken ?
                        wrkKaikeis.FindAll(k => k.ReceSbt.Substring(1, 1) != "2") :
                        wrkKaikeis.FindAll(k => k.ReceSbt.Substring(1, 1) == "2");

                    if (wrkKaikei != null && wrkKaikei.Count >= 1)
                    {
                        int removeId = wrkKaikei.Max(k => k.HokenId);
                        result.RemoveAll(r => r.PtId == excludeRece.PtId && r.SinYm == excludeRece.SinYm && r.HokenId == removeId);

                        //_emrLogger.WriteLogMsg(this, nameof(FindKaikeiDetail), $"SeihoSp RemoveKaikeiDetail ptId:{excludeRece.PtId} sinYm:{excludeRece.SinYm} hokenId:{removeId}");
                    }
                }
            }
            #endregion

            #region '返戻月遅れ分の内、結合されるIDの不足分を追加する'
            bool addKaikeiData = false;

            var syahoReces = result.FindAll(r =>
                r.SinYm != seikyuYm &&
                r.HokenKbn == HokenKbn.Syaho
            ).GroupBy(r => new { r.PtId, r.SinYm, r.HokenId, r.IsNoHoken, r.SeikyuKbn });

            foreach (var syahoRece in syahoReces)
            {
                var joinKaikei = (
                    from kaikeiDetail in kaikeiDetails
                    join raiinInf in raiinInfs on
                        new { kaikeiDetail.HpId, kaikeiDetail.PtId, kaikeiDetail.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    where
                        kaikeiDetail.HpId == hpId &&
                        kaikeiDetail.PtId == syahoRece.Key.PtId &&
                        kaikeiDetail.SinDate >= syahoRece.Key.SinYm * 100 + 1 &&
                        kaikeiDetail.SinDate <= syahoRece.Key.SinYm * 100 + 99 &&
                        kaikeiDetail.HokenId != syahoRece.Key.HokenId &&
                        kaikeiDetail.HokenKbn == HokenKbn.Syaho
                    orderby
                        kaikeiDetail.HpId, kaikeiDetail.PtId, kaikeiDetail.SortKey
                    select new
                    {
                        kaikeiDetail,
                        syahoRece.Key.SeikyuKbn,
                        raiinInf.KaId,
                        raiinInf.TantoId
                    }
                );

                var joinKaikeis = joinKaikei.AsEnumerable().Select(
                    data =>
                        new KaikeiDetailModel(
                            data.kaikeiDetail,
                            data.SeikyuKbn,
                            data.KaId,
                            data.TantoId
                        )
                    ).ToList();

                var regReces = joinKaikeis.FindAll(k => k.IsNoHoken == !syahoRece.Key.IsNoHoken);
                if (regReces != null && regReces.Count >= 1)
                {
                    var joinId = regReces.Max(r => r.HokenId);

                    bool exists = result.Any(r =>
                        r.PtId == syahoRece.Key.PtId &&
                        r.SinYm == syahoRece.Key.SinYm &&
                        r.HokenId == joinId
                    );
                    if (!exists)
                    {
                        result.AddRange(joinKaikeis.FindAll(k => k.HokenId == joinId));
                        addKaikeiData = true;

                        //_emrLogger.WriteLogMsg(this, nameof(FindKaikeiDetail), $"SeihoSp JoinKaikeiDetail - ptId:{syahoRece.Key.PtId} sinYm:{syahoRece.Key.SinYm} hokenId:{joinId}");
                    }
                }
            }

            if (addKaikeiData)
            {
                result = result.OrderBy(r => r.PtId).ThenBy(r => r.SortKey).ToList();
            }
            #endregion

            #endregion

            return result;
        }

        /// <summary>
        /// 患者情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptIds">患者ID</param>
        /// <param name="seikyuYm">請求年月</param>
        /// <returns></returns>
        public List<ReceInfEditModel> FindReceInfEdit(int hpId, List<long> ptIds, int seikyuYm)
        {
            var receInfEdit = _tenantDataContext.ReceInfEdits.FindListQueryable(r =>
                r.HpId == hpId &&
                r.SeikyuYm == seikyuYm &&
                r.IsDeleted == DeleteStatus.None
            );

            //患者指定
            if (ptIds?.Count >= 1)
            {
                receInfEdit = receInfEdit.Where(r => ptIds.Contains(r.PtId));
            }

            var result = receInfEdit.AsEnumerable().Select(
                data => new ReceInfEditModel(data)
            ).ToList();

            return result;
        }


        /// <summary>
        /// 患者情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <returns></returns>
        public PtInfModel FindPtInf(int hpId, long ptId)
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
            return new PtInfModel(ptInf);
        }

        /// <summary>
        /// 主保険情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="hokenId">保険ID</param>
        /// <returns></returns>
        public PtHokenInfModel FindHokenInf(int hpId, long ptId, int hokenId, int sinYm)
        {
            int sinDate = sinYm * 100 + 1;

            var ptHokenInfs = _tenantDataContext.PtHokenInfs.FindListQueryableNoTrack();
            var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(h =>
                h.PrefNo == 0 &&
                h.StartDate <= sinDate
            );

            var joinQuery = (
                from ptHokenInf in ptHokenInfs
                join hokenMst in hokenMsts on
                    new { ptHokenInf.HpId, ptHokenInf.HokenNo, ptHokenInf.HokenEdaNo } equals
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo }
                where
                    ptHokenInf.HpId == hpId &&
                    ptHokenInf.PtId == ptId &&
                    ptHokenInf.HokenId == hokenId &&
                    ptHokenInf.IsDeleted == DeleteStatus.None
                orderby
                    hokenMst.StartDate descending
                select new
                {
                    ptHokenInf,
                    hokenMst
                }
            );

            var ptRousaiTenki = _tenantDataContext.PtRousaiTenkis.FindListQueryableNoTrack(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.HokenId == hokenId &&
                r.EndDate >= sinYm &&
                r.IsDeleted == DeleteStatus.None
            ).OrderBy(r => r.EndDate).ToList();

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new PtHokenInfModel(
                        data.ptHokenInf,
                        data.hokenMst,
                        ptRousaiTenki.FirstOrDefault()?.Sinkei ?? 0,
                        ptRousaiTenki.FirstOrDefault()?.Tenki ?? 0
                    )
                )
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 公費情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="kohiId">公費ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <param name="hokensyaNo">保険者番号</param>
        /// <returns></returns>
        public List<PtKohiModel> FindKohiInf(int hpId, long ptId, int kohi0Id, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id,
            int sinYm, int seikyuYm, string hokensyaNo, int hokenKbn)
        {
            int sinDate = sinYm * 100 + 1;

            var ptKohis = _tenantDataContext.PtKohis.FindListQueryableNoTrack();
            var expHokNos = _tenantDataContext.ExceptHokensyas.FindListQueryableNoTrack(e =>
                e.HokensyaNo == hokensyaNo
            );

            var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack();
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(
                h => h.StartDate <= (h.SeikyuYm == 1 ? seikyuYm * 100 + 31 : sinDate)
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
            var actHokenMsts = (
                from hokenMst in hokenMsts
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                select
                    hokenMst
            );

            var joinQuery = (
                from ptKohi in ptKohis
                join hokenMst in actHokenMsts on
                    new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo }
                join expHokNo in expHokNos on
                    new { hokenMst.HpId, hokenMst.PrefNo, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.StartDate } equals
                    new { expHokNo.HpId, expHokNo.PrefNo, expHokNo.HokenNo, expHokNo.HokenEdaNo, expHokNo.StartDate } into eJoin
                from ej in eJoin.DefaultIfEmpty()
                where
                    ptKohi.HpId == hpId &&
                    ptKohi.PtId == ptId &&
                    (ptKohi.HokenId == kohi0Id ||
                     ptKohi.HokenId == kohi1Id || ptKohi.HokenId == kohi2Id ||
                     ptKohi.HokenId == kohi3Id || ptKohi.HokenId == kohi4Id) &&
                    ptKohi.IsDeleted == DeleteStatus.None
                select new
                {
                    ptKohi,
                    hokenMst,
                    expHokNo = !String.IsNullOrEmpty(ej.HokensyaNo)
                }
            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new PtKohiModel(
                        data.ptKohi,
                        data.hokenMst,
                        data.expHokNo,
                        hokenKbn
                    )
                ).ToList();
            return result;
        }


        /// <summary>
        /// 特記事項の名称取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <param name="tokkiCd">特記事項コード</param>
        /// <returns></returns>
        public string FindTokkiName(int hpId, int sinYm, string tokkiCd)
        {
            if (_tokkiMstModels == null)
            {
                var tokkiMsts = _tenantDataContext.TokkiMsts.FindListNoTrack();

                _tokkiMstModels = tokkiMsts.Select(t => new TokkiMstModel(t)).ToList();
            }

            int fromDate = sinYm * 100 + 1;
            int toDate = sinYm * 100 + 31;

            TokkiMstModel tokkiMst = _tokkiMstModels.Where(t =>
                t.StartDate <= fromDate && toDate <= t.EndDate &&
                t.TokkiCd == tokkiCd
            ).OrderByDescending(t => t.StartDate).FirstOrDefault();

            return tokkiMst?.TokkiName;
        }

        /// <summary>
        /// 高額療養費の限度額取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="ageKbn">年齢区分</param>
        /// <param name="kogakuKbn">高額療養費区分</param>
        /// <param name="isTasukai">多数回該当</param>
        /// <returns></returns>
        public dynamic FindKyotoKogakuLimit(int hpId, int sinYm, int ageKbn, int kogakuKbn, int isTasukai)
        {
            if (_kogakuLimitModels == null)
            {
                var kogakuLimits = _tenantDataContext.KogakuLimits.FindListNoTrack();

                _kogakuLimitModels = kogakuLimits.Select(k => new KogakuLimitModel(k)).ToList();
            }

            int sinDate = sinYm * 100 + 1;
            int wrkKbn = 0;
            if (ageKbn == 1)
            {
                //70歳以上
                if (new int[] { 4, 5 }.Contains(kogakuKbn))
                {
                    wrkKbn = kogakuKbn;
                }
            }
            else
            {
                //70歳未満
                wrkKbn = 28;
                if (new int[] { 29, 30 }.Contains(kogakuKbn))
                {
                    wrkKbn = kogakuKbn;
                }
            }

            KogakuLimitModel kogakuLimit = _kogakuLimitModels.Where(k =>
                k.AgeKbn == ageKbn &&
                k.StartDate <= sinDate && sinDate <= k.EndDate &&
                k.KogakuKbn == wrkKbn
            ).OrderByDescending(k => k.StartDate).FirstOrDefault();

            return new
            {
                Limit = isTasukai == 1 && (kogakuLimit?.TasuLimit ?? 0) > 0 ? kogakuLimit?.TasuLimit ?? 0 : kogakuLimit?.BaseLimit ?? 0,
                Adjust = isTasukai == 1 ? 0 : kogakuLimit?.AdjustLimit ?? 0
            };
        }

        /// <summary>
        /// 特記事項の名称取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <returns></returns>
        public int FindHpPrefCd(int hpId, int sinYm)
        {
            var hpInfs = _tenantDataContext.HpInfs.FindListNoTrack(h =>
                h.HpId == hpId &&
                h.StartDate <= sinYm * 100 + 31
            ).OrderByDescending(h => h.StartDate).Select(h => h.PrefNo).ToList();

            if (hpInfs != null)
            {
                return hpInfs.First();
            }

            hpInfs = _tenantDataContext.HpInfs.FindListNoTrack(h =>
                h.HpId == hpId
            ).OrderByDescending(h => h.StartDate).Select(h => h.PrefNo).ToList();

            return hpInfs.First();
        }
    }
}
