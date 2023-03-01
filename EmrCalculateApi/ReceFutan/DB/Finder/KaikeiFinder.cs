using EmrCalculateApi.ReceFutan.Models;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using PostgreDataContext;

namespace EmrCalculateApi.ReceFutan.DB.Finder
{
    class KaikeiFinder
    {
        private readonly TenantDataContext _tenantDataContext;
        public KaikeiFinder(TenantDataContext tenantDataContext)
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
        public List<KaikeiDetailModel> FindKaikeiDetail(int hpId, long ptId, int sinYm)
        {
            int fromSinDate = sinYm * 100 + 1;
            int toSinDate = sinYm * 100 + 99;

            var kaikeiDetails = _tenantDataContext.KaikeiDetails.FindListQueryableNoTrack();
            var raiinInfs = _tenantDataContext.RaiinInfs.FindListQueryableNoTrack();

            var joinQuery = (
                from kaikeiDetail in kaikeiDetails
                join raiinInf in raiinInfs on
                    new { kaikeiDetail.HpId, kaikeiDetail.PtId, kaikeiDetail.RaiinNo } equals
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
                where
                    kaikeiDetail.HpId == hpId &&
                    kaikeiDetail.SinDate >= fromSinDate &&
                    kaikeiDetail.SinDate <= toSinDate
                orderby
                    kaikeiDetail.HpId, kaikeiDetail.PtId, kaikeiDetail.SortKey
                select new
                {
                    kaikeiDetail,
                    SeikyuKbn = 0,
                    raiinInf.KaId,
                    raiinInf.TantoId
                }

            );

            //患者指定
            if (ptId > 0)
            {
                joinQuery = joinQuery.Where(k => k.kaikeiDetail.PtId == ptId);
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
            return result;
        }
    }
}
