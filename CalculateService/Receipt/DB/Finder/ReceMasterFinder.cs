using CalculateService.Constants;
using CalculateService.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using CalculateService.Ika.Models;
using Helper.Common;
using Domain.Constant;
using CalculateService.Interface;
using CalculateService.Receipt.Models;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace CalculateService.Receipt.DB.Finder
{
    public class ReceMasterFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public ReceMasterFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 医療機関情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <returns></returns>
        public HpInfModel FindHpInf(int hpId, int sinDate)
        {
            return new HpInfModel(
                _tenantDataContext.HpInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate)
                    .OrderByDescending(p => p.StartDate)
                    .FirstOrDefault());
        }

        /// <summary>
        /// 単位マスタを取得する
        /// </summary>
        /// <param name="sinDate"></param>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public UnitMstModel FindUnitMst(int hpId, int sinDate, string Unit)
        {
            return new UnitMstModel(
                _tenantDataContext.UnitMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.UnitName == Unit)
                    .FirstOrDefault());
        }
        public List<JyusinbiDataModel> FindReceInfJd(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
        {
            var receInfJds = _tenantDataContext.ReceInfJds.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SeikyuYm == seikyuYm &&
                p.SinYm == sinYm &&
                p.HokenId == hokenId
            );
            var entities = receInfJds.AsEnumerable().Select(
            data =>
                new ReceInfJdModel(data)
            ).ToList();

            List<JyusinbiDataModel> results = new List<JyusinbiDataModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new JyusinbiDataModel(new ReceInfJdModel(entity.ReceInfJd)));
            });

            return results;
        }

        public List<EFRaiinInfModel> FindRaiinDatas(int hpId, int sinYm)
        {
            var sinKouiCounts = _tenantDataContext.SinKouiCounts.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.SinYm == sinYm
            );

            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.SinDate >= sinYm * 100 + 1 &&
                p.SinDate <= sinYm * 100 + 31 &&
                p.IsDeleted == DeleteStatus.None
            );

            var kaMsts = _tenantDataContext.KaMsts.FindListQueryableNoTrack(p =>
                p.HpId == hpId &&
                p.IsDeleted == DeleteStatus.None
            );

            var KaYousikis = _tenantDataContext.KacodeReceYousikis.FindListQueryableNoTrack();

            var join =
                (
                    from sinKoui in sinKouiCounts
                    join raiinInf in raiinInfs on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.RaiinNo } equals
                        new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo } into raiinJoins
                    from raiinJoin in raiinJoins
                    join kaMst in kaMsts on
                        new { raiinJoin.HpId, raiinJoin.KaId } equals
                        new { kaMst.HpId, kaMst.KaId } into kaMstJoins
                    from kaJoin in kaMstJoins.DefaultIfEmpty()
                    join kaYousiki in KaYousikis on
                        new { kaCode = kaJoin.ReceKaCd } equals
                        new { kaCode = kaYousiki.ReceKaCd } into kaYousikiJoins
                    from kaYousikiJoin in kaYousikiJoins.DefaultIfEmpty()
                    select new
                    {
                        sinKoui,
                        raiinJoin,
                        kaJoin,
                        kaYousikiJoin
                    }
                ).ToList();

            List<EFRaiinInfModel> results =
                new List<EFRaiinInfModel>();

            join?.ForEach(p =>
            {
                results.Add(new EFRaiinInfModel(p.raiinJoin, p.sinKoui, p.kaJoin, p.kaYousikiJoin));
            }
            );

            return results;
        }

        /// <summary>
        /// EFファイル用施設コードを取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <returns></returns>
        public string GetSisetuCd(int hpId, int sinDate)
        {
            string ret = string.Empty;
            var gene =
                _tenantDataContext.SystemGenerationConfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate &&
                    p.GrpCd == 100001 &&
                    p.GrpEdaNo == 0)
                .OrderBy(p => p.StartDate)
                .FirstOrDefault();

            if (gene != null)
            {
                ret = gene.Param;
            }
            var hpInf =
                _tenantDataContext.HpInfs.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate)
                    .OrderByDescending(p => p.StartDate)
                    .FirstOrDefault();

            if (hpInf != null)
            {
                if (ret == string.Empty)
                {
                    ret = hpInf.HpCd;
                }

                ret = hpInf.PrefNo.ToString().PadLeft(2, '0') + ret.PadLeft(7, '0');
            }
            else
            {
                ret = "000000000";
            }

            return ret;
        }
    }
}
