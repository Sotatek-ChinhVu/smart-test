using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using PostgreDataContext;
using Reporting.Sijisen.Model;

namespace Reporting.Sijisen.DB
{
    public class CoSijisenFinder : RepositoryBase, ICoSijisenFinder
    {
        public CoSijisenFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
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
                    new CoPtInfModel(data.ptInf, sinDate, data.ptCmtJoin, null, null, null)
                )
                .ToList();

            List<CoPtInfModel> results = new List<CoPtInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new CoPtInfModel(
                        entity.PtInf,
                        entity.SinDate,
                        entity.PtCmtInf,
                        FindPtAlrgyDrug(hpId, ptId, sinDate),
                        FindPtAlrgyFood(hpId, ptId, sinDate),
                        FindPtAlrgyElse(hpId, ptId, sinDate)
                    ));
            });

            return results.FirstOrDefault();
        }
        /// <summary>
        /// 患者薬剤アレルギー情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <returns></returns>
        private List<CoPtAlrgyDrugModel> FindPtAlrgyDrug(int hpId, long ptId, int sinDate)
        {
            var ptAlrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.StartDate <= sinDate &&
                    (p.EndDate >= sinDate || p.EndDate == 0) &&
                    p.IsDeleted == DeleteStatus.None
                ).ToList();

            List<CoPtAlrgyDrugModel> results = new List<CoPtAlrgyDrugModel>();

            if (ptAlrgyDrugs != null)
            {
                foreach (PtAlrgyDrug ptAlrgyDrug in ptAlrgyDrugs)
                {
                    results.Add(new CoPtAlrgyDrugModel(ptAlrgyDrug));
                }
            }

            return results;
        }
        /// <summary>
        /// 患者食物アレルギー情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <returns></returns>
        private List<CoPtAlrgyFoodModel> FindPtAlrgyFood(int hpId, long ptId, int sinDate)
        {
            var ptAlrgyFoods = NoTrackingDataContext.PtAlrgyFoods.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.StartDate <= sinDate &&
                    (p.EndDate >= sinDate || p.EndDate == 0) &&
                    p.IsDeleted == DeleteStatus.None
                );
            var foodAlrgyKbns = NoTrackingDataContext.M12FoodAlrgyKbn.Where(m => m.HpId == hpId);

            var join = (

                    from ptAlrgyFood in ptAlrgyFoods
                    join foodAlrgyKbn in foodAlrgyKbns on
                        new { ptAlrgyFood.AlrgyKbn } equals
                        new { AlrgyKbn = foodAlrgyKbn.FoodKbn }
                    select new
                    {
                        ptAlrgyFood,
                        foodAlrgyKbn
                    }
                );

            var entities = join.AsEnumerable().Select(
                data =>
                    new CoPtAlrgyFoodModel(data.ptAlrgyFood, data.foodAlrgyKbn)
                )
                .ToList();

            List<CoPtAlrgyFoodModel> results = new List<CoPtAlrgyFoodModel>();

            entities?.ForEach(entity =>
            {
                results.Add(
                    new CoPtAlrgyFoodModel(
                        entity.PtAlrgyFood,
                        entity.FoodAlrgyKbn
                    ));
            });

            return results;
        }
        /// <summary>
        /// 患者アレルギー情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <returns></returns>
        private List<CoPtAlrgyElseModel> FindPtAlrgyElse(int hpId, long ptId, int sinDate)
        {
            var ptAlrgyElses = NoTrackingDataContext.PtAlrgyElses.Where(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.StartDate <= sinDate &&
                    (p.EndDate >= sinDate || p.EndDate == 0) &&
                    p.IsDeleted == DeleteStatus.None
                ).ToList();

            List<CoPtAlrgyElseModel> results = new List<CoPtAlrgyElseModel>();

            if (ptAlrgyElses != null)
            {
                foreach (PtAlrgyElse ptAlrgyElse in ptAlrgyElses)
                {
                    results.Add(new CoPtAlrgyElseModel(ptAlrgyElse));
                }
            }

            return results;
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
            var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts.Where(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var raiinCmtInfs = NoTrackingDataContext.RaiinCmtInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.CmtKbn == 1 &&
                o.RaiinNo == raiinNo
                );
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo == raiinNo &&
                //p.Status >= 5 &&
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
                join uketukeSbtMst in uketukeSbtMsts on
                    new { raiinInf.HpId, raiinInf.UketukeSbt } equals
                    new { uketukeSbtMst.HpId, UketukeSbt = uketukeSbtMst.KbnId } into uketukeJoins
                from uketukeJoin in uketukeJoins.DefaultIfEmpty()
                join raiinCmtInf in raiinCmtInfs on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo } equals
                    new { raiinCmtInf.HpId, raiinCmtInf.PtId, raiinCmtInf.RaiinNo } into raiinCmtInfJoins
                from raiinCmtInfJoin in raiinCmtInfJoins.DefaultIfEmpty()
                where
                    raiinInf.HpId == hpId &&
                    raiinInf.PtId == ptId &&
                    raiinInf.SinDate == sinDate &&
                    raiinInf.IsDeleted == DeleteStatus.None
                orderby
                    raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, ("0000" + raiinInf.SinStartTime).Substring(raiinInf.SinStartTime.Length, 4), raiinInf.OyaRaiinNo, raiinInf.RaiinNo
                select new
                {
                    raiinInf,
                    kaMst = ka,
                    userMst = user,
                    uketukeMst = uketukeJoin,
                    raiinCmtInf = raiinCmtInfJoin
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoRaiinInfModel(data.raiinInf, data.kaMst, data.userMst, data.uketukeMst, data.raiinCmtInf)
                )
                .ToList();

            List<CoRaiinInfModel> results = new List<CoRaiinInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CoRaiinInfModel(entity.RaiinInf, entity.KaMst, entity.UserMst, entity.UketukeSbtMst, entity.RaiinCmtInf));
            });

            return results.FirstOrDefault();
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
        public List<CoRaiinInfModel> FindOtherRaiinInfData(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var kaMsts = NoTrackingDataContext.KaMsts.Where(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var userMsts = NoTrackingDataContext.UserMsts.Where(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts.Where(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var raiinCmtInfs = NoTrackingDataContext.RaiinCmtInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.CmtKbn == 1 &&
                o.RaiinNo != raiinNo
                );
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo != raiinNo &&
                //p.Status >= 5 &&
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
                join uketukeSbtMst in uketukeSbtMsts on
                    new { raiinInf.HpId, raiinInf.UketukeSbt } equals
                    new { uketukeSbtMst.HpId, UketukeSbt = uketukeSbtMst.KbnId } into uketukeJoins
                from uketukeJoin in uketukeJoins.DefaultIfEmpty()
                join raiinCmtInf in raiinCmtInfs on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo } equals
                    new { raiinCmtInf.HpId, raiinCmtInf.PtId, raiinCmtInf.RaiinNo } into raiinCmtInfJoins
                from raiinCmtInfJoin in raiinCmtInfJoins.DefaultIfEmpty()
                where
                    raiinInf.HpId == hpId &&
                    raiinInf.PtId == ptId &&
                    raiinInf.SinDate == sinDate &&
                    raiinInf.IsDeleted == DeleteStatus.None
                orderby
                    raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, ("0000" + raiinInf.SinStartTime).Substring(raiinInf.SinStartTime.Length, 4), raiinInf.OyaRaiinNo, raiinInf.RaiinNo
                select new
                {
                    raiinInf,
                    kaMst = ka,
                    userMst = user,
                    uketukeMst = uketukeJoin,
                    raiinCmtInf = raiinCmtInfJoin
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoRaiinInfModel(data.raiinInf, data.kaMst, data.userMst, data.uketukeMst, data.raiinCmtInf)
                )
                .ToList();

            List<CoRaiinInfModel> results = new List<CoRaiinInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CoRaiinInfModel(entity.RaiinInf, entity.KaMst, entity.UserMst, entity.UketukeSbtMst, entity.RaiinCmtInf));
            });

            return results;
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
        public List<CoOdrInfModel> FindOdrInf(int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns)
        {
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo == raiinNo &&
                o.IsDeleted == DeleteStatus.None);

            // 絞り込み
            var notAndConditions = odrInfs;
            foreach ((int from, int to) odrKouiKbn in odrKouiKbns)
            {
                notAndConditions =
                    notAndConditions.Where(p => !(p.OdrKouiKbn >= odrKouiKbn.from && p.OdrKouiKbn <= odrKouiKbn.to));
            }
            odrInfs = odrInfs.Except(notAndConditions);

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
        public List<CoOdrInfDetailModel> FindOdrInfDetail(int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns)
        {
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo == raiinNo &&
                o.IsDeleted == DeleteStatus.None);

            // 絞り込み
            var notAndConditions = odrInfs;
            foreach ((int from, int to) odrKouiKbn in odrKouiKbns)
            {
                notAndConditions =
                    notAndConditions.Where(p => !(p.OdrKouiKbn >= odrKouiKbn.from && p.OdrKouiKbn <= odrKouiKbn.to));
            }
            odrInfs = odrInfs.Except(notAndConditions);

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate);
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
                t.HpId == hpId &&
                t.StartDate <= sinDate &&
                (t.EndDate >= sinDate || t.EndDate == 12341234) &&
                t.ItemCd != null &&
                t.ItemCd.Length > 0);
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(k =>
                    k.HpId == hpId &&
                    k.IsDelete == DeleteStatus.None
                );
            var materialMsts = NoTrackingDataContext.MaterialMsts.Where(m =>
                    m.HpId == hpId
                );
            var containerMsts = NoTrackingDataContext.ContainerMsts.Where(c =>
                    c.HpId == hpId
                );

            var joinQuery = (
                from odrInf in odrInfs
                join odrInfDetail in odrInfDetails on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                join tenMst in tenMsts on
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd.Trim() } equals
                    new { tenMst.HpId, tenMst.ItemCd } into oJoin
                from oj in oJoin.DefaultIfEmpty()
                join kensaMst in kensaMsts on
                    new { oj.HpId, oj.KensaItemCd, oj.KensaItemSeqNo } equals
                    new { kensaMst.HpId, kensaMst.KensaItemCd, kensaMst.KensaItemSeqNo } into kensaMstJoins
                from kensaMstJoin in kensaMstJoins.DefaultIfEmpty()
                join materialMst in materialMsts on
                    new { kensaMstJoin.HpId, MaterialCd = (long)kensaMstJoin.MaterialCd } equals
                    new { materialMst.HpId, materialMst.MaterialCd } into materialMstJoins
                from materialMstJoin in materialMstJoins.DefaultIfEmpty()
                join containerMst in containerMsts on
                    new { kensaMstJoin.HpId, ContainerCd = (long)kensaMstJoin.ContainerCd } equals
                    new { containerMst.HpId, containerMst.ContainerCd } into containerMstJoins
                from containerMstJoin in containerMstJoins.DefaultIfEmpty()
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.InoutKbn, odrInf.TosekiKbn, odrInf.SikyuKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
                select new
                {
                    odrInfDetail,
                    odrInf,
                    tenMst = oj,
                    kensaMst = kensaMstJoin,
                    materialMst = materialMstJoin,
                    containerMst = containerMstJoin
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoOdrInfDetailModel(
                        data.odrInfDetail,
                        data.odrInf,
                        data.tenMst,
                        data.kensaMst,
                        data.materialMst,
                        data.containerMst
                    )
                )
                .ToList();
            List<CoOdrInfDetailModel> results = new List<CoOdrInfDetailModel>();

            entities?.ForEach(entity =>
            {

                results.Add(
                    new CoOdrInfDetailModel(
                        entity.OdrInfDetail,
                        entity.OdrInf,
                        entity.TenMst,
                        entity.KensaMst,
                        entity.MaterialMst,
                        entity.ContainerMst
                        ));

            }
            );

            return results;
        }

        /// <summary>
        /// 予約オーダー情報取得
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <returns>
        /// 指定の患者の指定の診療日のオーダー情報
        /// 削除分は除く
        /// </returns>
        public List<CoRsvkrtOdrInfModel> FindRsvKrtOdrInf(int hpId, long ptId, int rsvDate, List<(int from, int to)> odrKouiKbns)
        {
            var rsvKrtOdrInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                (o.RsvDate == rsvDate || o.RsvDate == 99999999) &&
                o.OdrKouiKbn >= 13 &&
                o.IsDeleted == DeleteStatus.None);

            // 絞り込み
            var notAndConditions = rsvKrtOdrInfs;
            foreach ((int from, int to) odrKouiKbn in odrKouiKbns)
            {
                notAndConditions =
                    notAndConditions.Where(p => !(p.OdrKouiKbn >= odrKouiKbn.from && p.OdrKouiKbn <= odrKouiKbn.to));
            }
            rsvKrtOdrInfs = rsvKrtOdrInfs.Except(notAndConditions);

            var joinQuery = (
                from rsvKrtOdrInf in rsvKrtOdrInfs
                where
                    rsvKrtOdrInf.HpId == hpId &&
                    rsvKrtOdrInf.PtId == ptId &&
                    rsvKrtOdrInf.IsDeleted == DeleteStatus.None
                orderby
                    rsvKrtOdrInf.RsvkrtNo, rsvKrtOdrInf.OdrKouiKbn, rsvKrtOdrInf.SortNo, rsvKrtOdrInf.RpNo
                select new
                {
                    rsvKrtOdrInf
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoRsvkrtOdrInfModel(data.rsvKrtOdrInf)
                )
                .ToList();

            List<CoRsvkrtOdrInfModel> results = new List<CoRsvkrtOdrInfModel>();

            entities?.ForEach(entity =>
            {
                results.Add(new CoRsvkrtOdrInfModel(entity.RsvkrtOdrInf));
            });

            return results;
        }

        /// <summary>
        /// 予約オーダー詳細情報を取得する
        /// </summary>
        /// <param name="hpId">医療機関識別ID </param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        public List<CoRsvkrtOdrInfDetailModel> FindRsvKrtOdrInfDetail(int hpId, long ptId, int rsvDate, List<(int from, int to)> odrKouiKbns)
        {
            var rsvKrtOdrInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                (o.RsvDate == rsvDate || o.RsvDate == 99999999) &&
                o.OdrKouiKbn >= 13 &&
                o.IsDeleted == DeleteStatus.None);

            // 絞り込み
            var notAndConditions = rsvKrtOdrInfs;
            foreach ((int from, int to) odrKouiKbn in odrKouiKbns)
            {
                notAndConditions =
                    notAndConditions.Where(p => !(p.OdrKouiKbn >= odrKouiKbn.from && p.OdrKouiKbn <= odrKouiKbn.to));
            }
            rsvKrtOdrInfs = rsvKrtOdrInfs.Except(notAndConditions);


            var rsvKrtOdrInfDetails = NoTrackingDataContext.RsvkrtOdrInfDetails.Where(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                (o.RsvDate == rsvDate || o.RsvDate == 99999999));
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
                t.HpId == hpId &&
                t.StartDate <= rsvDate &&
                (t.EndDate >= rsvDate || t.EndDate == 12341234));
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(k =>
                    k.HpId == hpId &&
                    k.IsDelete == DeleteStatus.None
                );
            var materialMsts = NoTrackingDataContext.MaterialMsts.Where(m =>
                    m.HpId == hpId
                );
            var containerMsts = NoTrackingDataContext.ContainerMsts.Where(c =>
                    c.HpId == hpId
                );

            var joinQuery = (
                from rsvKrtOdrInf in rsvKrtOdrInfs
                join rsvKrtOdrInfDetail in rsvKrtOdrInfDetails on
                    new { rsvKrtOdrInf.HpId, rsvKrtOdrInf.PtId, rsvKrtOdrInf.RsvkrtNo, rsvKrtOdrInf.RpNo, rsvKrtOdrInf.RpEdaNo } equals
                    new { rsvKrtOdrInfDetail.HpId, rsvKrtOdrInfDetail.PtId, rsvKrtOdrInfDetail.RsvkrtNo, rsvKrtOdrInfDetail.RpNo, rsvKrtOdrInfDetail.RpEdaNo }
                join tenMst in tenMsts on
                    new { rsvKrtOdrInfDetail.HpId, ItemCd = rsvKrtOdrInfDetail.ItemCd.Trim() } equals
                    new { tenMst.HpId, tenMst.ItemCd } into oJoin
                from oj in oJoin.DefaultIfEmpty()
                join kensaMst in kensaMsts on
                    new { oj.HpId, oj.KensaItemCd, oj.KensaItemSeqNo } equals
                    new { kensaMst.HpId, kensaMst.KensaItemCd, kensaMst.KensaItemSeqNo } into kensaMstJoins
                from kensaMstJoin in kensaMstJoins.DefaultIfEmpty()
                join materialMst in materialMsts on
                    new { kensaMstJoin.HpId, MaterialCd = (long)kensaMstJoin.MaterialCd } equals
                    new { materialMst.HpId, materialMst.MaterialCd } into materialMstJoins
                from materialMstJoin in materialMstJoins.DefaultIfEmpty()
                join containerMst in containerMsts on
                    new { kensaMstJoin.HpId, ContainerCd = (long)kensaMstJoin.ContainerCd } equals
                    new { containerMst.HpId, containerMst.ContainerCd } into containerMstJoins
                from containerMstJoin in containerMstJoins.DefaultIfEmpty()
                orderby
                    rsvKrtOdrInf.RsvkrtNo, rsvKrtOdrInf.OdrKouiKbn, rsvKrtOdrInf.SortNo, rsvKrtOdrInfDetail.RpNo, rsvKrtOdrInfDetail.RpEdaNo, rsvKrtOdrInfDetail.RowNo
                select new
                {
                    rsvKrtOdrInfDetail,
                    rsvKrtOdrInf,
                    tenMst = oj,
                    kensaMst = kensaMstJoin,
                    materialMst = materialMstJoin,
                    containerMst = containerMstJoin
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoRsvkrtOdrInfDetailModel(
                        data.rsvKrtOdrInfDetail,
                        data.rsvKrtOdrInf,
                        data.tenMst,
                        data.kensaMst,
                        data.materialMst,
                        data.containerMst
                    )
                )
                .ToList();
            List<CoRsvkrtOdrInfDetailModel> results = new List<CoRsvkrtOdrInfDetailModel>();

            entities?.ForEach(entity =>
            {

                results.Add(
                    new CoRsvkrtOdrInfDetailModel(
                        entity.RsvkrtOdrInfDetail,
                        entity.RsvkrtOdrInf,
                        entity.TenMst,
                        entity.KensaMst,
                        entity.MaterialMst,
                        entity.ContainerMst
                        ));

            }
            );

            return results;
        }

        /// <summary>
        /// 来院区分情報を取得する
        /// </summary>
        /// <param name="hpId">医用機関識別ID</param>
        /// <param name="ptId">患者番号</param>
        /// <param name="sinDate">診療日</param>
        /// <param name="raiinNo">来院番号</param>
        /// <returns></returns>
        public List<CoRaiinKbnInfModel> FindRaiinKbnInf(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var raiinKbnInfs = NoTrackingDataContext.RaiinKbnInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo == raiinNo &&
                p.IsDelete == DeleteStatus.None
            );

            var raiinKbnDtls = NoTrackingDataContext.RaiinKbnDetails.Where(p =>
                p.HpId == hpId &&
                p.IsDeleted == DeleteStatus.None
                );

            var join = (

                    from raiinKbnInf in raiinKbnInfs
                    join raiinkbnDtl in raiinKbnDtls on
                        new { raiinKbnInf.HpId, raiinKbnInf.GrpId, raiinKbnInf.KbnCd } equals
                        new { raiinkbnDtl.HpId, GrpId = raiinkbnDtl.GrpCd, raiinkbnDtl.KbnCd } into raiinKbnDtlJoins
                    from raiinKbnDtlJoin in raiinKbnDtlJoins.DefaultIfEmpty()
                    select new
                    {
                        raiinKbnInf,
                        raiinKbnDtlJoin
                    }

                );

            var entities = join.AsEnumerable().Select(
                data =>
                    new CoRaiinKbnInfModel(
                        data.raiinKbnInf,
                        data.raiinKbnDtlJoin
                    )
                )
                .ToList();
            List<CoRaiinKbnInfModel> results = new List<CoRaiinKbnInfModel>();

            entities?.ForEach(entity =>
            {

                results.Add(
                    new CoRaiinKbnInfModel(
                        entity.RaiinKbnInf,
                        entity.RaiinKbnDetail
                        ));

            }
            );

            return results;

        }

        /// <summary>
        /// 来院区分マスタを取得する
        /// </summary>
        /// <param name="hpId"></param>
        /// <returns></returns>
        public List<CoRaiinKbnMstModel> FindRaiinKbnMst(int hpId)
        {
            var raiinKbnMsts = NoTrackingDataContext.RaiinKbnMsts.Where(p =>
                p.HpId == hpId &&
                p.IsDeleted == DeleteStatus.None
            ).ToList();

            List<CoRaiinKbnMstModel> results = new List<CoRaiinKbnMstModel>();

            if (raiinKbnMsts != null)
            {
                foreach (RaiinKbnMst raiinKbnMst in raiinKbnMsts)
                {
                    results.Add(new CoRaiinKbnMstModel(raiinKbnMst));
                }
            }

            return results;
        }

        /// <summary>
        /// 最終来院日を取得する（状態が計算以上の来院の中で最大の日）
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <returns></returns>
        public int GetLastSinDate(int hpId, long ptId)
        {
            int ret = 0;

            var raiinInf = NoTrackingDataContext.RaiinInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.Status >= 5 &&
                p.IsDeleted == DeleteStatus.None
                ).ToList();

            if (raiinInf != null && raiinInf.Any())
            {
                ret = raiinInf.Max(p => p.SinDate);
            }

            return ret;
        }
    }
}
