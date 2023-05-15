using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.Common.DB
{
    public class CoHokenMstFinder : RepositoryBase, ICoHokenMstFinder
    {

        public CoHokenMstFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public List<CoKohiHoubetuMstModel> GetKohiHoubetuMst(int hpId, int seikyuYm)
        {
            int sinDate = seikyuYm * 100 + 31;

            var hokenMstAll = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId);
            //請求月末日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(h =>
                h.HpId == hpId &&
                h.StartDate <= sinDate &&
                new int[] { HokenSbtKbn.Seiho, HokenSbtKbn.Bunten, HokenSbtKbn.Ippan }.Contains(h.HokenSbtKbn)
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
            var hokenMsts = (
                from hokenMst in hokenMstAll
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                group new
                {
                    hokenMst.PrefNo,
                    hokenMst.Houbetu,
                    hokenMst.HokenNameCd
                } by new { hokenMst.PrefNo, hokenMst.Houbetu, hokenMst.HokenNameCd } into hokenGroup
                select new
                {
                    hokenGroup.Key.PrefNo,
                    hokenGroup.Key.Houbetu,
                    hokenGroup.Key.HokenNameCd
                }
            ).ToList();

            return
                hokenMsts.Select(
                    x => new CoKohiHoubetuMstModel()
                    {
                        PrefNo = x.PrefNo,
                        Houbetu = x.Houbetu,
                        HokenNameCd = x.HokenNameCd
                    }
                ).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
