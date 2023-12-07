using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3010.Models;
using System.Linq.Expressions;

namespace Reporting.Statistics.Sta3010.DB;

public class CoSta3010Finder : RepositoryBase, ICoSta3010Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3010Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
    }
    public void ReleaseResource()
    {
        _hpInfFinder.ReleaseResource();
        DisposeDataContext();
    }

    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return _hpInfFinder.GetHpInf(hpId, sinDate);
    }

    /// <summary>
    /// オーダーセット一覧取得
    /// </summary>
    /// <param name="printConf"></param>
    public List<CoOdrSetModel> GetOdrSet(int hpId, CoSta3010PrintConf printConf)
    {
        //セットの世代を取得
        int generationId = GetGenerationId(hpId, printConf.StdDate);

        var setKbnMsts = NoTrackingDataContext.SetKbnMsts.Where(x => x.HpId == hpId && x.GenerationId == generationId && x.IsDeleted == 0);

        var setMsts = NoTrackingDataContext.SetMsts.Where(x => x.HpId == hpId && x.GenerationId == generationId && x.IsDeleted == 0);

        //条件１-セット区分
        var setKbnExpression = CreateSetKbnExpression(printConf.SetKbns);
        if (setKbnExpression != null)
        {
            setMsts = setMsts.Where(setKbnExpression);
        }

        var setOdrInfs = NoTrackingDataContext.SetOdrInf.Where(x => x.HpId == hpId && x.IsDeleted == 0);

        var setOdrInfDetails = NoTrackingDataContext.SetOdrInfDetail.Where(x => x.HpId == hpId);

        var tenMsts = NoTrackingDataContext.TenMsts.Where(x => x.HpId == hpId);

        //項目の有効期限
        var maxTenMsts = tenMsts
            .GroupBy(x => new { x.HpId, x.ItemCd })
            .Select(x => new
            {
                x.Key.HpId,
                x.Key.ItemCd,
                MaxStartDate = x.Max(d => d.StartDate),
                MaxEndDate = x.Max(d => d.EndDate)
            });

        //基準日の世代
        var stdDateTenMsts = tenMsts.Where(x => x.StartDate <= printConf.StdDate && printConf.StdDate <= x.EndDate)
            .Select(x => new { x.HpId, x.ItemCd, x.StartDate, x.EndDate, x.BuiKbn, x.CmtSbt, x.KensaItemCd, x.KensaItemSeqNo });

        //最新の世代
        var latestTenMsts = (
            from tenMst in tenMsts
            join maxTenMst in maxTenMsts on
                new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate } equals
                new { maxTenMst.HpId, maxTenMst.ItemCd, StartDate = maxTenMst.MaxStartDate }
            select new { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate, tenMst.EndDate, tenMst.BuiKbn, tenMst.CmtSbt, tenMst.KensaItemCd, tenMst.KensaItemSeqNo }
            );

        //最新の世代から基準日が期限内である項目を除く
        latestTenMsts = latestTenMsts
            .Where(x => !(x.StartDate <= printConf.StdDate && printConf.StdDate <= x.EndDate));

        //基準日の世代(基準日に有効な項目)　＋　最新の世代(基準日に有効じゃない項目)
        var unionTenMsts = stdDateTenMsts.Union(latestTenMsts);

        var kensaMsts = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId);

        var setOdrInfDetailList = setOdrInfDetails.ToList();
        var joinDetails = (
            from setOdrInfDetail in setOdrInfDetailList
            join unionTenMst in unionTenMsts on
                new { setOdrInfDetail.HpId, setOdrInfDetail.ItemCd } equals
                new { unionTenMst.HpId, unionTenMst.ItemCd } into unionTenMstJoins
            from unionTenMstJoin in unionTenMstJoins.DefaultIfEmpty()
            join maxTenMst in maxTenMsts on
                new { HpId = unionTenMstJoin != null ? unionTenMstJoin.HpId : 1, ItemCd = unionTenMstJoin != null ? unionTenMstJoin.ItemCd : string.Empty } equals
                new { maxTenMst.HpId, maxTenMst.ItemCd } into maxTenMstJoins
            from maxTenMstJoin in maxTenMstJoins.DefaultIfEmpty()
            join kensaMst in kensaMsts on
                new { HpId = unionTenMstJoin != null ? unionTenMstJoin.HpId : 1, KensaItemCd = unionTenMstJoin != null ? unionTenMstJoin.KensaItemCd : string.Empty, KensaItemSeqNo = unionTenMstJoin != null ? unionTenMstJoin.KensaItemSeqNo : 0 } equals
                new { kensaMst.HpId, kensaMst.KensaItemCd, kensaMst.KensaItemSeqNo } into kensaMstJoins
            from kensaMstJoin in kensaMstJoins.DefaultIfEmpty()
            select
                new
                {
                    setOdrInfDetail.HpId,
                    setOdrInfDetail.SetCd,
                    setOdrInfDetail.RpNo,
                    setOdrInfDetail.RpEdaNo,
                    setOdrInfDetail.RowNo,
                    setOdrInfDetail.ItemCd,
                    setOdrInfDetail.ItemName,
                    setOdrInfDetail.Suryo,
                    setOdrInfDetail.UnitName,
                    setOdrInfDetail.SyohoKbn,
                    setOdrInfDetail.SyohoLimitKbn,
                    setOdrInfDetail.DrugKbn,
                    BuiKbn = unionTenMstJoin != null ? unionTenMstJoin.BuiKbn : 0,
                    CmtSbt = unionTenMstJoin != null ? unionTenMstJoin.CmtSbt : 0,
                    KensaItemCd = kensaMstJoin != null ? kensaMstJoin.KensaItemCd : null,
                    KensaItemSeqNo = kensaMstJoin != null ? kensaMstJoin.KensaItemSeqNo : 0,
                    CenterItemCd1 = kensaMstJoin != null ? kensaMstJoin.CenterItemCd1 : null,
                    CenterItemCd2 = kensaMstJoin != null ? kensaMstJoin.CenterItemCd2 : null,
                    MaxEndDate = maxTenMstJoin != null ? maxTenMstJoin.MaxEndDate : 0
                }
            );

        var setKbnMstList = setKbnMsts.ToList();
        var joinQuery = (
            from setKbnMst in setKbnMstList
            join setMst in setMsts on
                new { setKbnMst.HpId, setKbnMst.GenerationId, setKbnMst.SetKbn } equals
                new { setMst.HpId, setMst.GenerationId, setMst.SetKbn }
            join setOdrInf in setOdrInfs on
                new { setMst.HpId, setMst.SetCd } equals
                new { setOdrInf.HpId, setOdrInf.SetCd } into setOdrInfJoins
            from setOdrInfJoin in setOdrInfJoins.DefaultIfEmpty()
            join joinDetail in joinDetails on
                new { HpId = setOdrInfJoin != null ? setOdrInfJoin.HpId : 1, SetCd = setOdrInfJoin != null ? setOdrInfJoin.SetCd : 0, RpNo = setOdrInfJoin != null ? setOdrInfJoin.RpNo : 0, RpEdaNo = setOdrInfJoin != null ? setOdrInfJoin.RpEdaNo : 0 } equals
                new { joinDetail.HpId, joinDetail.SetCd, joinDetail.RpNo, joinDetail.RpEdaNo } into joinDetailJoins
            from joinDetailJoin in joinDetailJoins.DefaultIfEmpty()
            select
                new
                {
                    setKbnMst,
                    setMst,
                    setOdrInf = setOdrInfJoin ?? new(),
                    joinDetail = joinDetailJoin
                }
            ).Distinct().ToList();

        #region 条件２-対象データ

        var tgtDetails = joinDetails;
        switch (printConf.TgtData)
        {
            case 1:
                {
                    //期限切れ
                    tgtDetails = tgtDetails.Where(x => x.MaxEndDate < printConf.StdDate);
                    break;
                }
            case 2:
                {
                    //項目選択
                    //検索ワード
                    if ((printConf.ListSearchWord?.Count ?? 0) > 0)
                    {
                        if (printConf.SearchOpt == 1)
                        {
                            //and検索
                            tgtDetails = tgtDetails.Where(x => (printConf.ListSearchWord ?? new()).All(key => x.ItemName.Contains(key)));
                        }
                        else
                        {
                            //or検索
                            tgtDetails = tgtDetails.Where(x => (printConf.ListSearchWord ?? new()).Any(key => x.ItemName.Contains(key)));
                        }
                    }

                    //検索項目
                    if ((printConf.ItemCds?.Count ?? 0) > 0)
                    {
                        tgtDetails = tgtDetails.Where(x => (printConf.ItemCds ?? new()).Any(key => x.ItemCd.Contains(key)));
                    }
                    break;
                }
            case 3:
                {
                    //フリーコメント(処方箋コメントも含む)
                    tgtDetails = tgtDetails.Where(x => x.ItemCd == string.Empty || x.ItemCd == null);
                    break;
                }
            case 4:
                {
                    //部位
                    int[] BuiCmtSbts = new int[] { 20, 21, 22 };
                    tgtDetails = tgtDetails.Where(x => x.BuiKbn > 0 && !BuiCmtSbts.Contains(x.CmtSbt));
                    break;
                }
        }

        if (printConf.TgtData > 0)
        {
            if (printConf.OtherItemOpt == 1)
            {
                //セット内の他の項目も含む
                joinQuery = (
                from setKbnMst in setKbnMsts
                join setMst in setMsts on
                    new { setKbnMst.HpId, setKbnMst.GenerationId, setKbnMst.SetKbn } equals
                    new { setMst.HpId, setMst.GenerationId, setMst.SetKbn }
                join setOdrInf in setOdrInfs on
                    new { setMst.HpId, setMst.SetCd } equals
                    new { setOdrInf.HpId, setOdrInf.SetCd }
                join tgtDetail in tgtDetails on
                    new { setOdrInf.HpId, setOdrInf.SetCd } equals
                    new { tgtDetail.HpId, tgtDetail.SetCd }
                join joinDetail in joinDetails on
                    new { setOdrInf.HpId, setOdrInf.SetCd, setOdrInf.RpNo, setOdrInf.RpEdaNo } equals
                    new { joinDetail.HpId, joinDetail.SetCd, joinDetail.RpNo, joinDetail.RpEdaNo }
                select
                    new
                    {
                        setKbnMst,
                        setMst,
                        setOdrInf,
                        joinDetail
                    }
                ).Distinct().ToList();
            }
            else
            {
                //セット内の他の項目は含まない
                joinQuery = (
                from setKbnMst in setKbnMsts
                join setMst in setMsts on
                    new { setKbnMst.HpId, setKbnMst.GenerationId, setKbnMst.SetKbn } equals
                    new { setMst.HpId, setMst.GenerationId, setMst.SetKbn }
                join setOdrInf in setOdrInfs on
                    new { setMst.HpId, setMst.SetCd } equals
                    new { setOdrInf.HpId, setOdrInf.SetCd }
                join tgtDetail in tgtDetails on
                    new { setOdrInf.HpId, setOdrInf.SetCd, setOdrInf.RpNo, setOdrInf.RpEdaNo } equals
                    new { tgtDetail.HpId, tgtDetail.SetCd, tgtDetail.RpNo, tgtDetail.RpEdaNo }
                select
                    new
                    {
                        setKbnMst,
                        setMst,
                        setOdrInf,
                        joinDetail = tgtDetail
                    }
                ).Distinct().ToList();
            }

        }
        #endregion

        var retData = joinQuery.AsEnumerable().Select(data => new CoOdrSetModel()
        {
            SetKbn = data.setKbnMst.SetKbn,
            SetKbnEdaNo = data.setMst.SetKbnEdaNo,
            SetKbnName = data.setKbnMst.SetKbnName ?? string.Empty,
            Level1 = data.setMst.Level1,
            Level2 = data.setMst.Level2,
            Level3 = data.setMst.Level3,
            SetCd = data.setMst.SetCd,
            SetName = data.setMst.SetName ?? string.Empty,
            WeightKbn = data.setMst.WeightKbn,
            RpNo = data.setOdrInf?.RpNo ?? -1,
            RpEdaNo = data.setOdrInf?.RpEdaNo ?? -1,
            OdrKouiKbn = data.setOdrInf?.OdrKouiKbn ?? -1,
            InoutKbn = data.setOdrInf?.InoutKbn ?? -1,
            SikyuKbn = data.setOdrInf?.SikyuKbn ?? -1,
            SyohoSbt = data.setOdrInf?.SyohoSbt ?? -1,
            SanteiKbn = data.setOdrInf?.SanteiKbn ?? -1,
            TosekiKbn = data.setOdrInf?.TosekiKbn ?? -1,
            SortNo = data.setOdrInf?.SortNo ?? -1,
            GroupKoui = CIUtil.GetGroupKoui(data.setOdrInf?.OdrKouiKbn ?? -1),
            RowNo = data.joinDetail?.RowNo ?? -1,
            ItemCd = data.joinDetail?.ItemCd ?? string.Empty,
            ItemName = data.joinDetail?.ItemName ?? string.Empty,
            Suryo = data.joinDetail?.Suryo ?? 0,
            UnitName = data.joinDetail?.UnitName ?? string.Empty,
            SyohoKbn = data.joinDetail?.SyohoKbn ?? -1,
            SyohoLimitKbn = data.joinDetail?.SyohoLimitKbn ?? -1,
            DrugKbn = data.joinDetail?.DrugKbn ?? -1,
            KensaItemCd = data.joinDetail?.KensaItemCd ?? string.Empty,
            CenterItemCd1 = data.joinDetail?.CenterItemCd1 ?? string.Empty,
            CenterItemCd2 = data.joinDetail?.CenterItemCd2 ?? string.Empty,
            MaxEndDate = data.joinDetail?.MaxEndDate ?? 99999999
        }
        ).ToList();

        return retData;
    }

    /// <summary>
    /// セットの世代取得
    /// </summary>
    /// <param name="stdDate">基準日</param>
    /// <returns></returns>
    private int GetGenerationId(int hpId, int stdDate)
    {
        var generation = NoTrackingDataContext.SetGenerationMsts
            .Where(x => x.HpId == hpId && x.StartDate <= stdDate && x.IsDeleted == 0)
            .OrderByDescending(x => x.StartDate)
            .FirstOrDefault();

        return generation?.GenerationId ?? 0;
    }

    /// <summary>
    /// セット区分ANDセット枝番のOR条件を作成する
    /// </summary>
    /// <param name="listSetKbns">セット区分-セット枝番リスト</param>
    /// <returns></returns>
    private Expression<Func<SetMst, bool>>? CreateSetKbnExpression(List<List<int>> listSetKbns)
    {
        var param = Expression.Parameter(typeof(SetMst));
        Expression? expression = null;

        if (listSetKbns != null && listSetKbns.Count > 0)
        {
            for (int i = 0; i < listSetKbns.Count; i++)
            {
                if (listSetKbns[i] == null) continue;
                var valSetKbn = Expression.Constant(i + 1);
                var memberSetKbn = Expression.Property(param, nameof(SetMst.SetKbn));
                var expressionSetKbn = Expression.Equal(valSetKbn, memberSetKbn);

                foreach (int edaNo in listSetKbns[i])
                {
                    var valSetKbnEdaNo = Expression.Constant(edaNo);
                    var memberSetKbnEdaNo = Expression.Property(param, nameof(SetMst.SetKbnEdaNo));
                    var expressionSetKbnEdaNo = Expression.And(expressionSetKbn, Expression.Equal(valSetKbnEdaNo, memberSetKbnEdaNo));

                    expression = expression == null ? expressionSetKbnEdaNo : Expression.Or(expression, expressionSetKbnEdaNo);
                }
            }
        }

        return expression != null
            ? Expression.Lambda<Func<SetMst, bool>>(body: expression, parameters: param)
            : null;
    }
}
