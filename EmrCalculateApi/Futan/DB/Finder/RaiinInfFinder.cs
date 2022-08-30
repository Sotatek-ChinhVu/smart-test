using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Interface;
using Helper.Common;
using PostgreDataContext;

namespace EmrCalculateApi.Futan.DB.Finder
{
#pragma warning disable CA1827 // Do not use Count() or LongCount() when Any() can be used
#pragma warning disable S1155 // "Any()" should be used to test for emptiness
#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable CS8625

    class RaiinInfFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        public RaiinInfFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
        }

        /// <summary>
        /// 来院情報の取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<RaiinTensuModel> FindRaiinInf(
            int hpId, long ptId, int sinDate, long raiinNo,
            ref List<SinKouiCountModel> sinKouiCounts,
            ref List<SinKouiModel> sinKouis,
            ref List<SinKouiDetailModel> sinKouiDetails,
            ref List<SinRpInfModel> sinRpInfs,
            ref List<RaiinInfModel> raiinInfs
        )
        {
            var hokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId
                    //p.IsDeleted == DeleteStatus.None
                    );
            var ptKohis = _tenantDataContext.PtKohis.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId
                    //p.IsDeleted == DeleteStatus.None
                    );
            var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId
                    );
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(
                h => h.HpId == hpId && h.StartDate <= sinDate
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
            var kohiPriorities = _tenantDataContext.KohiPriorities.FindListQueryableNoTrack();

            //保険番号マスタの取得
            var houbetuMsts = (
                from hokenMst in hokenMsts
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                select new
                {
                    hokenMst.HpId,
                    hokenMst.PrefNo,
                    hokenMst.HokenNo,
                    hokenMst.HokenEdaNo,
                    hokenMst.Houbetu
                }
            );

            //公費の優先順位を取得
            var ptKohiQuery = (
                from ptKohi in ptKohis
                join houbetuMst in houbetuMsts on
                    new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                    new { houbetuMst.HpId, houbetuMst.HokenNo, houbetuMst.HokenEdaNo, houbetuMst.PrefNo }
                join kPriority in kohiPriorities on
                    new { houbetuMst.PrefNo, houbetuMst.Houbetu } equals
                    new { kPriority.PrefNo, kPriority.Houbetu } into kohiPriorityJoin
                from kohiPriority in kohiPriorityJoin.DefaultIfEmpty()
                where
                    ptKohi.HpId == hpId &&
                    ptKohi.PtId == ptId &&
                    ptKohi.IsDeleted == DeleteStatus.None
                select new
                {
                    ptKohi.HpId,
                    ptKohi.PtId,
                    KohiId = ptKohi.HokenId,
                    ptKohi.PrefNo,
                    houbetuMst.Houbetu,
                    PriorityNo = kohiPriority == null ? "99999" : kohiPriority.PriorityNo
                }
            ).ToList();

            //診療点数を取得
            if (sinKouiCounts == null)
            {
                //var wrkList = _tenantDataContext.SinKouiCounts.FindListNoTrack(x =>
                //    x.HpId == hpId &&
                //    x.PtId == ptId &&
                //    x.SinYm == sinDate / 100
                //).ToList();
                //sinKouiCounts = wrkList.Select(x => new SinKouiCountModel(x)).ToList();
                sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListNoTrack(x =>
                    x.HpId == hpId &&
                    x.PtId == ptId &&
                    x.SinYm == sinDate / 100
                ).Select(x => new SinKouiCountModel(x)).ToList();
            }
            if (sinKouis == null)
            {
                //var wrkList = _tenantDataContext.SinKouis.FindListNoTrack(x =>
                //    x.HpId == hpId &&
                //    x.PtId == ptId &&
                //    x.SinYm == sinDate / 100
                //).ToList();
                //sinKouis = wrkList.Select(x => new SinKouiModel(x)).ToList();
                sinKouis = _tenantDataContext.SinKouis.FindListNoTrack(x =>
                    x.HpId == hpId &&
                    x.PtId == ptId &&
                    x.SinYm == sinDate / 100
                ).Select(x => new SinKouiModel(x)).ToList();

            }
            if (sinKouiDetails == null)
            {
                //var wrkList = _tenantDataContext.SinKouiDetails.FindListNoTrack(x =>
                //    x.HpId == hpId &&
                //    x.PtId == ptId &&
                //    x.SinYm == sinDate / 100
                //).ToList();
                //sinKouiDetails = wrkList.Select(x => new SinKouiDetailModel(x, null)).ToList();
                sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListNoTrack(x =>
                    x.HpId == hpId &&
                    x.PtId == ptId &&
                    x.SinYm == sinDate / 100
                ).Select(x => new SinKouiDetailModel(x, null)).ToList();
            }
            if (sinRpInfs == null)
            {
                //var wrkList = _tenantDataContext.SinRpInfs.FindListNoTrack(x =>
                //    x.HpId == hpId &&
                //    x.PtId == ptId &&
                //    x.SinYm == sinDate / 100
                //).ToList();
                //sinRpInfs = wrkList.Select(x => new SinRpInfModel(x)).ToList();
                sinRpInfs = _tenantDataContext.SinRpInfs.FindListNoTrack(x =>
                    x.HpId == hpId &&
                    x.PtId == ptId &&
                    x.SinYm == sinDate / 100
                ).Select(x => new SinRpInfModel(x)).ToList();
            }

            #region 診療点数を取得
            var sinTensuQuery = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiCount.HpId == hpId &&
                    sinKouiCount.PtId == ptId &&
                    sinKouiCount.SinYm == sinDate / 100 &&
                    sinKouiCount.SinDay == sinDate - sinDate / 100 * 100 &&
                    sinKoui.EntenKbn == 0 &&
                    sinRpInf.SanteiKbn != 2    //自費算定を除く
                group new
                {
                    sinKouiCount.HpId,
                    sinKouiCount.PtId,
                    sinKouiCount.RaiinNo,
                    sinKoui.HokenPid,
                    sinKoui.HokenId,
                    TotalTen = sinKoui.Ten * sinKouiCount.Count
                } by new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RaiinNo, sinKoui.HokenPid, sinKoui.HokenId } into sinKouiGroup
                select new
                {
                    sinKouiGroup.Key.HpId,
                    sinKouiGroup.Key.PtId,
                    sinKouiGroup.Key.RaiinNo,
                    sinKouiGroup.Key.HokenPid,
                    sinKouiGroup.Key.HokenId,
                    Tensu = (int)sinKouiGroup.Sum(x => Math.Floor(x.TotalTen)),  //小数点以下切り捨て
                    Jihi = 0,
                    OutTax = 0,
                    InclTax = 0,
                    JihiTaxFree = 0,
                    JihiOutTaxNr = 0,
                    JihiOutTaxGen = 0,
                    JihiTaxNr = 0,
                    JihiTaxGen = 0,
                    OutTaxNr = 0,
                    OutTaxGen = 0,
                    InclTaxNr = 0,
                    InclTaxGen = 0,
                    RousaiEnTensu = 0
                }
            );
            #endregion

            #region 自費診療の負担額を取得
            var jihiQuery = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiCount.HpId == hpId &&
                    sinKouiCount.PtId == ptId &&
                    sinKouiCount.SinYm == sinDate / 100 &&
                    sinKouiCount.SinDay == sinDate - sinDate / 100 * 100 &&
                    (sinKoui.CdKbn == "JS" || sinRpInf.SanteiKbn == 2)
                group new
                {
                    sinKouiCount.HpId,
                    sinKouiCount.PtId,
                    sinKouiCount.RaiinNo,
                    sinKoui.HokenPid,
                    sinKoui.HokenId,
                    sinKoui.KazeiKbn,
                    TotalTen = sinKoui.EntenKbn == 0 ? sinKoui.Ten * EntenRate.Val * sinKouiCount.Count :
                        sinKoui.Ten * sinKouiCount.Count
                } by new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RaiinNo, sinKoui.HokenPid, sinKoui.HokenId } into sinKouiGroup
                select new
                {
                    sinKouiGroup.Key.HpId,
                    sinKouiGroup.Key.PtId,
                    sinKouiGroup.Key.RaiinNo,
                    sinKouiGroup.Key.HokenPid,
                    sinKouiGroup.Key.HokenId,
                    Tensu = 0,
                    Jihi = (int)sinKouiGroup.Sum(x => Math.Floor(x.TotalTen)),  //小数点以下切り捨て
                    OutTax = 0,
                    InclTax = 0,
                    JihiTaxFree = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 0 ? x.TotalTen : 0)),
                    JihiOutTaxNr = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 1 ? x.TotalTen : 0)),
                    JihiOutTaxGen = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 2 ? x.TotalTen : 0)),
                    JihiTaxNr = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 3 ? x.TotalTen : 0)),
                    JihiTaxGen = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 4 ? x.TotalTen : 0)),
                    OutTaxNr = 0,
                    OutTaxGen = 0,
                    InclTaxNr = 0,
                    InclTaxGen = 0,
                    RousaiEnTensu = 0
                }
            );
            #endregion

            #region 自費診療(外税) を取得
            var outTaxQuery = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                where
                    sinKouiCount.HpId == hpId &&
                    sinKouiCount.PtId == ptId &&
                    sinKouiCount.SinYm == sinDate / 100 &&
                    sinKouiCount.SinDay == sinDate - sinDate / 100 * 100 &&
                    sinKoui.EntenKbn == 1 &&
                    sinKoui.CdKbn == "SZ"
                group new
                {
                    sinKouiCount.HpId,
                    sinKouiCount.PtId,
                    sinKouiCount.RaiinNo,
                    sinKoui.HokenPid,
                    sinKoui.HokenId,
                    sinKoui.KazeiKbn,
                    TotalTen = sinKoui.Ten * sinKouiCount.Count
                } by new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RaiinNo, sinKoui.HokenPid, sinKoui.HokenId } into sinKouiGroup
                select new
                {
                    sinKouiGroup.Key.HpId,
                    sinKouiGroup.Key.PtId,
                    sinKouiGroup.Key.RaiinNo,
                    sinKouiGroup.Key.HokenPid,
                    sinKouiGroup.Key.HokenId,
                    Tensu = 0,
                    Jihi = 0,
                    OutTax = (int)sinKouiGroup.Sum(x => Math.Floor(x.TotalTen)),
                    InclTax = 0,
                    JihiTaxFree = 0,
                    JihiOutTaxNr = 0,
                    JihiOutTaxGen = 0,
                    JihiTaxNr = 0,
                    JihiTaxGen = 0,
                    OutTaxNr = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 1 ? x.TotalTen : 0)),
                    OutTaxGen = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 2 ? x.TotalTen : 0)),
                    InclTaxNr = 0,
                    InclTaxGen = 0,
                    RousaiEnTensu = 0
                }
            );
            #endregion

            #region 自費診療(内税) を取得
            var inclTaxQuery = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                where
                    sinKouiCount.HpId == hpId &&
                    sinKouiCount.PtId == ptId &&
                    sinKouiCount.SinYm == sinDate / 100 &&
                    sinKouiCount.SinDay == sinDate - sinDate / 100 * 100 &&
                    sinKoui.EntenKbn == 1 &&
                    sinKoui.CdKbn == "UZ"
                group new
                {
                    sinKouiCount.HpId,
                    sinKouiCount.PtId,
                    sinKouiCount.RaiinNo,
                    sinKoui.HokenPid,
                    sinKoui.HokenId,
                    sinKoui.KazeiKbn,
                    TotalZei = sinKoui.Zei * sinKouiCount.Count
                } by new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RaiinNo, sinKoui.HokenPid, sinKoui.HokenId } into sinKouiGroup
                select new
                {
                    sinKouiGroup.Key.HpId,
                    sinKouiGroup.Key.PtId,
                    sinKouiGroup.Key.RaiinNo,
                    sinKouiGroup.Key.HokenPid,
                    sinKouiGroup.Key.HokenId,
                    Tensu = 0,
                    Jihi = 0,
                    OutTax = 0,
                    InclTax = (int)sinKouiGroup.Sum(x => Math.Floor(x.TotalZei)),
                    JihiTaxFree = 0,
                    JihiOutTaxNr = 0,
                    JihiOutTaxGen = 0,
                    JihiTaxNr = 0,
                    JihiTaxGen = 0,
                    OutTaxNr = 0,
                    OutTaxGen = 0,
                    InclTaxNr = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 3 ? x.TotalZei : 0)),
                    InclTaxGen = (int)sinKouiGroup.Sum(x => Math.Floor(x.KazeiKbn == 4 ? x.TotalZei : 0)),
                    RousaiEnTensu = 0
                }
            );
            #endregion

            #region 労災円負担を取得
            //上記以外の金額（労災用）該当項目の有無をチェック
            var EnQuery = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                where
                    sinKouiCount.HpId == hpId &&
                    sinKouiCount.PtId == ptId &&
                    sinKouiCount.SinYm == sinDate / 100 &&
                    sinKouiCount.SinDay == sinDate - sinDate / 100 * 100 &&
                    sinKoui.EntenKbn == 1 &&
                    sinKoui.CdKbn != "JS" &&
                    sinKoui.CdKbn != "SZ" &&
                    sinKoui.CdKbn != "UZ"
                group new
                {
                    sinKouiCount.HpId,
                    sinKouiCount.PtId,
                    sinKouiCount.RaiinNo,
                    sinKoui.HokenPid,
                    sinKoui.HokenId,
                    TotalTen = sinKoui.Ten * sinKouiCount.Count / EntenRate.Val
                } by new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.RaiinNo, sinKoui.HokenPid, sinKoui.HokenId } into sinKouiGroup
                select new
                {
                    sinKouiGroup.Key.HpId,
                    sinKouiGroup.Key.PtId,
                    sinKouiGroup.Key.RaiinNo,
                    sinKouiGroup.Key.HokenPid,
                    sinKouiGroup.Key.HokenId,
                    Tensu = 0,
                    Jihi = 0,
                    OutTax = 0,
                    InclTax = 0,
                    JihiTaxFree = 0,
                    JihiOutTaxNr = 0,
                    JihiOutTaxGen = 0,
                    JihiTaxNr = 0,
                    JihiTaxGen = 0,
                    OutTaxNr = 0,
                    OutTaxGen = 0,
                    InclTaxNr = 0,
                    InclTaxGen = 0,
                    RousaiEnTensu = (int)sinKouiGroup.Sum(x => Math.Floor(x.TotalTen))  //小数点以下切り捨て
                }
            );
            #endregion

            //診療点数合計
            var sumTensuConcat = sinTensuQuery.Concat(jihiQuery).Concat(outTaxQuery).Concat(inclTaxQuery).Concat(EnQuery);
            var sumTensuQuery = (
                from sumTen in sumTensuConcat
                group sumTen by new { sumTen.HpId, sumTen.PtId, sumTen.RaiinNo, sumTen.HokenPid, sumTen.HokenId } into sumTenGroup
                select new
                {
                    sumTenGroup.Key.HpId,
                    sumTenGroup.Key.PtId,
                    sumTenGroup.Key.RaiinNo,
                    sumTenGroup.Key.HokenPid,
                    sumTenGroup.Key.HokenId,
                    Tensu = sumTenGroup.Sum(x => x.Tensu),
                    Jihi = sumTenGroup.Sum(x => x.Jihi),
                    OutTax = sumTenGroup.Sum(x => x.OutTax),
                    InclTax = sumTenGroup.Sum(x => x.InclTax),
                    JihiTaxFree = sumTenGroup.Sum(x => x.JihiTaxFree),
                    JihiOutTaxNr = sumTenGroup.Sum(x => x.JihiOutTaxNr),
                    JihiOutTaxGen = sumTenGroup.Sum(x => x.JihiOutTaxGen),
                    JihiTaxNr = sumTenGroup.Sum(x => x.JihiTaxNr),
                    JihiTaxGen = sumTenGroup.Sum(x => x.JihiTaxGen),
                    OutTaxNr = sumTenGroup.Sum(x => x.OutTaxNr),
                    OutTaxGen = sumTenGroup.Sum(x => x.OutTaxGen),
                    InclTaxNr = sumTenGroup.Sum(x => x.InclTaxNr),
                    InclTaxGen = sumTenGroup.Sum(x => x.InclTaxGen),
                    RousaiEnTensu = sumTenGroup.Sum(x => x.RousaiEnTensu)
                }
            );

            //診療点数に公費の優先順位を結合
            var raiinKohis = (
                from sumTensu in sumTensuQuery
                join hokPattern in hokenPatterns on
                    new { sumTensu.HpId, sumTensu.PtId, sumTensu.HokenPid } equals
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.HokenPid }
                join ptKohis1 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi1Id } equals
                    new { ptKohis1.HpId, ptKohis1.PtId, Kohi1Id = ptKohis1.KohiId } into kohi1Join
                from ptKohi1 in kohi1Join.DefaultIfEmpty()
                join ptKohis2 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi2Id } equals
                    new { ptKohis2.HpId, ptKohis2.PtId, Kohi2Id = ptKohis2.KohiId } into kohi2Join
                from ptKohi2 in kohi2Join.DefaultIfEmpty()
                join ptKohis3 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi3Id } equals
                    new { ptKohis3.HpId, ptKohis3.PtId, Kohi3Id = ptKohis3.KohiId } into kohi3Join
                from ptKohi3 in kohi3Join.DefaultIfEmpty()
                join ptKohis4 in ptKohiQuery on
                    new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi4Id } equals
                    new { ptKohis4.HpId, ptKohis4.PtId, Kohi4Id = ptKohis4.KohiId } into kohi4Join
                from ptKohi4 in kohi4Join.DefaultIfEmpty()
                select new
                {
                    sumTensu,
                    Kohi1Houbetu = ptKohi1 == null ? "999" : ptKohi1.Houbetu,
                    Kohi1PriorityNo = ptKohi1 == null ? "99999" : ptKohi1.PriorityNo,
                    Kohi2Houbetu = ptKohi2 == null ? "999" : ptKohi2.Houbetu,
                    Kohi2PriorityNo = ptKohi2 == null ? "99999" : ptKohi2.PriorityNo,
                    Kohi3Houbetu = ptKohi3 == null ? "999" : ptKohi3.Houbetu,
                    Kohi3PriorityNo = ptKohi3 == null ? "99999" : ptKohi3.PriorityNo,
                    Kohi4Houbetu = ptKohi4 == null ? "999" : ptKohi4.Houbetu,
                    Kohi4PriorityNo = ptKohi4 == null ? "99999" : ptKohi4.PriorityNo,
                }
            ).ToList();

            //来院情報取得
            if (raiinInfs == null)
            {
                //var wrkList = _tenantDataContext.RaiinInfs.FindListNoTrack(r =>
                //    r.HpId == hpId &&
                //    r.PtId == ptId &&
                //    r.SinDate == sinDate &&
                //    r.IsDeleted == DeleteStatus.None &&
                //    r.Status >= RaiinState.Calculate
                //).ToList();
                //raiinInfs = wrkList.Select(x => new RaiinInfModel(x)).ToList();
                raiinInfs = _tenantDataContext.RaiinInfs.FindListNoTrack(r =>
                    r.HpId == hpId &&
                    r.PtId == ptId &&
                    r.SinDate == sinDate &&
                    r.IsDeleted == DeleteTypes.None &&
                    r.Status >= RaiinState.Calculate
                ).Select(x => new RaiinInfModel(x)).ToList();
            }

            //来院情報に診療点数と公費の優先順位を結合
            var joinList = (
                from raiinInf in raiinInfs
                join raiinKohi in raiinKohis on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo } equals
                    new { raiinKohi.sumTensu.HpId, raiinKohi.sumTensu.PtId, raiinKohi.sumTensu.RaiinNo }
                select new
                {
                    raiinInf,
                    raiinKohi
                }
            );

            //試算
            bool trialCalc = raiinNo > 0;
            //試算の場合は１来院のみ
            if (trialCalc)
            {
                joinList = joinList.Where(x => x.raiinInf.RaiinNo == raiinNo);
            }

            //結果出力
            var result = joinList.Select(
                x => new RaiinTensuModel()
                {
                    HpId = x.raiinInf.HpId,
                    PtId = x.raiinInf.PtId,
                    SinDate = x.raiinInf.SinDate,
                    RaiinNo = x.raiinInf.RaiinNo,
                    OyaRaiinNo = x.raiinInf.OyaRaiinNo,
                    SinStartTime = x.raiinInf.SinStartTime,
                    HokenPid = x.raiinKohi.sumTensu.HokenPid,
                    HokenId = x.raiinKohi.sumTensu.HokenId,
                    Kohi1Houbetu = x.raiinKohi.Kohi1Houbetu,
                    Kohi1PriorityNo = x.raiinKohi.Kohi1PriorityNo,
                    Kohi2Houbetu = x.raiinKohi.Kohi2Houbetu,
                    Kohi2PriorityNo = x.raiinKohi.Kohi2PriorityNo,
                    Kohi3Houbetu = x.raiinKohi.Kohi3Houbetu,
                    Kohi3PriorityNo = x.raiinKohi.Kohi3PriorityNo,
                    Kohi4Houbetu = x.raiinKohi.Kohi4Houbetu,
                    Kohi4PriorityNo = x.raiinKohi.Kohi4PriorityNo,
                    Tensu = x.raiinKohi.sumTensu.Tensu,
                    JihiFutan = x.raiinKohi.sumTensu.Jihi,
                    OutTax = x.raiinKohi.sumTensu.OutTax,
                    InclTax = x.raiinKohi.sumTensu.InclTax,
                    JihiTaxFree = x.raiinKohi.sumTensu.JihiTaxFree,
                    JihiOutTaxNr = x.raiinKohi.sumTensu.JihiOutTaxNr,
                    JihiOutTaxGen = x.raiinKohi.sumTensu.JihiOutTaxGen,
                    JihiTaxNr = x.raiinKohi.sumTensu.JihiTaxNr,
                    JihiTaxGen = x.raiinKohi.sumTensu.JihiTaxGen,
                    OutTaxNr = x.raiinKohi.sumTensu.OutTaxNr,
                    OutTaxGen = x.raiinKohi.sumTensu.OutTaxGen,
                    InclTaxNr = x.raiinKohi.sumTensu.InclTaxNr,
                    InclTaxGen = x.raiinKohi.sumTensu.InclTaxGen,
                    RousaiEnTensu = x.raiinKohi.sumTensu.RousaiEnTensu,
                    SyosaisinKbn = x.raiinInf.SyosaisinKbn
                }
            ).OrderBy(x => x.SortKey).ToList();


            //実日数(+保険区分の設定)
            foreach (var wrkRaiin in result)
            {
                wrkRaiin.JituNisu = JituNisuCount(
                    wrkRaiin, sinKouiCounts, sinKouis, sinKouiDetails, sinRpInfs
                );
            }

            //妊婦
            foreach (var wrkRaiin in result)
            {
                wrkRaiin.IsNinpu = IsNinpu(
                    wrkRaiin, sinKouiCounts, sinKouis, sinKouiDetails, sinRpInfs
                );
            }

            //在医総及び在医総管
            foreach (var wrkRaiin in result)
            {
                wrkRaiin.IsZaiiso = IsZaiiso(
                    wrkRaiin, sinKouiCounts, sinKouis, sinKouiDetails, sinRpInfs
                );
            }

            //労災・自賠
            foreach (var wrkRaiin in result)
            {
                RousaiJibaiAggregate(wrkRaiin, sinKouiCounts, sinKouis, sinKouiDetails, sinRpInfs);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        //private int SyosaisinKbn(int hpId, long raiinNo)
        //{
        //    var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack();
        //    var odrDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack(
        //        x => x.ItemCd == ItemCdConst.SyosaiKihon
        //    );

        //    double result = (
        //        from odrInf in odrInfs
        //        join odrDetail in odrDetails on
        //            new { odrInf.HpId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
        //            new { odrDetail.HpId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
        //        where
        //                    odrInf.HpId == hpId &&
        //                    odrInf.RaiinNo == raiinNo &&
        //                    odrInf.IsDeleted == DeleteStatus.None
        //        orderby
        //            odrDetail.SinDate, odrDetail.RaiinNo
        //        select new
        //        {
        //            Suryo = odrDetail.Suryo
        //        }
        //    ).FirstOrDefault().Suryo;

        //    return (int)result;
        //}

        /// <summary>
        /// 実日数カウント
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="tensu">診療点数</param>
        /// <returns></returns>
        private bool JituNisuCount(
            RaiinTensuModel raiinTensu, List<SinKouiCountModel> sinKouiCounts,
            List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinKouiDetails, List<SinRpInfModel> sinRpInfs
        )
        {
            //診療明細を取得

            //var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack();
            //var sinKouis = _tenantDataContext.SinKouis.FindListQueryableNoTrack();
            //var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack();
            //var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack();
            //var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack();
            //var tenMstKeys = _tenantDataContext.TenMsts.FindListQueryableNoTrack(
            //    x => x.JitudayCount != 0 && x.StartDate <= raiinTensu.SinDate
            //).GroupBy(
            //    x => new { x.HpId, x.ItemCd }
            //).Select(
            //    x => new
            //    {
            //        HpId = x.Key.HpId,
            //        ItemCd = x.Key.ItemCd,
            //        StartDate = x.Max(d => d.StartDate)
            //    }
            //);

            //診察日を基準とした点数マスタの取得
            //var curTenMsts = (
            //    from tenMst in tenMsts
            //    join tenMstKey in tenMstKeys on
            //        new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
            //        new { tenMstKey.HpId, tenMstKey.ItemCd, tenMstKey.StartDate }
            //    select new
            //    {
            //        HpId = tenMst.HpId,
            //        ItemCd = tenMst.ItemCd,
            //        JitudayCount = tenMst.JitudayCount
            //    }
            //);

            //実日数をカウントする項目が算定されているか
            var sinCur = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join sinKouiDetail in sinKouiDetails on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                    //join tenMst in curTenMsts on
                    //    new { sinKouiDetail.HpId, sinKouiDetail.ItemCd } equals
                    //    new { tenMst.HpId, tenMst.ItemCd }
                where
                    sinKouiCount.HpId == raiinTensu.HpId &&
                    sinKouiCount.RaiinNo == raiinTensu.RaiinNo &&
                    sinRpInf.SanteiKbn != 2 &&    //自費算定を除く
                    sinKoui.HokenPid == raiinTensu.HokenPid
                select new
                {
                    //JitudayCount = tenMst.JitudayCount,
                    JitudayCount = sinKouiDetail.TenMst?.JitudayCount ?? 0,
                    HokenKbn = sinRpInf.HokenKbn
                }
            ).ToList();
            bool dayCount = sinCur.Sum(x => x.JitudayCount) >= 1;
            //※保険区分を設定しておく !!!!
            if (sinCur != null && sinCur.Any())
            {
                raiinTensu.HokenKbn = sinCur.Max(x => x.HokenKbn);
            }
            else
            {
                raiinTensu.HokenKbn = 0;
            }

            //実日数をカウントする項目が算定されている -> OK
            if (dayCount) return true;

            //診療行為ベース
            var sinKouiBases = (
                from sinKouiCount in sinKouiCounts
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join sinKouiDetail in sinKouiDetails on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                where
                    sinKouiCount.HpId == raiinTensu.HpId &&
                    sinKouiCount.PtId == raiinTensu.PtId &&
                    sinRpInf.SanteiKbn != 2 &&       //自費算定を除く
                    sinRpInf.HokenKbn == raiinTensu.HokenKbn
                select new
                {
                    SinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay,
                    ItemCd = sinKouiDetail.ItemCd
                }
            );

            //同月内に地域包括診療料を算定しているか
            bool tiikiHoukatu = (
                from sinKoui in sinKouiBases
                where
                    (sinKoui.ItemCd == ItemCdConst.IgakuTiikiHoukatu1 ||
                     sinKoui.ItemCd == ItemCdConst.IgakuTiikiHoukatu2 ||
                     sinKoui.ItemCd == ItemCdConst.IgakuNintiTiikiHoukatu1 ||
                     sinKoui.ItemCd == ItemCdConst.IgakuNintiTiikiHoukatu2) &&
                    sinKoui.SinDate >= raiinTensu.SinDate / 100 * 100 + 1 &&
                    sinKoui.SinDate <= raiinTensu.SinDate
                select sinKoui
            ).Count() >= 1;

            //再診料が算定できない期間に行われた地域包括診療の日数を実日数に含む
            if (tiikiHoukatu && raiinTensu.SyosaisinKbn >= 1) return true;

            //日前
            int daysAgo6 = CIUtil.DateTimeToInt(CIUtil.IntToDate(raiinTensu.SinDate).AddDays(-6));
            int daysAgo13 = CIUtil.DateTimeToInt(CIUtil.IntToDate(raiinTensu.SinDate).AddDays(-13));

            if (raiinTensu.Tensu >= 1)
            {
                //前月の場合は診療データを追加
                if (daysAgo13 / 100 < raiinTensu.SinDate / 100)
                {
                    var wrkKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack();
                    var wrkKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack();
                    var wrkRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack();

                    //診療行為ベース
                    var wrkKouiBases = (
                        from sinKouiCount in wrkKouiCounts
                        join sinRpInf in wrkRpInfs on
                            new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                            new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                        join sinKouiDetail in wrkKouiDetails on
                            new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                            new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                        where
                            sinKouiCount.HpId == raiinTensu.HpId &&
                            sinKouiCount.PtId == raiinTensu.PtId &&
                            sinKouiCount.SinYm == daysAgo13 / 100 &&  //前月分
                            sinRpInf.SanteiKbn != 2 &&                //自費算定を除く
                            sinRpInf.HokenKbn == raiinTensu.HokenKbn
                        select new
                        {
                            SinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay,
                            ItemCd = sinKouiDetail.ItemCd
                        }
                    ).ToList();

                    //前月分を結合
                    sinKouiBases = sinKouiBases.Union(wrkKouiBases);
                }

                //外来リハの初再診料が算定できない期間を実日数としてカウントする
                bool gairaiRiha = (
                    from sinKoui in sinKouiBases
                    where
                        (sinKoui.ItemCd == ItemCdConst.IgakuGairaiRiha1 &&
                         sinKoui.SinDate >= daysAgo6 &&
                         sinKoui.SinDate <= raiinTensu.SinDate) ||
                        (sinKoui.ItemCd == ItemCdConst.IgakuGairaiRiha2 &&
                         sinKoui.SinDate >= daysAgo13 &&
                         sinKoui.SinDate <= raiinTensu.SinDate)
                    select sinKoui
                ).Count() >= 1;

                if (gairaiRiha) return true;
            }

            return false;
        }

        /// <summary>
        /// 妊婦加算
        /// </summary>
        /// <param name="raiinTensu"></param>
        /// <param name="sinKouiCounts"></param>
        /// <param name="sinKouis"></param>
        /// <param name="sinKouiDetails"></param>
        /// <param name="sinRpInfs"></param>
        /// <returns></returns>
        private bool IsNinpu(
                    RaiinTensuModel raiinTensu, List<SinKouiCountModel> sinKouiCounts,
                    List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinKouiDetails, List<SinRpInfModel> sinRpInfs
                )
        {
            //妊婦加算が算定されているか
            var sinNinpu = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join sinKouiDetail in sinKouiDetails on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                where
                    sinKouiCount.HpId == raiinTensu.HpId &&
                    sinKouiCount.RaiinNo == raiinTensu.RaiinNo &&
                    sinRpInf.SanteiKbn != 2 &&    //自費算定を除く
                    sinKoui.HokenPid == raiinTensu.HokenPid
                select new
                {
                    sinKouiDetail
                }
            ).ToList();

            return
                sinNinpu.Where(
                    s =>
                        ItemCdConst.ninpuKasanls.Any(
                            key => s.sinKouiDetail.ItemCd?.Contains(key) == true
                        )
                ).Count() >= 1;
        }

        /// <summary>
        /// 在医総及び在医総管
        /// </summary>
        /// <param name="raiinTensu"></param>
        /// <param name="sinKouiCounts"></param>
        /// <param name="sinKouis"></param>
        /// <param name="sinKouiDetails"></param>
        /// <param name="sinRpInfs"></param>
        /// <returns></returns>
        private bool IsZaiiso(
                    RaiinTensuModel raiinTensu, List<SinKouiCountModel> sinKouiCounts,
                    List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinKouiDetails, List<SinRpInfModel> sinRpInfs
                )
        {
            int sinZaiiso = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join sinKouiDetail in sinKouiDetails on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                where
                    sinKouiCount.HpId == raiinTensu.HpId &&
                    sinKouiCount.RaiinNo == raiinTensu.RaiinNo &&
                    sinRpInf.SanteiKbn != 2 &&    //自費算定を除く
                    sinKoui.HokenPid == raiinTensu.HokenPid &&
                    sinKouiDetail.TenMst?.CdKbn == "C" &&
                    sinKouiDetail.TenMst?.CdKbnno == 2 &&
                    sinKouiDetail.TenMst?.Kokuji2 == "1"
                select new
                {
                    sinKouiDetail
                }
            ).Count();

            return sinZaiiso > 0;
        }


        /// <summary>
        /// 労災自賠合計点数
        /// </summary>
        /// <param name="raiinTensu"></param>
        /// <param name="sinKouiCounts"></param>
        /// <param name="sinKouis"></param>
        /// <param name="sinKouiDetails"></param>
        /// <param name="sinRpInfs"></param>
        private void RousaiJibaiAggregate(
            RaiinTensuModel raiinTensu, List<SinKouiCountModel> sinKouiCounts,
            List<SinKouiModel> sinKouis, List<SinKouiDetailModel> sinKouiDetails, List<SinRpInfModel> sinRpInfs
        )
        {
            var sinJoin = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiCount.HpId == raiinTensu.HpId &&
                    sinKouiCount.RaiinNo == raiinTensu.RaiinNo &&
                    sinKoui.HokenPid == raiinTensu.HokenPid &&
                    sinRpInf.SanteiKbn != 2    //自費算定を除く
                select new
                {
                    Tensu = sinKoui.Ten * sinKouiCount.Count,
                    sinKouiCount.Count,
                    sinKoui.SyukeiSaki,
                    sinKoui.EntenKbn
                }
            );

            var joinDetail = (
                from sinKouiCount in sinKouiCounts
                join sinKoui in sinKouis on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join sinKouiDetail in sinKouiDetails on
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo } equals
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                where
                    sinKouiCount.HpId == raiinTensu.HpId &&
                    sinKouiCount.RaiinNo == raiinTensu.RaiinNo &&
                    sinKoui.HokenPid == raiinTensu.HokenPid &&
                    sinRpInf.SanteiKbn != 2    //自費算定を除く
                select new
                {
                    sinKoui.SyukeiSaki,
                    sinKouiDetail.TenMst.Ten,
                    sinKouiDetail.Suryo
                }
            );

            switch (raiinTensu.HokenKbn)
            {
                case 1: //労災
                case 2: //労災アフターケア
                    //労災イ点負担額
                    raiinTensu.RousaiIFutan = raiinTensu.Tensu;

                    //労災ロ円負担額
                    raiinTensu.RousaiRoFutan = (int)sinJoin.Where(x =>
                        new string[] { "A110", "A120", "A130", "A131", "A180" }.Contains(x.SyukeiSaki)
                    ).Sum(
                        x => Math.Floor(x.Tensu)    //小数点以下切り捨て
                    );
                    break;
                case 3: //自賠
                    //健保準拠
                    if (_systemConfigProvider.GetJibaiJunkyo() == 0)
                    {
                        //自賠健保点数
                        raiinTensu.JibaiKenpoTensu = raiinTensu.Tensu;
                    }
                    //労災準拠
                    else
                    {
                        string[] jibaiYakuzaiSyukeisaki =
                            new string[] {
                                    ReceSyukeisaki.TouyakuNaiYakuzai,
                                    ReceSyukeisaki.TouyakuTon,
                                    ReceSyukeisaki.TouyakuGaiYakuzai,
                                    ReceSyukeisaki.ChusyaYakuzai,
                                    ReceSyukeisaki.SyotiYakuzai,
                                    ReceSyukeisaki.OpeYakuzai,
                                    ReceSyukeisaki.Kensayakuzai,
                                    ReceSyukeisaki.GazoYakuzai,
                                    ReceSyukeisaki.SonotaYakuzai
                                };

                        //自賠イ技術点数
                        raiinTensu.JibaiITensu = (int)sinJoin.Where(x =>
                            !jibaiYakuzaiSyukeisaki.Contains(x.SyukeiSaki) &&
                            x.EntenKbn == 0
                        ).Sum(
                            x => Math.Floor(x.Tensu)    //小数点以下切り捨て
                        );

                        //自賠ロ薬剤点数
                        raiinTensu.JibaiRoTensu = (int)sinJoin.Where(x =>
                            jibaiYakuzaiSyukeisaki.Contains(x.SyukeiSaki)
                        ).Sum(
                            x => Math.Floor(x.Tensu)    //小数点以下切り捨て
                        );

                        //自賠ハ円診察負担額
                        raiinTensu.JibaiHaFutan = (int)sinJoin.Where(x =>
                            new string[] { "A110", "A120", "A130", "A131" }.Contains(x.SyukeiSaki)
                        ).Sum(
                            x => Math.Floor(x.Tensu)    //小数点以下切り捨て
                        );
                    }

                    //自賠ニ円他負担額
                    raiinTensu.JibaiNiFutan = (int)sinJoin.Where(x =>
                        x.SyukeiSaki == "A180"
                    ).Sum(
                        x => Math.Floor(x.Tensu)    //小数点以下切り捨て
                    );

                    //自賠ホ診断書料
                    raiinTensu.JibaiHoSindan = (int)sinJoin.Where(x =>
                        x.SyukeiSaki == "ZZZ0"
                    ).Sum(
                        x => Math.Floor(x.Tensu)    //小数点以下切り捨て
                    );

                    //自賠ホ診断書料枚数
                    raiinTensu.JibaiHoSindanCount = (int)joinDetail.Where(x =>
                        x.SyukeiSaki == "ZZZ0" &&
                        x.Ten != 0                  //コメント除く
                    ).Sum(
                        x => x.Suryo
                    );

                    //自賠ヘ明細書料
                    raiinTensu.JibaiHeMeisai = (int)sinJoin.Where(x =>
                        x.SyukeiSaki == "ZZZ1"
                    ).Sum(
                        x => Math.Floor(x.Tensu)    //小数点以下切り捨て
                    );

                    //自賠ヘ明細書料枚数
                    raiinTensu.JibaiHeMeisaiCount = (int)joinDetail.Where(x =>
                        x.SyukeiSaki == "ZZZ1" &&
                        x.Ten != 0                  //コメント除く
                    ).Sum(
                        x => x.Suryo
                    );

                    break;
            }

        }
    }

}
