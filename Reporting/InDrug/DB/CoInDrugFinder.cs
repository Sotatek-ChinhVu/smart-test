using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Reporting.Calculate.Extensions;
using Reporting.InDrug.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.InDrug.DB
{
    public class CoInDrugFinder : RepositoryBase, ICoInDrugFinder
    {
        public CoInDrugFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        { 
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate)
        {

            var ptInfs = NoTrackingDataContext.PtInfs.FindListNoTrack(p =>
                     p.HpId == hpId &&
                     p.PtId == ptId &&
            p.IsDelete == DeleteStatus.None
            );

            var ptCmts = NoTrackingDataContext.PtCmtInfs.FindListNoTrack(p =>
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

            entities?.ForEach(entity => {
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
            var ptAlrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.FindListNoTrack(p =>
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
            var ptAlrgyFoods = NoTrackingDataContext.PtAlrgyFoods.FindListNoTrack(p =>
                   p.HpId == hpId &&
                   p.PtId == ptId &&
                   p.StartDate <= sinDate &&
                   (p.EndDate >= sinDate || p.EndDate == 0) &&
            p.IsDeleted == DeleteStatus.None
                );
            var foodAlrgyKbns = NoTrackingDataContext.M12FoodAlrgyKbn.FindListNoTrack();

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

            entities?.ForEach(entity => {
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
            var ptAlrgyElses = NoTrackingDataContext.PtAlrgyElses.FindListNoTrack(p =>
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
            var kaMsts = NoTrackingDataContext.KaMsts.FindListNoTrack(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var userMsts = NoTrackingDataContext.UserMsts.FindListNoTrack(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts.FindListNoTrack(o =>
                o.HpId == hpId &&
                o.IsDeleted == DeleteStatus.None);
            var raiinCmtInfs = NoTrackingDataContext.RaiinCmtInfs.FindListNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.CmtKbn == 1 &&
            o.RaiinNo == raiinNo
                );
            var raiinInfs = NoTrackingDataContext.RaiinInfs.FindListNoTrack(p =>
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
                    userMst = user
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoRaiinInfModel(data.raiinInf, data.kaMst, data.userMst)
                )
                .ToList();

            List<CoRaiinInfModel> results = new List<CoRaiinInfModel>();

            entities?.ForEach(entity => {
                results.Add(new CoRaiinInfModel(entity.RaiinInf, entity.KaMst, entity.UserMst));
            });

            return results.FirstOrDefault();
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
            const string conFncName = nameof(FindOdrInf);

            var odrInfs = NoTrackingDataContext.OdrInfs.FindListNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo == raiinNo &&
                o.InoutKbn == 0 &&
                new int[] { 21, 22, 23, 28, 100, 101 }.Contains(o.OdrKouiKbn) &&
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

            entities?.ForEach(entity => {
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
            const string conFncName = nameof(FindOdrInfDetail);

            var odrInfs = NoTrackingDataContext.OdrInfs.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                o.RaiinNo == raiinNo &&
                o.InoutKbn == 0 &&
                new int[] { 21, 22, 23, 28, 100, 101 }.Contains(o.OdrKouiKbn) &&
            o.IsDeleted == DeleteStatus.None);

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.FindListQueryableNoTrack(o =>
                o.HpId == hpId &&
                o.PtId == ptId &&
                o.SinDate == sinDate &&
                !(o.ItemCd != null && o.ItemCd.StartsWith("8") && o.ItemCd.Length == 9));
            var tenMsts = NoTrackingDataContext.TenMsts.FindListQueryableNoTrack(t =>
                t.HpId == hpId &&
                t.StartDate <= sinDate &&
                (t.EndDate >= sinDate || t.EndDate == 12341234));

            var joinQuery = (
                from odrInf in odrInfs
                join odrInfDetail in odrInfDetails on
                    new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                    new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                join tenMst in tenMsts on
                    new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd.Trim() } equals
                    new { tenMst.HpId, tenMst.ItemCd } into oJoin
                from oj in oJoin.DefaultIfEmpty()
                orderby
                    odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.InoutKbn, odrInf.TosekiKbn, odrInf.SikyuKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
                select new
                {
                    odrInfDetail,
                    odrInf,
                    tenMst = oj
                }
            );

            var entities = joinQuery.AsEnumerable().Select(
                data =>
                    new CoOdrInfDetailModel(
                        data.odrInfDetail,
                        data.odrInf,
                        data.tenMst
                    )
                )
                .ToList();
            List<CoOdrInfDetailModel> results = new List<CoOdrInfDetailModel>();

            entities?.ForEach(entity => {

                results.Add(
                    new CoOdrInfDetailModel(
                        entity.OdrInfDetail,
                        entity.OdrInf,
                        entity.TenMst
                        ));

            }
            );

            return results;
        }
    }
}
