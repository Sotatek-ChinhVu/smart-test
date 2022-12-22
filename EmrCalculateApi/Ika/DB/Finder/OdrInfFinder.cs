using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using EmrCalculateApi.Ika.Constants;
using EmrCalculateApi.Interface;
using Domain.Constant;
using EmrCalculateApi.Constants;
using Infrastructure.Interfaces;

namespace EmrCalculateApi.Ika.DB.Finder
{
    public class OdrInfFinder 
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public OdrInfFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        /// <summary>
        /// オーダー情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>
        /// 指定の患者の指定の診療日のオーダー情報
        /// 削除分は除く
        /// </returns>
        public List<OdrInfModel> FindOdrInfData(int hpId, long ptId, int sinDate)
        {
            const string conFncName = nameof(FindOdrInfData);

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                (o.SanteiKbn == SanteiKbnConst.Santei || o.SanteiKbn == SanteiKbnConst.Jihi) &&
                o.IsDeleted == DeleteTypes.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
                o.HpId == hpId );
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.SinDate == sinDate &&
                r.Status >= 5 &&    // 計算状態以上
                r.IsDeleted == DeleteTypes.None);

            var joinQuery = (
                from odrInf in odrInfs
                join PtHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
                join RaiinInf in raiinInfs on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo } equals
                    new { RaiinInf.HpId, RaiinInf.PtId, RaiinInf.RaiinNo }
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.IsDeleted == DeleteTypes.None
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInf.RpNo
                select new
                {
                    odrInf,
                    PtHokenPattern,
                    RaiinInf
                }
            );
            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new OdrInfModel(data.odrInf, new PtHokenPatternModel(data.PtHokenPattern), new RaiinInfModel(data.RaiinInf, null))
                )
                .ToList();

            List<OdrInfModel> results = new List<OdrInfModel>();

            entities?.ForEach(entity => {
                results.Add(new OdrInfModel(entity.OdrInf, entity.PtHokenPattern, entity.RaiinInf));
            });

            return results;
        }

        public List<OdrDtlTenModel> FindOdrInfDetailData(int hpId, long ptId, int sinDate)
        {
            const string conFncName = nameof(FindOdrInfDetailData);

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&                
                //(o.SanteiKbn == SanteiKbnConst.Santei || o.SanteiKbn == SanteiKbnConst.Jihi) &&
                o.IsDeleted == DeleteStatus.None);
            var odrInfDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate);

            var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                t.HpId == hpId &&
                t.StartDate <= sinDate &&
                (t.EndDate >= sinDate));

            var cmtKbnMsts = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(c =>
                c.HpId == hpId &&
                c.StartDate <= sinDate &&
                c.EndDate >= sinDate);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
                o.HpId == hpId);
            var ipnKasanMsts = _tenantDataContext.IpnKasanMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.EndDate >= sinDate &&
                i.IsDeleted == DeleteStatus.None);
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.SinDate == sinDate &&
                r.Status >= RaiinState.Calculate &&    // 計算状態以上
                r.IsDeleted == DeleteTypes.None);
            //var yakkaSyusaiMstsSub = _tenantDataContext.YakkaSyusaiMstRepository.FindListQueryable(y =>
            //    y.HpId == hpId &&
            //    y.StartDate <= sinDate &&
            //    y.EndDate >= sinDate &&
            //    (new string[] { "2", "3" }.Contains(y.Kbn) || y.JunSenpatu == 1)
            //    );
            //var yakkaSyusaiMsts = (
            //    from yakkaSyusaiMst in yakkaSyusaiMstsSub
            //    group yakkaSyusaiMst by new { hpId = yakkaSyusaiMst.HpId, ipnNameCd = yakkaSyusaiMst.YakkaCd.Substring(0, 9)} into A
            //    select new
            //    {
            //        HpId = A.Key.hpId,
            //        IpnNameCd = A.Key.ipnNameCd
            //    }
                
            //    );
            var ipnMinYakkaMstsSub1 = _tenantDataContext.IpnMinYakkaMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.IsDeleted == DeleteStatus.None);
            var ipnMinYakkaMstsSub2 = (
                from ipnMinYakka in ipnMinYakkaMstsSub1
                group ipnMinYakka by new { hpId = ipnMinYakka.HpId, ipnNameCd = ipnMinYakka.IpnNameCd } into A
                select new { HpId = A.Key.hpId, IpnNameCd = A.Key.ipnNameCd, StartDate = A.Max(a => a.StartDate) }
                );
            var ipnMinYakkaMsts = (
                from ipnMinS1 in ipnMinYakkaMstsSub1
                join ipnMinS2 in ipnMinYakkaMstsSub2 on
                    new { ipnMinS1.HpId, ipnMinS1.IpnNameCd, ipnMinS1.StartDate } equals
                    new { ipnMinS2.HpId, ipnMinS2.IpnNameCd, ipnMinS2.StartDate }
                select new
                {
                    ipnMinS1.HpId,
                    ipnMinS1.IpnNameCd,
                    ipnMinS1.Yakka,
                    ipnMinS1.StartDate
                }
                );

            var joinQuery = (
                from odrInf in odrInfs
                //where
                //    odrInf.HpId == hpId &&
                //    odrInf.PtId == ptId &&
                //    odrInf.SinDate == sinDate &&
                //    odrInf.IsDeleted == DeleteStatus.None
                join odrInfDetail in odrInfDetails on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                join PtHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
                join raiinInf in raiinInfs on
                    new { odrInfDetail.HpId, odrInfDetail.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.RaiinNo }
                join tenMst in tenMsts on
                    //new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd.Trim() } equals
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd } equals
                    new { tenMst.HpId, ItemCd = tenMst.ItemCd } into oJoin
                from oj in oJoin.DefaultIfEmpty()
                join cmtKbnMst in cmtKbnMsts on
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd } equals
                    new { cmtKbnMst.HpId, ItemCd = cmtKbnMst.ItemCd } into oJoin2
                from oj2 in oJoin2.DefaultIfEmpty()
                //join tenMst2 in tenMsts2 on
                //    new { oj == null ? "": oj.HpId, ItemCd = oj == null ? "": oj.SanteiItemCd } equals
                //    new { tenMst2.HpId, ItemCd = tenMst2.ItemCd } into oJoin2
                //from oj2 in oJoin2.DefaultIfEmpty()
                join ipnKasanMst in ipnKasanMsts on
                    new { oj.HpId, oj.IpnNameCd } equals
                    new { ipnKasanMst.HpId, ipnKasanMst.IpnNameCd } into oJoin3
                from oj3 in oJoin3.DefaultIfEmpty()
                join ipnMinYakkaMst in ipnMinYakkaMsts on
                    new { oj.HpId, oj.IpnNameCd } equals
                    new { ipnMinYakkaMst.HpId, ipnMinYakkaMst.IpnNameCd } into oJoin4
                from oj4 in oJoin4.DefaultIfEmpty()
                //join yakkaSyusaiMst in yakkaSyusaiMsts on
                //    new { oj.HpId, oj.ItemCd } equals
                //    new { yakkaSyusaiMst.HpId, yakkaSyusaiMst.ItemCd } into oJoin5
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
                select new
                {
                    odrInfDetail,
                    //tenMst = oj2 == null ? oj: oj2,
                    tenMst = oj,
                    cmtKbnMst = oj2,

                    //PtHokenPattern,
                    hokenKbn = PtHokenPattern.HokenKbn,
                    hokenPid = PtHokenPattern.HokenPid,
                    hokenId = PtHokenPattern.HokenId,
                    hokenSbt = PtHokenPattern.HokenSbtCd,
                    //odrInf,
                    odrKouiKbn = odrInf.OdrKouiKbn,
                    santeiKbn = odrInf.SanteiKbn,
                    inoutKbn = odrInf.InoutKbn,
                    syohoSbt = odrInf.SyohoSbt,
                    daysCnt = odrInf.DaysCnt,
                    sortNo = odrInf.SortNo,
                    //raiinInf,
                    sinStartTime = raiinInf.SinStartTime,
                    //ipnKasanMst = oj3
                    kasan1 = (oj3 == null ? 0 : oj3.Kasan1),
                    kasan2 = (oj3 == null ? 0 : oj3.Kasan2),
                    minYakka = (oj4 == null ? 0 : oj4.Yakka)
                    //kbn = (oj5 == null ? "" : oj5.Kbn),
                    //junSenpatu = (oj5 == null ? 0 : oj5.JunSenpatu)
                }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new OdrDtlTenModel(
                        data.odrInfDetail,
                        data.tenMst == null ? null : new TenMstModel(data.tenMst),   
                        data.cmtKbnMst,
                        "",
                        //new TenMstModel(data.tenMst),
                        //data.PtHokenPattern,
                        data.hokenKbn,
                        data.hokenPid,
                        data.hokenId,
                        data.hokenSbt,
                        //data.odrInf,
                        data.odrKouiKbn,
                        data.santeiKbn,
                        data.inoutKbn,
                        data.syohoSbt,
                        data.daysCnt,
                        data.sortNo,
                        //data.ipnKasanMst,
                        data.kasan1,
                        data.kasan2,
                        //data.raiinInf
                        data.sinStartTime,
                        data.minYakka
                        //data.kbn,
                        //data.junSenpatu
                    )
                )
                .ToList();
            List<OdrDtlTenModel> results = new List<OdrDtlTenModel>();

            entities?.ForEach(entity => {
                //if ((entity.TenMst == null && entity.ItemCd != null && entity.ItemCd != "") ||
                //    (entity.SanteiItemCd != entity.ItemCd && entity.ItemCd != null && entity.ItemCd != "" && entity.SanteiItemCd != "9999999999"))
                if ((entity.TenMst == null && string.IsNullOrEmpty(entity.ItemCd) == false) ||
                    (entity.SanteiItemCd != entity.ItemCd && string.IsNullOrEmpty(entity.ItemCd) == false && entity.SanteiItemCd != ItemCdConst.NoSantei && !entity.ItemCd.StartsWith("Z")))
                {
                    var tenEntities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                        p.HpId == hpId &&
                        p.StartDate <= sinDate &&
                        (p.EndDate >= sinDate || p.EndDate == 12341234) &&
                        p.ItemCd == entity.SanteiItemCd)
                    .OrderBy(p => p.HpId)
                    .ThenBy(p => p.ItemCd)
                    .ThenByDescending(p => p.StartDate);

                    var cmtEntities = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(p=>
                         p.HpId == hpId &&
                        p.StartDate <= sinDate &&
                        (p.EndDate >= sinDate || p.EndDate == 12341234) &&
                        p.ItemCd == entity.SanteiItemCd)
                    .OrderBy(p => p.HpId)
                    .ThenBy(p => p.ItemCd)
                    .ThenByDescending(p => p.StartDate);
                    
                    if (entity.TenMst == null || entity.TenMst != null && entity.TenMst.SanteigaiKbn != 1)
                    {
                        results.Add(
                            new OdrDtlTenModel(
                                entity.OdrInfDetail,
                                tenEntities.FirstOrDefault() == null ? entity.TenMst : new TenMstModel(tenEntities.FirstOrDefault()),
                                cmtEntities.FirstOrDefault() == null ? entity.CmtKbnMst : cmtEntities.FirstOrDefault(),
                                entity.ItemCd.StartsWith("IGE") || (entity.TenMst != null && entity.TenMst.SanteiItemCd == ItemCdConst.KensaIge) ? 
                                    (entity.TenMst.ReceName != "" ? entity.TenMst.ReceName.Trim() : entity.ItemName.Trim()) : "",
                                //entity.PtHokenPattern, 
                                entity.HokenKbn,
                                entity.HokenPid,
                                entity.HokenId,
                                entity.HokenSbt,
                                //entity.OdrInf, 
                                entity.OdrKouiKbn,
                                entity.SanteiKbn,
                                entity.InoutKbn,
                                entity.SyohoSbt,
                                entity.DaysCnt,
                                entity.SortNo,
                                //entity.IpnKasanMst, 
                                entity.Kasan1,
                                entity.Kasan2,
                                //entity.RaiinInf
                                entity.SinStartTime,
                                entity.MinYakka
                                //entity.Kbn,
                                //entity.JunSenpatu
                                ));
                    }
                }
                else if ((entity.TenMst != null && entity.TenMst.SanteigaiKbn != 1) ||
                        (string.IsNullOrEmpty(entity.ItemCd)) 
                        )
                {
                    results.Add(
                            new OdrDtlTenModel(
                                entity.OdrInfDetail,
                                entity.TenMst,
                                entity.CmtKbnMst,
                                "",
                                //entity.PtHokenPattern, 
                                entity.HokenKbn,
                                entity.HokenPid,
                                entity.HokenId,
                                entity.HokenSbt,
                                //entity.OdrInf, 
                                entity.OdrKouiKbn,
                                entity.SanteiKbn,
                                entity.InoutKbn,
                                entity.SyohoSbt,
                                entity.DaysCnt,
                                entity.SortNo,
                                //entity.IpnKasanMst, 
                                entity.Kasan1,
                                entity.Kasan2,
                                //entity.RaiinInf
                                entity.SinStartTime,
                                entity.MinYakka
                                //entity.Kbn,
                                //entity.JunSenpatu
                                ));

                    if(string.IsNullOrEmpty(results.Last().ItemCd) == false &&
                        results.Last().ItemCd.Length == 9 &&
                        results.Last().ItemCd.StartsWith("Z"))
                    {
                        // Z特材の場合、算定用項目コードのマスタからマスタ種別と点数識別を取得
                        var tenEntities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                            p.HpId == hpId &&
                            p.StartDate <= sinDate &&
                            (p.EndDate >= sinDate || p.EndDate == 12341234) &&
                            p.ItemCd == entity.SanteiItemCd)
                        .OrderBy(p => p.HpId)
                        .ThenBy(p => p.ItemCd)
                        .ThenByDescending(p => p.StartDate);

                        if(tenEntities.FirstOrDefault() != null)
                        {
                            results.Last().Z_MasterSbt = tenEntities.First().MasterSbt;
                            results.Last().Z_TenId = tenEntities.First().TenId;
                        }
                    }
                }
            }
            );

            return results;
        }
        public (List<OdrInfModel> , List<OdrDtlTenModel>) FindOdrInfDetailDatas(int hpId, long ptId, int sinDate, long excludeRaiinNo = 0)
        {
            const string conFncName = nameof(FindOdrInfDetailDatas);

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo != excludeRaiinNo &&
                (o.SanteiKbn == SanteiKbnConst.Santei || o.SanteiKbn == SanteiKbnConst.Jihi) &&
                o.IsDeleted == DeleteStatus.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId
                );
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.SinDate == sinDate &&
                r.RaiinNo != excludeRaiinNo &&
                r.Status >= RaiinState.Calculate &&    // 計算状態以上
                r.IsDeleted == DeleteTypes.None);

            //var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
            //    o.HpId == hpId &&
            //    o.PtId == ptId &&
            //    o.SinDate == sinDate &&
            //    o.IsDeleted == DeleteStatus.None);
            var odrInfDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo != excludeRaiinNo);

            var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                t.HpId == hpId &&
                t.StartDate <= sinDate &&
                (t.EndDate >= sinDate));
            var cmtKbnMsts = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(c =>
                c.HpId == hpId &&
                c.StartDate <= sinDate &&
                c.EndDate >= sinDate);
            //var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
            //    o.HpId == hpId);
            var ipnKasanMsts = _tenantDataContext.IpnKasanMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.EndDate >= sinDate &&
                i.IsDeleted == DeleteStatus.None);
            //var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
            //    r.HpId == hpId &&
            //    r.PtId == ptId &&
            //    r.SinDate == sinDate &&
            //    r.Status >= 5 &&    // 計算状態以上
            //    r.IsDeleted == DeleteStatus.None);

            var ipnMinYakkaMstsSub1 = _tenantDataContext.IpnMinYakkaMsts.FindListQueryableNoTrack(i =>
                i.HpId == hpId &&
                i.StartDate <= sinDate &&
                i.IsDeleted == DeleteStatus.None);
            var ipnMinYakkaMstsSub2 = (
                from ipnMinYakka in ipnMinYakkaMstsSub1
                group ipnMinYakka by new { hpId = ipnMinYakka.HpId, ipnNameCd = ipnMinYakka.IpnNameCd } into A
                select new { HpId = A.Key.hpId, IpnNameCd = A.Key.ipnNameCd, StartDate = A.Max(a => a.StartDate) }
                );
            var ipnMinYakkaMsts = (
                from ipnMinS1 in ipnMinYakkaMstsSub1
                join ipnMinS2 in ipnMinYakkaMstsSub2 on
                    new { ipnMinS1.HpId, ipnMinS1.IpnNameCd, ipnMinS1.StartDate } equals
                    new { ipnMinS2.HpId, ipnMinS2.IpnNameCd, ipnMinS2.StartDate }
                select new
                {
                    ipnMinS1.HpId,
                    ipnMinS1.IpnNameCd,
                    ipnMinS1.Yakka,
                    ipnMinS1.StartDate
                }
                );

            var joinQuery = (
                from odrInf in odrInfs.AsEnumerable()
                join odrInfDetail in odrInfDetails on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                join PtHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
                join raiinInf in raiinInfs on
                    new { odrInfDetail.HpId, odrInfDetail.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.RaiinNo }
                join tenMst in tenMsts on
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd } equals
                    new { tenMst.HpId, ItemCd = tenMst.ItemCd } into oJoin
                from oj in oJoin.DefaultIfEmpty()
                join cmtKbnMst in cmtKbnMsts on
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd } equals
                    new { cmtKbnMst.HpId, ItemCd = cmtKbnMst.ItemCd } into oJoin2
                from oj2 in oJoin2.DefaultIfEmpty()
                join ipnKasanMst in ipnKasanMsts on
                    //new { odrInfDetail.HpId, IpnNameCd=odrInfDetail.IpnCd } equals
                    new { oj.HpId, oj.IpnNameCd } equals
                    new { ipnKasanMst.HpId, ipnKasanMst.IpnNameCd } into oJoin3
                from oj3 in oJoin3.DefaultIfEmpty()
                join ipnMinYakkaMst in ipnMinYakkaMsts on
                    //new { odrInfDetail.HpId, IpnNameCd=odrInfDetail.IpnCd } equals
                    new { oj.HpId, oj.IpnNameCd } equals
                    new { ipnMinYakkaMst.HpId, ipnMinYakkaMst.IpnNameCd } into oJoin4
                from oj4 in oJoin4.DefaultIfEmpty()
                join ipnKasanMst in ipnKasanMsts on
                    new { odrInfDetail.HpId, IpnNameCd=odrInfDetail.IpnCd } equals
                    new { ipnKasanMst.HpId, ipnKasanMst.IpnNameCd } into oJoin5
                from oj5 in oJoin5.DefaultIfEmpty()
                join ipnMinYakkaMst in ipnMinYakkaMsts on
                    new { odrInfDetail.HpId, IpnNameCd=odrInfDetail.IpnCd } equals
                    new { ipnMinYakkaMst.HpId, ipnMinYakkaMst.IpnNameCd } into oJoin6
                from oj6 in oJoin6.DefaultIfEmpty()
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
                select new
                {
                    odrInfDetail,
                    //tenMst = oj2 == null ? oj: oj2,
                    tenMst = oj,
                    cmtKbnMst = oj2,
                    //PtHokenPattern,
                    hokenKbn = PtHokenPattern.HokenKbn,
                    hokenPid = PtHokenPattern.HokenPid,
                    hokenId = PtHokenPattern.HokenId,
                    hokenSbt = PtHokenPattern.HokenSbtCd,
                    //odrInf,
                    odrKouiKbn = odrInf.OdrKouiKbn,
                    santeiKbn = odrInf.SanteiKbn,
                    inoutKbn = odrInf.InoutKbn,
                    syohoSbt = odrInf.SyohoSbt,
                    daysCnt = odrInf.DaysCnt,
                    sortNo = odrInf.SortNo,
                    //raiinInf,
                    sinStartTime = raiinInf.SinStartTime,
                    //ipnKasanMst = oj3
                    kasan1 = (oj3 != null ? oj3.Kasan1 : (oj5 != null ? oj5.Kasan1 : 0)),
                    kasan2 = (oj3 != null ? oj3.Kasan2 : (oj5 != null ? oj5.Kasan2 : 0)),
                    minYakka = (oj4 != null ? oj4.Yakka : (oj6 != null ? oj6.Yakka : 0))
                    //kbn = (oj5 == null ? "" : oj5.Kbn),
                    //junSenpatu = (oj5 == null ? 0 : oj5.JunSenpatu)
                    ,odrInf, PtHokenPattern, raiinInf
                }
            ).ToList();

            List<OdrInfModel> retInfs = new List<OdrInfModel>();
            List<OdrDtlTenModel> retDtls = new List<OdrDtlTenModel>();

            long id = 0;

            if (joinQuery != null && joinQuery.Any())
            {
                joinQuery?.ForEach(data =>
                {

                    if (id != data.odrInf.Id)
                    {
                        id = data.odrInf.Id;

                        retInfs.Add(new OdrInfModel(data.odrInf, new PtHokenPatternModel(data.PtHokenPattern), new RaiinInfModel(data.raiinInf, null)));
                    }

                    var entity = new
                        OdrDtlTenModel(
                        data.odrInfDetail,
                        data.tenMst == null ? null : new TenMstModel(data.tenMst),
                        data.cmtKbnMst,
                        "",
                        //new TenMstModel(data.tenMst),
                        //data.PtHokenPattern,
                        data.hokenKbn,
                        data.hokenPid,
                        data.hokenId,
                        data.hokenSbt,
                        //data.odrInf,
                        data.odrKouiKbn,
                        data.santeiKbn,
                        data.inoutKbn,
                        data.syohoSbt,
                        data.daysCnt,
                        data.sortNo,
                        //data.ipnKasanMst,
                        data.kasan1,
                        data.kasan2,
                        //data.raiinInf
                        data.sinStartTime,
                        data.minYakka
                    //data.kbn,
                    //data.junSenpatu
                    );

                    //if ((entity.TenMst == null && entity.ItemCd != null && entity.ItemCd != "") ||
                    //    (entity.SanteiItemCd != entity.ItemCd && entity.ItemCd != null && entity.ItemCd != "" && entity.SanteiItemCd != "9999999999"))
                    if (entity.TenMst == null && string.IsNullOrEmpty(entity.ItemCd) == false && entity.ItemCd.StartsWith("KN")==false)
                    {
                        var tenMstStartDates = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                            t.HpId == hpId &&
                            t.StartDate <= sinDate &&
                            t.ItemCd == entity.ItemCd
                        ).GroupBy(p => new { p.HpId, p.ItemCd }).Select(p => new { p.Key.HpId, p.Key.ItemCd, StartDate = p.Max(q => q.StartDate) });

                        var tenMstBases = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                            t.HpId == hpId &&
                            t.StartDate <= sinDate &&
                            t.ItemCd == entity.ItemCd);

                        var tenEntities = (
                                from tenMst in tenMstBases
                                join tenStart in tenMstStartDates on
                                    new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                                    new { tenStart.HpId, tenStart.ItemCd, tenStart.StartDate }
                                join ipnKasanMst in ipnKasanMsts on
                                    new { tenMst.HpId, tenMst.IpnNameCd } equals
                                    new { ipnKasanMst.HpId, ipnKasanMst.IpnNameCd } into oJoin3
                                from oj3 in oJoin3.DefaultIfEmpty()
                                join ipnMinYakkaMst in ipnMinYakkaMsts on
                                    new { tenMst.HpId, tenMst.IpnNameCd } equals
                                    new { ipnMinYakkaMst.HpId, ipnMinYakkaMst.IpnNameCd } into oJoin4
                                from oj4 in oJoin4.DefaultIfEmpty()
                                select new
                                {
                                    tenMst,
                                    kasan1 = (oj3 != null ? oj3.Kasan1 : 0),
                                    kasan2 = (oj3 != null ? oj3.Kasan2 : 0),
                                    minYakka = (oj4 != null ? oj4.Yakka : 0)
                                }
                            ).OrderBy(p => p.tenMst.HpId)
                        .ThenBy(p => p.tenMst.ItemCd)
                        .ThenByDescending(p => p.tenMst.StartDate);

                        var cmtKbnMstStartDates = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(t=>
                            t.HpId == hpId &&
                            t.StartDate <= sinDate &&
                            t.ItemCd == entity.ItemCd
                        ).GroupBy(p => new { p.HpId, p.ItemCd }).Select(p => new { p.Key.HpId, p.Key.ItemCd, StartDate = p.Max(q => q.StartDate) });


                        var cmtKbnMstBases = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(t =>
                            t.HpId == hpId &&
                            t.StartDate <= sinDate &&
                            t.ItemCd == entity.ItemCd);

                        var cmtEntities = (
                                from cmtKbnMst in cmtKbnMstBases
                                join cmtStart in cmtKbnMstStartDates on
                                    new { cmtKbnMst.HpId, cmtKbnMst.ItemCd, cmtKbnMst.StartDate } equals
                                    new { cmtStart.HpId, cmtStart.ItemCd, cmtStart.StartDate }
                                select new
                                {
                                    cmtKbnMst
                                }
                            ).OrderBy(p => p.cmtKbnMst.HpId)
                        .ThenBy(p => p.cmtKbnMst.ItemCd)
                        .ThenByDescending(p => p.cmtKbnMst.StartDate);

                        //if (entity.TenMst == null || entity.TenMst != null && entity.TenMst.SanteigaiKbn != 1)
                        if (tenEntities == null || tenEntities.FirstOrDefault()?.tenMst == null || tenEntities.FirstOrDefault().tenMst.SanteigaiKbn != 1)
                        {
                            retDtls.Add(
                                new OdrDtlTenModel(
                                    entity.OdrInfDetail,
                                    tenEntities.FirstOrDefault() == null ? entity.TenMst : new TenMstModel(tenEntities.FirstOrDefault()?.tenMst ?? null),
                                    cmtEntities.FirstOrDefault() == null ? entity.CmtKbnMst : (cmtEntities.FirstOrDefault()?.cmtKbnMst ?? null),
                                    entity.ItemCd.StartsWith("IGE") || (entity.TenMst != null && entity.TenMst.SanteiItemCd == ItemCdConst.KensaIge) ?
                                        ((entity.TenMst != null && entity.TenMst.ReceName != "") ? entity.TenMst.ReceName.Trim() : entity.ItemName.Trim()) : "",
                                    //entity.PtHokenPattern, 
                                    entity.HokenKbn,
                                    entity.HokenPid,
                                    entity.HokenId,
                                    entity.HokenSbt,
                                    //entity.OdrInf, 
                                    entity.OdrKouiKbn,
                                    entity.SanteiKbn,
                                    entity.InoutKbn,
                                    entity.SyohoSbt,
                                    entity.DaysCnt,
                                    entity.SortNo,
                                    //entity.IpnKasanMst, 
                                    tenEntities.FirstOrDefault() == null ? entity.Kasan1 : tenEntities.FirstOrDefault()?.kasan1 ?? 0,
                                    tenEntities.FirstOrDefault() == null ? entity.Kasan2 : tenEntities.FirstOrDefault()?.kasan2 ?? 0,
                                    //entity.RaiinInf
                                    entity.SinStartTime,
                                    tenEntities.FirstOrDefault() == null ? entity.MinYakka : tenEntities.FirstOrDefault()?.minYakka ?? 0
                                    //entity.Kbn,
                                    //entity.JunSenpatu
                                    ));
                        }

                    } else
                    if ((entity.TenMst == null && string.IsNullOrEmpty(entity.ItemCd) == false) ||
                        (entity.SanteiItemCd != entity.ItemCd && string.IsNullOrEmpty(entity.ItemCd) == false && entity.SanteiItemCd != ItemCdConst.NoSantei && !entity.ItemCd.StartsWith("Z")))
                    {
                        // 点数マスタが取得できなくて、診療行為コードが空ではない　or　
                        // 診療行為コードと算定コードが異なり、診療行為コードが空ではなく、算定コードが算定しないコードではなく、Z項目ではない場合
                        // 算定項目コードを軸に取得する
                        var tenEntities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                            p.HpId == hpId &&
                            p.StartDate <= sinDate &&
                            (p.EndDate >= sinDate || p.EndDate == 12341234) &&
                            p.ItemCd == entity.SanteiItemCd)
                        .OrderBy(p => p.HpId)
                        .ThenBy(p => p.ItemCd)
                        .ThenByDescending(p => p.StartDate);

                        var cmtEntities = _tenantDataContext.CmtKbnMsts.FindListQueryableNoTrack(p =>
                            p.HpId == hpId &&
                            p.StartDate <= sinDate &&
                            p.EndDate >= sinDate &&
                            p.ItemCd == entity.SanteiItemCd)
                        .OrderBy(p => p.HpId)
                        .ThenBy(p => p.ItemCd)
                        .ThenByDescending(p => p.StartDate);

                        if (entity.TenMst == null || entity.TenMst != null && entity.TenMst.SanteigaiKbn != 1)
                        {
                            retDtls.Add(
                                new OdrDtlTenModel(
                                    entity.OdrInfDetail,
                                    tenEntities.FirstOrDefault() == null ? entity.TenMst : new TenMstModel(tenEntities.FirstOrDefault()),
                                    //cmtEntities.FirstOrDefault() == null ? entity.CmtKbnMst : cmtEntities.FirstOrDefault(),
                                    cmtEntities.FirstOrDefault(),
                                    entity.ItemCd.StartsWith("IGE") || (entity.TenMst != null && entity.TenMst.SanteiItemCd == ItemCdConst.KensaIge) ?
                                        (entity.TenMst.ReceName != "" ? entity.TenMst.ReceName.Trim() : entity.ItemName.Trim()) : "",
                                    //entity.PtHokenPattern, 
                                    entity.HokenKbn,
                                    entity.HokenPid,
                                    entity.HokenId,
                                    entity.HokenSbt,
                                    //entity.OdrInf, 
                                    entity.OdrKouiKbn,
                                    entity.SanteiKbn,
                                    entity.InoutKbn,
                                    entity.SyohoSbt,
                                    entity.DaysCnt,
                                    entity.SortNo,
                                    //entity.IpnKasanMst, 
                                    entity.Kasan1,
                                    entity.Kasan2,
                                    //entity.RaiinInf
                                    entity.SinStartTime,
                                    entity.MinYakka
                                    //entity.Kbn,
                                    //entity.JunSenpatu
                                    ));
                        }
                    }
                    else if ((entity.TenMst != null && entity.TenMst.SanteigaiKbn != 1) ||
                            (string.IsNullOrEmpty(entity.ItemCd))
                            )
                    {
                        retDtls.Add(
                                new OdrDtlTenModel(
                                    entity.OdrInfDetail,
                                    entity.TenMst,
                                    entity.CmtKbnMst,
                                    "",
                                    //entity.PtHokenPattern, 
                                    entity.HokenKbn,
                                    entity.HokenPid,
                                    entity.HokenId,
                                    entity.HokenSbt,
                                    //entity.OdrInf, 
                                    entity.OdrKouiKbn,
                                    entity.SanteiKbn,
                                    entity.InoutKbn,
                                    entity.SyohoSbt,
                                    entity.DaysCnt,
                                    entity.SortNo,
                                    //entity.IpnKasanMst, 
                                    entity.Kasan1,
                                    entity.Kasan2,
                                    //entity.RaiinInf
                                    entity.SinStartTime,
                                    entity.MinYakka
                                    //entity.Kbn,
                                    //entity.JunSenpatu
                                    ));

                        if (string.IsNullOrEmpty(retDtls.Last().ItemCd) == false &&
                            retDtls.Last().ItemCd.Length == 9 &&
                            retDtls.Last().ItemCd.StartsWith("Z"))
                        {
                            // Z特材の場合、算定用項目コードのマスタからマスタ種別と点数識別を取得
                            var tenEntities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                                p.HpId == hpId &&
                                p.StartDate <= sinDate &&
                                (p.EndDate >= sinDate || p.EndDate == 12341234) &&
                                p.ItemCd == entity.SanteiItemCd)
                            .OrderBy(p => p.HpId)
                            .ThenBy(p => p.ItemCd)
                            .ThenByDescending(p => p.StartDate);

                            if (tenEntities.FirstOrDefault() != null)
                            {
                                retDtls.Last().Z_MasterSbt = tenEntities.First().MasterSbt;
                                retDtls.Last().Z_TenId = tenEntities.First().TenId;
                            }
                        }
                    }

                });
            }

            return (retInfs, retDtls);
        }

        /// <summary>
        /// オーダーコメント情報
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>
        /// 指定の患者の指定の診療日のオーダーコメント情報
        /// </returns>
        public List<OdrInfCmtModel> FindOdrInfCmtData(int hpId, long ptId, int sinDate, long excludeRaiinNo = 0)
        {
            var odrInfCmts = _tenantDataContext.OdrInfCmts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo != excludeRaiinNo);
            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo != excludeRaiinNo &&
                (o.SanteiKbn == SanteiKbnConst.Santei || o.SanteiKbn == SanteiKbnConst.Jihi) &&
                o.IsDeleted == DeleteStatus.None);
            var joinQuery = (
                from odrInfCmt in odrInfCmts
                join odrInf in odrInfs on
                    new { odrInfCmt.HpId, odrInfCmt.RaiinNo, odrInfCmt.RpNo, odrInfCmt.RpEdaNo } equals
                    new { odrInf.HpId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.SinDate == sinDate &&
                    odrInf.IsDeleted == DeleteStatus.None
                orderby
                    odrInfCmt.RpNo, odrInfCmt.RpEdaNo, odrInfCmt.RowNo, odrInfCmt.EdaNo
                select new
                {
                    odrInfCmt
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new OdrInfCmtModel(data.odrInfCmt)
                )
                .ToList();

            List<OdrInfCmtModel> results = new List<OdrInfCmtModel>();

            entities?.ForEach(entity => {
                results.Add(new OdrInfCmtModel(entity.OdrInfCmt));
            });

            return results;
        }

        public bool CheckNaifuku5Syu(int hpId, long ptId, int sinDate, int excludeDate = 0)
        {
            const string conFncName = nameof(CheckNaifuku5Syu);

            int startDate = sinDate / 100 * 100 + 1;
            int endDate = sinDate;

            var odrInfDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.SinDate != excludeDate &&
                o.DrugKbn > 0);
            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.SinDate != excludeDate &&
                o.OdrKouiKbn == 21 &&
                o.SanteiKbn == SanteiKbnConst.Santei &&
                (o.SyohoSbt == 2 || (o.SyohoSbt == 0 && o.DaysCnt > _systemConfigProvider.GetSyohoRinjiDays())) &&
                o.IsDeleted == DeleteStatus.None);

            var joinTemp = (
                from odrInfDetail in odrInfDetails
                join odrInf in odrInfs on
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo } equals
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.SinDate >= startDate &&
                    odrInf.SinDate <= endDate &&
                    odrInf.OdrKouiKbn == 21 &&
                    odrInf.SanteiKbn == SanteiKbnConst.Santei &&
                    (odrInf.SyohoSbt == 2 || (odrInf.SyohoSbt == 0 && odrInf.DaysCnt > _systemConfigProvider.GetSyohoRinjiDays())) &&
                    odrInf.IsDeleted == DeleteStatus.None
                //group odrInfDetail by new { odrInfDetail.RaiinNo, odrInfDetail.ItemCd } into A
                //group odrInfDetail by new { odrInfDetail.RaiinNo } into A
                group odrInfDetail by new { odrInfDetail.RaiinNo, odrInfDetail.ItemCd } into A
                select new
                {
                    //sum = A.Sum(p => p.RaiinNo)
                    raiinNo = A.Key.RaiinNo,
                    //count = A.Count()
                    itemCd = A.Key.ItemCd
                }
            );

            var joinQuery = (
                from j in joinTemp
                group j by new {j.raiinNo} into A
                select new
                {
                    raiinNo = A.Key.raiinNo,
                    count = A.Count()
                }
                )
            .ToList();
            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            bool ret = false;
            if(joinQuery.Any(p=>p.count > 5))
            {
                ret = true;
            }
            return ret;
        }

        public bool CheckKouseisin(int hpId, long ptId, int sinDate, int excludeDate = 0)
        {
            const string conFncName = nameof(CheckKouseisin);

            int startDate = sinDate / 100 * 100 + 1;
            int endDate = sinDate;

            var odrInfDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.SinDate != excludeDate &&
                o.DrugKbn > 0);
            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.SinDate != excludeDate &&
                (o.OdrKouiKbn == 21 || o.OdrKouiKbn == 22 || o.OdrKouiKbn == 23) &&
                o.SanteiKbn == SanteiKbnConst.Santei &&
                //(o.SyohoSbt == 2 || (o.SyohoSbt == 0 && o.DaysCnt > _systemConfigProvider.GetSyohoRinjiDays())) &&
                o.IsDeleted == DeleteStatus.None);

            var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                t.HpId == hpId &&
                t.StartDate <= sinDate &&
                t.EndDate >= endDate &&
                (new int[] { 1, 2, 3, 4 }.Contains(t.KouseisinKbn)));

            var joinQuery = (
                from odrInfDetail in odrInfDetails
                join odrInf in odrInfs on
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo } equals
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                join tenMst in tenMsts on
                    new { odrInfDetail.HpId, odrInfDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd }
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.SinDate >= startDate &&
                    odrInf.SinDate <= endDate &&
                    odrInf.OdrKouiKbn == 21 &&
                    odrInf.SanteiKbn == SanteiKbnConst.Santei &&
                    !(new int[] { 3, 4 }.Contains(tenMst.KouseisinKbn) && (odrInf.SyohoSbt == 1 || (odrInf.SyohoSbt == 0 && odrInf.DaysCnt <= _systemConfigProvider.GetSyohoRinjiDays()))) &&
                    odrInf.IsDeleted == DeleteStatus.None //&&
                    //((tenMst.KouseisinKbn == 1 || tenMst.KouseisinKbn == 2) ||
                    //  ((tenMst.KouseisinKbn == 3 || tenMst.KouseisinKbn == 4) &&
                    //    (odrInf.SyohoSbt == 2 || (odrInf.SyohoSbt == 0 && odrInf.DaysCnt > _systemConfigProvider.GetSyohoRinjiDays()))
                    //  )
                    //)
                //group new { odrInfDetail, tenMst } by new { odrInfDetail.RaiinNo, ipnCd7 = CIUtil.Copy(tenMst.IpnNameCd, 1, 7) } into A
                group new { odrInfDetail, tenMst } by new { odrInfDetail.RaiinNo, ipnCd7 = 
                    (string.IsNullOrEmpty(tenMst.IpnNameCd) == false && tenMst.IpnNameCd.Length >= 7 ? tenMst.IpnNameCd.Substring(0, 7) : "") } into A
                select new
                {
                    //sum = A.Sum(p => p.odrInfDetail.RaiinNo)
                    raiinNo = A.Key.RaiinNo//,
                    //count = A.Count()
                }
                )
                .GroupBy(p => p.raiinNo).Select(p => new { p.Key, count = p.Count() })
                .ToList();

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            //return joinQuery.Any(p => p.count > 3);
            bool ret = false;
            
            if (joinQuery.Any(p => p.count > 3))
            {
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 指定日の属する月に院外処方のオーダーがあるかチェック
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        /// <param name="inout">
        /// 院内院外フラグ
        ///     0: 院内
        ///     1: 院外
        /// </param>
        /// <returns></returns>
        public bool InOutSyohoExists(int hpId, long ptId, int sinDate, int inout, int hokenSyu)
        {
            const string conFncName = nameof(InOutSyohoExists);
            int sinYm = sinDate / 100 * 100;

            List<int> hokenKbns = new List<int>();
            switch(hokenSyu)
            {
                case 0:
                    hokenKbns = new List<int> { 1, 2 };
                    break;
                case 1:
                    hokenKbns = new List<int> { 11, 12 };
                    break;
                case 2:
                    hokenKbns = new List<int> { 13 };
                    break;
                case 3:
                    hokenKbns = new List<int> { 14 };
                    break;
                default:
                    hokenKbns = new List<int> { 0 };
                    break;
            }

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                 o.HpId == hpId &&
                 o.PtId == ptId &&
                 o.SinDate >= sinYm + 1 &&
                 o.SinDate <= sinYm + 31 &&   // 厳密には31日のない月もあるが、当月チェックとしては31固定で構わない
                 (inout >= 0 ? o.InoutKbn == inout : true) &&
                 o.SanteiKbn == SanteiKbnConst.Santei &&
                 o.IsDeleted == DeleteStatus.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                hokenKbns.Contains(o.HokenKbn));
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.SinDate >= sinYm + 1 &&
                r.SinDate <= sinYm + 31 &&
                r.IsDeleted == DeleteTypes.None);   // 厳密には31日のない月もあるが、当月チェックとしては31固定で構わない);

            var joinQuery = (
                from odrInf in odrInfs
                join PtHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
                join RaiinInf in raiinInfs on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo } equals
                    new { RaiinInf.HpId, RaiinInf.PtId, RaiinInf.RaiinNo }
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.SinDate >= sinYm + 1 &&
                    odrInf.SinDate <= sinYm + 31 && // 厳密には31日のない月もあるが、当月チェックとしては31固定で構わない
                    (inout >= 0 ? odrInf.InoutKbn == inout : true) &&
                    odrInf.SanteiKbn == SanteiKbnConst.Santei && 
                    odrInf.OdrKouiKbn >= OdrKouiKbnConst.Naifuku &&
                    odrInf.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo &&
                    odrInf.IsDeleted == DeleteStatus.None
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInf.RpNo
                select new
                {
                    odrInf,
                    PtHokenPattern,
                    RaiinInf
                }
            );
            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new OdrInfModel(data.odrInf, new PtHokenPatternModel(data.PtHokenPattern), new RaiinInfModel(data.RaiinInf, null))
                )
                .ToList();

            List<OdrInfModel> results = new List<OdrInfModel>();

            entities?.ForEach(entity => {
                results.Add(new OdrInfModel(entity.OdrInf, entity.PtHokenPattern, entity.RaiinInf));
            });

            //return results.Any(p => p.HokenKbn == hokenKbn);
            return results.Any();
        }
        public bool InOutSyohoExistsHokenSyu(int hpId, long ptId, int sinDate, int inout, List<int> hokenSyus)
        {
            const string conFncName = nameof(InOutSyohoExists);
            int sinYm = sinDate / 100 * 100;

            List<int> hokenKbns = new List<int>();

            foreach (int hokenSyu in hokenSyus)
            {
                switch (hokenSyu)
                {
                    case 0:
                        hokenKbns.AddRange(new List<int> { 1, 2 });
                        break;
                    case 1:
                        hokenKbns.AddRange(new List<int> { 11, 12 });
                        break;
                    case 2:
                        hokenKbns.AddRange(new List<int> { 13 });
                        break;
                    case 3:
                        hokenKbns.AddRange(new List<int> { 14 });
                        break;
                    default:
                        hokenKbns.AddRange(new List<int> { 0 });
                        break;
                }
            }

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                 o.HpId == hpId &&
                 o.PtId == ptId &&
                 o.SinDate >= sinYm + 1 &&
                 o.SinDate <= sinYm + 31 &&   // 厳密には31日のない月もあるが、当月チェックとしては31固定で構わない
                 (inout >= 0 ? o.InoutKbn == inout : true) &&
                 o.SanteiKbn == SanteiKbnConst.Santei &&
                 o.IsDeleted == DeleteStatus.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                hokenKbns.Contains(o.HokenKbn));
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.SinDate >= sinYm + 1 &&
                r.SinDate <= sinYm + 31 &&
                r.IsDeleted == DeleteTypes.None);   // 厳密には31日のない月もあるが、当月チェックとしては31固定で構わない);

            var joinQuery = (
                from odrInf in odrInfs
                join PtHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
                join RaiinInf in raiinInfs on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo } equals
                    new { RaiinInf.HpId, RaiinInf.PtId, RaiinInf.RaiinNo }
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.SinDate >= sinYm + 1 &&
                    odrInf.SinDate <= sinYm + 31 && // 厳密には31日のない月もあるが、当月チェックとしては31固定で構わない
                    (inout >= 0 ? odrInf.InoutKbn == inout : true) &&
                    odrInf.SanteiKbn == SanteiKbnConst.Santei &&
                    odrInf.OdrKouiKbn >= OdrKouiKbnConst.Naifuku &&
                    odrInf.OdrKouiKbn <= OdrKouiKbnConst.Gaiyo &&
                    odrInf.IsDeleted == DeleteStatus.None
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInf.RpNo
                select new
                {
                    odrInf,
                    PtHokenPattern,
                    RaiinInf
                }
            );
            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new OdrInfModel(data.odrInf, new PtHokenPatternModel(data.PtHokenPattern), new RaiinInfModel(data.RaiinInf, null))
                )
                .ToList();

            List<OdrInfModel> results = new List<OdrInfModel>();

            entities?.ForEach(entity => {
                results.Add(new OdrInfModel(entity.OdrInf, entity.PtHokenPattern, entity.RaiinInf));
            });

            //return results.Any(p => p.HokenKbn == hokenKbn);
            return results.Any();
        }
        //public List<TodayHokenOdrInfModel> GetOdrInfs(long PtId, long raiinNo, int sinDate)
        //{
            //List<OdrInf> AllOdrInfs = _tenantDataContext.OdrInfs
            //    .FindListQueryable(odr => odr.PtId == PtId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10)
            //    .OrderBy(odr => odr.OdrKouiKbn)
            //    .ThenBy(odr => odr.RpNo)
            //    .ThenBy(odr => odr.RpEdaNo)
            //    .ThenBy(odr => odr.SortNo)
            //    .ToList();

            //List<OdrInfDetail> AllOdrInfDetails = _tenantDataContext.OdrInfDetails
            //    .FindListQueryable(odrDetail => odrDetail.PtId == PtId && odrDetail.RaiinNo == raiinNo && odrDetail.SinDate == sinDate)
            //    .OrderBy(odrDetail => odrDetail.RpNo)
            //    .ThenBy(odrDetail => odrDetail.RpEdaNo)
            //    .ThenBy(odrDetail => odrDetail.RowNo)
            //    .ToList();

            //// Find By Hoken
            //var hokenOdrInfs = AllOdrInfs
            //    .GroupBy(odr => odr.HokenPid)
            //    .Select(grp => grp.FirstOrDefault())
            //    .ToList();
            //List<TodayHokenOdrInfModel> result = new List<TodayHokenOdrInfModel>();

            //foreach (OdrInf hokenOdrInf in hokenOdrInfs)
            //{
            //    // Find By Group
            //    List<TodayGroupOdrInfModel> groupOdrInfModels = new List<TodayGroupOdrInfModel>();
            //    var groupOdrInfs = AllOdrInfs.Where(odr => odr.HokenPid == hokenOdrInf.HokenPid)
            //        .GroupBy(odr => new
            //        {
            //            odr.HokenPid,
            //            odr.GroupKoui,
            //            odr.InoutKbn,
            //            odr.SyohoSbt,
            //            odr.SikyuKbn,
            //            odr.TosekiKbn,
            //            odr.SanteiKbn
            //        })
            //        .Select(grp => grp.FirstOrDefault())
            //        .ToList();

            //    foreach (OdrInf groupOdrInf in groupOdrInfs)
            //    {
            //        // Find By RP
            //        List<TodayOdrInfModel> rpOdrInfModels = new List<TodayOdrInfModel>();
            //        var rpOdrInfs = AllOdrInfs.Where(odrInf => odrInf.HokenPid == hokenOdrInf.HokenPid
            //                                && odrInf.GroupKoui == groupOdrInf.GroupKoui
            //                                && odrInf.InoutKbn == groupOdrInf.InoutKbn
            //                                && odrInf.SyohoSbt == groupOdrInf.SyohoSbt
            //                                && odrInf.SikyuKbn == groupOdrInf.SikyuKbn
            //                                && odrInf.TosekiKbn == groupOdrInf.TosekiKbn
            //                                && odrInf.SanteiKbn == groupOdrInf.SanteiKbn)
            //                            .ToList();

            //        foreach (OdrInf rpOdrInf in rpOdrInfs)
            //        {
            //            // Find OdrInfDetail
            //            var odrInfDetails = AllOdrInfDetails
            //                .Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
            //                .ToList(); ;
            //            rpOdrInfModels.Add(new TodayOdrInfModel(rpOdrInf, odrInfDetails));
            //        }

            //        TodayGroupOdrInfModel groupOdrInfModel = new TodayGroupOdrInfModel(rpOdrInfModels);
            //        groupOdrInfModels.Add(groupOdrInfModel);
            //    }

            //    TodayHokenOdrInfModel todayHokenOdrInfModel = new TodayHokenOdrInfModel(groupOdrInfModels);

            //    result.Add(todayHokenOdrInfModel);
            //}

            //return result;
        //}
        
        public ReceptionModel GetRaiinInfByRaiinNo(long ptId, int sindate, long raiinNo)
        {
            RaiinInf raiinInf = null;
            try
            {
                if (raiinNo > 0)
                {
                    raiinInf = _tenantDataContext.RaiinInfs.FindListQueryable(p => p.HpId == 1 && p.PtId == ptId && p.SinDate == sindate
                                                                                    && p.RaiinNo == raiinNo && p.IsDeleted == DeleteTypes.None).FirstOrDefault();
                }
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, "GetRaiinInfByRaiinNo", E);
            }
            return new ReceptionModel(raiinInf ?? new RaiinInf() { PtId = ptId, SinDate = sindate });
        }

        /// <summary>
        /// 前回オーダー日を取得する
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        /// <param name="itemCd"></param>
        /// <returns></returns>
        public int GetZenkaiOdrDate(int hpId, long ptId, int sinDate, string itemCd)
        {
            const string conFncName = nameof(GetZenkaiOdrDate);

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate <= sinDate &&
                o.IsDeleted == DeleteStatus.None);
            var odrInfDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.ItemCd == itemCd &&
                o.SinDate <= sinDate);

            var joinQuery = (
                from odrInf in odrInfs
                join odrInfDetail in odrInfDetails on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                orderby
                    odrInf.SinDate descending
                select new
                {
                    odrInf.SinDate
                }
            );

            int result = 0;
            if(joinQuery != null && joinQuery.Any())
            {
                result = joinQuery.First().SinDate;
            }
            return result;
        }

        public List<int> GetOdrDays(int hpId, long ptId, int startDate, int endDate, List<string> itemCds, int hokenSyu, bool excludeSanteigai)
        {
            const string conFncName = nameof(GetOdrDays);
            List<int> hokenKbns = new List<int>();
            switch (hokenSyu)
            {
                case 0:
                    hokenKbns = new List<int> { 1, 2 };
                    break;
                case 1:
                    hokenKbns = new List<int> { 11, 12 };
                    break;
                case 2:
                    hokenKbns = new List<int> { 13 };
                    break;
                case 3:
                    hokenKbns = new List<int> { 14 };
                    break;
                default:
                    hokenKbns = new List<int> { 0 };
                    break;
            }

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                (excludeSanteigai ? o.SanteiKbn != SanteiKbnConst.SanteiGai : true) &&
                o.IsDeleted == DeleteStatus.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                hokenKbns.Contains(o.HokenKbn));

            var odrInfDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                itemCds.Contains(o.ItemCd) &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate);

            var joinQuery = (
                from odrInf in odrInfs
                join ptHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                join odrInfDetail in odrInfDetails on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                group odrInf by new { odrInf.SinDate } into A
                select new
                {
                    A.Key.SinDate
                }
            ).ToList();

            List<int> result = new List<int>();

            joinQuery?.ForEach(p=>
                {
                    result.Add(p.SinDate);
                }
                );

            return result;
        }
    }

}
