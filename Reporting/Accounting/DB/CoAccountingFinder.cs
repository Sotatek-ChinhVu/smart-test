using Domain.Constant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Reporting.Accounting.Model;

namespace Reporting.Accounting.DB;

public class CoAccountingFinder : RepositoryBase, ICoAccountingFinder
{
    public CoAccountingFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    /// <summary>
    /// 医療機関情報を取得する
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinDate"></param>
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
    /// 会計情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">検索開始日</param>
    /// <param name="endDate">検索終了日</param>
    /// <param name="raiinNos"></param>
    /// <returns></returns>
    ///         
    public List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId
        , int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, bool hokenSeikyu, bool jihiSeikyu, ref List<CoWarningMessage> warningMessages)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate &&
            p.HokenId == (hokenId > 0 ? hokenId : p.HokenId) &&
            p.PtId == (ptId > 0 ? ptId : p.PtId) &&
            (!raiinNos.Any() || raiinNos.Contains(p.RaiinNo)) &&
            (!hokenSeikyu || p.PtFutan > 0) &&
            (!jihiSeikyu || p.JihiFutan > 0)
        );

        // 来院情報の取得
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r =>
            r.HpId == hpId &&
            r.PtId == (ptId > 0 ? ptId : r.PtId) &&
            r.SinDate >= startDate &&
            r.SinDate <= endDate &&
            (miseisanKbn != 0 || r.Status == 9) &&
            r.IsDeleted == DeleteStatus.None
            );
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.IsDelete == DeleteStatus.None
        );

        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            (hokenKbn != 1 || new int[] { 1, 2 }.Contains(p.HokenKbn))
        );

        var hokenMsts = NoTrackingDataContext.HokenMsts;
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
            h => h.StartDate <= endDate && h.PrefNo == 0 && new int[] { 0, 1, 3, 4, 8, 9 }.Contains(h.HokenSbtKbn)
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

        var join = (
            from kaikeiInf in kaikeiInfs
            join raiinInf in raiinInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
            join ptInf in ptInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId } into ptInfJoins
            from ptInfJoin in ptInfJoins.DefaultIfEmpty()
            join ptHokenInf in ptHokenInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfJoins
            from ptHokenInfJoin in ptHokenInfJoins.DefaultIfEmpty()
            join hokenMst in houbetuMsts on
                new { ptHokenInfJoin.HpId, ptHokenInfJoin.HokenNo, ptHokenInfJoin.HokenEdaNo } equals
                new { hokenMst.hokenMst.HpId, hokenMst.hokenMst.HokenNo, hokenMst.hokenMst.HokenEdaNo } into hokenMstJoins
            from hokenMstJoin in hokenMstJoins.DefaultIfEmpty()
            where
                ptHokenInfJoin != null
            orderby
                kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.RaiinNo
            select new
            {
                kaikeiInf,
                ptInf = ptInfJoin,
                ptHokenInf = ptHokenInfJoin,
                hokenMst = hokenMstJoin
            }
            ).ToList();

        var entities = join.Select(
            data =>
                new CoKaikeiInfModel(
                    data.kaikeiInf,
                    data.ptInf,
                    data.ptHokenInf,
                    data.hokenMst?.hokenMst ?? new(),
                    FindPtKohi(
                        hpId, ptId, data.kaikeiInf.SinDate,
                        new HashSet<int> { data.kaikeiInf.Kohi1Id, data.kaikeiInf.Kohi2Id, data.kaikeiInf.Kohi3Id, data.kaikeiInf.Kohi4Id }),
                    FindSyunoSeikyu(hpId, data.kaikeiInf.PtId, data.kaikeiInf.RaiinNo),
                    FindKaikeiDetail(hpId, data.kaikeiInf.PtId, data.kaikeiInf.RaiinNo)
                )
            )
            .ToList();
        List<CoKaikeiInfModel> results = new List<CoKaikeiInfModel>();
        int maxNyukinDay = 0;

        long prePtNum = 0;
        List<CoWarningMessage> retWarningMessages = new List<CoWarningMessage>();

        entities?.ForEach(entity =>
        {

            if (entity.SyunoSeikyu.SeikyuGaku != entity.SyunoSeikyu.NewSeikyuGaku)
            {
                if (entity.PtNum != prePtNum)
                {
                    retWarningMessages.Add(new CoWarningMessage() { PtNum = entity.PtNum, WarningMessage = $"請求金額に差異がある来院があります。" });
                    prePtNum = entity.PtNum;
                }

                retWarningMessages.Last().Detail.Add($"診療日：{CIUtil.SDateToShowSDate(entity.SinDate)}");
            }

            if (saiKbn == 0 && entity.SyunoSeikyu.SeikyuGaku != entity.SyunoSeikyu.NewSeikyuGaku)
            {
                // 請求額と新請求額に差異がある場合は印字しない
            }
            else if (misyuKbn == 1 && entity.SyunoSeikyu.NyukinKbn != 1)
            {
                // 一部入金以外は印字しない
            }
            else if (seikyuKbn == 0 && entity.SyunoSeikyu.SeikyuGaku == 0)
            {
                // 請求額が0の場合は印字しない
            }
            else
            {
                results.Add(
                    new CoKaikeiInfModel(
                        entity.KaikeiInf,
                        entity.PtInf,
                        entity.PtHokenInf,
                        entity.HokenMst,
                        entity.PtKohis,
                        entity.SyunoSeikyu,
                        entity.KaikeiDetails
                    ));
                maxNyukinDay = Math.Max(maxNyukinDay, entity.SyunoSeikyu.MaxNyukinDate);
            }
        }
        );


        // 最大入金日を今回入金額として扱う日に設定していく
        foreach (CoKaikeiInfModel kaikeiInf in results)
        {
            kaikeiInf.SyunoSeikyu.TargetNyukinDate = maxNyukinDay;
        }

        warningMessages.AddRange(retWarningMessages);
        return results;
    }

    /// <summary>
    /// 会計情報を取得する（入金日ベース）
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">検索開始日</param>
    /// <param name="endDate">検索終了日</param>
    /// <param name="raiinNos"></param>
    /// <returns></returns>
    ///         
    public List<CoKaikeiInfModel> FindKaikeiInfNyukinBase(int hpId, long ptId, int startDate, int endDate, int hokenId
        , int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, bool hokenSeikyu, bool jihiSeikyu, ref List<CoWarningMessage> warningMessages)
    {
        var nyukinInfs = NoTrackingDataContext.SyunoNyukin.Where(p =>
            p.HpId == hpId &&
            p.NyukinDate >= startDate &&
            p.NyukinDate <= endDate &&
            ((ptId <= 0) || p.PtId == ptId)
        ).GroupBy(p => new { p.HpId, p.PtId, p.RaiinNo });

        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            (hokenId <= 0 || p.HokenId == hokenId) &&
            ((ptId <= 0) || p.PtId == ptId) &&
            (!hokenSeikyu || p.PtFutan > 0) &&
            (!jihiSeikyu || p.JihiFutan > 0)
        );
        // 来院情報の取得
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r =>
            r.HpId == hpId &&
            (ptId <= 0 || r.PtId == ptId) &&
            (miseisanKbn != 0 || r.Status == 9) &&
            r.IsDeleted == DeleteStatus.None);
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.IsDelete == DeleteStatus.None
        );

        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            ((hokenKbn != 1) || new int[] { 1, 2 }.Contains(p.HokenKbn)));

        var hokenMsts = NoTrackingDataContext.HokenMsts;
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
            h => h.StartDate <= endDate && h.PrefNo == 0 && new int[] { 0, 1, 3, 4, 8, 9 }.Contains(h.HokenSbtKbn)
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


        var join = (
            from nyukinInf in nyukinInfs
            join kaikeiInf in kaikeiInfs on
                new { nyukinInf.Key.HpId, nyukinInf.Key.PtId, nyukinInf.Key.RaiinNo } equals
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo }
            join raiinInf in raiinInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
            join ptInf in ptInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId } into ptInfJoins
            from ptInfJoin in ptInfJoins.DefaultIfEmpty()
            join ptHokenInf in ptHokenInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInfJoins
            from ptHokenInfJoin in ptHokenInfJoins.DefaultIfEmpty()
            join hokenMst in houbetuMsts on
                new { ptHokenInfJoin.HpId, ptHokenInfJoin.HokenNo, ptHokenInfJoin.HokenEdaNo } equals
                new { hokenMst.hokenMst.HpId, hokenMst.hokenMst.HokenNo, hokenMst.hokenMst.HokenEdaNo } into hokenMstJoins
            from hokenMstJoin in hokenMstJoins.DefaultIfEmpty()
            where
                ptHokenInfJoin != null
            select new
            {
                kaikeiInf,
                ptInf = ptInfJoin,
                ptHokenInf = ptHokenInfJoin,
                hokenMst = hokenMstJoin
            }
            ).ToList();

        var entities = join.Select(
            data =>
                new CoKaikeiInfModel(
                    data.kaikeiInf,
                    data.ptInf,
                    data.ptHokenInf,
                    data.hokenMst?.hokenMst ?? new(),
                    FindPtKohi(
                        hpId, ptId, data.kaikeiInf.SinDate,
                        new HashSet<int> { data.kaikeiInf.Kohi1Id, data.kaikeiInf.Kohi2Id, data.kaikeiInf.Kohi3Id, data.kaikeiInf.Kohi4Id }),
                    FindSyunoSeikyu(hpId, data.kaikeiInf.PtId, data.kaikeiInf.RaiinNo, startDate, endDate),
                    FindKaikeiDetail(hpId, data.kaikeiInf.PtId, data.kaikeiInf.RaiinNo)
                )
            )
            .ToList();
        List<CoKaikeiInfModel> results = new();
        int maxNyukinDate = 0;

        long prePtNum = 0;
        List<CoWarningMessage> retWarningMessages = new();

        entities?.ForEach(entity =>
        {
            if (entity.SyunoSeikyu.SeikyuGaku != entity.SyunoSeikyu.NewSeikyuGaku)
            {
                if (entity.PtNum != prePtNum)
                {
                    retWarningMessages.Add(new CoWarningMessage() { PtNum = entity.PtNum, WarningMessage = $"請求金額に差異がある来院があります。" });
                    prePtNum = entity.PtNum;
                }

                retWarningMessages.Last().Detail.Add($"診療日：{CIUtil.SDateToShowSDate(entity.SinDate)}");
            }

            if (saiKbn == 0 && entity.SyunoSeikyu.SeikyuGaku != entity.SyunoSeikyu.NewSeikyuGaku)
            {
                // 請求額と新請求額に差異がある場合は印字しない
            }
            else if (misyuKbn == 1 && entity.SyunoSeikyu.NyukinKbn != 1)
            {
                // 一部入金以外は印字しない
            }
            else if (seikyuKbn == 0 && entity.SyunoSeikyu.SeikyuGaku == 0)
            {
                // 請求額が0の場合は印字しない
            }
            else
            {
                results.Add(
                    new CoKaikeiInfModel(
                        entity.KaikeiInf,
                        entity.PtInf,
                        entity.PtHokenInf,
                        entity.HokenMst,
                        entity.PtKohis,
                        entity.SyunoSeikyu,
                        entity.KaikeiDetails
                    ));
                maxNyukinDate = Math.Max(maxNyukinDate, entity.SyunoSeikyu.MaxNyukinDate);
            }
        }
        );

        foreach (CoKaikeiInfModel kaikeiInf in results)
        {
            kaikeiInf.SyunoSeikyu.TargetNyukinDate = maxNyukinDate;
        }

        warningMessages.AddRange(retWarningMessages);
        return results;
    }

    public List<CoKaikeiInfListModel> FindKaikeiInfList(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
        int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, ref List<CoWarningMessage> warningMessages)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate
        );

        // 絞り込み
        var notAndConditions = kaikeiInfs;
        foreach (var ptCondition in ptConditions)
        {
            if (ptCondition.hokenId > 0)
            {
                notAndConditions =
                    notAndConditions.Where(p => !(p.PtId == ptCondition.ptId && p.HokenId == ptCondition.hokenId));
            }
            else
            {
                notAndConditions =
                    notAndConditions.Where(p => p.PtId != ptCondition.ptId);
            }
        }
        kaikeiInfs = kaikeiInfs.Except(notAndConditions);


        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            p.IsDelete == DeleteStatus.None
        );

        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p =>
            p.HpId == hpId &&
            (hokenKbn == 1 ? new int[] { 1, 2 }.Contains(p.HokenKbn) : true));

        var hokenMsts = NoTrackingDataContext.HokenMsts;
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
            h => h.StartDate <= endDate && h.PrefNo == 0 && new int[] { 0, 1, 3, 4, 8, 9 }.Contains(h.HokenSbtKbn)
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

        // 来院情報の取得
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r =>
            r.HpId == hpId &&
            r.SinDate >= startDate &&
            r.SinDate <= endDate &&
            (miseisanKbn != 0 || r.Status == 9) &&
            r.IsDeleted == DeleteStatus.None);

        // 収納請求
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate &&
            (saiKbn == 0 ? p.SeikyuGaku == p.NewSeikyuGaku : true) &&
            (misyuKbn == 1 ? p.NyukinKbn == 1 : true) &&
            (seikyuKbn == 0 ? p.SeikyuGaku > 0 : true)
        );

        // 収納入金
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate &&
            p.IsDeleted == DeleteStatus.None
        );

        // 収納入金をグループ化
        var syunoNyukinGroups = (
                from syunoNyukin in syunoNyukins
                group syunoNyukin by
                    new { syunoNyukin.HpId, syunoNyukin.PtId, syunoNyukin.SinDate, syunoNyukin.RaiinNo } into nyukinGroups
                select new
                {
                    nyukinGroups.Key.HpId,
                    nyukinGroups.Key.PtId,
                    nyukinGroups.Key.SinDate,
                    nyukinGroups.Key.RaiinNo,
                    NyukinGaku = nyukinGroups.Sum(p => p.NyukinGaku),
                    NyukinAdjust = nyukinGroups.Sum(p => p.AdjustFutan)
                }
            );

        // 収納請求と収納入金を結合
        var syunoInfs = (
                from syunoSeikyu in syunoSeikyus
                join syunoNyukin in syunoNyukinGroups on
                    new { syunoSeikyu.HpId, syunoSeikyu.PtId, syunoSeikyu.SinDate, syunoSeikyu.RaiinNo } equals
                    new { syunoNyukin.HpId, syunoNyukin.PtId, syunoNyukin.SinDate, syunoNyukin.RaiinNo } into syunoNyukinJoins
                from syunoNyukinJoin in syunoNyukinJoins.DefaultIfEmpty()
                select new
                {
                    syunoSeikyu,
                    syunoNyukinJoin
                }
            );

        // JOIN
        var join = (
            from kaikeiInf in kaikeiInfs
            join ptInf in ptInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join ptHokenInf in ptHokenInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            join hokenMst in houbetuMsts on
                new { ptHokenInf.HpId, ptHokenInf.HokenNo, ptHokenInf.HokenEdaNo } equals
                new { hokenMst.hokenMst.HpId, hokenMst.hokenMst.HokenNo, hokenMst.hokenMst.HokenEdaNo }
            join raiinInf in raiinInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
            join syunoInf in syunoInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.RaiinNo } equals
                new { syunoInf.syunoSeikyu.HpId, syunoInf.syunoSeikyu.PtId, syunoInf.syunoSeikyu.SinDate, syunoInf.syunoSeikyu.RaiinNo }
            where
                hokenMst.hokenMst.StartDate <= kaikeiInf.SinDate &&
                hokenMst.hokenMst.EndDate >= kaikeiInf.SinDate
            group new { kaikeiInf, ptInf, syunoInf } by
                new
                {
                    kaikeiInf.HpId,
                    kaikeiInf.PtId,
                    PtId2 = syunoInf.syunoSeikyu.PtId,
                    HpId2 = syunoInf.syunoSeikyu.HpId,
                    ptInf.PtNum,
                    ptInf.Name,
                    ptInf.KanaName,
                    ptInf.Birthday,
                    ptInf.Sex,
                    ptInf.HomePost,
                    ptInf.HomeAddress1,
                    ptInf.HomeAddress2,
                    ptInf.Tel1,
                    ptInf.Tel2,
                    ptInf.RenrakuTel
                } into kaikeiInfGroups
            select new
            {
                kaikeiInfGroups.Key.HpId,
                kaikeiInfGroups.Key.PtId,
                kaikeiInfGroups.Key.PtNum,
                kaikeiInfGroups.Key.Name,
                kaikeiInfGroups.Key.KanaName,
                kaikeiInfGroups.Key.Birthday,
                kaikeiInfGroups.Key.Sex,
                kaikeiInfGroups.Key.HomePost,
                kaikeiInfGroups.Key.HomeAddress1,
                kaikeiInfGroups.Key.HomeAddress2,
                kaikeiInfGroups.Key.Tel1,
                kaikeiInfGroups.Key.Tel2,
                kaikeiInfGroups.Key.RenrakuTel,
                SeikyuGaku = kaikeiInfGroups.Sum(p => p.syunoInf.syunoSeikyu.SeikyuGaku),
                NyukinGaku = kaikeiInfGroups.Sum(p => p.syunoInf.syunoNyukinJoin.NyukinGaku),
                NyukinAdjust = kaikeiInfGroups.Sum(p => p.syunoInf.syunoNyukinJoin.NyukinAdjust),
                Misyu = kaikeiInfGroups.Sum(p => p.syunoInf.syunoSeikyu.SeikyuGaku) - kaikeiInfGroups.Sum(p => p.syunoInf.syunoNyukinJoin.NyukinGaku),
                Tensu = kaikeiInfGroups.Sum(p => p.kaikeiInf.Tensu),
                TotalIryohi = kaikeiInfGroups.Sum(p => p.kaikeiInf.TotalIryohi),
                PtFutan = kaikeiInfGroups.Sum(p => p.kaikeiInf.PtFutan),
                AdjustFutan = kaikeiInfGroups.Sum(p => p.kaikeiInf.AdjustFutan),
                AdjustRound = kaikeiInfGroups.Sum(p => p.kaikeiInf.AdjustRound),
                TotalPtFutan = kaikeiInfGroups.Sum(p => p.kaikeiInf.TotalPtFutan),
                JihiFutan = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiFutan),
                JihiOuttax = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiOuttax),
                JihiTax = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiTax),
                JihiFutanFree = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiFutanTaxfree),
                JihiFutanOuttaxNr = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiFutanOuttaxNr),
                JihiFutanOuttaxGen = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiFutanOuttaxGen),
                JihiFutanTaxNr = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiFutanTaxNr),
                JihiFutanTaxGen = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiFutanTaxGen),
                JihiOuttaxNr = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiOuttaxNr),
                JihiOuttaxGen = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiOuttaxGen),
                JihiTaxNr = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiTaxNr),
                JihiTaxGen = kaikeiInfGroups.Sum(p => p.kaikeiInf.JihiTaxGen)
            }
            ).ToList();

        var entities = join.Select(
            data =>
                new CoKaikeiInfListModel(
                    data.HpId,
                        data.PtId,
                        data.PtNum,
                        data.Name,
                        data.KanaName,
                        data.Sex,
                        data.Birthday,
                        data.HomePost,
                        data.HomeAddress1,
                        data.HomeAddress2,
                        data.Tel1,
                        data.Tel2,
                        data.RenrakuTel,
                        // 請求額、入金額、未収額については、後で再設定する
                        // (この時点では、同一来院に複数保険種の保険を使用した場合、重複した金額が入ってしまう為）
                        data.SeikyuGaku,
                        data.NyukinGaku,
                        data.NyukinAdjust,
                        data.Misyu,
                        data.Tensu,
                        data.TotalIryohi,
                        data.PtFutan,
                        data.AdjustFutan,
                        data.AdjustRound,
                        data.TotalPtFutan,
                        data.JihiFutan,
                        data.JihiOuttax,
                        data.JihiTax,
                        data.JihiFutanFree,
                        data.JihiFutanOuttaxNr,
                        data.JihiFutanOuttaxGen,
                        data.JihiFutanTaxNr,
                        data.JihiFutanTaxGen,
                        data.JihiOuttaxNr,
                        data.JihiOuttaxGen,
                        data.JihiTaxNr,
                        data.JihiTaxGen,
                        null,
                        null,
                        null,
                        null,
                        null
                )
            )
            .ToList();
        List<CoKaikeiInfListModel> results = new List<CoKaikeiInfListModel>();
        List<CoKaikeiInfListModel> tmpResults = new List<CoKaikeiInfListModel>();

        entities?.ForEach(entity =>
        {

            // 収納請求、収納入金の情報を改めて取得しなおす
            var filterdSyunoInfs = (
                from syunoSeikyu in syunoSeikyus
                join syunoNyukin in syunoNyukinGroups on
                    new { syunoSeikyu.HpId, syunoSeikyu.PtId, syunoSeikyu.SinDate, syunoSeikyu.RaiinNo } equals
                    new { syunoNyukin.HpId, syunoNyukin.PtId, syunoNyukin.SinDate, syunoNyukin.RaiinNo } into syunoNyukinJoins
                from syunoNyukinJoin in syunoNyukinJoins.DefaultIfEmpty()
                where
                    syunoSeikyu.HpId == entity.HpId &&
                    syunoSeikyu.PtId == entity.PtId
                select new
                {
                    syunoSeikyu,
                    syunoNyukinJoin
                }
            );

            tmpResults.Add(
                new CoKaikeiInfListModel(
                    entity.HpId,
                    entity.PtId,
                    entity.PtNum,
                    entity.PtName,
                    entity.PtKanaName,
                    entity.Sex,
                    entity.BirthDay,
                    entity.PostCd,
                    entity.Address1,
                    entity.Address2,
                    entity.Tel1,
                    entity.Tel2,
                    entity.RenrakuTel,
                    // 請求額、入金額、入金調整額、未収額について重複のない金額を設定する
                    filterdSyunoInfs.Sum(p => p.syunoSeikyu.SeikyuGaku),
                    filterdSyunoInfs.Sum(p => p.syunoNyukinJoin.NyukinGaku),
                    filterdSyunoInfs.Sum(p => p.syunoNyukinJoin.NyukinAdjust),
                    filterdSyunoInfs.Sum(p => p.syunoSeikyu.SeikyuGaku) - filterdSyunoInfs.Sum(p => p.syunoNyukinJoin.NyukinGaku),
                    entity.Tensu,
                    entity.TotalIryohi,
                    entity.PtFutan,
                    entity.AdjustFutan,
                    entity.AdjustRound,
                    entity.TotalPtFutan,
                    entity.JihiFutan,
                    entity.JihiOuttax,
                    entity.JihiTax,
                    entity.JihiFutanFree,
                    entity.JihiFutanOuttaxNr,
                    entity.JihiFutanOuttaxGen,
                    entity.JihiFutanTaxNr,
                    entity.JihiFutanTaxGen,
                    entity.JihiOuttaxNr,
                    entity.JihiOuttaxGen,
                    entity.JihiTaxNr,
                    entity.JihiTaxGen,
                    FindJihiSinKoui(hpId, entity.PtId, startDate, endDate, ptConditions.FindAll(p => p.ptId == entity.PtId && p.hokenId > 0).Select(p => p.hokenId).ToList()),
                    FindPtMemo(hpId, entity.PtId),
                    FindPtGrpInf(hpId, entity.PtId),
                    FindTaxSum(hpId, entity.PtId, startDate, endDate),
                    FindJihiSbtKingaku(hpId, entity.PtId, startDate, endDate)
                ));

        }
        );

        // ソート
        if (sort == 0)
        {
            // 患者番号、カナ氏名順
            results = tmpResults.OrderBy(p => p.PtNum).ThenBy(p => p.PtKanaName).ToList();
        }
        else if (sort == 1)
        {
            // カナ氏名、患者番号順
            results = tmpResults.OrderBy(p => p.PtKanaName).ThenBy(p => p.PtNum).ToList();
        }
        else
        {
            // 入力順
            List<long> ptIds = ptConditions.GroupBy(p => p.ptId).Select(p => p.Key).ToList();

            foreach (long ptId in ptIds)
            {
                var tmps = tmpResults.FindAll(p => p.PtId == ptId).OrderBy(p => p.PtKanaName);

                foreach (var tmp in tmps)
                {
                    results.Add(tmp);
                }
            }
        }

        List<CoWarningMessage> temp = CheckKaikeiInfList(hpId, startDate, endDate, ptConditions, grpConditions,
            sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn);

        if (temp != null && temp.Any())
        {
            warningMessages.AddRange(temp);
        }

        return results;
    }

    public List<CoWarningMessage> CheckKaikeiInfList(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
        int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate
        );

        // 絞り込み
        var notAndConditions = kaikeiInfs;
        foreach ((long ptId, int hokenId) ptCondition in ptConditions)
        {
            if (ptCondition.hokenId > 0)
            {
                notAndConditions =
                    notAndConditions.Where(p => !(p.PtId == ptCondition.ptId && p.HokenId == ptCondition.hokenId));
            }
            else
            {
                notAndConditions =
                    notAndConditions.Where(p => p.PtId != ptCondition.ptId);
            }
        }
        kaikeiInfs = kaikeiInfs.Except(notAndConditions);


        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            p.IsDelete == DeleteStatus.None
        );

        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p =>
            p.HpId == hpId &&
            (hokenKbn != 1 || new int[] { 1, 2 }.Contains(p.HokenKbn)));

        var hokenMsts = NoTrackingDataContext.HokenMsts;

        // 来院情報の取得
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r =>
            r.HpId == hpId &&
            r.SinDate >= startDate &&
            r.SinDate <= endDate &&
            (miseisanKbn != 0 || r.Status == 9) &&
            r.IsDeleted == DeleteStatus.None
            );

        // 収納請求
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate
        );

        // 収納入金
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate &&
            p.IsDeleted == DeleteStatus.None
        );

        // 収納入金をグループ化
        var syunoNyukinGroups = (
                from syunoNyukin in syunoNyukins
                group syunoNyukin by
                    new { syunoNyukin.HpId, syunoNyukin.PtId, syunoNyukin.SinDate, syunoNyukin.RaiinNo } into nyukinGroups
                select new
                {
                    nyukinGroups.Key.HpId,
                    nyukinGroups.Key.PtId,
                    nyukinGroups.Key.SinDate,
                    nyukinGroups.Key.RaiinNo,
                    NyukinGaku = nyukinGroups.Sum(p => p.NyukinGaku),
                    NyukinAdjust = nyukinGroups.Sum(p => p.AdjustFutan)
                }
            );

        // 収納請求と収納入金を結合
        var syunoInfs = (
                from syunoSeikyu in syunoSeikyus
                join syunoNyukin in syunoNyukinGroups on
                    new { syunoSeikyu.HpId, syunoSeikyu.PtId, syunoSeikyu.SinDate, syunoSeikyu.RaiinNo } equals
                    new { syunoNyukin.HpId, syunoNyukin.PtId, syunoNyukin.SinDate, syunoNyukin.RaiinNo } into syunoNyukinJoins
                from syunoNyukinJoin in syunoNyukinJoins.DefaultIfEmpty()
                select new
                {
                    syunoSeikyu
                }
            );

        // JOIN
        var join = (
            from kaikeiInf in kaikeiInfs
            join ptInf in ptInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join ptHokenInf in ptHokenInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            join hokenMst in hokenMsts on
                new { ptHokenInf.HpId, ptHokenInf.HokenNo, ptHokenInf.HokenEdaNo } equals
                new { hokenMst.HpId, hokenMst.HokenNo, hokenMst.HokenEdaNo }
            join raiinInf in raiinInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.RaiinNo } equals
                new { raiinInf.HpId, raiinInf.PtId, raiinInf.RaiinNo }
            join syunoInf in syunoInfs on
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.RaiinNo } equals
                new { syunoInf.syunoSeikyu.HpId, syunoInf.syunoSeikyu.PtId, syunoInf.syunoSeikyu.SinDate, syunoInf.syunoSeikyu.RaiinNo }
            where
                hokenMst.StartDate <= kaikeiInf.SinDate &&
                hokenMst.EndDate >= kaikeiInf.SinDate
            select new
            {
                ptInf,
                syunoInf
            }
            ).ToList();

        long prePtNum = 0;
        int preSinDate = 0;
        List<CoWarningMessage> retWarningMessages = new();

        foreach (var j in join.FindAll(p => p.syunoInf.syunoSeikyu.SeikyuGaku != p.syunoInf.syunoSeikyu.NewSeikyuGaku))
        {
            if (j.ptInf.PtNum != prePtNum)
            {
                retWarningMessages.Add(new CoWarningMessage() { PtNum = j.ptInf.PtNum, WarningMessage = $"請求金額に差異がある来院があります。" });
                prePtNum = j.ptInf.PtNum;
                preSinDate = 0;
            }

            if (preSinDate != j.syunoInf.syunoSeikyu.SinDate)
            {
                retWarningMessages.Last().Detail.Add($"診療日：{CIUtil.SDateToShowSDate(j.syunoInf.syunoSeikyu.SinDate)}");
                preSinDate = j.syunoInf.syunoSeikyu.SinDate;
            }

        }

        return retWarningMessages;
    }

    public List<(int santeiKbn, int jihiSbt, double kingaku)> FindJihiSinKoui(int hpId, long ptId, int startDate, int endDate, List<int> hokenIds)
    {
        var sinRpInfs = NoTrackingDataContext.SinRpInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinYm >= startDate / 100 &&
            p.SinYm <= endDate / 100 &&
            p.IsDeleted == DeleteStatus.None
        );

        var sinKouis = NoTrackingDataContext.SinKouis.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinYm >= startDate / 100 &&
            p.SinYm <= endDate / 100 &&
            p.IsDeleted == DeleteStatus.None
        );

        if (hokenIds != null && hokenIds.Any())
        {
            sinKouis = sinKouis.Where(p => hokenIds.Contains(p.HokenId));
        }

        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinYm * 100 + p.SinDay >= startDate &&
            p.SinYm * 100 + p.SinDay <= endDate
        );

        var sinJoins = (

            from sinRp in sinRpInfs
            join sinKoui in sinKouis on
                new { sinRp.HpId, sinRp.PtId, sinRp.SinYm, sinRp.RpNo } equals
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo }
            join sinKouiCount in sinKouiCounts on
                new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo } equals
                new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
            where
                (sinRp.SanteiKbn == 2 || sinRp.SinId == 96 || sinKoui.CdKbn == "JS") && sinKoui.CdKbn != "SZ"
            select new
            {
                sinKoui.HpId,
                sinKoui.PtId,
                sinRp.SanteiKbn,
                sinKoui.JihiSbt,
                Kingaku = sinKoui.Ten * sinKouiCount.Count
            }
            ).ToList();

        var groups = (

            from sinJoin in sinJoins
            group sinJoin by
                new { sinJoin.HpId, sinJoin.PtId, sinJoin.SanteiKbn, sinJoin.JihiSbt } into sinGroups
            select new
            {
                sinGroups.Key.HpId,
                sinGroups.Key.PtId,
                sinGroups.Key.SanteiKbn,
                sinGroups.Key.JihiSbt,
                Kingaku = sinGroups.Sum(p => p.Kingaku)
            }
            );


        var entities = groups.AsEnumerable().Select(
            data =>
                (
                    data.SanteiKbn,
                    data.JihiSbt,
                    data.Kingaku
                )
            )
            .ToList();

        List<(int santeiKbn, int jihiSbt, double kingaku)> results = new List<(int santeiKbn, int jihiSbt, double kingaku)>();

        entities?.ForEach(entity =>
        {

            results.Add(
                (
                    entity.SanteiKbn,
                    entity.JihiSbt,
                    entity.Kingaku
                ));
        }
        );

        return results;

    }

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

        var entities = ptKohiQuery.Select(
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

    public CoSyunoSeikyuModel FindSyunoSeikyu(int hpId, long ptId, long raiinNo)
    {
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus.Where(p =>
            p.HpId == hpId &&
            (ptId <= 0 || p.PtId == ptId) &&
            p.RaiinNo == raiinNo
        )
        .FirstOrDefault();

        CoSyunoSeikyuModel result = new();

        if (syunoSeikyus != null)
        {
            result = new CoSyunoSeikyuModel(
                syunoSeikyus,
                FindSyunoNyukin(hpId, syunoSeikyus.PtId, syunoSeikyus.RaiinNo)
            );
        }
        return result;
    }

    public CoSyunoSeikyuModel FindSyunoSeikyu(int hpId, long ptId, long raiinNo, int startDate, int endDate)
    {
        var syunoSeikyus = NoTrackingDataContext.SyunoSeikyus.FirstOrDefault(p =>
            p.HpId == hpId &&
            (ptId <= 0 || p.PtId == ptId) &&
            p.RaiinNo == raiinNo
        );

        CoSyunoSeikyuModel result = new();

        if (syunoSeikyus != null)
        {
            result = new CoSyunoSeikyuModel(
                syunoSeikyus,
                FindSyunoNyukin(hpId, syunoSeikyus.PtId, syunoSeikyus.RaiinNo, startDate, endDate)
            );
        }
        return result;
    }

    public List<CoKaikeiDetailModel> FindKaikeiDetail(int hpId, long ptId, long raiinNo)
    {
        var kaikeiDetails = NoTrackingDataContext.KaikeiDetails.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.RaiinNo == raiinNo
        )
        .ToList();

        List<CoKaikeiDetailModel> results = new List<CoKaikeiDetailModel>();

        kaikeiDetails?.ForEach(entity =>
        {
            results.Add(new CoKaikeiDetailModel(entity));
        }
        );

        return results;
    }

    public List<CoSyunoNyukinModel> FindSyunoNyukin(int hpId, long ptId, long raiinNo)
    {
        var payMethodMsts = NoTrackingDataContext.PaymentMethodMsts.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        );
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.RaiinNo == raiinNo &&
            p.IsDeleted == DeleteStatus.None
        );

        var join = (
            from syunoNyukin in syunoNyukins
            join payMethodMst in payMethodMsts on
                new { syunoNyukin.HpId, syunoNyukin.PaymentMethodCd } equals
                new { payMethodMst.HpId, payMethodMst.PaymentMethodCd } into joinPayMethods
            from joinPayMethod in joinPayMethods.DefaultIfEmpty()
            select new
            {
                syunoNyukin = syunoNyukin,
                payMethod = joinPayMethod
            }
            ).ToList();

        List<CoSyunoNyukinModel> results = new();

        join?.ForEach(entity =>
        {

            results.Add(
                new CoSyunoNyukinModel(
                    entity.syunoNyukin,
                    entity.payMethod
                ));

        }
                    );

        return results;
    }

    public List<CoSyunoNyukinModel> FindSyunoNyukin(int hpId, long ptId, long raiinNo, int startDate, int endDate)
    {
        var payMethodMsts = NoTrackingDataContext.PaymentMethodMsts.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        );
        var syunoNyukins = NoTrackingDataContext.SyunoNyukin.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.RaiinNo == raiinNo &&
            p.NyukinDate >= startDate &&
            p.NyukinDate <= endDate &&
            p.IsDeleted == DeleteStatus.None
        );

        var join = (
            from syunoNyukin in syunoNyukins
            join payMethodMst in payMethodMsts on
                new { syunoNyukin.HpId, syunoNyukin.PaymentMethodCd } equals
                new { payMethodMst.HpId, payMethodMst.PaymentMethodCd } into joinPayMethods
            from joinPayMethod in joinPayMethods.DefaultIfEmpty()
            select new
            {
                syunoNyukin = syunoNyukin,
                payMethod = joinPayMethod
            }

            ).ToList();

        List<CoSyunoNyukinModel> results = new List<CoSyunoNyukinModel>();

        join?.ForEach(entity =>
        {

            results.Add(
                new CoSyunoNyukinModel(
                    entity.syunoNyukin,
                    entity.payMethod
                ));

        }
        );

        return results;
    }

    /// <summary>
    /// オーダー情報取得（院外処方）
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">開始診療日</param>
    /// <param name="endDate">終了診療日</param>
    /// <param name="raiinNos">来院番号のリスト</param>
    /// <returns>
    /// 指定の患者の指定の診療日のオーダー情報
    /// 削除分は除く
    /// </returns>
    public List<CoOdrInfModel> FindOdrInfData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos)
    {
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
            o.HpId == hpId &&
            (ptId <= 0 || o.PtId == ptId) &&
            o.SinDate >= startDate &&
            o.SinDate <= endDate &&
            (!raiinNos.Any() || raiinNos.Contains(o.RaiinNo)) &&
            o.InoutKbn == 1 &&
            new int[] { 21, 22, 23, 28 }.Contains(o.OdrKouiKbn) &&
            o.IsDeleted == DeleteStatus.None);

        var joinQuery = (
            from odrInf in odrInfs
            where
                odrInf.HpId == hpId &&
                odrInf.PtId == ptId &&
                odrInf.IsDeleted == DeleteStatus.None
            orderby
                odrInf.RaiinNo, odrInf.OdrKouiKbn, odrInf.SortNo, odrInf.RpNo
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
    /// オーダー情報詳細を取得する（院外処方）
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">開始診療日</param>
    /// <param name="endDate">終了診療日</param>
    /// <param name="raiinNos">来院番号のリスト</param>
    /// <returns></returns>
    public List<CoOdrInfDetailModel> FindOdrInfDetailData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos)
    {
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(o =>
            o.HpId == hpId &&
            (ptId <= 0 || o.PtId == ptId) &&
            o.SinDate >= startDate &&
            o.SinDate <= endDate &&
            (!raiinNos.Any() || raiinNos.Contains(o.RaiinNo)) &&
            o.InoutKbn == 1 &&
            new int[] { 21, 22, 23, 28 }.Contains(o.OdrKouiKbn) &&
            o.IsDeleted == DeleteStatus.None);
        var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(o =>
            o.HpId == hpId &&
            (ptId <= 0 || o.PtId == ptId) &&
            o.SinDate >= startDate &&
            o.SinDate <= endDate &&
            (!raiinNos.Any() || raiinNos.Contains(o.RaiinNo)) &&
            !((o.ItemCd ?? string.Empty).StartsWith("8") && (o.ItemCd ?? string.Empty).Length == 9));

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
                odrInf
            }
        );

        var entities = joinQuery.AsEnumerable().Select(
            data =>
                new CoOdrInfDetailModel(
                    data.odrInfDetail,
                    data.odrInf
                )
            )
            .ToList();
        List<CoOdrInfDetailModel> results = new List<CoOdrInfDetailModel>();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoOdrInfDetailModel(
                    entity.OdrInfDetail,
                    entity.OdrInf
                    ));

        }
        );

        return results;
    }

    /// <summary>
    /// 患者病名情報を取得する
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public List<CoPtByomeiModel> FindPtByomei(int hpId, long ptId, int startDate, int endDate)
    {
        var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p =>
            p.HpId == hpId &&
            (ptId <= 0 || p.PtId == ptId) &&
            p.StartDate <= endDate &&
            ((p.TenkiKbn == TenkiKbnConst.Continued && p.TenkiDate == 0) || p.TenkiDate >= startDate) &&
            p.IsDeleted == DeleteStatus.None
        )
            .ToList();

        List<CoPtByomeiModel> results = new List<CoPtByomeiModel>();

        ptByomeis?.ForEach(entity =>
        {
            results.Add(
                new CoPtByomeiModel(
                    entity
                    ));
        }
        );
        return results;
    }

    /// <summary>
    /// 自費種別マスタを取得する
    /// </summary>
    /// <returns></returns>
    public List<CoJihiSbtMstModel> FindJihiSbtMst(int hpId)
    {
        var jihiSbtMsts = NoTrackingDataContext.JihiSbtMsts.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        )
            .OrderBy(p => p.SortNo)
            .ToList();

        List<CoJihiSbtMstModel> results = new List<CoJihiSbtMstModel>();

        jihiSbtMsts?.ForEach(entity =>
        {
            results.Add(
                new CoJihiSbtMstModel(
                    entity
                    ));
        }
        );
        return results;
    }

    /// <summary>
    /// 所見情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">開始日</param>
    /// <param name="endDate">終了日</param>
    /// <param name="raiinNos">来院番号</param>
    /// <returns></returns>
    public List<CoKarteInfModel> FindKarteInf(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos)
    {
        var karteInfs = NoTrackingDataContext.KarteInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate &&
            raiinNos.Contains(p.RaiinNo) &&
            p.IsDeleted == DeleteStatus.None
        )
            .ToList();

        List<CoKarteInfModel> results = new List<CoKarteInfModel>();

        karteInfs?.ForEach(entity =>
        {
            results.Add(
                new CoKarteInfModel(
                    entity
                    ));
        }
        );
        return results;

    }

    /// <summary>
    /// 患者メモを取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <returns></returns>
    public CoPtMemoModel FindPtMemo(int hpId, long ptId)
    {
        return
            new CoPtMemoModel(
                NoTrackingDataContext.PtMemos.FirstOrDefault(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.IsDeleted == DeleteStatus.None
                ) ?? new()
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ptId"></param>
    /// <returns></returns>
    public List<CoPtGrpInfModel> FindPtGrpInf(int hpId, long ptId)
    {
        var ptGroupInfs = NoTrackingDataContext.PtGrpInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.IsDeleted == DeleteStatus.None
        );

        var ptGrpItems = NoTrackingDataContext.PtGrpItems.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        );

        var join = (
                from ptGrpInf in ptGroupInfs
                join ptGrpItem in ptGrpItems on
                    new { ptGrpInf.HpId, ptGrpInf.GroupId, ptGrpInf.GroupCode } equals
                    new { ptGrpItem.HpId, GroupId = ptGrpItem.GrpId, GroupCode = ptGrpItem.GrpCode }
                select new
                {
                    ptGrpInf,
                    ptGrpItem
                }
            ).ToList();

        List<CoPtGrpInfModel> results = new List<CoPtGrpInfModel>();

        join?.ForEach(entity =>
        {
            results.Add(
                new CoPtGrpInfModel(
                    entity.ptGrpInf,
                    entity.ptGrpItem
                    ));
        }
        );
        return results;
    }

    public List<TaxSum> FindTaxSum(int hpId, long ptId, int startDate, int endDate)
    {
        var systemGenerationConfs = NoTrackingDataContext.SystemGenerationConfs.Where(p =>
            p.HpId == hpId &&
            p.GrpCd == 3001
        ).ToList();

        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate >= startDate &&
                p.SinDate <= endDate
            ).GroupBy(
                p => new { p.HpId, p.PtId, SinYm = p.SinDate / 100 }
            ).Select(
                p => new
                {
                    p.Key.HpId,
                    p.Key.PtId,
                    p.Key.SinYm,
                    JihiFutanOuttaxNr = p.Sum(q => q.JihiFutanOuttaxNr),
                    JihiFutanOuttaxGen = p.Sum(q => q.JihiFutanOuttaxGen),
                    JihiFutanTaxNr = p.Sum(q => q.JihiFutanTaxNr),
                    JihiFutanTaxGen = p.Sum(q => q.JihiFutanTaxGen),
                    JihiOuttaxNr = p.Sum(q => q.JihiOuttaxNr),
                    JihiOuttaxGen = p.Sum(q => q.JihiOuttaxGen),
                    JihiTaxNr = p.Sum(q => q.JihiTaxNr),
                    JihiTaxGen = p.Sum(q => q.JihiTaxGen)
                }
            ).ToList();

        List<TaxSum> TaxSums = new List<TaxSum>();

        kaikeiInfs?.ForEach(kaikeiInf =>
        {
            var taxConfs =
                systemGenerationConfs.FindAll(p => p.GrpEdaNo == 0 && p.StartDate <= kaikeiInf.SinYm * 100 + 1 && p.EndDate >= kaikeiInf.SinYm * 100 + 1);

            if (taxConfs.Any())
            {
                AddTaxSum(taxConfs.First().Val, kaikeiInf.JihiFutanOuttaxNr, kaikeiInf.JihiFutanTaxNr, kaikeiInf.JihiOuttaxNr, kaikeiInf.JihiTaxNr);
            }

            taxConfs =
                systemGenerationConfs.FindAll(p => p.GrpEdaNo == 1 && p.StartDate <= kaikeiInf.SinYm * 100 + 1 && p.EndDate >= kaikeiInf.SinYm * 100 + 1);

            if (taxConfs.Any())
            {
                AddTaxSum(taxConfs.First().Val, kaikeiInf.JihiFutanOuttaxGen, kaikeiInf.JihiFutanTaxGen, kaikeiInf.JihiOuttaxGen, kaikeiInf.JihiTaxGen);
            }
        }
        );

        return TaxSums;

        #region local method
        void SetTaxSumParam(ref TaxSum AtaxSum, double AouttaxFutan, double AtaxFutan, int AoutTaxZei, int ATaxZei)
        {
            AtaxSum.OuttaxFutan += AouttaxFutan;
            AtaxSum.TaxFutan += AtaxFutan;
            AtaxSum.OuttaxZei += AoutTaxZei;
            AtaxSum.TaxZei += ATaxZei;
        }

        void AddTaxSum(int Arate, double AouttaxFutan, double AtaxFutan, int AoutTaxZei, int ATaxZei)
        {
            TaxSum taxSum;
            if (TaxSums.Any(p => p.Rate == Arate))
            {
                taxSum = TaxSums.FirstOrDefault(p => p.Rate == Arate) ?? new();
                SetTaxSumParam(ref taxSum, AouttaxFutan, AtaxFutan, AoutTaxZei, ATaxZei);
            }
            else
            {
                taxSum = new TaxSum();
                taxSum.Rate = Arate;
                SetTaxSumParam(ref taxSum, AouttaxFutan, AtaxFutan, AoutTaxZei, ATaxZei);

                TaxSums.Add(taxSum);
            }
        }
        #endregion
    }

    public List<CoPtGrpNameMstModel> FindPtGrpNameMst(int hpId)
    {
        var grpNameMsts = NoTrackingDataContext.PtGrpNameMsts.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        )
            .ToList();

        List<CoPtGrpNameMstModel> results = new List<CoPtGrpNameMstModel>();

        grpNameMsts?.ForEach(entity =>
        {
            results.Add(
                new CoPtGrpNameMstModel(
                    entity
                    ));
        }
        );
        return results;
    }
    /// <summary>
    /// 患者グループ項目マスタを取得する
    /// </summary>
    /// <returns></returns>
    /// 
    public List<CoPtGrpItemModel> FindPtGrpItemMst(int hpId)
    {
        var grpItemMsts = NoTrackingDataContext.PtGrpItems.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        )
            .ToList();

        List<CoPtGrpItemModel> results = new();

        grpItemMsts?.ForEach(entity =>
        {
            results.Add(
                new CoPtGrpItemModel(
                    entity
                    ));
        }
        );
        return results;
    }

    public List<CoRaiinInfModel> FindYoyakuRaiinInf(int hpId, long ptId, int sinDate)
    {
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinDate > sinDate &&
            p.Status == RaiinState.Reservation &&
            p.IsDeleted == DeleteStatus.None
            );

        var userMsts = NoTrackingDataContext.UserMsts.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        );

        var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        );

        var raiinCmtInfs = NoTrackingDataContext.RaiinCmtInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.CmtKbn == 1 &&
            p.IsDelete == DeleteStatus.None
        );

        var joinQuery = (
                from raiinInf in raiinInfs
                join userMst in userMsts on
                    new { raiinInf.HpId, UserId = raiinInf.YoyakuId } equals
                    new { userMst.HpId, userMst.UserId } into userMstJoins
                from userMstJoin in userMstJoins.DefaultIfEmpty()
                join uketukeSbtMst in uketukeSbtMsts on
                    new { raiinInf.HpId, KbnId = raiinInf.UketukeSbt } equals
                    new { uketukeSbtMst.HpId, uketukeSbtMst.KbnId } into uketukeSbtMstJoins
                from uketukeSbtMstJoin in uketukeSbtMstJoins.DefaultIfEmpty()
                join raiinCmtInf in raiinCmtInfs on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                    new { raiinCmtInf.HpId, raiinCmtInf.PtId, raiinCmtInf.SinDate, raiinCmtInf.RaiinNo } into raiinCmtInfJoins
                from raiinCmtInfJoin in raiinCmtInfJoins.DefaultIfEmpty()
                orderby
                    raiinInf.SinDate, raiinInf.YoyakuTime
                select new
                {
                    raiinInf,
                    userMstJoin,
                    uketukeSbtMstJoin,
                    raiinCmtInfJoin
                }
            );

        var entities = joinQuery.AsEnumerable().Select(
            data =>
                new CoRaiinInfModel(
                    data.raiinInf,
                    data.userMstJoin,
                    data.uketukeSbtMstJoin,
                    data.raiinCmtInfJoin
                )
            )
            .ToList();
        List<CoRaiinInfModel> results = new List<CoRaiinInfModel>();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoRaiinInfModel(
                    entity.RaiinInf,
                    entity.UserMst,
                    entity.UketukeSbtMst,
                    entity.RaiinCmtInf
                    ));

        }
        );

        return results;
    }

    public List<CoSystemGenerationConfModel> FindSystemGenerationConf(int hpId, int GrpCd)
    {
        var systemGenerationConfs = NoTrackingDataContext.SystemGenerationConfs.Where(p =>
            p.HpId == hpId &&
            p.GrpCd == GrpCd
        )
        .ToList();

        List<CoSystemGenerationConfModel> results = new List<CoSystemGenerationConfModel>();

        systemGenerationConfs?.ForEach(entity =>
        {
            results.Add(
                new CoSystemGenerationConfModel(
                    entity
                    ));
        }
        );
        return results;
    }

    public List<long> GetPtNums(int hpId, List<long> ptIds)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            ptIds.Contains(p.PtId) &&
            p.IsDelete == DeleteStatus.None
        ).ToList();

        List<long> results = new List<long>();

        ptInfs?.ForEach(entity =>
        {
            results.Add(entity.PtNum);
        }
        );

        return results;
    }

    public long GetPtNum(int hpId, long ptId)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId
        ).ToList();

        long result = 0;

        if (ptInfs != null && ptInfs.Any())
        {
            result = ptInfs.First().PtNum;
        }

        return result;
    }

    public List<(long, int)> FindPtInf(int hpId, List<(int grpId, string grpCd)> grpConditions)
    {
        var ptGroupInfs = NoTrackingDataContext.PtGrpInfs.Where(p =>
            p.HpId == hpId &&
            p.IsDeleted == DeleteStatus.None
        );

        // 絞り込み
        var notAndConditions = ptGroupInfs;
        foreach ((int grpId, string grpCd) grpCondition in grpConditions)
        {
            if (!string.IsNullOrEmpty(grpCondition.grpCd))
            {
                notAndConditions =
                    notAndConditions.Where(p => !(p.GroupId == grpCondition.grpId && p.GroupCode == grpCondition.grpCd));
            }
            else
            {
                notAndConditions =
                    notAndConditions.Where(p => p.GroupId != grpCondition.grpId);
            }
        }
        ptGroupInfs = ptGroupInfs.Except(notAndConditions);

        List<(long, int)> results = new();

        if (ptGroupInfs != null)
        {
            foreach (var ptGroupInf in ptGroupInfs)
            {
                results.Add((ptGroupInf.PtId, 0));
            }
        }

        return results;
    }

    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <returns></returns>
    public CoPtInfModel FindPtInf(int hpId, long ptId)
    {
        return
            new CoPtInfModel(
                NoTrackingDataContext.PtInfs.FirstOrDefault(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.IsDelete == DeleteStatus.None
                ) ?? new()
            );

    }

    /// <summary>
    /// 指定期間内の自費種別ごとの自費項目の金額を集計する
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public List<CoJihiSbtKingakuModel> FindJihiSbtKingaku(int hpId, long ptId, int startDate, int endDate)
    {
        var sinCounts = NoTrackingDataContext.SinKouiCounts.Where(s =>
            s.HpId == hpId &&
            s.PtId == ptId &&
            s.SinDate >= startDate &&
            s.SinDate <= endDate
        );

        var sinDtls = NoTrackingDataContext.SinKouiDetails.Where(d =>
            d.HpId == hpId &&
            d.PtId == ptId &&
            (d.OdrItemCd ?? string.Empty).StartsWith("J") &&
            d.IsDeleted == DeleteStatus.None
        );

        var tenMsts = NoTrackingDataContext.TenMsts.Where(t =>
            t.HpId == hpId &&
            t.ItemCd.StartsWith("J") &&
            t.StartDate <= endDate &&
            t.EndDate >= startDate
        );

        var sinDtlCounts =
            (from sinDtl in sinDtls
             join sinCount in sinCounts on
                 new { sinDtl.HpId, sinDtl.PtId, sinDtl.SinYm, sinDtl.RpNo, sinDtl.SeqNo } equals
                 new { sinCount.HpId, sinCount.PtId, sinCount.SinYm, sinCount.RpNo, sinCount.SeqNo } into sc
             from b in sc.DefaultIfEmpty()
             join tenMst in tenMsts on
                new { sinDtl.HpId, ItemCd = sinDtl.OdrItemCd } equals
                new { tenMst.HpId, tenMst.ItemCd } into tm
             from t in tm
             where
                t.StartDate <= b.SinDate && t.EndDate >= b.SinDate
             select new
             {
                 SinDtl = sinDtl,
                 SinCount = b,
                 JihiSbt = t.JihiSbt,
                 Kingaku = sinDtl.Ten
             }
            ).ToList();

        List<CoJihiSbtKingakuModel> results = new();

        sinDtlCounts?.ForEach(p =>
        {
            if (results.Any(q => q.JihiSbt == p.JihiSbt))
            {
                results.FirstOrDefault(r => r.JihiSbt == p.JihiSbt)!.Kingaku += p.Kingaku;
            }
            else
            {
                results.Add(new CoJihiSbtKingakuModel(p.JihiSbt, p.Kingaku));
            }
        }
        );

        return results;
    }

    public List<RaiinInfModel> GetOyaRaiinInfList(int hpId, List<long> raiinNoList, long ptId)
    {
        var oyaRaiinNoList = NoTrackingDataContext.RaiinInfs.Where(item =>
                                                        item.HpId == hpId
                                                        && item.PtId == ptId
                                                        && raiinNoList.Contains(item.RaiinNo))
                                                        .Select(item => item.OyaRaiinNo).ToList();
        var raiinList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.Status == RaiinState.Settled
                                                                            && oyaRaiinNoList.Contains(item.OyaRaiinNo))
                                                    .Select(item => new RaiinInfModel(item)).ToList();
        return raiinList;
    }

    public List<CoAccountDueListModel> GetAccountDueList(int hpId, long ptId)
    {
        // left table
        var seikyuList = NoTrackingDataContext.SyunoSeikyus.Where(item => item.HpId == hpId
                                                                          && item.PtId == ptId);

        var raiinList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                      && item.PtId == ptId
                                                                      && item.IsDeleted == DeleteTypes.None
                                                                      && item.Status > RaiinState.TempSave);

        var accountDueList = (from seikyu in seikyuList
                              join raiinItem in raiinList on new { seikyu.HpId, seikyu.PtId, seikyu.RaiinNo }
                                                          equals new { raiinItem.HpId, raiinItem.PtId, raiinItem.RaiinNo }
                              select new CoAccountDueListModel(seikyu.SinDate, seikyu.NyukinKbn, seikyu.RaiinNo, raiinItem.OyaRaiinNo)
                              )
                              .ToList();
        return accountDueList;
    }

    public List<PtGrpNameMstModel> GetPtGrpNameMstModels(int hpId)
    {
        var ptgrpNameMsts = NoTrackingDataContext.PtGrpNameMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0).ToList();
        var grpIdList = ptgrpNameMsts.Select(item => item.GrpId).Distinct().ToList();
        var ptGrpItems = NoTrackingDataContext.PtGrpItems.Where(x => x.HpId == hpId && x.IsDeleted == 0 && grpIdList.Contains(x.GrpId)).ToList();
        var query = from ptGrpName in ptgrpNameMsts
                    select new
                    {
                        PtGrpName = ptGrpName,
                        PtGrpItems = from ptGrpItem in ptGrpItems
                                     where ptGrpItem.GrpId == ptGrpName.GrpId
                                     select ptGrpItem
                    };
        var result = query.Select(x => new PtGrpNameMstModel(
                           x.PtGrpName,
                           x.PtGrpItems.AsEnumerable().Select(grpItem => new PtGrpItemModel(grpItem)).OrderBy(gi => gi.SortNo)
                       )).OrderBy(gr => gr.SortNo).ToList();
        return result;
    }

    public bool ExistCalculateRequest(int hpId, long ptId, int startDate, int endDate)
    {
        var calcStatuies = NoTrackingDataContext.CalcStatus.Any(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinDate >= startDate &&
            p.SinDate <= endDate &&
            p.Status == 1
        );

        return calcStatuies;
    }
}
