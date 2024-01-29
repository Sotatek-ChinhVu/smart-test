using Domain.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using CalculateService.Constants;
using Reporting.Sokatu.AfterCareSeikyu.Model;
using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.Common.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.AfterCareSeikyu.DB;

public class CoAfterCareSeikyuFinder : RepositoryBase, ICoAfterCareSeikyuFinder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    public CoAfterCareSeikyuFinder(ITenantProvider tenantProvider, ICoHpInfFinder coHpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = coHpInfFinder;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public CoHpInfModel GetHpInf(int hpId, int seikyuYm)
    {
        return _hpInfFinder.GetHpInf(hpId, seikyuYm);
    }

    public CoSeikyuInfModel GetSeikyuInf(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        CoSeikyuInfModel seikyuInf = new CoSeikyuInfModel();

        var receInfs = NoTrackingDataContext.ReceInfs;
        var receStatuses = NoTrackingDataContext.ReceStatuses;

        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);

        var receQuerys = (
            from receInf in receInfs
            join ptInf in ptInfs on
                new { receInf.HpId, receInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
            join wrkStatus in receStatuses on
                new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                new { wrkStatus.HpId, wrkStatus.PtId, wrkStatus.SeikyuYm, wrkStatus.HokenId, wrkStatus.SinYm } into statusJoin
            from receStatus in statusJoin.DefaultIfEmpty()
            where
                receInf.HpId == hpId &&
                receInf.SeikyuYm == seikyuYm &&
                receInf.HokenKbn == HokenKbn.AfterCare &&
                receInf.IsTester == 0
            orderby
                ptInf.KanaName, ptInf.PtNum
            select new
            {
                receInf,
                ptInf.Name,
                IsPaperRece = receStatus == null ? 0 : receStatus.IsPaperRece,
            }
        );

        //請求区分
        List<int> Codes = new();
        if (seikyuType.IsNormal) Codes.Add(Helper.Constants.SeikyuKbn.Normal);
        if (seikyuType.IsDelay) Codes.Add(Helper.Constants.SeikyuKbn.Delay);
        if (seikyuType.IsHenrei) Codes.Add(Helper.Constants.SeikyuKbn.Henrei);
        if (seikyuType.IsOnline) Codes.Add(Helper.Constants.SeikyuKbn.Online);

        if (seikyuType.IsPaper)
        {
            receQuerys = receQuerys.Where(r => r.IsPaperRece == 1 || Codes.Contains(r.receInf.SeikyuKbn));
        }
        else
        {
            receQuerys = receQuerys.Where(r => r.IsPaperRece == 0 && Codes.Contains(r.receInf.SeikyuKbn));
        }

        var receDatas = receQuerys.ToList();
        if (receDatas.Count >= 1)
        {
            //代表者名
            seikyuInf.DaihyoName = receDatas.First().Name;
            //請求人数（代表者以外の人数）
            seikyuInf.SeikyuNinzu = receDatas.Count - 1;
            //請求金額
            seikyuInf.SeikyuGaku = receDatas.Sum(r => r.receInf.RousaiIFutan + r.receInf.RousaiRoFutan);
        }

        //内訳書添付枚数（1日1枚×患者数）
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs;

        seikyuInf.MeisaiCount = (
            from r in receQuerys
            join k in kaikeiInfs on
                new { r.receInf.HpId, r.receInf.PtId, r.receInf.SinYm, r.receInf.HokenId } equals
                new { k.HpId, k.PtId, SinYm = k.SinDate / 100, k.HokenId }
            group k by
                new { k.PtId, k.SinDate, k.HokenId } into kg
            select
                kg.Key.PtId
        ).Count();

        return seikyuInf;
    }
}
