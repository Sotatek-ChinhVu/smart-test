using Domain.Constant;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Futan.Models;
using PostgreDataContext;

namespace EmrCalculateApi.Futan.DB.Finder
{
    class OdrInfFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        public OdrInfFinder(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        /// <summary>
        /// 同月内のオーダー情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<OdrInfModel> FindOdrInf(int hpId, long ptId, int sinDate)
        {
            int fromSinDate = sinDate / 100 * 100 + 1;
            int toSinDate = sinDate;

            var odrInfs = _tenantDataContext.OdrInfs.FindListQueryableNoTrack();
            var odrDetails = _tenantDataContext.OdrInfDetails.FindListQueryableNoTrack();
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.FindListQueryableNoTrack(
                p => p.IsDeleted == DeleteStatus.None
            );

            var joinQuery = (
                from odrInf in odrInfs
                join odrDetail in odrDetails on
                    new { odrInf.HpId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrDetail.HpId, odrDetail.RaiinNo, odrDetail.RpNo, odrDetail.RpEdaNo }
                join ptHokenPattern in ptHokenPatterns on
                    new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid }
                where
                            odrInf.HpId == hpId &&
                            odrInf.PtId == ptId &&
                            odrInf.SinDate >= fromSinDate &&
                            odrInf.SinDate <= toSinDate &&
                            odrInf.IsDeleted == DeleteStatus.None
                orderby
                    odrDetail.SinDate, odrDetail.RaiinNo
                select new
                {
                    odrInf,
                    odrDetail,
                    ptHokenPattern
                }
            );

            var result = joinQuery.AsEnumerable().Select(
                data =>
                    new OdrInfModel(
                        data.odrInf,
                        data.odrDetail,
                        data.ptHokenPattern
                    )
                )
                .ToList();
            return result;
        }
    }
}
