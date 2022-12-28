using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Futan.DB.Finder;
using EmrCalculateApi.Receipt.Models;

namespace EmrCalculateApi.Receipt.DB.Finder
{
    class HokenFinder 
    {
        FutancalcFinder _futanCalcFinder;
        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public HokenFinder(TenantDataContext tenantDataContext, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _futanCalcFinder = new FutancalcFinder(tenantDataContext);
            _tenantDataContext = tenantDataContext;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        public HokenDataModel FindHokenData(int hpId, long ptId, int hokenId)
        {
            if (hokenId == 0)
            {
                return null;
            }

            var ptHokenInfs = _tenantDataContext.PtHokenInfs.FindListQueryableNoTrack();

            var joinQuery = (
                from ptHokenInf in ptHokenInfs
                where
                    ptHokenInf.HpId == hpId &&
                    ptHokenInf.PtId == ptId &&
                    ptHokenInf.HokenId == hokenId &&
                    ptHokenInf.IsDeleted == DeleteStatus.None
                select new
                {
                    ptHokenInf,
                }
            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new HokenDataModel(
                        data.ptHokenInf
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
        /// <param name="sinDate">診療日</param>
        /// <param name="hokensyaNo">保険者番号</param>
        /// <returns></returns>
        public List<KohiDataModel> FindKohiData(int hpId, long ptId, int sinDate)
        {
            var ptKohis = _tenantDataContext.PtKohis.FindListQueryableNoTrack();
            var hokenMsts =
                _tenantDataContext.HokenMsts.FindListQueryableNoTrack(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate &&
                    p.EndDate >= sinDate
                    );

            var joinQuery = (
                from ptKohi in ptKohis
                join hokenMst in hokenMsts on
                    new { ptKohi.HpId, ptKohi.PrefNo, ptKohi.HokenNo, ptKohi.HokenEdaNo } equals
                    new { hokenMst.HpId, hokenMst.PrefNo, hokenMst.HokenNo, hokenMst.HokenEdaNo } into hokenMstJoins
                from hokenMstJoin in hokenMstJoins.DefaultIfEmpty()
                where
                    ptKohi.HpId == hpId &&
                    ptKohi.PtId == ptId &&
                    ptKohi.IsDeleted == DeleteStatus.None
                select new
                {
                    ptKohi,
                    hokenMstJoin
                }
            );

            return joinQuery.AsEnumerable().Select(
                data =>
                    new KohiDataModel(
                        data.ptKohi, data.hokenMstJoin
                    )
                )
                .ToList();
        }
    }
}
