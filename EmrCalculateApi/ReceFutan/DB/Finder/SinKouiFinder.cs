using EmrCalculateApi.ReceFutan.Models;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using PostgreDataContext;

namespace EmrCalculateApi.ReceFutan.DB.Finder
{
    class SinKouiFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        public SinKouiFinder(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        /// <summary>
        /// 会計情報の取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptIds">患者ID</param>
        /// <param name="sinYm">診療年月</param>
        /// <returns></returns>
        public bool IsSinKouiReceKisai(int hpId, long ptId, int sinYm, int hokenId, int hokenId2, int kohiId)
        {
            var sinKouis = _tenantDataContext.SinKouis.FindListQueryableNoTrack();
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(p =>
                (p.HokenId == hokenId || p.HokenId == hokenId2) &&
                (p.Kohi1Id == kohiId || p.Kohi2Id == kohiId || p.Kohi3Id == kohiId || p.Kohi4Id == kohiId)
            );

            var joinQuery = (
                from sinKoui in sinKouis
                join ptHokenPattern in ptHokenPatterns on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                where
                    sinKoui.HpId == hpId &&
                    sinKoui.PtId == ptId &&
                    sinKoui.SinYm == sinYm &&
                    sinKoui.IsDeleted == DeleteStatus.None &&
                    sinKoui.IsNodspRece == 0 &&
                    sinKoui.InoutKbn == 0 &&
                    sinKoui.DetailData != null
                select
                    sinKoui
            );

            return
                joinQuery.Count() >= 1;
        }
    }
}
