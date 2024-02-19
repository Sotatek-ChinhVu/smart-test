using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3070.Models;

namespace Reporting.Statistics.Sta3070.DB;

public class CoSta3070Finder : RepositoryBase, ICoSta3070Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3070Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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
    /// 
    /// </summary>
    /// <param name="printConf"></param>
    /// <returns></returns>
    public List<CoRaiinInfModel> GetRaiinInfs(int hpId, CoSta3070PrintConf printConf)
    {
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.IsDeleted == DeleteStatus.None && r.Status >= RaiinState.Calculate);
        var sinkanInfs = raiinInfs
            .GroupBy(m => new { m.HpId, m.PtId })
            .Select(m => new { m.Key.HpId, m.Key.PtId, MinSinDate = m.Min(d => d.SinDate) }).ToList();

        #region 検索条件
        raiinInfs = printConf.StartSinYmd > 0 ? raiinInfs.Where(r => printConf.StartSinYmd <= r.SinDate) : raiinInfs;
        raiinInfs = printConf.EndSinYmd > 0 ? raiinInfs.Where(r => r.SinDate <= printConf.EndSinYmd) : raiinInfs;
        raiinInfs = printConf.KaIds?.Count > 0 ? raiinInfs.Where(r => printConf.KaIds.Contains(r.KaId)) : raiinInfs;
        raiinInfs = printConf.TantoIds?.Count > 0 ? raiinInfs.Where(r => printConf.TantoIds.Contains(r.TantoId)) : raiinInfs;
        #endregion

        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteStatus.None);
        ptInfs = printConf.IsTester ? ptInfs : ptInfs.Where(p => p.HpId == hpId && p.IsTester != 1);

        var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId);
        var userMsts = NoTrackingDataContext.UserMsts.Where(x => x.HpId == hpId);
        IQueryable<KaikeiInf> kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(x => x.HpId == hpId);

        #region 条件指定(保険種別)
        if (printConf.HokenSbts?.Count >= 1)
        {
            //保険種別
            List<int> hokenKbns = new List<int>();
            if (printConf.HokenSbts.Contains(1)) hokenKbns.Add(1);                                              //社保
            if (printConf.HokenSbts.Contains(2) || printConf.HokenSbts.Contains(3)) hokenKbns.Add(2);           //国保・後期
            if (printConf.HokenSbts.Contains(10)) { hokenKbns.Add(11); hokenKbns.Add(12); hokenKbns.Add(13); }  //労災
            if (printConf.HokenSbts.Contains(11)) hokenKbns.Add(14);                                            //自賠
            if (printConf.HokenSbts.Contains(0) || printConf.HokenSbts.Contains(12)) hokenKbns.Add(0);          //自費・自レ

            kaikeiInfs = kaikeiInfs.Where(r => hokenKbns.Contains(r.HokenKbn));

            if (printConf.HokenSbts.Contains(2) && !printConf.HokenSbts.Contains(3))
            {
                //後期を除く
                kaikeiInfs = kaikeiInfs.Where(r => !(r.HokenKbn == 2 && r.HokenSbtCd / 100 == 3));
            }
            else if (!printConf.HokenSbts.Contains(2) && printConf.HokenSbts.Contains(3))
            {
                //国保一般・退職を除く
                kaikeiInfs = kaikeiInfs.Where(r => !(r.HokenKbn == 2 && r.HokenSbtCd / 100 != 3));
            }

            if (printConf.HokenSbts.Contains(0) && !printConf.HokenSbts.Contains(12))
            {
                //自費レセを除く
                kaikeiInfs = kaikeiInfs.Where(r => !(r.HokenKbn == 0 && r.Houbetu == "109"));
            }
            else if (!printConf.HokenSbts.Contains(0) && printConf.HokenSbts.Contains(12))
            {
                //自費を除く
                kaikeiInfs = kaikeiInfs.Where(r => !(r.HokenKbn == 0 && r.Houbetu == "108"));
            }
        }
        #endregion

        var joinQuery =
             from raiinInf in raiinInfs
             join ptInf in ptInfs on
                new { raiinInf.HpId, raiinInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
                //join sinkanInf in sinkanInfs on
                //    new { ptInf.HpId, ptInf.PtId } equals
                //    new { sinkanInf.HpId, sinkanInf.PtId }
             join kaikeiInf in kaikeiInfs on
                 new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                 new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.RaiinNo }
             join kaMst in kaMsts on
                 new { raiinInf.HpId, raiinInf.KaId } equals
                 new { kaMst.HpId, kaMst.KaId } into kaMstJoin
             from kaMstj in kaMstJoin.DefaultIfEmpty()
             join userMst in userMsts on
                 new { raiinInf.HpId, raiinInf.TantoId } equals
                 new { userMst.HpId, TantoId = userMst.UserId } into userMstJoin
             from tantoMst in userMstJoin.DefaultIfEmpty()
             select new
             {
                 raiinInf.HpId,
                 raiinInf.SinDate,
                 raiinInf.RaiinNo,
                 raiinInf.PtId,
                 ptInf.Sex,
                 ptInf.Birthday,
                 raiinInf.SyosaisinKbn,
                 raiinInf.JikanKbn,
                 //sinkanInf.MinSinDate,
                 raiinInf.KaId,
                 kaMstj.KaSname,
                 raiinInf.TantoId,
                 tantoMst.Sname,
                 kaikeiInf.HokenKbn,
                 kaikeiInf.HokenSbtCd,
                 kaikeiInf.HonkeKbn,
                 kaikeiInf.Houbetu
             };

        var retData = joinQuery.AsEnumerable().Select(
            data =>
                new CoRaiinInfModel()
                {
                    ReportKbn = printConf.ReportKbn,
                    HpId = data.HpId,
                    SinDate = data.SinDate,
                    RaiinNo = data.RaiinNo,
                    PtId = data.PtId,
                    Sex = data.Sex,
                    BirthDay = data.Birthday,
                    SyosaisinKbn = data.SyosaisinKbn,
                    JikanKbn = data.JikanKbn,
                    MinSinDate = 0, //MinSinDate = data.MinSinDate,
                    KaId = data.KaId,
                    KaSname = data.KaSname ?? string.Empty,
                    TantoId = data.TantoId,
                    TantoSname = data.Sname ?? string.Empty,
                    HokenKbn = data.HokenKbn,
                    HokenSbtCd = data.HokenSbtCd,
                    HonkeKbn = data.HonkeKbn,
                    Houbetu = data.Houbetu
                }
            ).ToList();

        retData.ForEach(
            r => r.MinSinDate = sinkanInfs.Where(s => s.HpId == r.HpId && s.PtId == r.PtId)
                                          .Select(s => s.MinSinDate).FirstOrDefault());

        return retData;
    }
}