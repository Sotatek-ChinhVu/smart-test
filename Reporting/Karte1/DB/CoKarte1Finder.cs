using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using PostgreDataContext;
using Reporting.Karte1.Model;

namespace Reporting.Karte1.DB;

public class CoKarte1Finder
{
    private readonly int HpId = Session.HospitalID;
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;

    public CoKarte1Finder(TenantNoTrackingDataContext tenantNoTrackingDataContext)
    {
        _tenantNoTrackingDataContext = tenantNoTrackingDataContext;
    }

    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <returns>患者情報</returns>
    public CoPtInfModel FindPtInf(long ptId, int sinDate)
    {

        var ptInfs = _tenantNoTrackingDataContext.PtInfs.Where(p =>
                 p.HpId == HpId &&
                 p.PtId == ptId &&
                 p.IsDelete == DeleteStatus.None
            );
        var ptMemos = _tenantNoTrackingDataContext.PtMemos.Where(p =>
                    p.HpId == HpId &&
                    p.PtId == ptId &&
                    p.IsDeleted == DeleteStatus.None);
        var ptCmtInfs = _tenantNoTrackingDataContext.PtCmtInfs.Where(p =>
                    p.HpId == HpId &&
                    p.PtId == ptId &&
                    p.IsDeleted == DeleteStatus.None);

        var join = (
                from ptInf in ptInfs
                join ptMemo in ptMemos on
                    new { ptInf.HpId, ptInf.PtId } equals
                    new { ptMemo.HpId, ptMemo.PtId } into ptMemoJoins
                from ptMemoJoin in ptMemoJoins.DefaultIfEmpty()
                join ptCmtInf in ptCmtInfs on
                    new { ptInf.HpId, ptInf.PtId } equals
                    new { ptCmtInf.HpId, ptCmtInf.PtId } into ptCmtInfJoins
                from ptCmtInfJoin in ptCmtInfJoins.DefaultIfEmpty()
                select new
                {
                    ptInf,
                    ptMemo = ptMemoJoin,
                    ptCmtInf = ptCmtInfJoin
                }

            );

        var entities = join.AsEnumerable().Select(
            data =>
                new CoPtInfModel(data.ptInf, data.ptMemo, data.ptCmtInf, sinDate)
            )
            .ToList();

        List<CoPtInfModel> results = new List<CoPtInfModel>();

        entities?.ForEach(entity =>
        {
            results.Add(
                new CoPtInfModel(
                    entity.PtInf,
                    entity.PtMemo,
                    entity.PtCmtInf,
                    entity.SinDate
                ));
        });

        return results.FirstOrDefault() ?? new();
    }

    /// <summary>
    /// 患者病名情報を取得する
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="tenkiByomei">true: 転帰病名も印字</param>
    /// <returns></returns>
    public List<CoPtByomeiModel> FindPtByomei(long ptId, int hokenPid, bool tenkiByomei)
    {
        List<int> tenkiKbns = new List<int> { TenkiKbnConst.Continued };

        if (tenkiByomei)
        {
            tenkiKbns.AddRange(new List<int> { TenkiKbnConst.Cured, TenkiKbnConst.Dead, TenkiKbnConst.Canceled, TenkiKbnConst.Other });
        }

        var ptByomeis = _tenantNoTrackingDataContext.PtByomeis.Where(p =>
            p.HpId == HpId &&
            (ptId > 0 ? p.PtId == ptId : true) &&
            (p.HokenPid == 0 || p.HokenPid == hokenPid) &&
            tenkiKbns.Contains(p.TenkiKbn) &&
            p.IsDeleted == DeleteStatus.None
        )
        .OrderBy(p => p.StartDate)
        .ThenBy(p => p.TenkiDate)
        .ThenBy(p => p.Byomei)
        .ThenBy(p => p.SyubyoKbn)
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
    /// 患者保険情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="hokenPid">保険組み合わせID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns></returns>
    public CoPtHokenInfModel FindPtHokenInf(long ptId, int hokenPid, int sinDate)
    {
        var hokenMsts = _tenantNoTrackingDataContext.HokenMsts;

        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = _tenantNoTrackingDataContext.HokenMsts.Where(
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
        var ptHokenPatterns = _tenantNoTrackingDataContext.PtHokenPatterns.Where(p =>
            p.HpId == HpId &&
            p.PtId == ptId &&
            p.HokenPid == hokenPid &&
            p.IsDeleted == DeleteStatus.None
        );

        var ptHokenInfs = _tenantNoTrackingDataContext.PtHokenInfs.Where(p =>
            p.HpId == HpId &&
            p.PtId == ptId &&
            p.IsDeleted == DeleteStatus.None
        );

        var ptKohis = _tenantNoTrackingDataContext.PtKohis.Where(p =>
            p.HpId == HpId &&
            p.PtId == ptId &&
            p.IsDeleted == DeleteStatus.None
        );

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
        var ptHokenPatternQuery = (
            from ptHokenPattern in ptHokenPatterns
            join ptHokenInf in ptHokenInfs on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId } into joinPtHokenInfs
            from joinPtHokenInf in joinPtHokenInfs.DefaultIfEmpty()
            join ptHokenMst in houbetuMsts on
                new { joinPtHokenInf.HpId, joinPtHokenInf.HokenNo, joinPtHokenInf.HokenEdaNo, PrefNo = 0 } equals
                new { ptHokenMst.hokenMst.HpId, ptHokenMst.hokenMst.HokenNo, ptHokenMst.hokenMst.HokenEdaNo, ptHokenMst.hokenMst.PrefNo } into joinPtHokenMsts
            from joinPtHokenMst in joinPtHokenMsts.DefaultIfEmpty()
            join ptKohi1 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi1Id } equals
                new { ptKohi1.HpId, ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into joinPtKohi1s
            from joinPtKohi1 in joinPtKohi1s.DefaultIfEmpty()
            join ptKohi1Mst in houbetuMsts on
                new { joinPtKohi1.HpId, joinPtKohi1.HokenNo, joinPtKohi1.HokenEdaNo, joinPtKohi1.PrefNo } equals
                new { ptKohi1Mst.hokenMst.HpId, ptKohi1Mst.hokenMst.HokenNo, ptKohi1Mst.hokenMst.HokenEdaNo, ptKohi1Mst.hokenMst.PrefNo } into joinPtKohi1Msts
            from joinPtKohi1Mst in joinPtKohi1Msts.DefaultIfEmpty()
            join ptKohi2 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi2Id } equals
                new { ptKohi2.HpId, ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into joinPtKohi2s
            from joinPtKohi2 in joinPtKohi2s.DefaultIfEmpty()
            join ptKohi2Mst in houbetuMsts on
                new { joinPtKohi2.HpId, joinPtKohi2.HokenNo, joinPtKohi2.HokenEdaNo, joinPtKohi2.PrefNo } equals
                new { ptKohi2Mst.hokenMst.HpId, ptKohi2Mst.hokenMst.HokenNo, ptKohi2Mst.hokenMst.HokenEdaNo, ptKohi2Mst.hokenMst.PrefNo } into joinPtKohi2Msts
            from joinPtKohi2Mst in joinPtKohi2Msts.DefaultIfEmpty()
            join ptKohi3 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi3Id } equals
                new { ptKohi3.HpId, ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into joinPtKohi3s
            from joinPtKohi3 in joinPtKohi3s.DefaultIfEmpty()
            join ptKohi3Mst in houbetuMsts on
                new { joinPtKohi3.HpId, joinPtKohi3.HokenNo, joinPtKohi3.HokenEdaNo, joinPtKohi3.PrefNo } equals
                new { ptKohi3Mst.hokenMst.HpId, ptKohi3Mst.hokenMst.HokenNo, ptKohi3Mst.hokenMst.HokenEdaNo, ptKohi3Mst.hokenMst.PrefNo } into joinPtKohi3Msts
            from joinPtKohi3Mst in joinPtKohi3Msts.DefaultIfEmpty()
            join ptKohi4 in ptKohis on
                new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.Kohi4Id } equals
                new { ptKohi4.HpId, ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into joinPtKohi4s
            from joinPtKohi4 in joinPtKohi4s.DefaultIfEmpty()
            join ptKohi4Mst in houbetuMsts on
                new { joinPtKohi4.HpId, joinPtKohi4.HokenNo, joinPtKohi4.HokenEdaNo, joinPtKohi4.PrefNo } equals
                new { ptKohi4Mst.hokenMst.HpId, ptKohi4Mst.hokenMst.HokenNo, ptKohi4Mst.hokenMst.HokenEdaNo, ptKohi4Mst.hokenMst.PrefNo } into joinPtKohi4Msts
            from joinPtKohi4Mst in joinPtKohi4Msts.DefaultIfEmpty()
            where
                joinPtHokenInf.HpId == HpId &&
                joinPtHokenInf.PtId == ptId &&
                joinPtHokenInf.IsDeleted == DeleteStatus.None
            select new
            {
                ptHokenInf = joinPtHokenInf,
                ptHokenMst = joinPtHokenMst.hokenMst,
                ptKohi1 = joinPtKohi1,
                ptKohi1Mst = joinPtKohi1Mst,
                ptKohi2 = joinPtKohi2,
                ptKohi2Mst = joinPtKohi2Mst,
                ptKohi3 = joinPtKohi3,
                ptKohi3Mst = joinPtKohi3Mst,
                ptKohi4 = joinPtKohi4,
                ptKohi4Mst = joinPtKohi4Mst
            }
        ).ToList();


        var entities = ptHokenPatternQuery.AsEnumerable().Select(
            data =>
                new CoPtHokenInfModel(
                    data.ptHokenInf,
                    data.ptHokenMst,
                    data.ptKohi1,
                    data.ptKohi1Mst?.hokenMst ?? new(),
                    data.ptKohi2,
                    data.ptKohi2Mst?.hokenMst ?? new(),
                    data.ptKohi3,
                    data.ptKohi3Mst?.hokenMst ?? new(),
                    data.ptKohi4,
                    data.ptKohi4Mst?.hokenMst ?? new()
                )
            )
            .ToList();

        if (!entities.Any())
        {
            return new CoPtHokenInfModel(new PtHokenInf(), new HokenMst(), new PtKohi(), new HokenMst(), new PtKohi(), new HokenMst(), new PtKohi(), new HokenMst(), new PtKohi(), new HokenMst());
        }

        return entities?.FirstOrDefault() ?? new();

    }
}
