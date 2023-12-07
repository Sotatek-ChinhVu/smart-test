using Domain.Constant;
using Emr.Report.OutDrug.Model;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.OutDrug.Model;

namespace Reporting.OutDrug.DB;

public class CoOutDrugFinder : RepositoryBase, ICoOutDrugFinder
{
    public CoOutDrugFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    /// <summary>
    /// オーダー情報取得
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns>
    /// 指定の患者の指定の診療日のオーダー情報
    /// 削除分は除く
    /// </returns>
    public List<CoOdrInfModel> FindOdrInfData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.SinDate == sinDate &&
            o.RaiinNo == raiinNo &&
            o.InoutKbn == 1 &&
            new int[] { 21, 22, 23, 28, 100, 101 }.Contains(o.OdrKouiKbn) &&
            o.IsDeleted == DeleteStatus.None);
        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(o =>
            o.HpId == hpId);

        var joinQuery = (
            from odrInf in odrInfs
            join PtHokenPattern in ptHokenPatterns on
                new { odrInf.HpId, odrInf.PtId, HokenPid = odrInf.HokenPid } equals
                new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
            where
                odrInf.HpId == hpId &&
                odrInf.PtId == ptId &&
                odrInf.IsDeleted == DeleteStatus.None
            orderby
                odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInf.RpNo
            select new
            {
                odrInf,
                PtHokenPattern
            }
        );
        var entities = joinQuery.AsEnumerable().Select(
            data =>
                new CoOdrInfModel(data.odrInf, data.PtHokenPattern)
            )
            .ToList();

        List<CoOdrInfModel> results = new List<CoOdrInfModel>();

        entities?.ForEach(entity =>
        {
            results.Add(new CoOdrInfModel(entity.OdrInf, entity.PtHokenPattern));
        });

        return results;
    }

    /// <summary>
    /// オーダー詳細情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns></returns>
    public List<CoOdrInfDetailModel> FindOdrInfDetailData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                  && item.PtId == ptId
                                                                  && item.SinDate == sinDate
                                                                  && item.RaiinNo == raiinNo
                                                                  && item.IsDeleted == DeleteStatus.None)
                                                   .ToList();
        // return if odrInfDetails not exits data
        if (!odrInfs.Any())
        {
            return new();
        }

        var hokenPidList = odrInfs.Select(item => item.HokenPid).Distinct().ToList();
        var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                              && item.PtId == ptId
                                                                              && item.SinDate == sinDate
                                                                              && !(item.ItemCd != null
                                                                                   && item.ItemCd.StartsWith("8") && item.ItemCd.Length == 9))
                                                               .ToList();
        // return if odrInfDetails not exits data
        if (!odrInfDetails.Any())
        {
            return new();
        }
        var itemCdList = odrInfDetails.Select(item => item.ItemCd).Distinct().ToList();
        var tenMsts = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                  && item.StartDate <= sinDate
                                                                  && (item.EndDate >= sinDate || item.EndDate == 12341234)
                                                                  && itemCdList.Contains(item.ItemCd))
                                                   .ToList();

        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId
                                                                                  && item.PtId == ptId
                                                                                  && hokenPidList.Contains(item.HokenPid))
                                                                   .ToList();
        var yohoMsts = NoTrackingDataContext.YohoMsts.Where(item => item.HpId == hpId
                                                                    && item.StartDate <= sinDate
                                                                    && (item.EndDate >= sinDate || item.EndDate == 12341234))
                                                     .ToList();

        var tenJoins = (
            from tenMst in tenMsts
            join yohoMst in yohoMsts on
              new { tenMst.HpId, tenMst.YohoCd } equals
              new { yohoMst.HpId, yohoMst.YohoCd } into yJoin
            from yj in yJoin.DefaultIfEmpty()
            select new
            {
                tenMst,
                yohoMst = yj
            }).ToList();

        var entities = (
            from odrInf in odrInfs
            join odrInfDetail in odrInfDetails on
                new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
            join tenJoin in tenJoins on
                new { odrInfDetail.HpId, ItemCd = odrInfDetail.ItemCd ?? string.Empty.Trim() } equals
                new { tenJoin.tenMst.HpId, tenJoin.tenMst.ItemCd } into oJoin
            join PtHokenPattern in ptHokenPatterns on
                new { odrInf.HpId, odrInf.PtId, odrInf.HokenPid } equals
                new { PtHokenPattern.HpId, PtHokenPattern.PtId, PtHokenPattern.HokenPid }
            from oj in oJoin.DefaultIfEmpty()
            orderby
                odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
            select new CoOdrInfDetailModel(
                    odrInfDetail,
                    odrInf,
                    oj?.tenMst,
                    PtHokenPattern,
                    oj?.yohoMst
                )).ToList();

        List<CoOdrInfDetailModel> results = new();

        entities?.ForEach(entity =>
        {
            results.Add(
                new CoOdrInfDetailModel(
                    entity.OdrInfDetail,
                    entity.OdrInf,
                    entity.TenMst,
                    entity.PtHokenPattern,
                    entity.YohoMst
                    ));
        });

        return results;
    }

    /// <summary>
    /// 患者公費情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="kohiIds">公費IDリスト</param>
    /// <returns>患者公費情報のリスト</returns>
    public List<CoPtKohiModel> FindPtKohi(int hpId, long ptId, int sinDate, HashSet<int> kohiIds)
    {
        var hokenMsts = NoTrackingDataContext.HokenMsts;
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
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

        var kohiPriorities = NoTrackingDataContext.KohiPriorities;
        var ptKohis = NoTrackingDataContext.PtKohis.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            kohiIds.Contains(p.HokenId)
        );
        //保険番号マスタの取得
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

        //公費の優先順位を取得
        var ptKohiQuery = (
            from ptKohi in ptKohis
            join houbetuMst in houbetuMsts on
                new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo, houbetuMst.hokenMst.PrefNo }
            join kPriority in kohiPriorities on
                new { houbetuMst.hokenMst.PrefNo, houbetuMst.hokenMst.Houbetu } equals
                new { kPriority.PrefNo, kPriority.Houbetu } into kohiPriorityJoin
            from kohiPriority in kohiPriorityJoin.DefaultIfEmpty()
            where
                ptKohi.HpId == hpId &&
                ptKohi.PtId == ptId &&
                ptKohi.IsDeleted == DeleteStatus.None
            select new
            {
                ptKohi,
                hokenMst = houbetuMst.hokenMst,
                kohiPriority
            }
        ).ToList();

        var entities = ptKohiQuery.AsEnumerable().Select(
            data =>
                new CoPtKohiModel(
                    data.ptKohi,
                    data.hokenMst,
                    data.kohiPriority
                )
            )
            .ToList();
        List<CoPtKohiModel> results = new List<CoPtKohiModel>();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoPtKohiModel(
                    entity.PtKohi,
                    entity.HokenMst,
                    entity.KohiPriority
                ));

        }
        );

        return results;
    }

    /// <summary>
    /// 医療機関情報を取得する
    /// </summary>
    /// <param name="sinDate">診療日</param>
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
    /// 来院情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns></returns>
    public CoRaiinInfModel FindRaiinInf(int hpId, long ptId, int sinDate, long raiinNo)
    {
        var raiinInfs =
            NoTrackingDataContext.RaiinInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo == raiinNo &&
                p.IsDeleted == DeleteStatus.None);
        var userMsts =
            NoTrackingDataContext.UserMsts.Where(p =>
                p.HpId == hpId &&
                p.JobCd == 1 &&
                p.IsDeleted == DeleteStatus.None
            );

        var join = (
            from raiinInf in raiinInfs
            join userMst in userMsts on
                new { raiinInf.HpId, UserId = raiinInf.TantoId } equals
                new { userMst.HpId, userMst.UserId } into userMstjoins
            from userMstJoin in userMstjoins.DefaultIfEmpty()
            select new
            {
                raiinInf,
                userMstJoin

            }
            ).ToList();

        var entities = join.AsEnumerable().Select(
            data =>
                new CoRaiinInfModel(
                    data.raiinInf,
                    data.userMstJoin
                )
            )
            .ToList();
        List<CoRaiinInfModel> results = new();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoRaiinInfModel(
                    entity.RaiinInf,
                    entity.UserMst
                ));

        }
        );

        if (results != null && results.Any())
        {
            return results.First();
        }
        else
        {
            return new();
        }
    }

    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns></returns>
    public CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate)
    {
        return new CoPtInfModel(
            NoTrackingDataContext.PtInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId)
                .FirstOrDefault() ?? new(),
                sinDate
            );
    }

    /// <summary>
    /// 患者保険情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="hokenId">保険ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns></returns>
    public CoPtHokenInfModel FindPtHoken(int hpId, long ptId, int hokenId, int sinDate)
    {
        var hokenMsts = NoTrackingDataContext.HokenMsts.Where(p => p.PrefNo == 0 && new int[] { 0, 1, 3, 4, 8 }.Contains(p.HokenSbtKbn));
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
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
        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.HokenId == hokenId);
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
        var ptHokenInfQuery = (
            from ptHokenInf in ptHokenInfs
            join houbetuMst in houbetuMsts on
                new { ptHokenInf.HpId, ptHokenInf.HokenNo, ptHokenInf.HokenEdaNo } equals
                new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo }
            where
                ptHokenInf.HpId == hpId &&
                ptHokenInf.PtId == ptId &&
                ptHokenInf.IsDeleted == DeleteStatus.None
            select new
            {
                ptHokenInf,
                hokenMst = houbetuMst.hokenMst
            }
        ).ToList();


        var entities = ptHokenInfQuery.AsEnumerable().Select(
            data =>
                new CoPtHokenInfModel(
                    data.ptHokenInf,
                    data.hokenMst
                )
            )
            .ToList();

        return entities?.FirstOrDefault() ?? new();

    }

    /// <summary>
    /// 指定の期間に指定の項目が算定されているかチェックする
    /// ※複数項目用
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">チェック開始日</param>
    /// <param name="endDate">チェック終了日</param>
    /// <param name="sinDate">診療日（除外する日）</param>
    /// <param name="itemCds">チェックする項目のリスト</param>
    /// <param name="santeiKbn">算定区分</param>
    /// <returns>true: 算定あり</returns>
    public bool CheckSanteiTerm(int hpId, long ptId, int startDate, int endDate, List<string> itemCds)
    {
        int startYm = startDate / 100;
        int endYm = endDate / 100;

        var sinRpInfs = NoTrackingDataContext.SinRpInfs.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.SinYm >= startYm &&
            o.SinYm <= endYm &&
            o.SanteiKbn == 0 &&
            o.IsDeleted == DeleteStatus.None
        );
        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.SinYm * 100 + o.SinDay >= startDate &&
            o.SinYm * 100 + o.SinDay <= endDate
            );
        var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinYm >= startYm &&
            p.SinYm <= endYm &&
            p.ItemCd != null &&
            itemCds.Contains(p.ItemCd) &&
            p.IsDeleted == DeleteStatus.None);

        var joinQuery = (
            from sinKouiDetail in sinKouiDetails
            join sinKouiCount in sinKouiCounts on
                new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
            join sinRpInf in sinRpInfs on
                new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
            where
                sinKouiDetail.HpId == hpId &&
                sinKouiDetail.PtId == ptId &&
                sinKouiDetail.SinYm >= startYm &&
                sinKouiDetail.SinYm <= endYm &&
                sinKouiDetail.ItemCd != null &&
                itemCds.Contains(sinKouiDetail.ItemCd) &&
                sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate
            group sinKouiDetail by sinKouiDetail.HpId
        );

        return joinQuery.Any();
    }

    /// <summary>
    /// 指定の期間に指定の項目がオーダーされているかチェックする
    /// ※複数項目用
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">チェック開始日</param>
    /// <param name="endDate">チェック終了日</param>
    /// <param name="sinDate">診療日（除外する日）</param>
    /// <param name="itemCds">チェックする項目のリスト</param>
    /// <param name="santeiKbn">算定区分</param>
    /// <returns>true: 算定あり</returns>
    public bool CheckOdrTerm(int hpId, long ptId, int startDate, int endDate, List<string> itemCds)
    {
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.SinDate >= startDate &&
            o.SinDate <= endDate &&
            o.IsDeleted == DeleteStatus.None);
        var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.ItemCd != null &&
            itemCds.Contains(o.ItemCd) &&
            o.SinDate >= startDate &&
            o.SinDate <= endDate);

        var joinQuery = (
            from odrInf in odrInfs
            join odrInfDetail in odrInfDetails on
                new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
            orderby
                odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
            select new
            {
                odrInfDetail,
            }
        );

        return joinQuery.Any();

    }
    /// <summary>
    /// 指定の来院に指定の項目がオーダーされているかチェックする
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="raiinNo">来院番号</param>
    /// <param name="itemCds">検索する項目の診療行為コードのリスト</param>
    /// <returns></returns>
    public bool CheckOdrRaiin(int hpId, long ptId, long raiinNo, List<string> itemCds)
    {
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.RaiinNo == raiinNo &&
            o.IsDeleted == DeleteStatus.None);
        var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o =>
            o.HpId == hpId &&
            o.PtId == ptId &&
            o.ItemCd != null &&
            itemCds.Contains(o.ItemCd) &&
            o.RaiinNo == raiinNo);

        var joinQuery = (
            from odrInf in odrInfs
            join odrInfDetail in odrInfDetails on
                new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo } equals
                new { odrInfDetail.HpId, odrInfDetail.PtId, odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
            orderby
                odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo, odrInfDetail.RowNo
            select new
            {
                odrInfDetail,
            }
        );

        return joinQuery.Any();

    }
    public int ExistMarucyo(int hpId, long ptId, int sinDate, int hokenId)
    {
        int ret = 0;

        if (hokenId == 0) return ret;

        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.HokenId == hokenId &&
            p.IsDeleted == DeleteStatus.None).ToList();

        var ptKohis = NoTrackingDataContext.PtKohis.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.StartDate <= sinDate &&
            p.EndDate >= sinDate &&
            p.IsDeleted == DeleteStatus.None);

        var hokenMsts = NoTrackingDataContext.HokenMsts;
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
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

        //保険番号マスタの取得
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

        //公費の優先順位を取得
        var ptKohiQuery = (
            from ptKohi in ptKohis
            join houbetuMst in houbetuMsts on
                new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo, houbetuMst.hokenMst.PrefNo }
            where
                ptKohi.HpId == hpId &&
                ptKohi.PtId == ptId &&
                houbetuMst.hokenMst.HokenSbtKbn == 2 &&
                ptKohi.IsDeleted == DeleteStatus.None
            select new
            {
                ptKohi,
                hokenMst = houbetuMst.hokenMst
            }
        ).ToList();

        ptHokenPatterns?.ForEach(ptHokenPattern =>
        {
            if (ptKohiQuery.Any(p =>
                (p.ptKohi.HokenId == ptHokenPattern.Kohi1Id && p.ptKohi.HokenEdaNo == 1) ||
                (p.ptKohi.HokenId == ptHokenPattern.Kohi2Id && p.ptKohi.HokenEdaNo == 1) ||
                (p.ptKohi.HokenId == ptHokenPattern.Kohi3Id && p.ptKohi.HokenEdaNo == 1) ||
                (p.ptKohi.HokenId == ptHokenPattern.Kohi4Id && p.ptKohi.HokenEdaNo == 1)))
            {
                if (ret < 2)
                {
                    ret = 2;
                }
            }
            else if (ptKohiQuery.Any(p =>
                (p.ptKohi.HokenId == ptHokenPattern.Kohi1Id && p.ptKohi.HokenEdaNo == 0) ||
                (p.ptKohi.HokenId == ptHokenPattern.Kohi2Id && p.ptKohi.HokenEdaNo == 0) ||
                (p.ptKohi.HokenId == ptHokenPattern.Kohi3Id && p.ptKohi.HokenEdaNo == 0) ||
                (p.ptKohi.HokenId == ptHokenPattern.Kohi4Id && p.ptKohi.HokenEdaNo == 0)) && ret < 1)
            {
                ret = 1;
            }
        }
        );

        return ret;
    }

    /// <summary>
    /// 補足用法情報を取得する
    /// </summary>
    public List<CoYohoHosoku> FindYohoHosoku(int hpId, string itemCd, int sinDate)
    {
        var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
            t.HpId == hpId &&
            t.StartDate <= sinDate &&
            (t.EndDate >= sinDate || t.EndDate == 12341234));

        var hosokuYohos = NoTrackingDataContext.YohoHosokus.Where(h =>
                 h.HpId == hpId &&
                 h.IsDeleted == DeleteStatus.None &&
                 h.ItemCd == itemCd &&
                 h.StartDate <= sinDate);

        //最新世代の用法補足に絞る
        var grpHosokuYohos = hosokuYohos
            .GroupBy(h => new { h.HpId, h.ItemCd, h.StartDate })
            .Select(h => new { h.Key.HpId, h.Key.ItemCd, MaxStartDate = h.Max(y => y.StartDate) });

        var latestHosokuYohos = (
            from hosokuYoho in hosokuYohos
            join grpHosokuYoho in grpHosokuYohos on
                new { hosokuYoho.HpId, hosokuYoho.ItemCd, hosokuYoho.StartDate } equals
                new { grpHosokuYoho.HpId, grpHosokuYoho.ItemCd, StartDate = grpHosokuYoho.MaxStartDate }
            select new { hosokuYoho }
            );

        //点マスタを外部結合する
        var joinQuery = (
            from latestHosokuYoho in latestHosokuYohos
            join tenMst in tenMsts on
                new { latestHosokuYoho.hosokuYoho.HpId, latestHosokuYoho.hosokuYoho.HosokuItemCd } equals
                new { tenMst.HpId, HosokuItemCd = tenMst.ItemCd } into tenMstJoins
            from tenMstJoin in tenMstJoins.DefaultIfEmpty()
            select new { latestHosokuYoho.hosokuYoho, tenMstJoin }
            ).ToList();

        List<CoYohoHosoku> results = new List<CoYohoHosoku>();

        joinQuery?.ForEach(entity =>
        {
            results.Add(
                new CoYohoHosoku(
                    entity.hosokuYoho,
                    entity.tenMstJoin
                    ));
        }
        );

        return results.OrderBy(r => r.SortNo).ToList();
    }

    public List<CoEpsChk> FindEPSChecks(int hpId, long ptId, long raiinNo)
    {
        var epsChks = NoTrackingDataContext.EpsChks.Where(e =>
            e.HpId == hpId &&
            e.PtId == ptId &&
            e.RaiinNo == raiinNo &&
            e.IsDeleted == 0);

        var epsChkDetails = NoTrackingDataContext.EpsChkDetails.Where(d =>
                 d.HpId == hpId);

        var joinQuery = from epsChk in epsChks
                        join epsChkDetail in epsChkDetails on
                            new { epsChk.HpId, epsChk.PtId, epsChk.RaiinNo, epsChk.SeqNo } equals
                            new { epsChkDetail.HpId, epsChkDetail.PtId, epsChkDetail.RaiinNo, epsChkDetail.SeqNo }
                        select new
                        {
                            epsChk,
                            epsChkDetail
                        };

        var entities = joinQuery.AsEnumerable().Select(
            data =>
                new CoEpsChk(data.epsChk, data.epsChkDetail)
            )
            .ToList();

        List<CoEpsChk> results = new List<CoEpsChk>();

        entities?.ForEach(entity =>
        {
            results.Add(new CoEpsChk(entity.EpsChk, entity.EpsChkDetail));
        });

        return results;
    }

    public CoYohoMstModel FindYohoMst(int hpId, string yohoCd, int sinDate)
    {
        return new CoYohoMstModel(NoTrackingDataContext.YohoMsts.Where(y =>
           y.HpId == hpId &&
           y.YohoCd == yohoCd &&
           y.StartDate <= sinDate &&
           (y.EndDate >= sinDate || y.EndDate == 12341234))
            .OrderByDescending(y => y.StartDate)
            .FirstOrDefault() ?? new());
    }

    public bool IsSingleDosageUnit(int hpId, string unitName)
    {
        return NoTrackingDataContext.SingleDoseMsts.Any(s => s.HpId == hpId && s.UnitName == unitName);
    }

    public CoDosageDrugModel GetDosageDrugModel(string yjCode)
    {
        var dosageDrug = NoTrackingDataContext.DosageDrugs.FirstOrDefault(item => item.YjCd == yjCode);
        return new CoDosageDrugModel(dosageDrug ?? new());

    }

    /// <summary>
    /// 今月オンライン資格確認した保険
    /// </summary>
    /// <param name="ptId">患者番号</param>
    /// <param name="sinDate">診療日</param>
    /// <returns>今月オンライン資格確認した保険ID</returns>
    public int GetOnlineConfirmedHokenId(int hpId, long ptId, int sinDate)
    {
        DateTime dtSinDate = new DateTime(sinDate / 10000, sinDate / 100 % 100, sinDate % 100);
        DateTime firstDate = new DateTime(sinDate / 10000, sinDate / 100 % 100, 1);

        var ptHokenCheck = NoTrackingDataContext.PtHokenChecks.Where(p =>
          p.HpId == hpId &&
          p.PtID == ptId &&
          firstDate <= p.CheckDate &&
          p.CheckDate <= dtSinDate &&
          p.IsDeleted == DeleteStatus.None &&
          p.CheckCmt == "オンライン資格確認"
          )
          .OrderByDescending(p => p.CheckDate)
          .Select(p => p.HokenId)
          .FirstOrDefault();

        return ptHokenCheck;
    }

    /// <summary>
    /// 電子処方箋登録情報
    /// </summary>
    /// <param name="ptId">患者番号</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>電子処方箋登録情報のリスト</returns>
    public List<CoEpsPrescription> FindEpsPrescription(int hpId, long ptId, long raiinNo)
    {
        var epsPrescriptions = NoTrackingDataContext.EpsPrescriptions.Where(e =>
            e.HpId == hpId &&
            e.PtId == ptId &&
            e.RaiinNo == raiinNo &&
            e.Status == 0).ToList();

        List<CoEpsPrescription> results = new();

        epsPrescriptions?.ForEach(entity =>
        {
            results.Add(new CoEpsPrescription(entity));
        });

        return results;
    }

    /// <summary>
    /// 電子処方箋の処方内容控え取得
    /// </summary>
    /// <param name="ptId">患者番号</param>
    /// <param name="raiinNo">来院番号</param>
    /// <returns>電子処方箋の処方内容控えのリスト</returns>
    public List<CoEpsReference> GetEpsReferences(int hpId, long ptId, long raiinNo)
    {
        var epsPrescriptions = NoTrackingDataContext.EpsPrescriptions.Where(e =>
            e.HpId == hpId &&
            e.PtId == ptId &&
            e.RaiinNo == raiinNo &&
            e.Status == 0 &&
            e.IssueType == 1);

        var epsReferences = NoTrackingDataContext.EpsReferences.Where(e => e.HpId == hpId);

        var joinQuery = from epsPrescription in epsPrescriptions
                        join epsReference in epsReferences on
                            new { epsPrescription.HpId, epsPrescription.PrescriptionId } equals
                            new { epsReference.HpId, epsReference.PrescriptionId }
                        orderby epsPrescription.RefileCount
                        select new
                        {
                            epsReference
                        };

        var entities = joinQuery.AsEnumerable().Select(
            data =>
                new CoEpsReference(data.epsReference)
            )
            .ToList();

        List<CoEpsReference> results = new List<CoEpsReference>();

        entities?.ForEach(entity =>
        {
            results.Add(new CoEpsReference(entity.EpsReference));
        });

        return results;
    }
}
