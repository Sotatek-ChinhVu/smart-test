using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.CommonMasters.Config;
using Reporting.Yakutai.Model;

namespace Reporting.Yakutai.DB
{
    public class CoYakutaiFinder : RepositoryBase, ICoYakutaiFinder
    {
        private readonly ISystemConfig _systemConfig;
        public CoYakutaiFinder(ISystemConfig systemConfig, ITenantProvider tenantProvider) : base(tenantProvider)
        {
            _systemConfig = systemConfig;
        }

        public void ReleaseResource()
        {
            _systemConfig.ReleaseResource();
            DisposeDataContext();
        }

        /// <summary>
        /// 医療機関情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <returns></returns>
        public CoHpInfModel FindHpInf(int hpId, int sinDate)
        {
            return new CoHpInfModel(
                NoTrackingDataContext.HpInfs.Where(p =>
                    p.HpId == hpId &&
                    p.StartDate <= sinDate)
                    .OrderByDescending(p => p.StartDate)
                    .FirstOrDefault() ?? new());
        }

        /// <summary>
        /// 患者情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>患者情報</returns>
        public CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate)
        {

            var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
                     p.HpId == hpId &&
                     p.PtId == ptId
                );

            var ptCmts = NoTrackingDataContext.PtCmtInfs.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.IsDeleted == DeleteStatus.None
                );

            var join = (

                    from ptInf in ptInfs
                    join ptCmt in ptCmts on
                        new { ptInf.HpId, ptInf.PtId } equals
                        new { ptCmt.HpId, ptCmt.PtId } into ptCmtJoins
                    from ptCmtJoin in ptCmtJoins.DefaultIfEmpty()
                    select new
                    {
                        ptInf,
                        ptCmtJoin
                    }

                );

            var entities = join.AsEnumerable().Select(
                data =>
                    new CoPtInfModel(data.ptInf, sinDate)
                )
                .ToList();

            List<CoPtInfModel> results = new();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new CoPtInfModel(
                        entity.PtInf,
                        entity.SinDate
                    ));
            });

            return results.FirstOrDefault() ?? new();
        }

        /// <summary>
        /// 来院情報取得に診療科マスタとユーザーマスタを結合したデータを取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns>
        /// 指定の患者の指定の診療日の来院情報
        /// SIN_START_TIME順にソート
        /// </returns>
        public CoRaiinInfModel FindRaiinInfData(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var kaMsts = NoTrackingDataContext.KaMsts.Where(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var userMsts = NoTrackingDataContext.UserMsts.Where(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo == raiinNo &&
                p.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from raiinInf in raiinInfs
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into kaJoin
                from ka in kaJoin.DefaultIfEmpty()
                join userMst in userMsts on
                    new { raiinInf.HpId, raiinInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into userJoin
                from user in userJoin.DefaultIfEmpty()
                where
                    raiinInf.HpId == hpId &&
                    raiinInf.PtId == ptId &&
                    raiinInf.SinDate == sinDate &&
                    raiinInf.IsDeleted == DeleteStatus.None
                orderby
                    raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, ("0000" + raiinInf.SinStartTime).Substring((raiinInf.SinStartTime ?? string.Empty).Length, 4), raiinInf.OyaRaiinNo, raiinInf.RaiinNo
                select new
                {
                    raiinInf,
                    kaMst = ka,
                    userMst = user
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoRaiinInfModel(data.raiinInf, data.kaMst, data.userMst)
                )
                .ToList();

            List<CoRaiinInfModel> results = new List<CoRaiinInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CoRaiinInfModel(entity.RaiinInf, entity.KaMst, entity.UserMst));
            });

            return results.FirstOrDefault() ?? new();
        }
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
        public List<CoOdrInfModel> FindOdrInf(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo == raiinNo &&
                o.InoutKbn == 0 &&
                new int[] { 21, 22, 23, 28 }.Contains(o.OdrKouiKbn) &&
                o.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from odrInf in odrInfs
                where
                    odrInf.HpId == hpId &&
                    odrInf.PtId == ptId &&
                    odrInf.IsDeleted == DeleteStatus.None
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.InoutKbn, odrInf.TosekiKbn, odrInf.SikyuKbn, odrInf.SortNo, odrInf.RpNo, odrInf.RpEdaNo
                select new
                {
                    odrInf
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoOdrInfModel(data.odrInf)
                )
                .ToList();

            List<CoOdrInfModel> results = new List<CoOdrInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CoOdrInfModel(entity.OdrInf));
            });

            return results;
        }

        /// <summary>
        /// オーダー詳細情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID </param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        public List<CoOdrInfDetailModel> FindOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo == raiinNo &&
            o.InoutKbn == 0 &&
                new int[] { 21, 22, 23, 28 }.Contains(o.OdrKouiKbn) &&
                o.IsDeleted == DeleteStatus.None);

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
            o.SinDate == sinDate &&
                !(o.ItemCd != null && o.ItemCd.StartsWith("8") && o.ItemCd.Length == 9));
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
            t.HpId == hpId &&
            t.StartDate <= sinDate &&
                (t.EndDate >= sinDate || t.EndDate == 12341234));
            var yohoInfMsts = NoTrackingDataContext.YohoInfMsts.Where(y =>
                y.HpId == hpId
            );

            var joinQuery = (
                from odrInf in odrInfs
                join odrInfDetail in odrInfDetails on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                join tenMst in tenMsts on
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd ?? string.Empty.Trim() } equals
                    new { tenMst.HpId, tenMst.ItemCd } into oJoin
                from oj in oJoin.DefaultIfEmpty()
                join yohoInfMst in yohoInfMsts on
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd ?? string.Empty.Trim() } equals
                    new { yohoInfMst.HpId, yohoInfMst.ItemCd } into yJoin
                from yj in yJoin.DefaultIfEmpty()
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.InoutKbn, odrInf.TosekiKbn, odrInf.SikyuKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
                select new
                {
                    odrInfDetail,
                    odrInf,
                    tenMst = oj,
                    yohoInfMst = yj
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoOdrInfDetailModel(
                        data.odrInfDetail,
                        data.odrInf,
                        data.tenMst,
                        data.yohoInfMst,
                        _systemConfig
                    )
                )
                .ToList();
            List<CoOdrInfDetailModel> results = new List<CoOdrInfDetailModel>();

            entities?.ForEach(entity =>
            {
                if (!(entity.TenMst != null && entity.TenMst.IsNodspYakutai == 1))
                {
                    results.Add(
                        new CoOdrInfDetailModel(
                            entity.OdrInfDetail,
                            entity.OdrInf,
                            entity.TenMst ?? new(),
                            entity.YohoInfMst,
                            _systemConfig
                            ));
                }
            }
            );

            return results;
        }
        public List<CoSingleDoseMstModel> FindSingleDoseMst(int hpId)
        {

            var singleDoses = NoTrackingDataContext.SingleDoseMsts.Where(p =>
                     p.HpId == hpId
                ).ToList();

            List<CoSingleDoseMstModel> results = new List<CoSingleDoseMstModel>();

            singleDoses?.ForEach(entity =>
            {
                results.Add(
                    new CoSingleDoseMstModel(
                        entity
                    ));
            });

            return results;
        }
    }
}
