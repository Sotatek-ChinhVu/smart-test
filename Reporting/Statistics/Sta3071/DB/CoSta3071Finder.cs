using Domain.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.DB;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3071.Models;

namespace Reporting.Statistics.Sta3071.DB;

public class CoSta3071Finder : RepositoryBase, ICoSta3071Finder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSta3071Finder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
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

    public List<CoRaiinInfModel> GetRaiinInfs(int hpId, CoSta3071PrintConf printConf)
    {
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.IsDeleted == DeleteStatus.None && r.Status >= RaiinState.Calculate);
        var sinkanInfs = raiinInfs
            .GroupBy(m => new { m.HpId, m.PtId })
            .Select(m => new { m.Key.HpId, m.Key.PtId, MinSinDate = m.Min(d => d.SinDate) });

        #region 検索条件
        raiinInfs = printConf.StartSinYmd > 0 ? raiinInfs.Where(r => printConf.StartSinYmd <= r.SinDate) : raiinInfs;
        raiinInfs = printConf.EndSinYmd > 0 ? raiinInfs.Where(r => r.SinDate <= printConf.EndSinYmd) : raiinInfs;
        raiinInfs = printConf.KaIds?.Count > 0 ? raiinInfs.Where(r => printConf.KaIds.Contains(r.KaId)) : raiinInfs;
        raiinInfs = printConf.TantoIds?.Count > 0 ? raiinInfs.Where(r => printConf.TantoIds.Contains(r.TantoId)) : raiinInfs;
        #endregion

        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.IsDelete == DeleteStatus.None);
        ptInfs = printConf.IsTester ? ptInfs : ptInfs.Where(p => p.IsTester != 1);

        var kaMsts = NoTrackingDataContext.KaMsts.Where(k => k.IsDeleted == DeleteStatus.None);
        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.IsDeleted == DeleteStatus.None);
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs;

        var joinQueries =
             from raiinInf in raiinInfs
             join ptInf in ptInfs on
                new { raiinInf.HpId, raiinInf.PtId } equals
                new { ptInf.HpId, ptInf.PtId }
             join sinkanInf in sinkanInfs on
                 new { ptInf.HpId, ptInf.PtId } equals
                 new { sinkanInf.HpId, sinkanInf.PtId }
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
                 raiinInf,
                 ptInf,
                 sinkanInf,
                 kaikeiInf,
                 kaMstj,
                 tantoMst
             };

        var joinDatas = joinQueries.AsEnumerable().Select(
            data =>
                new CoRaiinInfModel()
                {
                    HpId = hpId,
                    ReportKbnV = printConf.ReportKbnV,
                    ReportKbnH = printConf.ReportKbnH,
                    SinDate = data.raiinInf.SinDate,
                    RaiinNo = data.raiinInf.RaiinNo,
                    PtId = data.raiinInf.PtId,
                    Sex = data.ptInf.Sex,
                    BirthDay = data.ptInf.Birthday,
                    SyosaisinKbn = data.raiinInf.SyosaisinKbn,
                    JikanKbn = data.raiinInf.JikanKbn,
                    MinSinDate = data.sinkanInf.MinSinDate,
                    KaId = data.raiinInf.KaId,
                    KaSname = data.kaMstj?.KaSname,
                    TantoId = data.raiinInf.TantoId,
                    TantoSname = data.tantoMst?.Sname,
                    HokenKbn = data.kaikeiInf.HokenKbn,
                    HokenSbtCd = data.kaikeiInf.HokenSbtCd,
                    Houbetu = data.kaikeiInf.Houbetu
                }
        ).ToList();

        #region 患者グループ情報を追加
        var ptGrpInfs = NoTrackingDataContext.PtGrpInfs.Where(p => p.IsDeleted == DeleteStatus.None);
        var ptGrpNames = NoTrackingDataContext.PtGrpNameMsts.Where(p => p.IsDeleted == DeleteStatus.None);
        var ptGrpItems = NoTrackingDataContext.PtGrpItems.Where(p => p.IsDeleted == DeleteStatus.None);

        var ptGrpJoins =
             from ptGrpInf in ptGrpInfs
             join ptGrpName in ptGrpNames on
                new { ptGrpInf.HpId, ptGrpInf.GroupId } equals
                new { ptGrpName.HpId, GroupId = ptGrpName.GrpId }
             join ptGrpItem in ptGrpItems on
                 new { ptGrpInf.HpId, ptGrpInf.GroupId, ptGrpInf.GroupCode } equals
                 new { ptGrpItem.HpId, GroupId = ptGrpItem.GrpId, GroupCode = ptGrpItem.GrpCode }
             select new
             {
                 ptGrpInf.HpId,
                 ptGrpInf.PtId,
                 ptGrpInf.GroupId,
                 ptGrpInf.GroupCode,
                 ptGrpName.GrpName,
                 ptGrpItem.GrpCodeName
             };

        foreach (var joinData in joinDatas)
        {
            joinData.PtGrps = new List<CoRaiinInfModel.PtGrp>();
            var grpInfs = ptGrpJoins.Where(p => p.HpId == joinData.HpId && p.PtId == joinData.PtId);
            foreach (var grpInf in grpInfs)
            {
                joinData.PtGrps.Add(new CoRaiinInfModel.PtGrp() { GrpId = grpInf.GroupId, GrpName = grpInf.GrpName, GrpCode = grpInf.GroupCode, GrpCodeName = grpInf.GrpCodeName });
            }
        }
        #endregion

        //集計項目に該当しない患者を除く
        joinDatas = joinDatas.Where(j => j.ReportKbnVValue != null && j.ReportKbnHValue != null).ToList();

        return joinDatas ?? new();
    }

    public string GetPtGrpName(int hpId, int grpId)
    {
        var ptGrpNames = NoTrackingDataContext.PtGrpNameMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteStatus.None);
        return ptGrpNames.FirstOrDefault(p => p.GrpId == grpId)?.GrpName ?? string.Empty;
    }
}