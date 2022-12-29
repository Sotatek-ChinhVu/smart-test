using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Ika.Constants;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;

namespace EmrCalculateApi.Ika.DB.Finder
{
    public class SanteiFinder
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public SanteiFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// SIN_RP_INFを取得する（指定日が属する月）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<SinRpInfModel> FindSinRpInfData(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;

            return DoFindSinRpInfData(
                _tenantDataContext.SinRpInfs.FindListQueryable(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm ).ToList()
                    );
                                    
            //List<SinRpInfModel> results = new List<SinRpInfModel>();

            //sinRpInfs?.ForEach(entity =>
            //{
            //    results.Add(new SinRpInfModel(entity));
            //});

            //return results;
        }
        public List<SinRpInfModel> FindSinRpInfDataNoTrack(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;

            return DoFindSinRpInfData(
                _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm).ToList()
                    );

            //List<SinRpInfModel> results = new List<SinRpInfModel>();

            //sinRpInfs?.ForEach(entity =>
            //{
            //    results.Add(new SinRpInfModel(entity));
            //});

            //return results;
        }
        public List<SinRpInfModel> FindSinRpInfDataNoTrack(int hpId, int sinYm)
        {            
            return DoFindSinRpInfData(
                _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.SinYm == sinYm).ToList()
                    );

            //List<SinRpInfModel> results = new List<SinRpInfModel>();

            //sinRpInfs?.ForEach(entity =>
            //{
            //    results.Add(new SinRpInfModel(entity));
            //});

            //return results;
        }
        public List<SinRpInfModel> DoFindSinRpInfData(List<SinRpInf> sinRpInfs)
        {
            //int sinYm = sinDate / 100;

            //var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(s =>
            //        s.HpId == hpId &&
            //        s.PtId == ptId &&
            //        s.SinYm == sinYm).ToList();

            List<SinRpInfModel> results = new List<SinRpInfModel>();

            sinRpInfs?.ForEach(entity =>
            {
                results.Add(new SinRpInfModel(entity));
            });

            return results;
        }

        private IQueryable<ReceInf> GetReceInfVar(int hpId, int seikyuYm, int mode, bool includeTester, List<int> seikyuKbns)
        {
            List<List<int>> toHokenKbn =
                new List<List<int>>
                {
                                new List<int>{ 1 },     // 社保
                                new List<int>{ 2 },     // 国保
                                new List<int>{ 11,12 },  // 労災
                                new List<int>{ 13 }  // アフターケア
                };

            List<int> hokenKbn = toHokenKbn[mode];

            List<int> isTester = null;
            if(includeTester)
            {
                isTester = new List<int> { 0, 1 };
            }
            else
            {
                isTester = new List<int> { 0 };
            }

            return
                _tenantDataContext.ReceInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.SeikyuYm == seikyuYm &&
                    seikyuKbns.Contains(p.SeikyuKbn) &&
                    hokenKbn.Contains(p.HokenKbn) &&
                    isTester.Contains(p.IsTester));
        }
        private IQueryable<KaikeiInf> GetKaikeiInfVar(int hpId)
        {
            return
                            _tenantDataContext.KaikeiInfs.FindListQueryableNoTrack(p =>
                                p.HpId == hpId
                                );


        }
        private IQueryable<RaiinInf> GetRaiinInfVar(int hpId, int tantoId, int kaId)
        {
            return
                _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    (tantoId > 0 ? p.TantoId == tantoId : true) &&
                    (kaId > 0 ? p.KaId == kaId : true) &&
                    p.Status >= 5 &&
                    p.IsDeleted == DeleteStatus.None
                );


        }
        public List<SinRpInfModel> FindSinRpInfDataForRece(int hpId, int seikyuYm, int mode, bool includeTester, List<int> seikyuKbns, int kaId, int tantoId)
        {

            var sinRps = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(s =>
                    s.HpId == hpId);

            int tantoIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptTantoIdTarget() == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptKaIdTarget() == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }
            //var receInfs = _tenantDataContext.ReceInfs.FindListQueryableNoTrack(r =>
            //    r.HpId == hpId &&
            //    r.SeikyuYm == seikyuYm);
            var receInfs = GetReceInfVar(hpId, seikyuYm, mode, includeTester, seikyuKbns);
            var kaikeiInfs = GetKaikeiInfVar(hpId);
            var raiinInfs = GetRaiinInfVar(hpId, tantoIdRaiin, kaIdRaiin);
            var kaikei_raiins = (
                    from kaikeiInf in kaikeiInfs
                    join raiinInf in raiinInfs on
                        new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                    select new
                    {
                        A.Key.HpId,
                        A.Key.PtId,
                        A.Key.SinYm,
                        A.Key.HokenId
                    }
                );

            var joinQuery = (
                from sinRp in sinRps
                where (
                    (
                        from receInf in receInfs
                        where
                            receInf.HpId == hpId &&
                            receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.HpId == sinRp.HpId &&
                            r.PtId == sinRp.PtId &&
                            r.SinYm == sinRp.SinYm
                    )
                )
                select new
                {
                    sinRp
                }
                
            );

            if(tantoIdRaiin > 0 || kaIdRaiin > 0)
            {
                // 担当医、診療科の指定があった場合は、kaikei_raiinを条件に含める
                // 含めると遅くなることがある
                joinQuery = (
                    from sinRp in sinRps
                    where (
                        (
                            from receInf in receInfs
                            join kaikei_raiin in kaikei_raiins on
                                new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                                new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                            where
                                receInf.HpId == hpId &&
                                receInf.SeikyuYm == seikyuYm
                            select receInf
                        ).Any(
                            r =>
                                r.HpId == sinRp.HpId &&
                                r.PtId == sinRp.PtId &&
                                r.SinYm == sinRp.SinYm
                        )
                    )
                    select new
                    {
                        sinRp
                    }
                );
            }

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new SinRpInfModel(
                        data.sinRp
                )
            )
            .ToList();

            return result;
        }

        /// <summary>
        /// SIN_KOUIを取得する（指定日が属する月）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<SinKouiModel> FindSinKouiData(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;

            return DoFindSinKouiData(
                _tenantDataContext.SinKouis.FindListQueryable(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm).ToList()
                    );

            //List<SinKouiModel> results = new List<SinKouiModel>();

            //sinKouis?.ForEach(entity =>
            //{
            //    results.Add(new SinKouiModel(entity));
            //});

            //return results;
        }
        public List<SinKouiModel> FindSinKouiDataNoTrack(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;

            return DoFindSinKouiData(
                _tenantDataContext.SinKouis.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm).ToList()
                    );

            //List<SinKouiModel> results = new List<SinKouiModel>();

            //sinKouis?.ForEach(entity =>
            //{
            //    results.Add(new SinKouiModel(entity));
            //});

            //return results;
        }
        public List<SinKouiModel> FindSinKouiDataNoTrack(int hpId, int sinYm)
        {
            return DoFindSinKouiData(
                _tenantDataContext.SinKouis.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.SinYm == sinYm).ToList()
                    );
        }

        public List<SinKouiModel> DoFindSinKouiData(List<SinKoui> sinKouis)
        {
            //int sinYm = sinDate / 100;

            //var sinKouis = _tenantDataContext.SinKouis.FindListQueryableNoTrack(s =>
            //        s.HpId == hpId &&
            //        s.PtId == ptId &&
            //        s.SinYm == sinYm).ToList();

            List<SinKouiModel> results = new List<SinKouiModel>();

            sinKouis?.ForEach(entity =>
            {
                results.Add(new SinKouiModel(entity));
            });

            return results;
        }

        public List<SinKouiModel> FindSinKouiDataForRece(int hpId, int seikyuYm, int mode, bool includeTester, List<int> seikyuKbns, int kaId, int tantoId)
        {

            var sinKouis = _tenantDataContext.SinKouis.FindListQueryableNoTrack(s =>
                    s.HpId == hpId);
            //var receInfs = _tenantDataContext.ReceInfs.FindListQueryableNoTrack(r =>
            //    r.HpId == hpId &&
            //    r.SeikyuYm == seikyuYm);

            int tantoIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptTantoIdTarget() == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptKaIdTarget() == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }

            var receInfs = GetReceInfVar(hpId, seikyuYm, mode, includeTester, seikyuKbns);
            var kaikeiInfs = GetKaikeiInfVar(hpId);
            var raiinInfs = GetRaiinInfVar(hpId, tantoIdRaiin, kaIdRaiin);
            var kaikei_raiins = (
                    from kaikeiInf in kaikeiInfs
                    join raiinInf in raiinInfs on
                        new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                    select new
                    {
                        A.Key.HpId,
                        A.Key.PtId,
                        A.Key.SinYm,
                        A.Key.HokenId
                    }
                );

            var joinQuery = (
                from sinKoui in sinKouis
                where (
                    (
                        from receInf in receInfs
                        where
                            receInf.HpId == hpId &&
                            receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.HpId == sinKoui.HpId &&
                            r.PtId == sinKoui.PtId &&
                            r.SinYm == sinKoui.SinYm
                    )
                )
                select new
                {
                    sinKoui
                }
            );

            if(tantoIdRaiin > 0 || kaIdRaiin > 0)
            {
                // 担当医、診療科の指定があった場合は、kaikei_raiinを条件に含める
                // 含めると遅くなることがある
                joinQuery = (
                    from sinKoui in sinKouis
                    where (
                        (
                            from receInf in receInfs
                            join kaikei_raiin in kaikei_raiins on
                                new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                                new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                            where
                                receInf.HpId == hpId &&
                                receInf.SeikyuYm == seikyuYm
                            select receInf
                        ).Any(
                            r =>
                                r.HpId == sinKoui.HpId &&
                                r.PtId == sinKoui.PtId &&
                                r.SinYm == sinKoui.SinYm
                        )
                    )
                    select new
                    {
                        sinKoui
                    }
                );
            }

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new SinKouiModel(
                        data.sinKoui
                )
            )
            .ToList();

            return result;
        }

        /// <summary>
        /// SIN_KOUI_DETAILを取得する（指定日が属する月）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<SinKouiDetailModel> FindSinKouiDetailData(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;
            return DoFindSinKouiDetailData(
                hpId, sinDate,
                _tenantDataContext.SinKouiDetails.FindListQueryable(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm)
                    );
            //var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
            //    t.HpId == hpId &&
            //    t.StartDate <= sinDate &&
            //    (t.EndDate >= sinDate || t.EndDate == 12341234));
            //var joinQuery = (
            //    from sinKouiDetail in sinKouiDetails
            //    join tenMst in tenMsts on
            //        new { sinKouiDetail.HpId, sinKouiDetail.ItemCd} equals
            //        new { tenMst.HpId, tenMst.ItemCd} into tm
            //    from a in tm.DefaultIfEmpty()
            //    select new
            //    {
            //        sinKouiDetail,
            //        tenMst = a
            //    }
            //    );

            //var entities = joinQuery.AsEnumerable().Select(
            //    data =>
            //        new SinKouiDetailModel(
            //            data.sinKouiDetail,
            //            data.tenMst
            //        )
            //    )
            //    .ToList();
            //List<SinKouiDetailModel> results = new List<SinKouiDetailModel>();

            //entities?.ForEach(entity =>
            //{
            //    results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst?.TenMst ?? null));
            //});

            //return results;
        }
        public List<SinKouiDetailModel> FindSinKouiDetailDataNoTrack(int hpId, long ptId, int sinDate, List<long> raiinNos)
        {
            int sinFrom = sinDate;
            int sinTo = sinDate;

            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
                r.HpId == hpId &&
                r.PtId == ptId &&
                r.SinDate >= sinDate / 100 * 100 + 1 &&
                r.SinDate <= sinDate / 100 * 100 + 31 &&
                raiinNos.Contains(r.RaiinNo) &&
                r.IsDeleted == DeleteStatus.None
            );

            if(raiinInfs != null && raiinInfs.Any())
            {
                sinFrom = raiinInfs.Min(r => r.SinDate);
                sinTo = raiinInfs.Max(r => r.SinDate);
            }

            int sinYm = sinDate / 100;
            return DoFindSinKouiDetailDataTenMstPeriod(
                hpId, sinDate,
                _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm),
                sinFrom,
                sinTo
            );
        }
        public List<SinKouiDetailModel> FindSinKouiDetailDataNoTrack(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;
            return DoFindSinKouiDetailData(
                hpId, sinDate,
                _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm)
            );
            //var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
            //    t.HpId == hpId &&
            //    t.StartDate <= sinDate &&
            //    (t.EndDate >= sinDate || t.EndDate == 12341234));
            //var joinQuery = (
            //    from sinKouiDetail in sinKouiDetails
            //    join tenMst in tenMsts on
            //        new { sinKouiDetail.HpId, sinKouiDetail.ItemCd } equals
            //        new { tenMst.HpId, tenMst.ItemCd } into tm
            //    from a in tm.DefaultIfEmpty()
            //    select new
            //    {
            //        sinKouiDetail,
            //        tenMst = a
            //    }
            //    );

            //var entities = joinQuery.AsEnumerable().Select(
            //    data =>
            //        new SinKouiDetailModel(
            //            data.sinKouiDetail,
            //            data.tenMst
            //        )
            //    )
            //    .ToList();
            //List<SinKouiDetailModel> results = new List<SinKouiDetailModel>();

            //entities?.ForEach(entity =>
            //{
            //    results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst?.TenMst ?? null));
            //});

            //return results;
        }
        //public List<SinKouiDetailModel> FindSinKouiDetailDataNoTrack(int hpId, int sinYm)
        //{
        //    return DoFindSinKouiDetailData(
        //        hpId, CIUtil.GetLastDateOfMonth(sinYm * 100 + 1),
        //        _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
        //            s.HpId == hpId &&
        //            s.SinYm == sinYm)
        //    );
        //}

        /// <summary>
        /// 診療行為詳細データを取得する
        /// ※TEN_MSTの取得について、もしかすると、SIN_KOUI_COUNTと結合し、正確に診療日から抽出する必要があるかもしれない
        /// 　ただ、2020/07/23時点で、月中で変わるような設定を参照しているところはないし、速度面で不安があるので、指定日のみを頼りに処理することにする
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="sinDate"></param>
        /// <param name="sinKouiDetails"></param>
        /// <returns></returns>
        public List<SinKouiDetailModel> DoFindSinKouiDetailData(int hpId, int sinDate, IQueryable<SinKouiDetail> sinKouiDetails)
        {
            //int sinYm = sinDate / 100;
            //var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
            //        s.HpId == hpId &&
            //        s.PtId == ptId &&
            //        s.SinYm == sinYm);
            //var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
            //    t.HpId == hpId &&
            //    t.StartDate <= sinDate &&
            //    (t.EndDate >= sinDate || t.EndDate == 12341234));

            // マスタ上に重複がある場合の対策を行う場合（基本的に期間重複がないようにマスタ側で対応するが、どうしても対応できない事態に備えた対応方法をメモとして残す）
            //var tenMstsBase = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
            //    t.HpId == hpId &&
            //    t.StartDate <= sinDate &&
            //    (t.EndDate >= sinDate || t.EndDate == 12341234));

            //var tenMstsMax = (
            //        from tenMst in tenMstsBase
            //        where tenMst.StartDate < sinDate && (tenMst.EndDate >= sinDate || tenMst.EndDate == 12341234)
            //        group tenMst by new { tenMst.HpId, tenMst.ItemCd } into A
            //        select new { HpId = A.Key.HpId, ItemCd = A.Key.ItemCd, StartDate = A.Max(a => a.StartDate) }
            //    );
            //var tenMsts = (
            //        from tenMst in tenMstsBase
            //        join tenMstMax in tenMstsMax on
            //            new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
            //            new { tenMstMax.HpId, tenMstMax.ItemCd, tenMstMax.StartDate }
            //        select new
            //        {
            //            tenMst
            //        }
            //    );

            // ===================================================================================================

            // 診療日時点で有効な点数マスタを取得する
            //var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
            var tenMsts = _tenantDataContext.TenMsts.FindListQueryable(t =>
                t.HpId == hpId &&
                t.StartDate <= sinDate &&
                (t.EndDate >= sinDate));

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join tenMst in tenMsts on
                    new { sinKouiDetail.HpId, sinKouiDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd } into tm
                from a in tm.DefaultIfEmpty()
                select new
                {
                    sinKouiDetail,
                    tenMst = a
                }
                );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new SinKouiDetailModel(
                        data.sinKouiDetail,
                        data.tenMst
                    )
                )
                .ToList();
            List<SinKouiDetailModel> results = new List<SinKouiDetailModel>();

            entities?.ForEach(entity =>
            {
                if (entity.TenMst == null && entity.OdrItemCd.StartsWith("Z"))
                {
                    //var odrTenMst = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                    //    t.HpId == hpId &&
                    //    t.StartDate <= sinDate &&
                    //    (t.EndDate >= sinDate || t.EndDate == 12341234) &&
                    //    t.ItemCd == entity.OdrItemCd)
                    //.ToList();

                    // 点数マスタを取得できなかった特材については、指定日に最も近い点数マスタを取得しておく
                    // ※もしかすると、SIN_KOUI_COUNTと結合し、正確に診療日から抽出する必要があるかもしれない
                    // 　ただ、2020/07/23時点で、月中で変わるような設定を参照しているところはないし、速度面で不安があるので、指定日のみを頼りに処理することにする

                    int stdDate = sinDate;
                    var tenMstsBase = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                        t.HpId == hpId &&
                        t.StartDate <= stdDate &&
                        t.ItemCd == entity.SinKouiDetail.OdrItemCd);

                    var tenMstsMax = (
                            from tenMst in tenMstsBase
                            where tenMst.StartDate < stdDate
                            group tenMst by new { tenMst.HpId, tenMst.ItemCd } into A
                            select new { HpId = A.Key.HpId, ItemCd = A.Key.ItemCd, StartDate = A.Max(a => a.StartDate) }
                        );
                    var odrTenMst = (
                            from tenMst in tenMstsBase
                            join tenMstMax in tenMstsMax on
                                new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                                new { tenMstMax.HpId, tenMstMax.ItemCd, tenMstMax.StartDate }
                            select new
                            {
                                tenMst
                            }
                        ).OrderByDescending(p => p.tenMst.StartDate).ToList();
                    if (odrTenMst != null && odrTenMst.Any())
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, odrTenMst.First().tenMst));
                    }
                    else
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst?.TenMst ?? null));
                    }
                }
                else
                {
                    results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst?.TenMst ?? null));
                }
            });

            return results;
        }
        public List<SinKouiDetailModel> DoFindSinKouiDetailDataTenMstPeriod(int hpId, int sinDate, IQueryable<SinKouiDetail> sinKouiDetails, int sinFrom = 0, int sinTo = 0)
        {
            int sinDateFrom = sinDate / 100 * 100 + 1;
            int sinDateTo = sinDate / 100 * 100 + 31;

            if (sinFrom > 0)
            {
                sinDateFrom = sinFrom;
            }
            if (sinTo > 0)
            {
                sinDateTo = sinTo;
            }

            // 診療月時点で有効な点数マスタを取得する
            var tenMaxs = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                t.HpId == hpId &&
                t.StartDate <= (sinDateTo) &&
                (t.EndDate >= (sinDateFrom)))
                .GroupBy(p => p.ItemCd)
                .Select(p => new { ItemCd = p.Key, StartDate = p.Max(q => q.StartDate) });

            var tenBases = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                t.HpId == hpId &&
                t.StartDate <= (sinDateTo) &&
                (t.EndDate >= (sinDateFrom)));

            var tenMsts = (

                from tenBase in tenBases
                join tenMax in tenMaxs on
                    new { tenBase.ItemCd, tenBase.StartDate } equals
                    new { tenMax.ItemCd, tenMax.StartDate }
                select new
                {
                    tenMst = tenBase
                }
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join tenMst in tenMsts on
                    new { sinKouiDetail.HpId, sinKouiDetail.ItemCd } equals
                    new { tenMst.tenMst.HpId, tenMst.tenMst.ItemCd } into tm
                from a in tm.DefaultIfEmpty()
                select new
                {
                    sinKouiDetail,
                    tenMst = a
                }
                );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new SinKouiDetailModel(
                        data.sinKouiDetail,
                        (data.tenMst != null ? data.tenMst.tenMst : null)
                    )
                )
                .ToList();
            List<SinKouiDetailModel> results = new List<SinKouiDetailModel>();

            entities?.ForEach(entity =>
            {
                if (entity.TenMst == null && entity.OdrItemCd.StartsWith("Z"))
                {
                    int stdDate = sinDate;
                    var tenMstsBase = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                        t.HpId == hpId &&
                        t.StartDate <= stdDate &&
                        t.ItemCd == entity.SinKouiDetail.OdrItemCd);

                    var tenMstsMax = (
                            from tenMst in tenMstsBase
                            where tenMst.StartDate < stdDate
                            group tenMst by new { tenMst.HpId, tenMst.ItemCd } into A
                            select new { HpId = A.Key.HpId, ItemCd = A.Key.ItemCd, StartDate = A.Max(a => a.StartDate) }
                        );
                    var odrTenMst = (
                            from tenMst in tenMstsBase
                            join tenMstMax in tenMstsMax on
                                new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                                new { tenMstMax.HpId, tenMstMax.ItemCd, tenMstMax.StartDate }
                            select new
                            {
                                tenMst
                            }
                        ).OrderByDescending(p => p.tenMst.StartDate).ToList();
                    if (odrTenMst != null && odrTenMst.Any())
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, odrTenMst.First().tenMst));
                    }
                    else
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst?.TenMst ?? null));
                    }
                }
                else
                {
                    results.Add(new SinKouiDetailModel(entity.SinKouiDetail, entity.TenMst?.TenMst ?? null));
                }
            });

            return results;
        }
        private (int, int) GetReceInfTerm(int hpId, int seikyuYm)
        {
            int min = 0;
            int max = 9999999;

            var receInfs = _tenantDataContext.ReceInfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.SeikyuYm == seikyuYm
            );

            var receInfTerm = (
                    from receInf in receInfs
                    select new
                    {
                        minDate = receInfs.Min(p => p.SinYm),
                        maxDate = receInfs.Max(p => p.SinYm)
                    }
                );

            if(receInfTerm != null && receInfTerm.Any())
            {
                min = receInfTerm.First().minDate;
                max = receInfTerm.First().maxDate;
            }

            return (min, max);
        }

        public List<SinKouiDetailModel> FindSinKouiDetailDataForRece(int hpId, int seikyuYm, int mode, bool includeTester, List<int> seikyuKbns, int kaId, int tantoId)
        {
            //(int min, int max) = GetReceInfTerm(hpId, seikyuYm);

            int tantoIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptTantoIdTarget() == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptKaIdTarget() == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }

            //var receInfs = _tenantDataContext.ReceInfs.FindListQueryableNoTrack(r =>
            //    r.HpId == hpId &&
            //    r.SeikyuYm == seikyuYm);
            var receInfs = GetReceInfVar(hpId, seikyuYm, mode, includeTester, seikyuKbns);

            // ここでフィルタをかけると遅くなるのでかけない
            //var kaikeiInfs = GetKaikeiInfVar(hpId);
            //var raiinInfs = GetRaiinInfVar(hpId, tantoIdRaiin, kaIdRaiin);
            //var kaikei_raiins = (
            //        from kaikeiInf in kaikeiInfs
            //        join raiinInf in raiinInfs on
            //            new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
            //            new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
            //        group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
            //        select new
            //        {
            //            A.Key.HpId,
            //            A.Key.PtId,
            //            A.Key.SinYm,
            //            A.Key.HokenId
            //        }
            //    );

            //var sinDtls = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
            //        s.HpId == hpId &&
            //        s.SinYm >= min &&
            //        s.SinYm <= max);

            var sinDtlsA = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(s =>
                s.HpId == hpId
            );
            var sinDtls = (
                    from sinDtl in sinDtlsA
                    where
                    (
                        from receInf in receInfs
                        where
                            receInf.HpId == hpId &&
                            receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.HpId == sinDtl.HpId &&
                            r.PtId == sinDtl.PtId &&
                            r.SinYm == sinDtl.SinYm
                    )
                    select new
                    {
                        sinDtl
                    }
                );

            //var sinCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(s =>
            //        s.HpId == hpId &&
            //        s.SinYm >= min &&
            //        s.SinYm <= max);
            var sinCountsA = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(s =>
                    s.HpId == hpId
                );

            var sinCounts = (
                    from sinCount in sinCountsA
                    where
                    (
                        from receInf in receInfs
                        //join kaikei_raiin in kaikei_raiins on
                        //    new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                        //    new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                        where
                            receInf.HpId == hpId &&
                            receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.HpId == sinCount.HpId &&
                            r.PtId == sinCount.PtId &&
                            r.SinYm == sinCount.SinYm
                    )
                    select new
                    {
                        sinCount
                    }
                );

            var sinCountMaxs = (
                    from sinCount in sinCounts
                    group new { sinCount } by new { sinCount.sinCount.HpId, sinCount.sinCount.PtId, sinCount.sinCount.SinYm, sinCount.sinCount.RpNo, sinCount.sinCount.SeqNo } into A
                    select new
                    {
                        HpId = A.Key.HpId,
                        PtId = A.Key.PtId,
                        SinYm = A.Key.SinYm,
                        RpNo = A.Key.RpNo,
                        SeqNo = A.Key.SeqNo,
                        LastDate = A.Max(a => a.sinCount.sinCount.SinDate)
                    }
                );

            //var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
            //    t.HpId == hpId);

            // 当該項目の最終算定日時点で有効な点数マスタを取得する
            var tenMstAs = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                t.HpId == hpId);
            var tenMsts = (
                from sinDtl in sinDtls
                join sinCount in sinCountMaxs on
                    new { sinDtl.sinDtl.HpId, sinDtl.sinDtl.PtId, sinDtl.sinDtl.SinYm, sinDtl.sinDtl.RpNo, sinDtl.sinDtl.SeqNo } equals
                    new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } into sc
                from b in sc.DefaultIfEmpty()
                join tenMst in tenMstAs on
                    new { sinDtl.sinDtl.HpId, sinDtl.sinDtl.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd } into tm
                from a in tm.DefaultIfEmpty()
                where (
                    /*(                        
                        a.StartDate <= (b == null ? sinDtl.SinYm * 100 + 28 : b.LastDate) &&
                        (a.EndDate >= (b == null ? sinDtl.SinYm * 100 + 28 : b.LastDate) || a.EndDate == 12341234)
                    )
                    &&
                    */
                    (
                        a.StartDate <= sinDtl.sinDtl.SinYm * 100 + 31 &&
                        (a.EndDate >= sinDtl.sinDtl.SinYm * 100 + 1 || a.EndDate == 12341234)
                    )
                    &&
                    (
                        from receInf in receInfs
                        where
                            receInf.HpId == hpId &&
                            receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.HpId == sinDtl.sinDtl.HpId &&
                            r.PtId == sinDtl.sinDtl.PtId &&
                            r.SinYm == sinDtl.sinDtl.SinYm &&
                            (a != null ? r.SinYm * 100 + 1 <= a.EndDate : true) &&
                            (a != null ? r.SinYm * 100 + 31 >= a.StartDate : true)
                    )
                )
                select new
                {
                    HpId = sinDtl.sinDtl.HpId,
                    PtId = sinDtl.sinDtl.PtId,
                    SinYm = sinDtl.sinDtl.SinYm,
                    RpNo = sinDtl.sinDtl.RpNo,
                    SeqNo = sinDtl.sinDtl.SeqNo,
                    RowNo = sinDtl.sinDtl.RowNo,
                    TenMst = a
                }
            );

            var joinQuery = (
                from sinDtl in sinDtls
                join sinCount in sinCountMaxs on
                    new { sinDtl.sinDtl.HpId, sinDtl.sinDtl.PtId, sinDtl.sinDtl.RpNo, sinDtl.sinDtl.SeqNo } equals
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } into sc
                from b in sc.DefaultIfEmpty()
                join tenMst in tenMsts on
                    new { sinDtl.sinDtl.HpId, sinDtl.sinDtl.PtId, sinDtl.sinDtl.SinYm, sinDtl.sinDtl.RpNo, sinDtl.sinDtl.SeqNo, sinDtl.sinDtl.RowNo } equals
                    new { tenMst.HpId, tenMst.PtId, tenMst.SinYm, tenMst.RpNo, tenMst.SeqNo, tenMst.RowNo } into tm
                from a in tm.DefaultIfEmpty()
                where (
                        (
                            a.TenMst.StartDate <= (b == null ? sinDtl.sinDtl.SinYm * 100 + 28 : b.LastDate) &&
                            (a.TenMst.EndDate >= (b == null ? sinDtl.sinDtl.SinYm * 100 + 28 : b.LastDate) || a.TenMst.EndDate == 12341234)
                        )
                        &&
                    (
                        from receInf in receInfs
                        where
                            receInf.HpId == hpId &&
                            receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.HpId == sinDtl.sinDtl.HpId &&
                            r.PtId == sinDtl.sinDtl.PtId &&
                            r.SinYm == sinDtl.sinDtl.SinYm
                    )
                )
                select new
                {
                    SinKouiDetail = sinDtl,
                    SinKouiCount = b,
                    TenMst = a.TenMst
                }

            ).ToList();

            List<SinKouiDetailModel> results = new List<SinKouiDetailModel>();

            joinQuery.ForEach(entity =>
            {
                //if (entity.TenMst == null && entity.SinKouiDetail.OdrItemCd.StartsWith("Z"))
                if (entity.TenMst == null || (string.IsNullOrEmpty(entity.SinKouiDetail.sinDtl.OdrItemCd) == false && entity.SinKouiDetail.sinDtl.OdrItemCd.StartsWith("Z")))
                {
                    // 点数マスタが取得できなかった or 特材の場合（特材は、電算データの作成に必要な情報があって、オーダー診療行為コード基準で取得する必要がある）
                        
                    // 診療日に最も近い点数マスタを取得
                    int stdDate = (entity.SinKouiCount == null ? entity.SinKouiDetail.sinDtl.SinYm * 100 + 1 : entity.SinKouiCount.LastDate);
                    var tenMstsBase = _tenantDataContext.TenMsts.FindListQueryableNoTrack(t =>
                        t.HpId == hpId &&
                        t.StartDate <= stdDate &&
                        t.ItemCd == entity.SinKouiDetail.sinDtl.OdrItemCd);

                    var tenMstsMax = (
                            from tenMst in tenMstsBase
                            where tenMst.StartDate < stdDate
                            group tenMst by new { tenMst.HpId, tenMst.ItemCd } into A
                            select new { HpId = A.Key.HpId, ItemCd = A.Key.ItemCd, StartDate = A.Max(a => a.StartDate) }
                        );
                    var odrTenMst = (
                            from tenMst in tenMstsBase
                            join tenMstMax in tenMstsMax on
                                new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                                new { tenMstMax.HpId, tenMstMax.ItemCd, tenMstMax.StartDate }
                            select new
                            {
                                tenMst
                            }
                        ).OrderByDescending(p=>p.tenMst.StartDate).ToList();
                    
                    if (odrTenMst != null && odrTenMst.Any())
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail.sinDtl, odrTenMst.First().tenMst));
                    }
                    else
                    {
                        results.Add(new SinKouiDetailModel(entity.SinKouiDetail.sinDtl, entity.TenMst?? null));
                    }

                    if((string.IsNullOrEmpty(entity.SinKouiDetail.sinDtl.OdrItemCd) == false && entity.SinKouiDetail.sinDtl.OdrItemCd.StartsWith("Z")))
                    {
                        // Z特材の場合、算定用項目コードのマスタからマスタ種別と点数識別を取得
                        var tenEntities = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                            p.HpId == hpId &&
                            p.StartDate <= stdDate &&
                            (p.EndDate >= stdDate || p.EndDate == 12341234) &&
                            p.ItemCd == entity.SinKouiDetail.sinDtl.ItemCd)
                        .OrderBy(p => p.HpId)
                        .ThenBy(p => p.ItemCd)
                        .ThenByDescending(p => p.StartDate);

                        if (tenEntities.FirstOrDefault() != null)
                        {                                
                            results.Last().Z_TenId = tenEntities.First().TenId;
                        }
                    }
                }
                else
                {
                    results.Add(new SinKouiDetailModel(entity.SinKouiDetail.sinDtl, entity.TenMst ?? null));
                }
            });

            return results;
        }

        /// <summary>
        /// SIN_KOUI_COUNT取得（指定日が属する月）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>SIN_KOUI_COUNTのリスト</returns>
        public List<SinKouiCountModel> FindSinKouiCountData(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;

            return DoFindSinKouiCountData(
                _tenantDataContext.SinKouiCounts.FindListQueryable(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm).ToList()
                    );

            //List<SinKouiCountModel> results = new List<SinKouiCountModel>();

            //sinKouiCounts?.ForEach(entity =>
            //{
            //    results.Add(new SinKouiCountModel(entity));
            //});

            //return results;
        }
        public List<SinKouiCountModel> FindSinKouiCountDataNoTrack(int hpId, long ptId, int sinDate)
        {
            int sinYm = sinDate / 100;

            return DoFindSinKouiCountData(
                _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.SinYm == sinYm).ToList()
                    );

            //List<SinKouiCountModel> results = new List<SinKouiCountModel>();

            //sinKouiCounts?.ForEach(entity =>
            //{
            //    results.Add(new SinKouiCountModel(entity));
            //});

            //return results;
        }
        public List<SinKouiCountModel> FindSinKouiCountDataNoTrack(int hpId, int sinYm)
        {

            return DoFindSinKouiCountData(
                _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.SinYm == sinYm).ToList()
                    );
        }

        public List<SinKouiCountModel> DoFindSinKouiCountData(List<SinKouiCount> sinKouiCounts)
        {
            //int sinYm = sinDate / 100;

            //var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(s =>
            //        s.HpId == hpId &&
            //        s.PtId == ptId &&
            //        s.SinYm == sinYm).ToList();

            List<SinKouiCountModel> results = new List<SinKouiCountModel>();

            sinKouiCounts?.ForEach(entity =>
            {
                results.Add(new SinKouiCountModel(entity));
            });

            return results;
        }
        public List<SinKouiCountModel> FindSinKouiCountDataForRece(int hpId, int seikyuYm, int mode, bool includeTester, List<int> seikyuKbns, int kaId, int tantoId)
        {

            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(s =>
                    s.HpId == hpId);
            //var receInfs = _tenantDataContext.ReceInfs.FindListQueryableNoTrack(r =>
            //    r.HpId == hpId &&
            //    r.SeikyuYm == seikyuYm);

            int tantoIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptTantoIdTarget() == 1)
            {
                tantoIdRaiin = tantoId;
                tantoId = 0;
            }

            int kaIdRaiin = 0;
            if (_systemConfigProvider.GetReceiptKaIdTarget() == 1)
            {
                kaIdRaiin = kaId;
                kaId = 0;
            }

            var receInfs = GetReceInfVar(hpId, seikyuYm, mode, includeTester, seikyuKbns);
            var kaikeiInfs = GetKaikeiInfVar(hpId);
            var raiinInfs = GetRaiinInfVar(hpId, tantoIdRaiin, kaIdRaiin);
            var kaikei_raiins = (
                    from kaikeiInf in kaikeiInfs
                    join raiinInf in raiinInfs on
                        new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                    group kaikeiInf by new { kaikeiInf.HpId, kaikeiInf.PtId, SinYm = kaikeiInf.SinDate / 100, kaikeiInf.HokenId } into A
                    select new
                    {
                        A.Key.HpId,
                        A.Key.PtId,
                        A.Key.SinYm,
                        A.Key.HokenId
                    }
                );

            var joinQuery = (
                from sinKouiCount in sinKouiCounts
                where (
                    (
                        from receInf in receInfs
                        where
                            receInf.HpId == hpId &&
                            receInf.SeikyuYm == seikyuYm
                        select receInf
                    ).Any(
                        r =>
                            r.HpId == sinKouiCount.HpId &&
                            r.PtId == sinKouiCount.PtId &&
                            r.SinYm == sinKouiCount.SinYm
                    )
                )
                select new
                {
                    sinKouiCount
                }
            );

            if(tantoIdRaiin > 0 || kaIdRaiin > 0)
            {
                // 担当医、診療科の指定があった場合は、kaikei_raiinを条件に含める
                // 含めると遅くなることがある
                joinQuery = (
                    from sinKouiCount in sinKouiCounts
                    where (
                        (
                            from receInf in receInfs
                            join kaikei_raiin in kaikei_raiins on
                                new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                                new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                            where
                                receInf.HpId == hpId &&
                                receInf.SeikyuYm == seikyuYm
                            select receInf
                        ).Any(
                            r =>
                                r.HpId == sinKouiCount.HpId &&
                                r.PtId == sinKouiCount.PtId &&
                                r.SinYm == sinKouiCount.SinYm
                        )
                    )
                    select new
                    {
                        sinKouiCount
                    }
                );
            }

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new SinKouiCountModel(
                        data.sinKouiCount
                )
            )
            .ToList();

            return result;
        }
        /// <summary>
        /// SIN_RP_NO_INF取得（指定日が属する月）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>SIN_RP_NO_INFのリスト</returns>
        //public List<SinRpNoInfModel> FindSinRpNoInfData(int hpId, long ptId, int sinDate)
        //{
        //    int sinYm = sinDate / 100;

        //    var sinRpNoInfs = _tenantDataContext.SinRpNoInfs.FindListQueryable(s =>
        //            s.HpId == hpId &&
        //            s.PtId == ptId &&
        //            s.SinYm == sinYm).ToList();

        //    List<SinRpNoInfModel> results = new List<SinRpNoInfModel>();

        //    sinRpNoInfs?.ForEach(entity =>
        //    {
        //        results.Add(new SinRpNoInfModel(entity));
        //    });

        //    return results;
        //}
        /// <summary>
        /// 指定の日に指定の項目が算定されているかチェックする
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">チェックする日</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCd">チェックする項目</param>
        /// <returns>ture: 算定されている</returns>
        public bool CheckSanteiDay(int hpId, long ptId, int sinDate, long raiinNo, string itemCd, int hokenKbn)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            return CheckSanteiDay(hpId, ptId, sinDate, raiinNo, itemCds, hokenKbn);
        }

        /// <summary>
        /// 指定の日に指定の項目が算定されているかチェックする
        /// ※複数項目用
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">チェックする日</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCds">チェックする項目のリスト</param>
        /// <returns>ture: 算定されている</returns>
        public bool CheckSanteiDay(int hpId, long ptId, int sinDate, long raiinNo, List<string> itemCds, int hokenKbn)
        {
            return CheckSanteiTerm(hpId, ptId, sinDate, sinDate, sinDate, raiinNo, itemCds, hokenKbn);
        }

        /// <summary>
        /// 指定の期間に指定の項目が算定されているかチェックする
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <param name="sinDate">診療日（除外する日）</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCd">チェックする項目</param>
        /// <returns></returns>
        public bool CheckSanteiTerm(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, string itemCd, int hokenKbn)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            return CheckSanteiTerm(hpId, ptId, startDate, endDate, sinDate, raiinNo, itemCds, hokenKbn);
        }

        /// <summary>
        /// 指定の期間に指定の項目が算定されているかチェックする
        /// ※複数項目用
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <param name="sinDate">診療日（除外する日）</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCds">チェックする項目のリスト</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>true: 算定あり</returns>
        public bool CheckSanteiTerm(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, int hokenKbn, int santeiKbn = SanteiKbnConst.Santei)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;

            List<int> checkHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());    // MakeCheckHokenKbn(hokenKbn);
            List<int> checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                //o.SinYm * 100 + o.SinDay >= startDate &&
                o.SinDate >= startDate &&
                //o.SinYm * 100 + o.SinDay <= endDate &&
                o.SinDate <= endDate &&
                //o.SinYm * 100 + o.SinDay != sinDate &&
                o.SinDate != sinDate &&
                o.RaiinNo != raiinNo
                );
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                itemCds.Contains(p.ItemCd));

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                    sinKouiCount.SinDate >= startDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate &&
                    sinKouiCount.SinDate <= endDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay != sinDate
                    sinKouiCount.SinDate != sinDate
                group sinKouiDetail by sinKouiDetail.HpId
            );

            return joinQuery.Any();
        }

        /// <summary>
        /// 指定の期間に指定の項目が何回算定されているかカウントする
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">カウント開始日</param>
        /// <param name="endDate">カウント終了日</param>
        /// <param name="sinDate">診療日（除外する日）</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCd">カウントする項目</param>
        /// <returns>算定回数</returns>
        public double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, string itemCd, int hokenKbn, List<int> santeiKbns = null)
        {
            List<string> itemCds = new List<string> { itemCd };
            return SanteiCount(hpId, ptId, startDate, endDate, sinDate, raiinNo, itemCds, hokenKbn, santeiKbns);
        }

        /// <summary>
        /// 指定の期間に指定の項目が何回算定されているかカウントする
        /// ※複数項目用
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">カウント開始日</param>
        /// <param name="endDate">カウント終了日</param>
        /// <param name="sinDate">診療日（除外する日）</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCds">カウントする項目のリスト</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定回数</returns>
        public double SanteiCount(
            int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, 
            List<string> itemCds, int hokenKbn, List<int> santeiKbns = null, List<int> hokenKbns = null)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;

            List<int> checkHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());    // MakeCheckHokenKbn(hokenKbn);

            if(hokenKbns != null)
            {
                checkHokenKbn = hokenKbns;
            }

            List<int> checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            if(santeiKbns != null)
            {
                checkSanteiKbn = santeiKbns;
            }

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == SanteiKbnConst.Santei
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                //o.SinYm * 100 + o.SinDay >= startDate &&
                o.SinDate >= startDate &&
                //o.SinYm * 100 + o.SinDay <= endDate &&
                o.SinDate <= endDate &&
                //o.SinYm * 100 + o.SinDay != sinDate &&
                o.SinDate != sinDate &&
                o.RaiinNo != raiinNo);
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                itemCds.Contains(p.ItemCd)
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                    sinKouiCount.SinDate >= startDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate &&
                    sinKouiCount.SinDate <= endDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay != sinDate &&
                    sinKouiCount.SinDate != sinDate &&
                    sinKouiCount.RaiinNo != raiinNo
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 指定の期間に指定の項目が何回算定されているかカウントする
        /// ※複数項目用
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">カウント開始日</param>
        /// <param name="endDate">カウント終了日</param>
        /// <param name="sinDate">診療日（除外する日）</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="hokatuKbn">カウントする項目の包括区分</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <param name="hokenKbn">保険区分</param>
        /// <returns>算定回数</returns>
        public double SanteiCountByHokatuKbn(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, int hokatuKbn, int hokenKbn, int santeiKbn = SanteiKbnConst.Santei)
        {
            //List<int> pHokenKbn = new List<int>() { hokenKbn };
            //if(hokenKbn == 4)
            //{
            //    // 自費保険の場合、健保も対象にする
            //    pHokenKbn.Add(0);
            //}
            List<int> pHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());
            List<int> pSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            int startYm = startDate / 100;
            int endYm = endDate / 100;

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                pHokenKbn.Contains(o.HokenKbn)  &&
                //o.SanteiKbn == santeiKbn
                pSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                //o.SinYm * 100 + o.SinDay >= startDate &&
                o.SinDate >= startDate &&
                //o.SinYm * 100 + o.SinDay <= endDate &&
                o.SinDate <= endDate &&
                //o.SinYm * 100 + o.SinDay != sinDate &&
                (sinDate > 0 ? o.SinDate != sinDate : true) &&
                (raiinNo > 0 ? o.RaiinNo != raiinNo : true));
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm 
                );
            var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.StartDate <= endDate &&
                p.EndDate >= startDate &&
                p.HokatuKbn == hokatuKbn
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join tenMst in tenMsts on
                    new { sinKouiDetail.HpId, sinKouiDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                    sinKouiCount.SinDate >= startDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate &&
                    sinKouiCount.SinDate <= endDate 
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay != sinDate
                    //sinKouiCount.SinDate != sinDate
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sinDate"></param>
        /// <param name="raiinNo"></param>
        /// <param name="hokatuKbn"><0の時未指定</param>
        /// <param name="cdKbn">空の時未指定</param>
        /// <param name="cdKbnno"><0の時未指定</param>
        /// <param name="cdEdano"><0の時未指定</param>
        /// <param name="cdKouno"><0の時未指定</param>
        /// <param name="hokenKbn"></param>
        /// <param name="santeiKbn"></param>
        /// <returns></returns>
        public double SanteiCountByHokatuKbn(
            int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, 
            int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno, int hokenKbn, int santeiKbn = SanteiKbnConst.Santei)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;

            //List<int> pHokenKbn = new List<int>() { hokenKbn };
            //if(hokenKbn == 4)
            //{
            //    // 自費保険の場合、健保も含める
            //    pHokenKbn.Add(0);
            //}
            List<int> pHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());
            List<int> pSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                pHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn
                pSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                //o.SinYm * 100 + o.SinDay >= startDate &&
                o.SinDate >= startDate &&
                //o.SinYm * 100 + o.SinDay <= endDate &&
                o.SinDate <= endDate &&
                //o.SinYm * 100 + o.SinDay != sinDate &&
                (sinDate > 0 ? o.SinDate != sinDate : true) &&
                (raiinNo > 0 ? o.RaiinNo != raiinNo : true));
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm
                );
            var tenMsts = _tenantDataContext.TenMsts.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.StartDate <= endDate &&
                p.EndDate >= startDate &&
                (hokatuKbn >= 0 ? p.HokatuKbn == hokatuKbn : true) &&
                (cdKbn == "" ? p.CdKbn == cdKbn : true) &&
                (cdKbnno >= 0 ? p.CdKbnno == cdKbnno : true) &&
                (cdEdano >= 0 ? p.CdEdano == cdEdano : true) &&
                (cdKouno >= 0 ? p.CdKouno == cdKouno : true)
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join tenMst in tenMsts on
                    new { sinKouiDetail.HpId, sinKouiDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                    sinKouiCount.SinDate >= startDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate &&
                    sinKouiCount.SinDate <= endDate 
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay != sinDate]
                    //sinKouiCount.SinDate != sinDate
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }


        }

        /// <summary>
        /// 指定日以前で、指定の項目が算定されている最近の日付を返す
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="baseDate">基準日</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCd">検索する項目</param>
        /// <returns>
        /// 指定の項目が算定されている最近の日付
        /// 算定がない場合は0
        /// </returns>
        public int FindLastSanteiDate(int hpId, long ptId, int baseDate, int sinDate, long raiinNo, string itemCd, int hokenKbn)
        {
            List<string> itemCds = new List<string> { itemCd };
            return FindLastSanteiDate(hpId, ptId, baseDate, sinDate, raiinNo, itemCds, hokenKbn);
        }

        /// <summary>
        /// <=基準日で、指定の項目が算定されている最近の日付を返す
        /// ※複数項目用
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="baseDate">基準日</param>
        /// <param name="sinDate">除外する診療日</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCds">検索する項目のリスト</param>
        /// <returns>
        /// 指定の項目が算定されている最近の日付
        /// 算定がない場合は0
        /// </returns>
        public int FindLastSanteiDate(int hpId, long ptId, int baseDate, int sinDate, long raiinNo, List<string> itemCds, int hokenKbn, int santeiKbn = SanteiKbnConst.Santei, List<int> hokenKbns = null)
        {            
            int baseYm = baseDate / 100;

            List<int> checkHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());// MakeCheckHokenKbn(hokenKbn);
            if(hokenKbns != null)
            {
                checkHokenKbn = hokenKbns;
            }

            List<int> checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm <= baseYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                //o.SinYm * 100 + o.SinDay < baseDate &&
                o.SinDate <= baseDate &&
                //o.SinYm * 100 + o.SinDay != sinDate &&
                o.SinDate != sinDate &&
                o.RaiinNo != raiinNo);
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm <= baseDate / 100 &&
                itemCds.Contains(p.ItemCd)
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm <= baseYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay < baseDate &&
                    sinKouiCount.SinDate <= baseDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay != sinDate &&
                    sinKouiCount.SinDate != sinDate &&
                    sinKouiCount.RaiinNo != raiinNo
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                //select new { max = A.Max(a => a.sinKouiCount.SinYm * 100 + a.sinKouiCount.SinDay) }
                select new { max = A.Max(a => a.sinKouiCount.SinDate) }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().max;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 指定日以前で、指定の項目が算定されている最初の日付を返す
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="baseDate">基準日</param>
        /// <param name="itemCd">検索する項目</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <returns>
        /// 指定の項目が算定されている最初の日付
        /// 算定がない場合は0
        /// </returns>
        public int FindFirstSanteiDate(int hpId, long ptId, int baseDate, long raiinNo, string itemCd, int hokenKbn)
        {
            List<string> itemCds = new List<string> { itemCd };

            return FindFirstSanteiDate(hpId, ptId, baseDate, raiinNo, itemCds, hokenKbn);
        }

        /// <summary>
        /// 指定日以前で、指定の項目が算定されている最初の日付を返す
        /// ※複数項目用
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="baseDate">基準日</param>
        /// <param name="raiinNo">除外する来院番号</param>
        /// <param name="itemCds">検索する項目のリスト</param>
        /// <returns>
        /// 指定の項目が算定されている最初の日付
        /// 算定がない場合は0
        /// </returns>
        public int FindFirstSanteiDate(int hpId, long ptId, int baseDate, long raiinNo, List<string> itemCds, int hokenKbn, int santeiKbn = SanteiKbnConst.Santei)
        {
            const string conFncName = nameof(FindFirstSanteiDate);

            int baseYm = baseDate / 100;
            List<int> checkHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());// MakeCheckHokenKbn(hokenKbn);
            List<int> checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm <= baseYm &&
                //o.HokenKbn != 4 && // 自費以外
                checkHokenKbn.Contains(hokenKbn) &&
                //o.SanteiKbn == santeiKbn
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                //o.SinYm * 100 + o.SinDay < baseDate &&
                o.SinDate < baseDate &&
                o.RaiinNo != raiinNo);
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm <= baseYm &&
                itemCds.Contains(p.ItemCd)
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm <= baseYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay < baseDate
                    sinKouiCount.SinDate < baseDate
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                //select new { min = A.Min(a => a.sinKouiCount.SinYm * 100 + a.sinKouiCount.SinDay) }
                select new { min = A.Min(a => a.sinKouiCount.SinDate) }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().min;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 指定の項目の算定日を取得する（複数指定）
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns></returns>
        public List<SanteiDaysModel> GetSanteiDays(int hpId, long ptId, int startDate, int endDate, long raiinNo, int sinDate, List<string> itemCds, int hokenKbn, bool excludeSanteiGai, List<int> santeiKbns = null)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;

            List<int> santeiKbnls = new List<int> { 0 };
            if(excludeSanteiGai == false)
            {
                santeiKbnls.Add(1);
            }

            List<int> checkHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());// MakeCheckHokenKbn(hokenKbn);
            List<int> checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            if(santeiKbns != null)
            {
                checkSanteiKbn = santeiKbns;
            }

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //santeiKbnls.Contains(o.SanteiKbn)
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                //o.SinYm * 100 + o.SinDay >= startDate &&
                o.SinDate >= startDate &&
                //o.SinYm * 100 + o.SinDay <= endDate &&
                o.SinDate <= endDate &&
                //o.SinYm * 100 + o.SinDay != sinDate);
                o.SinDate != sinDate);
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                itemCds.Contains(p.ItemCd)
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    //itemCds.Contains(sinKouiDetail.ItemCd) &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                    sinKouiCount.SinDate >= startDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate &&
                    sinKouiCount.SinDate <= endDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay != sinDate
                    sinKouiCount.SinDate != sinDate
                //group new { sinKouiCount, sinKouiDetail } by new { sinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay, itemCd = sinKouiDetail.ItemCd } into A
                group new { sinKouiCount, sinKouiDetail } by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd } into A
                orderby A.Key.sinDate
                select new { A.Key.sinDate, A.Key.itemCd}
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());

            var entities = joinQuery.ToList();

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            entities?.ForEach(entity => {
                results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd));
            });

            return results;

        }
        /// <summary>
        /// 背反処理専用
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="raiinNo"></param>
        /// <param name="sinDate"></param>
        /// <param name="itemCds"></param>
        /// <param name="hokenKbn"></param>
        /// <param name="excludeSanteiGai"></param>
        /// <returns></returns>
        public List<SanteiDaysModel> GetSanteiDaysHaihan(int hpId, long ptId, int startDate, int endDate, long raiinNo, int sinDate, List<string> itemCds, int hokenKbn, bool excludeSanteiGai)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;
            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            if (itemCds.Any(p => p.StartsWith("J")) ||
                itemCds.Any(p => p.StartsWith("Z")))
            {
                List<int> santeiKbnls = new List<int> { 0 };
                if (excludeSanteiGai == false)
                {
                    santeiKbnls.Add(1);
                }

                List<int> checkHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());
                List<int> checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

                var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                    o.HpId == hpId &&
                    o.PtId == ptId &&
                    o.SinYm >= startYm &&
                    o.SinYm <= endYm &&
                    checkHokenKbn.Contains(o.HokenKbn)
                );
                var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                    o.HpId == hpId &&
                    o.PtId == ptId &&
                    o.SinDate >= startDate &&
                    o.SinDate <= endDate &&
                    o.SinDate != sinDate);
                var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm >= startYm &&
                    p.SinYm <= endYm &&
                    (itemCds.FindAll(q => q.StartsWith("J")).Contains(p.ItemCd) ||
                     itemCds.FindAll(q => q.StartsWith("Z")).Contains(p.ItemCd))
                    );

                var joinQuery = (
                    from sinKouiDetail in sinKouiDetails
                    join sinKouiCount in sinKouiCounts on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                    join sinRpInf in sinRpInfs on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                        new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                    where
                        sinKouiDetail.HpId == hpId &&
                        sinKouiDetail.PtId == ptId &&
                        sinKouiDetail.SinYm >= startYm &&
                        sinKouiDetail.SinYm <= endYm &&
                        sinKouiCount.SinDate >= startDate &&
                        sinKouiCount.SinDate <= endDate &&
                        sinKouiCount.SinDate != sinDate
                    group new { sinKouiCount, sinKouiDetail } by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd, odrItemCd = sinKouiDetail.OdrItemCd, sinRpInf.SanteiKbn } into A
                    orderby A.Key.sinDate
                    select new { A.Key.sinDate, A.Key.itemCd, A.Key.odrItemCd, A.Key.SanteiKbn }
                );

                var entities = joinQuery.ToList();

                entities?.ForEach(entity =>
                {
                    if (entity.itemCd.StartsWith("J") || (entity.odrItemCd.StartsWith("Z") && checkSanteiKbn.Contains(entity.SanteiKbn)))
                    {
                        results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd, entity.odrItemCd));
                    }
                });
            }
            return results;

        }
        /// <summary>
        /// 指定の項目の算定日を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="startDate">チェック開始日</param>
        /// <param name="endDate">チェック終了日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="hokenKbn">保険区分</param>
        /// <returns></returns>
        public List<SanteiDaysModel> GetSanteiDays(int hpId, long ptId, int startDate, int endDate, long raiinNo, int sinDate, string itemCd, int hokenKbn, int hokenId = 0, List<int> santeiKbns = null)
        {
            int startYm = startDate / 100;
            int endYm = endDate / 100;

            List<int> checkHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling()); // MakeCheckHokenKbn(hokenKbn);
            List<int> checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn, _systemConfigProvider.GetHokensyuHandling());

            if(santeiKbns != null)
            {
                checkSanteiKbn = santeiKbns;
            }

            var sinRpInfs = _tenantDataContext.SinRpInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                checkSanteiKbn.Contains(o.SanteiKbn)
            );
            var sinKouis = _tenantDataContext.SinKouis.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                (hokenId > 0 ? o.HokenId == hokenId : true)
            );
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate >= startDate &&//o.SinYm * 100 + o.SinDay >= startDate &&
                o.SinDate <= endDate && //o.SinYm * 100 + o.SinDay <= endDate &&
                o.SinDate != sinDate//o.SinYm * 100 + o.SinDay != sinDate
                );
            var sinKouiDetails = _tenantDataContext.SinKouiDetails.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                p.ItemCd == itemCd
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join sinKoui in sinKouis on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo }
                where
                    sinKouiDetail.HpId == hpId &&
                    sinKouiDetail.PtId == ptId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    sinKouiDetail.ItemCd == itemCd &&
                    sinKouiCount.SinDate >= startDate && //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                    sinKouiCount.SinDate <= endDate && //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate &&
                    sinKouiCount.SinDate != sinDate//sinKouiCount.SinYm * 100 + sinKouiCount.SinDay != sinDate                    
                //group new { sinKouiCount, sinKouiDetail } by new { sinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay, itemCd = sinKouiDetail.ItemCd } into A
                group new { sinKouiCount, sinKouiDetail } by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd } into A
                orderby A.Key.sinDate
                select new { A.Key.sinDate, A.Key.itemCd }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());

            var entities = joinQuery.ToList();

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            entities?.ForEach(entity => {
                results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd));
            });

            return results;

        }

        /// <summary>
        /// 算定情報詳細を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>算定情報詳細のリスト</returns>
        public List<SanteiInfDetailModel> FindSanteiInfDetail(int hpId, long ptId, int sinDate)
        {
            var santeiInfDetails = _tenantDataContext.SanteiInfDetails.FindListQueryableNoTrack(s =>
                    s.HpId == hpId &&
                    s.PtId == ptId &&
                    s.KisanDate <= sinDate &&
                    s.EndDate >= sinDate &&
                    s.IsDeleted == DeleteStatus.None).ToList();

            List<SanteiInfDetailModel> results = new List<SanteiInfDetailModel>();

            santeiInfDetails?.ForEach(entity =>
            {
                results.Add(new SanteiInfDetailModel(entity));
            });

            return results;
        }

        /// <summary>
        /// リロード
        /// </summary>        
        public void SinRpInfReload(SinRpInf sinRpInf)
        {
            //_tenantDataContext.SinRpInfs.Reload(sinRpInf);
        }

        public void SinRpNoInfReload(SinRpNoInf sinRpNoInf)
        {
            //_tenantDataContext.SinRpNoInfs.Reload(sinRpNoInf);
        }

        public void SinKouiReload(SinKoui sinKoui)
        {
            //_tenantDataContext.SinKouis.Reload(sinKoui);
        }

        public void SinKouiCountReload(SinKouiCount sinKouiCount)
        {
            //_tenantDataContext.SinKouiCounts.Reload(sinKouiCount);
        }

        public void SinKouiDetailReload(SinKouiDetail sinKouiDetail)
        {
            //_tenantDataContext.SinKouiDetails.Reload(sinKouiDetail);
        }

        public void WrkSinRpInfReload(WrkSinRpInf wrkSinRpInf)
        {
            //_tenantDataContext.WrkSinRpInfs.Reload(wrkSinRpInf);
        }

        public void WrkSinKouiReload(WrkSinKoui wrkSinKoui)
        {
            //_tenantDataContext.WrkSinKouis.Reload(wrkSinKoui);
        }

        public void WrkSinKouiDetailReload(WrkSinKouiDetail wrkSinKouiDtl)
        {
            //_tenantDataContext.WrkSinKouiDetails.Reload(wrkSinKouiDtl);
        }

        public void WrkSinKouiDetailDelReload(WrkSinKouiDetailDel wrkSinKouiDtlDel)
        {
            //_tenantDataContext.WrkSinKouiDetailDels.Reload(wrkSinKouiDtlDel);
        }
               
    }
}
