using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using PostgreDataContext;
using Reporting.Byomei.Models;

namespace Reporting.Byomei.DB
{
    public class CoPtByomeiFinder
    {
        private readonly int HpId = Session.HospitalID;
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        public CoPtByomeiFinder(TenantNoTrackingDataContext tenantNoTrackingDataContext)
        {
            _tenantNoTrackingDataContext = tenantNoTrackingDataContext;
        }

        /// <summary>
        /// 患者情報を取得する
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <returns>患者情報</returns>
        public PtInf FindPtInf(long ptId)
        {
            return _tenantNoTrackingDataContext.PtInfs.FirstOrDefault(p =>
                     p.HpId == HpId &&
                     p.PtId == ptId &&
                     p.IsDelete == DeleteStatus.None) ?? new PtInf();
        }

        /// <summary>
        /// 患者病名を取得する
        /// 取得期間開始日～終了日の間に有効な病名を取得する
        /// 指定した保険IDと共通病名（保険ID=0）の病名を取得する
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="fromDay">取得期間開始日</param>
        /// <param name="toDay">取得期間終了日</param>
        /// <param name="tenkiIn">true:転帰病名も取得する</param>
        /// <param name="hokenIds">保険ID</param>
        /// <returns>患者病名</returns>
        public List<PtByomei> GetPtByomei(long ptId, int fromDay, int toDay,
            bool tenkiIn, List<int> hokenIds)
        {
            // 共通病名は常に
            List<int> tgtHokenIds = new List<int>();
            tgtHokenIds.AddRange(hokenIds);
            tgtHokenIds.Add(0);

            return _tenantNoTrackingDataContext.PtByomeis.Where(p =>
                             p.HpId == HpId &&
                             p.PtId == ptId &&
                             (tenkiIn == false ? p.TenkiKbn <= TenkiKbnConst.Continued : true) &&
                             (p.StartDate <= toDay && (p.TenkiKbn <= TenkiKbnConst.Continued || p.TenkiDate >= fromDay)) &&
                             tgtHokenIds.Contains(p.HokenPid) &&
                             p.IsDeleted == DeleteTypes.None
                ).OrderBy(p => p.StartDate)
                    .ThenBy(p => p.TenkiDate)
                    .ThenBy(p => p.Byomei)
                    .ThenBy(p => p.SyubyoKbn).ToList();
        }

        /// <summary>
        /// 患者保険情報を取得する
        /// </summary>
        /// <param name="ptId">患者ID</param>
        /// <param name="hokenIds">保険ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns></returns>
        public List<CoPtHokenInfModel> GetPtHokenInf(long ptId, List<int> hokenIds, int sinDate)
        {
            var hokenMsts = _tenantNoTrackingDataContext.HokenMsts.Where(p => p.PrefNo == 0 && new int[] { 0, 1, 3, 4, 8 }.Contains(p.HokenSbtKbn));
            //診療日基準で保険番号マスタのキー情報を取得
            var hokenMstKeys = _tenantNoTrackingDataContext.HokenMsts.Where(
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
            var ptHokenInfs = _tenantNoTrackingDataContext.PtHokenInfs.Where(p =>
                    p.HpId == HpId &&
                    p.PtId == ptId &&
                    hokenIds.Contains(p.HokenId));

            var houbetuMsts = (
                from hokenMst in hokenMsts
                join hokenKey in hokenMstKeys on
                    new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo, hokenMst.PrefNo, hokenMst.StartDate } equals
                    new { hokenKey.HpId, hokenKey.HokenNo, hokenKey.HokenEdaNo, hokenKey.PrefNo, hokenKey.StartDate }
                select new
                {
                    hokenMst
                }
            );

            var result = (
                from ptHokenInf in ptHokenInfs
                join houbetuMst in houbetuMsts on
                    new { ptHokenInf.HpId, ptHokenInf.HokenNo, ptHokenInf.HokenEdaNo } equals
                    new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo }
                where
                    ptHokenInf.HpId == HpId &&
                    ptHokenInf.PtId == ptId &&
                    ptHokenInf.IsDeleted == DeleteStatus.None
                select new CoPtHokenInfModel(ptHokenInf , houbetuMst.hokenMst)
            ).ToList();

            return result;
        }
    }
}
