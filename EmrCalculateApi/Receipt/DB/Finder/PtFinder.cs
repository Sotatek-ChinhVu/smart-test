using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Models;
using EmrCalculateApi.Constants;
using Infrastructure.Interfaces;
using System.Runtime.CompilerServices;

namespace EmrCalculateApi.Receipt.DB.Finder
{
    class PtFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public PtFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
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
        /// 傷病名情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<SyobyoDataModel> FindSyobyoData(int hpId, long ptId, int sinYm, int hokenId, int outputYm)
        {
            var ptByomeis = _tenantDataContext.PtByomeis.FindListNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.StartDate <= sinYm * 100 + 31 &&
                ((p.TenkiKbn == TenkiKbnConst.Continued && p.TenkiDate <= 0) || p.TenkiDate >= sinYm * 100 + 1) &&
                (p.HokenPid == 0 || p.HokenPid == hokenId) &&
                p.IsNodspRece == 0 &&
                p.IsDeleted == DeleteStatus.None)
                .OrderBy(p=>p.StartDate)
                .ThenByDescending(p=>p.SyubyoKbn)
                .ThenBy(p=>p.SortNo)
                .ToList();

            List<SyobyoDataModel> results = new List<SyobyoDataModel>();

            ptByomeis?.ForEach(ptByomei =>
                {
                    results.Add(new SyobyoDataModel(ptByomei, sinYm, outputYm, _emrLogger));
                }
            );

            return results;
        }

        /// <summary>
        /// 保険組み合わせマスタ取得
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinDate"></param>
        /// <returns></returns>
        public List<PtHokenPatternModel> FindPtHokenPattern(int hpId, long ptId, int sinDate)
        {
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    //p.StartDate <= sinDate &&
                    //p.EndDate >= sinDate &&
                    p.IsDeleted == DeleteStatus.None)
                .OrderBy(p => p.HpId)
                .ThenBy(p => p.StartDate)
                .ToList();

            var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack();
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(
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

            var kohiPriorities = _tenantDataContext.KohiPriorities.FindListQueryableNoTrack();
            var ptKohis = _tenantDataContext.PtKohis.FindListQueryableNoTrack();
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
                    hokenMst.Houbetu,
                    hokenMst.HokenSbtKbn
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
                    ptKohi.HokenNo,
                    ptKohi.HokenEdaNo,
                    KohiId = ptKohi.HokenId,
                    ptKohi.PrefNo,
                    houbetuMst.Houbetu,
                    houbetuMst.HokenSbtKbn,
                    //kohiPriority.PriorityNo
                    PriorityNo = kohiPriority == null ? "99999" : kohiPriority.PriorityNo
                }
            ).ToList();

            //var entities = (
            //    from hokPattern in ptHokenPatterns
            //    join ptKohis1 in ptKohiQuery on
            //        new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi1Id } equals
            //        new { ptKohis1.HpId, ptKohis1.PtId, Kohi1Id = ptKohis1.KohiId } into kohi1Join
            //    from ptKohi1 in kohi1Join.DefaultIfEmpty()
            //    join ptKohis2 in ptKohiQuery on
            //        new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi2Id } equals
            //        new { ptKohis2.HpId, ptKohis2.PtId, Kohi2Id = ptKohis2.KohiId } into kohi2Join
            //    from ptKohi2 in kohi2Join.DefaultIfEmpty()
            //    join ptKohis3 in ptKohiQuery on
            //        new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi3Id } equals
            //        new { ptKohis3.HpId, ptKohis3.PtId, Kohi3Id = ptKohis3.KohiId } into kohi3Join
            //    from ptKohi3 in kohi3Join.DefaultIfEmpty()
            //    join ptKohis4 in ptKohiQuery on
            //        new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi4Id } equals
            //        new { ptKohis4.HpId, ptKohis4.PtId, Kohi4Id = ptKohis4.KohiId } into kohi4Join
            //    from ptKohi4 in kohi4Join.DefaultIfEmpty()
            //    where
            //        hokPattern.HpId == hpId &&
            //        hokPattern.PtId == ptId &&
            //        hokPattern.IsDeleted == DeleteStatus.None
            //    select new
            //    {
            //        hokPattern,
            //        Kohi1PrefNo = ptKohi1 == null ? 99 : ptKohi1.PrefNo,
            //        Kohi1Houbetu = ptKohi1 == null ? "99" : ptKohi1.Houbetu,
            //        Kohi1HokenSbtKbn = ptKohi1 == null ? 0 : ptKohi1.HokenSbtKbn,
            //        kohi1HokenNo = ptKohi1 == null ? 0 : ptKohi1.HokenNo,
            //        kohi1HokenEdaNo = ptKohi1 == null ? 0 : ptKohi1.HokenEdaNo,
            //        Kohi1PriorityNo = ptKohi1 == null ? 99 : ptKohi1.PriorityNo,
            //        Kohi2PrefNo = ptKohi2 == null ? 99 : ptKohi2.PrefNo,
            //        Kohi2Houbetu = ptKohi2 == null ? "99" : ptKohi2.Houbetu,
            //        Kohi2HokenSbtKbn = ptKohi2 == null ? 0 : ptKohi2.HokenSbtKbn,
            //        kohi2HokenNo = ptKohi2 == null ? 0 : ptKohi2.HokenNo,
            //        kohi2HokenEdaNo = ptKohi2 == null ? 0 : ptKohi2.HokenEdaNo,
            //        Kohi2PriorityNo = ptKohi2 == null ? 99 : ptKohi2.PriorityNo,
            //        Kohi3PrefNo = ptKohi3 == null ? 99 : ptKohi3.PrefNo,
            //        Kohi3Houbetu = ptKohi3 == null ? "99" : ptKohi3.Houbetu,
            //        Kohi3HokenSbtKbn = ptKohi3 == null ? 0 : ptKohi3.HokenSbtKbn,
            //        kohi3HokenNo = ptKohi3 == null ? 0 : ptKohi3.HokenNo,
            //        kohi3HokenEdaNo = ptKohi3 == null ? 0 : ptKohi3.HokenEdaNo,
            //        Kohi3PriorityNo = ptKohi3 == null ? 99 : ptKohi3.PriorityNo,
            //        Kohi4PrefNo = ptKohi4 == null ? 99 : ptKohi4.PrefNo,
            //        Kohi4Houbetu = ptKohi4 == null ? "99" : ptKohi4.Houbetu,
            //        Kohi4HokenSbtKbn = ptKohi4 == null ? 0 : ptKohi4.HokenSbtKbn,
            //        kohi4HokenNo = ptKohi4 == null ? 0 : ptKohi4.HokenNo,
            //        kohi4HokenEdaNo = ptKohi4 == null ? 0 : ptKohi4.HokenEdaNo,
            //        Kohi4PriorityNo = ptKohi4 == null ? 99 : ptKohi4.PriorityNo,
            //    }
            //).ToList();

            List<PtHokenPatternModel> results = new List<PtHokenPatternModel>();

            ptHokenPatterns?.ForEach(entity =>
            {
                string kohi1Houbetu = "99";
                int kohi1HokenSbtKbn = 0;
                int kohi1HokenNo = 0;
                int kohi1HokenEdaNo = 0;
                string kohi1PriorityNo = "99999";
                string kohi2Houbetu = "99";
                int kohi2HokenSbtKbn = 0;
                int kohi2HokenNo = 0;
                int kohi2HokenEdaNo = 0;
                string kohi2PriorityNo = "99999";
                string kohi3Houbetu = "99";
                int kohi3HokenSbtKbn = 0;
                int kohi3HokenNo = 0;
                int kohi3HokenEdaNo = 0;
                string kohi3PriorityNo = "99999";
                string kohi4Houbetu = "99";
                int kohi4HokenSbtKbn = 0;
                int kohi4HokenNo = 0;
                int kohi4HokenEdaNo = 0;
                string kohi4PriorityNo = "99999";


                if (entity.Kohi1Id > 0)
                {
                    var ret = ptKohiQuery.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.KohiId == entity.Kohi1Id);

                    if(ret != null)
                    {
                        kohi1Houbetu = ret.Houbetu;
                        kohi1HokenSbtKbn = ret.HokenSbtKbn;
                        kohi1HokenNo = ret.HokenNo;
                        kohi1HokenEdaNo = ret.HokenEdaNo;
                        kohi1PriorityNo = ret.PriorityNo;
                    }
                }
                if (entity.Kohi2Id > 0)
                {
                    var ret = ptKohiQuery.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.KohiId == entity.Kohi2Id);

                    if (ret != null)
                    {
                        kohi2Houbetu = ret.Houbetu;
                        kohi2HokenSbtKbn = ret.HokenSbtKbn;
                        kohi2HokenNo = ret.HokenNo;
                        kohi2HokenEdaNo = ret.HokenEdaNo;
                        kohi2PriorityNo = ret.PriorityNo;
                    }
                }
                if (entity.Kohi3Id > 0)
                {
                    var ret = ptKohiQuery.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.KohiId == entity.Kohi3Id);

                    if (ret != null)
                    {
                        kohi3Houbetu = ret.Houbetu;
                        kohi3HokenSbtKbn = ret.HokenSbtKbn;
                        kohi3HokenNo = ret.HokenNo;
                        kohi3HokenEdaNo = ret.HokenEdaNo;
                        kohi3PriorityNo = ret.PriorityNo;
                    }
                }
                if (entity.Kohi4Id > 0)
                {
                    var ret = ptKohiQuery.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.KohiId == entity.Kohi4Id);

                    if (ret != null)
                    {
                        kohi4Houbetu = ret.Houbetu;
                        kohi4HokenSbtKbn = ret.HokenSbtKbn;
                        kohi4HokenNo = ret.HokenNo;
                        kohi4HokenEdaNo = ret.HokenEdaNo;
                        kohi4PriorityNo = ret.PriorityNo;
                    }
                }

                results.Add(
                    new PtHokenPatternModel(
                        entity,
                        kohi1Houbetu, kohi1HokenSbtKbn, kohi1HokenNo, kohi1HokenEdaNo, kohi1PriorityNo,
                        kohi2Houbetu, kohi2HokenSbtKbn, kohi2HokenNo, kohi2HokenEdaNo, kohi2PriorityNo,
                        kohi3Houbetu, kohi3HokenSbtKbn, kohi3HokenNo, kohi3HokenEdaNo, kohi3PriorityNo,
                        kohi4Houbetu, kohi4HokenSbtKbn, kohi4HokenNo, kohi4HokenEdaNo, kohi4PriorityNo));
            });

            return results;
        }

        ///// <summary>
        ///// 保険組み合わせマスタ取得
        ///// </summary>
        ///// <param name="hpId"></param>
        ///// <param name="ptId"></param>
        ///// <param name="sinDate"></param>
        ///// <returns></returns>
        //public List<PtHokenPatternModel> FindPtHokenPattern(int hpId, long ptId, int sinDate)
        //{
        //    var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(p =>
        //            p.HpId == hpId &&
        //            p.PtId == ptId &&
        //            //p.StartDate <= sinDate &&
        //            //p.EndDate >= sinDate &&
        //            p.IsDeleted == DeleteStatus.None)
        //        .OrderBy(p => p.HpId)
        //        .ThenBy(p => p.StartDate)
        //        .ToList();

        //    var hokenMsts = _tenantDataContext.HokenMsts.FindListQueryableNoTrack();
        //    //診療日基準で保険番号マスタのキー情報を取得
        //    var hokenMstKeys = _tenantDataContext.HokenMsts.FindListQueryableNoTrack(
        //        h => h.StartDate <= sinDate
        //    ).GroupBy(
        //        x => new { x.HpId, x.PrefNo, x.HokenNo, x.HokenEdaNo }
        //    ).Select(
        //        x => new
        //        {
        //            x.Key.HpId,
        //            x.Key.PrefNo,
        //            x.Key.HokenNo,
        //            x.Key.HokenEdaNo,
        //            StartDate = x.Max(d => d.StartDate)
        //        }
        //    );

        //    var kohiPriorities = _tenantDataContext.KohiPriorityRepository.FindListQueryableNoTrack();
        //    var ptKohis = _tenantDataContext.PtKohis.FindListQueryableNoTrack();
        //    //保険番号マスタの取得
        //    var houbetuMsts = (
        //        from hokenMst in hokenMsts
        //        join hokenKey in hokenMstKeys on
        //            new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
        //            new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
        //        select new
        //        {
        //            hokenMst.HpId,
        //            hokenMst.PrefNo,
        //            hokenMst.HokenNo,
        //            hokenMst.HokenEdaNo,
        //            hokenMst.Houbetu,
        //            hokenMst.HokenSbtKbn
        //        }
        //    );

        //    //公費の優先順位を取得
        //    var ptKohiQuery = (
        //        from ptKohi in ptKohis
        //        join houbetuMst in houbetuMsts on
        //            new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
        //            new { houbetuMst.HpId, houbetuMst.HokenNo, houbetuMst.HokenEdaNo, houbetuMst.PrefNo }
        //        join kPriority in kohiPriorities on
        //            new { houbetuMst.PrefNo, houbetuMst.Houbetu } equals
        //            new { kPriority.PrefNo, kPriority.Houbetu } into kohiPriorityJoin
        //        from kohiPriority in kohiPriorityJoin.DefaultIfEmpty()
        //        where
        //            ptKohi.HpId == hpId &&
        //            ptKohi.PtId == ptId &&
        //            ptKohi.IsDeleted == DeleteStatus.None
        //        select new
        //        {
        //            ptKohi.HpId,
        //            ptKohi.PtId,
        //            ptKohi.HokenNo,
        //            ptKohi.HokenEdaNo,
        //            KohiId = ptKohi.HokenId,
        //            ptKohi.PrefNo,
        //            houbetuMst.Houbetu,
        //            houbetuMst.HokenSbtKbn,
        //            //kohiPriority.PriorityNo
        //            PriorityNo = kohiPriority == null ? 99 : kohiPriority.PriorityNo
        //        }
        //    );

        //    var entities = (
        //        from hokPattern in ptHokenPatterns
        //        join ptKohis1 in ptKohiQuery on
        //            new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi1Id } equals
        //            new { ptKohis1.HpId, ptKohis1.PtId, Kohi1Id = ptKohis1.KohiId } into kohi1Join
        //        from ptKohi1 in kohi1Join.DefaultIfEmpty()
        //        join ptKohis2 in ptKohiQuery on
        //            new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi2Id } equals
        //            new { ptKohis2.HpId, ptKohis2.PtId, Kohi2Id = ptKohis2.KohiId } into kohi2Join
        //        from ptKohi2 in kohi2Join.DefaultIfEmpty()
        //        join ptKohis3 in ptKohiQuery on
        //            new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi3Id } equals
        //            new { ptKohis3.HpId, ptKohis3.PtId, Kohi3Id = ptKohis3.KohiId } into kohi3Join
        //        from ptKohi3 in kohi3Join.DefaultIfEmpty()
        //        join ptKohis4 in ptKohiQuery on
        //            new { hokPattern.HpId, hokPattern.PtId, hokPattern.Kohi4Id } equals
        //            new { ptKohis4.HpId, ptKohis4.PtId, Kohi4Id = ptKohis4.KohiId } into kohi4Join
        //        from ptKohi4 in kohi4Join.DefaultIfEmpty()
        //        where
        //            hokPattern.HpId == hpId &&
        //            hokPattern.PtId == ptId &&
        //            hokPattern.IsDeleted == DeleteStatus.None
        //        select new
        //        {
        //            hokPattern,
        //            Kohi1PrefNo = ptKohi1 == null ? 99 : ptKohi1.PrefNo,
        //            Kohi1Houbetu = ptKohi1 == null ? "99" : ptKohi1.Houbetu,
        //            Kohi1HokenSbtKbn = ptKohi1 == null ? 0 : ptKohi1.HokenSbtKbn,
        //            kohi1HokenNo = ptKohi1 == null ? 0 : ptKohi1.HokenNo,
        //            kohi1HokenEdaNo = ptKohi1 == null ? 0 : ptKohi1.HokenEdaNo,
        //            Kohi1PriorityNo = ptKohi1 == null ? 99 : ptKohi1.PriorityNo,
        //            Kohi2PrefNo = ptKohi2 == null ? 99 : ptKohi2.PrefNo,
        //            Kohi2Houbetu = ptKohi2 == null ? "99" : ptKohi2.Houbetu,
        //            Kohi2HokenSbtKbn = ptKohi2 == null ? 0 : ptKohi2.HokenSbtKbn,
        //            kohi2HokenNo = ptKohi2 == null ? 0 : ptKohi2.HokenNo,
        //            kohi2HokenEdaNo = ptKohi2 == null ? 0 : ptKohi2.HokenEdaNo,
        //            Kohi2PriorityNo = ptKohi2 == null ? 99 : ptKohi2.PriorityNo,
        //            Kohi3PrefNo = ptKohi3 == null ? 99 : ptKohi3.PrefNo,
        //            Kohi3Houbetu = ptKohi3 == null ? "99" : ptKohi3.Houbetu,
        //            Kohi3HokenSbtKbn = ptKohi3 == null ? 0 : ptKohi3.HokenSbtKbn,
        //            kohi3HokenNo = ptKohi3 == null ? 0 : ptKohi3.HokenNo,
        //            kohi3HokenEdaNo = ptKohi3 == null ? 0 : ptKohi3.HokenEdaNo,
        //            Kohi3PriorityNo = ptKohi3 == null ? 99 : ptKohi3.PriorityNo,
        //            Kohi4PrefNo = ptKohi4 == null ? 99 : ptKohi4.PrefNo,
        //            Kohi4Houbetu = ptKohi4 == null ? "99" : ptKohi4.Houbetu,
        //            Kohi4HokenSbtKbn = ptKohi4 == null ? 0 : ptKohi4.HokenSbtKbn,
        //            kohi4HokenNo = ptKohi4 == null ? 0 : ptKohi4.HokenNo,
        //            kohi4HokenEdaNo = ptKohi4 == null ? 0 : ptKohi4.HokenEdaNo,
        //            Kohi4PriorityNo = ptKohi4 == null ? 99 : ptKohi4.PriorityNo,
        //        }
        //    ).ToList();

        //    List<PtHokenPatternModel> results = new List<PtHokenPatternModel>();

        //    entities?.ForEach(entity =>
        //    {
        //        results.Add(
        //            new PtHokenPatternModel(
        //                entity.hokPattern,
        //                entity.Kohi1PrefNo, entity.Kohi1Houbetu, entity.Kohi1HokenSbtKbn, entity.kohi1HokenNo, entity.kohi1HokenEdaNo, entity.Kohi1PriorityNo,
        //                entity.Kohi2PrefNo, entity.Kohi2Houbetu, entity.Kohi2HokenSbtKbn, entity.kohi2HokenNo, entity.kohi2HokenEdaNo, entity.Kohi2PriorityNo,
        //                entity.Kohi3PrefNo, entity.Kohi3Houbetu, entity.Kohi3HokenSbtKbn, entity.kohi3HokenNo, entity.kohi3HokenEdaNo, entity.Kohi3PriorityNo,
        //                entity.Kohi4PrefNo, entity.Kohi4Houbetu, entity.Kohi4HokenSbtKbn, entity.kohi4HokenNo, entity.kohi4HokenEdaNo, entity.Kohi4PriorityNo));
        //    });

        //    return results;
        //}

        public List<Futan.Models.KaikeiInfModel> GetKaikeiInf(int hpId, long ptId, int sinDate, List<long> raiinNos)
        {
            if(raiinNos == null)
            {
                raiinNos = new List<long>();
                raiinNos.Add(0);
            }
            var entities = 
                _tenantDataContext.KaikeiInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    (p.SinDate >= sinDate / 100 * 100 + 1 && p.SinDate <= sinDate / 100 * 100 + 31) &&
                    raiinNos.Contains(p.RaiinNo))
                    .ToList();

            List<Futan.Models.KaikeiInfModel> results = new List<Futan.Models.KaikeiInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new Futan.Models.KaikeiInfModel(entity));
            });

            return results;
        }
        public List<Futan.Models.KaikeiInfModel> GetKaikeiInf(int hpId, long ptId, int sinYm, int hokenId)
        {            
            var entities =
                _tenantDataContext.KaikeiInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.HokenId == hokenId &&
                    (p.SinDate >= sinYm * 100 + 1 && p.SinDate <= sinYm * 100 + 31))
                    .ToList();

            List<Futan.Models.KaikeiInfModel> results = new List<Futan.Models.KaikeiInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new Futan.Models.KaikeiInfModel(entity));
            });

            return results;
        }
        public List<ReceInfModel> GetReceInf(int hpId, long ptId, int sinDate, int seikyuYm, int hokenId)
        {
            int sinYm = sinDate / 100;
            int firstDate = sinYm * 100 + 1;
            int lastDate = sinYm * 100 + 1;

            var receInfs =
                _tenantDataContext.ReceInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.SeikyuYm == (seikyuYm > 0 ? seikyuYm : p.SeikyuYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId));
            var ptInfs =
                _tenantDataContext.PtInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.IsDelete == DeleteStatus.None);
            var ptHokens =
                _tenantDataContext.PtHokenInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.IsDeleted == DeleteStatus.None
                );
            var hokenMsts =
                _tenantDataContext.HokenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= lastDate &&
                    p.EndDate >= firstDate
                );
            var ptHokenJoins = (
                    from ptHoken in ptHokens
                    join hokenMst in hokenMsts on
                        new { ptHoken.HpId, ptHoken.HokenNo, ptHoken.HokenEdaNo, PrefNo = 0 } equals
                        new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo }
                    select new
                    {
                        ptHoken,
                        hokenMst
                    }
                );

            var receSeikyus =
                _tenantDataContext.ReceSeikyus.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.SeikyuYm == (seikyuYm > 0 ? seikyuYm : p.SeikyuYm) &&
                    p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
                    p.IsDeleted == DeleteStatus.None
                );

            var entities = (
                        from receInf in receInfs
                        join ptInf in ptInfs on
                            new { receInf.HpId, receInf.PtId } equals
                            new { ptInf.HpId, ptInf.PtId } into oJoin2
                        from oj2 in oJoin2.DefaultIfEmpty()
                        join ptHokenJoin in ptHokenJoins on
                            new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                            new { ptHokenJoin.ptHoken.HpId, ptHokenJoin.ptHoken.PtId, ptHokenJoin.ptHoken.HokenId } into oJoin3
                        from oj3 in oJoin3.DefaultIfEmpty()
                        join receSeikyu in receSeikyus on
                            new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm } equals
                            new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuJoins
                        from receSeikyuJoin in receSeikyuJoins.DefaultIfEmpty()
                        select new
                        {
                            receInf,
                            ptInf = oj2,
                            ptHokenJoin = oj3,
                            receSeikyu = receSeikyuJoin
                        }
                    ).ToList();

            List<Receipt.Models.ReceInfModel> results = new List<Receipt.Models.ReceInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new Receipt.Models.ReceInfModel(entity.receInf, entity.ptInf, entity.ptHokenJoin.ptHoken, entity.ptHokenJoin.hokenMst, entity.receSeikyu, null, null, 0));
            });

            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">
        /// 0:社保
        /// 1:国保
        /// 2:労災(初回分)
        /// 3:労災(2回目以降分)
        /// </param>
        /// <param name="hpId">医療機関識別コード</param>
        /// <param name="seikyuYm">請求年月</param>
        /// <param name="kaId">診療科ID 0:未指定</param>
        /// <param name="tantoId">担当医ID 0:未指定</param>
        /// <param name="includeTeseter">テスト患者を含むかどうか</param>
        /// <param name="paperMode">
        ///     紙レセプトの扱い
        ///     0-紙レセプト含む
        ///     1-紙レセプト除く
        ///     2-紙レセプトのみ
        /// </param>
        /// <param name="seikyuKbns">請求区分</param>
        /// <returns></returns>
        public List<ReceInfModel> FindReceInf(int mode, int hpId, int seikyuYm, int kaId, int tantoId, bool includeTeseter, int paperMode, List<int> seikyuKbns)
        {
            List<List<int>> toHokenKbn =
                new List<List<int>>
                {
                    new List<int>{ 1 },     // 社保
                    new List<int>{ 2 },     // 国保
                    new List<int>{ 11,12 },  // 労災
                    new List<int>{ 11,12 },  // 労災
                    new List<int>{ 13 }  // アフターケア
                };

            List<int> hokenKbn = toHokenKbn[mode];

            List<int> isTester = null;
            if(includeTeseter)
            {
                isTester = new List<int> { 0, 1 };
            }
            else
            {
                isTester = new List<int> { 0 };
            }

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

            List<List<int>> toIsPaper =
                new List<List<int>>
                {
                    new List<int>{ 0, 1 },     // 紙含む
                    new List<int>{ 0 },     // 紙除く
                    new List<int>{ 1 }  // 紙のみ
                };

            List<int> isPaper = toIsPaper[paperMode];
            
            var receInfs =
                _tenantDataContext.ReceInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.SeikyuYm == seikyuYm &&
                    seikyuKbns.Contains(p.SeikyuKbn) &&
                    (kaId > 0 ? p.KaId == kaId : true) &&
                    (tantoId > 0 ? p.TantoId == tantoId : true) &&
                    hokenKbn.Contains(p.HokenKbn) &&
                    isTester.Contains(p.IsTester));
            var receStatusies =
                _tenantDataContext.ReceStatuses.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.SeikyuYm == seikyuYm &&
                    //isPaper.Contains(p.IsPaperRece) &&
                    p.IsDeleted == DeleteStatus.None);
            var ptInfs =
                _tenantDataContext.PtInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.IsDelete == DeleteStatus.None);
            var ptHokens =
                _tenantDataContext.PtHokenInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.IsDeleted == DeleteStatus.None
                );
            var receSeikyus =
                _tenantDataContext.ReceSeikyus.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.IsDeleted == DeleteStatus.None
                );
            var kaikeiInfs =
                _tenantDataContext.KaikeiInfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId
                );

            var raiinInfs =
                _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    (tantoIdRaiin > 0 ? p.TantoId == tantoIdRaiin : true) &&
                    (kaIdRaiin > 0 ? p.KaId == kaIdRaiin : true) &&
                    p.Status >= 5 &&
                    p.IsDeleted == DeleteStatus.None
                );

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
            int countMin = 0;
            int countMax = 9999;

            if (mode == 2)
            {
                countMin = 1;
                countMax = 1;
            }
            else if (mode == 3)
            {
                countMin = 2;
                countMax = 9999;
            }

            var join = (
                from receInf in receInfs
                join receStatus in receStatusies on
                    new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                    new { receStatus.HpId, receStatus.PtId, receStatus.SeikyuYm, receStatus.HokenId, receStatus.SinYm } into receStatusJoins
                from receStatusJoin in receStatusJoins.DefaultIfEmpty()
                join ptInf in ptInfs on
                    new { receInf.HpId, receInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId } into ptInfJoins
                from ptInfJoin in ptInfJoins //.DefaultIfEmpty()
                            join ptHoken in ptHokens on
                    new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                    new { ptHoken.HpId, ptHoken.PtId, ptHoken.HokenId } into ptHokenJoins
                from ptHokenJoin in ptHokenJoins//.DefaultIfEmpty()
                            join receSeikyu in receSeikyus on
                    new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm } equals
                    new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuJoins
                from receSeikyuJoin in receSeikyuJoins.DefaultIfEmpty()
                select new
                {
                    receInf,
                    ptInf = ptInfJoin,
                    ptHokenInf = ptHokenJoin,
                    receSeikyu = receSeikyuJoin,
                    receStatus = receStatusJoin,
                    IsPaperRece = receStatusJoin == null ? 0 : receStatusJoin.IsPaperRece,
                }
            );

            if (tantoIdRaiin > 0 || kaIdRaiin > 0)
            {
                join = (
                    from receInf in receInfs
                    join kaikei_raiin in kaikei_raiins on
                        new { receInf.HpId, receInf.PtId, receInf.SinYm, receInf.HokenId } equals
                        new { kaikei_raiin.HpId, kaikei_raiin.PtId, kaikei_raiin.SinYm, kaikei_raiin.HokenId }
                    join receStatus in receStatusies on
                        new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                        new { receStatus.HpId, receStatus.PtId, receStatus.SeikyuYm, receStatus.HokenId, receStatus.SinYm } into receStatusJoins
                    from receStatusJoin in receStatusJoins.DefaultIfEmpty()
                    join ptInf in ptInfs on
                        new { receInf.HpId, receInf.PtId } equals
                        new { ptInf.HpId, ptInf.PtId } into ptInfJoins
                    from ptInfJoin in ptInfJoins //.DefaultIfEmpty()
                    join ptHoken in ptHokens on
                        new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                        new { ptHoken.HpId, ptHoken.PtId, ptHoken.HokenId } into ptHokenJoins
                    from ptHokenJoin in ptHokenJoins//.DefaultIfEmpty()
                    join receSeikyu in receSeikyus on
                        new { receInf.HpId, receInf.PtId, receInf.HokenId, receInf.SinYm } equals
                        new { receSeikyu.HpId, receSeikyu.PtId, receSeikyu.HokenId, receSeikyu.SinYm } into receSeikyuJoins
                    from receSeikyuJoin in receSeikyuJoins.DefaultIfEmpty()
                    select new
                    {
                        receInf,
                        ptInf = ptInfJoin,
                        ptHokenInf = ptHokenJoin,
                        receSeikyu = receSeikyuJoin,
                        receStatus = receStatusJoin,
                        IsPaperRece = receStatusJoin == null ? 0 : receStatusJoin.IsPaperRece,
                    }
                );
            }

            var entities = join.ToList().FindAll(p=>isPaper.Contains(p.IsPaperRece));                        

            List<ReceInfModel> results = new List<ReceInfModel>();

            List<HokenMst> hokenMsts =
                _tenantDataContext.HokenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PrefNo == 0 
                    ).Select(p => p).ToList();

            entities?.ForEach(entity =>
            {
                //var hokenMsts =
                //    _tenantDataContext.HokenMsts.FindListQueryableNoTrack(p =>
                //        p.HpId == hpId &&
                //        p.PrefNo == 0 &&
                //        p.HokenNo == entity.ptHokenInf.HokenNo &&
                //        p.HokenEdaNo == entity.ptHokenInf.HokenEdaNo &&
                //        p.StartDate <= entity.receInf.SinYm * 100 + 1 &&
                //        p.EndDate >= entity.receInf.SinYm * 100 + 1
                //        );
                HokenMst hokenMst = null;
                if (hokenMsts != null && hokenMsts.Any(p => 
                    p.HokenNo == entity.ptHokenInf.HokenNo &&
                    p.HokenEdaNo == entity.ptHokenInf.HokenEdaNo &&
                    p.StartDate <= entity.receInf.SinYm * 100 + 1 &&
                    p.EndDate >= entity.receInf.SinYm * 100 + 1))
                {
                    hokenMst = hokenMsts.First(p =>
                        p.HokenNo == entity.ptHokenInf.HokenNo &&
                        p.HokenEdaNo == entity.ptHokenInf.HokenEdaNo &&
                        p.StartDate <= entity.receInf.SinYm * 100 + 1 &&
                        p.EndDate >= entity.receInf.SinYm * 100 + 1);
                }

                int rousaiCount = 0;

                if (new int[] { 2, 3 }.Contains(mode))
                {
                    // 労災回数
                    rousaiCount = entity.ptHokenInf.RousaiReceCount;

                    var rousaiReceInfs =
                        _tenantDataContext.ReceInfs.FindListNoTrack(p =>
                            p.HpId == entity.receInf.HpId &&
                            p.PtId == entity.receInf.PtId &&
                            p.HokenId == entity.receInf.HokenId &&
                            p.SinYm >= entity.ptHokenInf.RyoyoStartDate / 100 &&
                            //p.SinYm <= entity.receInf.SinYm &&
                            //p.SeikyuYm <= seikyuYm &&
                            p.SinYm <= seikyuYm &&
                            p.Tensu > 0 &&
                            //seikyuKbns.Contains(p.SeikyuKbn) &&
                            hokenKbn.Contains(p.HokenKbn))
                        .GroupBy(p => new { HpId = p.HpId, PtId = p.PtId, SinYm = p.SinYm, HokenId = p.HokenId })
                        .Select(p => new { HpId = p.Key.HpId, PtId = p.Key.PtId, p.Key.SinYm, HokenId = p.Key.HokenId });
                    if (rousaiReceInfs != null && rousaiReceInfs.Any())
                    {
                        rousaiCount += rousaiReceInfs.Count();
                    }
                }

                if (rousaiCount >= countMin && rousaiCount <= countMax)
                {
                    results.Add(
                        new ReceInfModel(entity.receInf, entity.ptInf, entity.ptHokenInf, hokenMst, entity.receSeikyu, entity.receStatus, null, rousaiCount));
                }
            });

            return results;
        }

        /// <summary>
        /// レセコメント情報を取得する
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinYm"></param>
        /// <param name="hokenId"></param>
        /// <returns></returns>
        public List<ReceCmtModel> FindReceCmt(int hpId, long ptId, int sinYm, int hokenId, int hokenId2)
        {
            var entities =
                _tenantDataContext.ReceCmts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    (p.HokenId == hokenId || p.HokenId == hokenId2) &&
                    p.IsDeleted == DeleteStatus.None)                    
                    .OrderBy(p=>p.CmtKbn)
                    .ThenBy(p=>p.HokenId)
                    .ThenBy(p=>p.SeqNo)
                    .ToList();

            List<ReceCmtModel> results = new List<ReceCmtModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new ReceCmtModel(entity));
            });

            return results;
        }

        public List<SyojyoSyokiModel> FindSyoukiInf(int hpId, long ptId, int sinYm, int hokenId)
        {
            var entities =
                _tenantDataContext.SyoukiInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.HokenId == hokenId &&
                    p.IsDeleted == DeleteStatus.None)
                    .OrderBy(p => p.SortNo)
                    .ToList();

            List<SyojyoSyokiModel> results = new List<SyojyoSyokiModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new SyojyoSyokiModel(entity, _emrLogger));
            });

            return results;
        }

        public List<PtHokenInfModel> FindPtHokenInf(int hpId, long ptId, int hokenId)
        {
            var entities =
                _tenantDataContext.PtHokenInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.HokenId == hokenId &&
                    p.IsDeleted == DeleteStatus.None)
                    .ToList();

            List<PtHokenInfModel> results = new List<PtHokenInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new PtHokenInfModel(entity));
            });

            return results;
        }

        public PtKyuseiModel FindPtKyusei(int hpId, long ptId, int lastDate)
        {
            var entities =
                _tenantDataContext.PtKyuseis.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.EndDate >= lastDate &&
                    p.IsDeleted == DeleteStatus.None)
                    .OrderByDescending(p=>p.EndDate)
                    .ToList();

            PtKyuseiModel result = null;

            if(entities != null && entities.Any())
            {
                result = new PtKyuseiModel(entities.First());
            }

            return result;
        }

        /// <summary>
        /// 履歴管理
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinYm"></param>
        /// <param name="hokenId"></param>
        /// <returns></returns>
        public RecedenRirekiInfModel FindRecedenRirekiInf(int hpId, long ptId, int sinYm, int hokenId)
        {
            var entities =
                _tenantDataContext.RecedenRirekiInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.HokenId == hokenId &&
                    p.IsDeleted == DeleteStatus.None)
                    .ToList();

            RecedenRirekiInfModel result = null;

            if (entities != null && entities.Any())
            {
                result = new RecedenRirekiInfModel(entities.First());
            }

            return result;
        }

        /// <summary>
        /// 患者労災転帰事由情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinYm">診療月</param>
        /// <param name="hokenId">保険ID</param>
        /// <returns></returns>
        public PtRousaiTenki FindPtRousaiTenki(int hpId, long ptId, int sinYm, int hokenId)
        {
            var entities =
                _tenantDataContext.PtRousaiTenkis.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.HokenId == hokenId &&
                    p.EndDate >= sinYm &&
                    p.IsDeleted == DeleteStatus.None)
                    .OrderBy(p => p.EndDate)
                    .ToList();

            PtRousaiTenki result = null;

            if (entities != null && entities.Any())
            {
                result = entities.First();
            }

            return result;
        }

        /// <summary>
        /// 傷病の経過を取得する
        /// </summary>
        /// <param name="hpId">医療機関ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <param name="hokenId">保険番号</param>
        /// <returns></returns>
        public SyobyoKeikaModel FindSyobyoKeika(int hpId, long ptId, int sinYm, int hokenId)
        {
            var entities =
                _tenantDataContext.SyobyoKeikas.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.HokenId == hokenId &&
                    p.IsDeleted == DeleteStatus.None)
                .OrderBy(p=>p.SinYm)
                .ThenBy(p=>p.SinDay)
                .ThenBy(p=>p.SeqNo)
                .ToList();

            SyobyoKeikaModel result = null;

            if (entities != null && entities.Any())
            {
                result = new SyobyoKeikaModel(entities.First());
            }

            return result;
        }
        /// <summary>
        /// 傷病の経過を取得する
        /// </summary>
        /// <param name="hpId">医療機関ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <param name="hokenId">保険番号</param>
        /// <returns></returns>
        public SyobyoKeikaModel FindSyobyoKeikaForAfterCare(int hpId, long ptId, int sinDate, int hokenId)
        {
            var entities =
                _tenantDataContext.SyobyoKeikas.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinDate / 100 &&
                    p.SinDay == sinDate % 100 &&
                    p.HokenId == hokenId &&
                    p.IsDeleted == DeleteStatus.None)
                .OrderBy(p => p.SinYm)
                .ThenBy(p => p.SinDay)
                .ThenBy(p => p.SeqNo)
                .ToList();

            SyobyoKeikaModel result = null;

            if (entities != null && entities.Any())
            {
                result = new SyobyoKeikaModel(entities.First());
            }

            return result;
        }
        /// <summary>
        /// レセ負担区分
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="sinYm"></param>
        /// <returns></returns>
        public List<ReceFutanKbnModel> FindReceFutanKbn(int hpId, long ptId, int sinYm, int seikyuYm)
        {
            var receFutanKbns =
                _tenantDataContext.ReceFutanKbns.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm);

            if(seikyuYm > 0)
            {
                receFutanKbns = receFutanKbns.Where(p => p.SeikyuYm == seikyuYm);
            }

            var entities = receFutanKbns.OrderByDescending(p => p.SeikyuYm)
                    .ToList();

            //var entities =
            //    _tenantDataContext.ReceFutanKbnRepository.FindListQueryableNoTrack(p =>
            //        p.HpId == hpId &&
            //        p.PtId == ptId &&
            //        (seikyuYm > 0 ? p.SeikyuYm == seikyuYm : true) &&
            //        p.SinYm == sinYm)
            //        .OrderByDescending(p=>p.SeikyuYm)
            //        .ToList();

            List<ReceFutanKbnModel> results = new List<ReceFutanKbnModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new ReceFutanKbnModel(entity));
            });

            return results;
        }
        public int ZenkaiKensaDate(int HpId, long ptId, int sinDate, int hokenId)
        {
            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == HpId &&
                o.PtId == ptId &&
                o.SinDate <= sinDate &&
                o.OdrKouiKbn >= OdrKouiKbnConst.KensaMin &&
                o.OdrKouiKbn <= OdrKouiKbnConst.GazoMax &&
                o.IsDeleted == DeleteStatus.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(o =>
                o.HpId == HpId &&
                o.HokenId == hokenId);
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(r =>
                r.HpId == HpId &&
                r.PtId == ptId &&
                r.SinDate <= sinDate &&
                r.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from odrInf in odrInfs
                join PtHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                    new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
                join RaiinInf in raiinInfs on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo } equals
                    new { RaiinInf.HpId, RaiinInf.PtId, RaiinInf.RaiinNo }
                where
                    odrInf.HpId == HpId &&
                    odrInf.PtId == ptId &&
                    odrInf.IsDeleted == DeleteStatus.None
                group odrInf by odrInf.SinDate into A
                orderby
                    A.Key descending
                select new
                {
                    SinDate = A.Key
                }
            );
            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());
            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    data.SinDate
                )
                .ToList();

            List<int> results = new List<int>();

            entities?.ForEach(entity => {
                results.Add(entity);
            });

            int ret = 0;
            if (results.Any())
            {
                ret = results.First();
            }

            return ret;
        }
    }
}
