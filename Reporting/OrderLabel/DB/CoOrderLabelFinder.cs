using Domain.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.OrderLabel.Model;

namespace Reporting.OrderLabel.DB;

public class CoOrderLabelFinder : RepositoryBase, ICoOrderLabelFinder
{
    public CoOrderLabelFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
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
        var notAndConditions = odrInfs.Where(p => p.OdrKouiKbn != 10);
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
        var notAndConditions = odrInfs.Where(p => p.OdrKouiKbn != 10);
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
            (t.EndDate >= sinDate || t.EndDate == 12341234));

        var joinQuery = (
            from odrInf in odrInfs
            join odrInfDetail in odrInfDetails on
                new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
            join tenMst in tenMsts on
                new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd ?? string.Empty.Trim() } equals
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

        entities?.ForEach(entity =>
        {

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
    public List<CoRsvkrtOdrInfModel> FindRsvKrtOdrInf(int hpId, long ptId, int rsvDate, long rsvkrtNo, List<(int from, int to)> odrKouiKbns)
    {
        var rsvKrtOdrInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.RsvDate == rsvDate &&
            o.RsvkrtNo == rsvkrtNo &&
            o.OdrKouiKbn >= 11 &&
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
    /// <param name="rsvDate">診療日</param>
    /// <param name="rsvkrtNo">来院番号</param>
    /// <returns></returns>
    public List<CoRsvkrtOdrInfDetailModel> FindRsvKrtOdrInfDetail(int hpId, long ptId, int rsvDate, long rsvkrtNo, List<(int from, int to)> odrKouiKbns)
    {
        var rsvKrtOdrInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.RsvDate == rsvDate &&
            o.RsvkrtNo == rsvkrtNo &&
            o.OdrKouiKbn >= 11 &&
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
            o.RsvDate == rsvDate);
        var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
            t.HpId == hpId &&
            t.StartDate <= rsvDate &&
            (t.EndDate >= rsvDate || t.EndDate == 12341234));

        var joinQuery = (
            from rsvKrtOdrInf in rsvKrtOdrInfs
            join rsvKrtOdrInfDetail in rsvKrtOdrInfDetails on
                new { rsvKrtOdrInf.HpId, rsvKrtOdrInf.PtId, rsvKrtOdrInf.RsvkrtNo, rsvKrtOdrInf.RpNo, rsvKrtOdrInf.RpEdaNo } equals
                new { rsvKrtOdrInfDetail.HpId, rsvKrtOdrInfDetail.PtId, rsvKrtOdrInfDetail.RsvkrtNo, rsvKrtOdrInfDetail.RpNo, rsvKrtOdrInfDetail.RpEdaNo }
            join tenMst in tenMsts on
                new { rsvKrtOdrInfDetail.HpId, ItemCd = rsvKrtOdrInfDetail.ItemCd ?? string.Empty.Trim() } equals
                new { tenMst.HpId, tenMst.ItemCd } into oJoin
            from oj in oJoin.DefaultIfEmpty()
            orderby
                rsvKrtOdrInf.RsvkrtNo, rsvKrtOdrInf.OdrKouiKbn, rsvKrtOdrInf.SortNo, rsvKrtOdrInfDetail.RpNo, rsvKrtOdrInfDetail.RpEdaNo, rsvKrtOdrInfDetail.RowNo
            select new
            {
                rsvKrtOdrInfDetail,
                rsvKrtOdrInf,
                tenMst = oj
            }
        );

        var entities = joinQuery.AsEnumerable().Select(
            data =>
                new CoRsvkrtOdrInfDetailModel(
                    data.rsvKrtOdrInfDetail,
                    data.rsvKrtOdrInf,
                    data.tenMst
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
                    entity.TenMst
                    ));

        }
        );

        return results;
    }

    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="hpId">医療機関識別ID</param>
    /// <param name="ptId">患者ID</param>
    /// <returns>患者情報</returns>
    public CoPtInfModel FindPtInf(int hpId, long ptId)
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
                new CoPtInfModel(data.ptInf)
            )
            .ToList();

        List<CoPtInfModel> results = new();

        entities?.ForEach(entity =>
        {
            results.Add(
                new CoPtInfModel(
                    entity.PtInf
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

        List<CoRaiinInfModel> results = new();

        entities?.ForEach(entity =>
        {
            results.Add(new CoRaiinInfModel(entity.RaiinInf, entity.KaMst, entity.UserMst));
        });

        return results.FirstOrDefault() ?? new();
    }

    public List<CoYoyakuModel> FindYoyaku(int hpId, long ptId, int sinDate)
    {
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinDate > sinDate &&
            p.Status == RaiinState.Reservation &&
            p.IsDeleted == DeleteStatus.None
        );

        var rsvInfs = NoTrackingDataContext.RsvInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinDate > sinDate
        );

        var rsvFrameMsts = NoTrackingDataContext.RsvFrameMsts.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        );

        var joinQuery = (
                from raiinInf in raiinInfs
                join rsvInf in rsvInfs on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                    new { rsvInf.HpId, rsvInf.PtId, rsvInf.SinDate, rsvInf.RaiinNo } into rsvInfJoins
                from rsvInfJoin in rsvInfJoins.DefaultIfEmpty()
                join rsvFrameMst in rsvFrameMsts on
                    new { rsvInfJoin.HpId, rsvInfJoin.RsvFrameId } equals
                    new { rsvFrameMst.HpId, rsvFrameMst.RsvFrameId } into rsvFrameMstJoins
                from rsvFrameMstJoin in rsvFrameMstJoins.DefaultIfEmpty()
                select new
                {
                    raiinInf,
                    rsvInfJoin,
                    rsvFrameMstJoin
                }

            );

        var entities = joinQuery.AsEnumerable().Select(
            data =>
                new CoYoyakuModel(data.raiinInf.SinDate, data.raiinInf?.YoyakuTime ?? string.Empty, data.rsvFrameMstJoin?.RsvFrameName ?? string.Empty)
            )
            .ToList();

        List<CoYoyakuModel> results = new List<CoYoyakuModel>();

        entities?.ForEach(entity =>
        {
            results.Add(new CoYoyakuModel(entity.SinDate, entity.Time, entity.Frame));
        });

        return results;
    }

    public List<CoUserMstModel> FindUserMst(int hpId)
    {
        var userMsts = NoTrackingDataContext.UserMsts.Where(p =>
            p.HpId == hpId
        ).ToList();

        List<CoUserMstModel> results = new List<CoUserMstModel>();

        userMsts?.ForEach(entity =>
        {
            results.Add(
                new CoUserMstModel(
                    entity
                    ));
        }
        );

        return results;
    }
}