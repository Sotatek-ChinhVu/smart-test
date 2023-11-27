using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Karte3.Model;

namespace Reporting.Karte3.DB;

public class CoKarte3Finder : RepositoryBase, ICoKarte3Finder
{
    public CoKarte3Finder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    /// <summary>
    /// 患者情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <returns>患者情報</returns>
    public CoPtInfModel FindPtInf(int hpId, long ptId)
    {
        return new CoPtInfModel(NoTrackingDataContext.PtInfs.FirstOrDefault(p =>
                 p.HpId == hpId &&
                 p.PtId == ptId));
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
                ptHokenInf.HokenId == hokenId &&
                ptHokenInf.IsDeleted == DeleteStatus.None
            select new
            {
                ptHokenInf,
                houbetuMst.hokenMst
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

        return entities.FirstOrDefault();

    }

    public List<CoSinKouiModel> FindSinKoui(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi)
    {
        List<int> santeiKbnls = new();
        List<string> cdKbnls = new();
        int maxSinId = 0;
        List<int> jihiSinId = new() { 0 };

        if (includeHoken)
        {
            santeiKbnls.Add(0);
            cdKbnls.AddRange(new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "R" });
            maxSinId = 95;
        }

        if (includeJihi)
        {
            santeiKbnls.Add(2);
            cdKbnls.AddRange(new List<string> { "JS", "SZ", "SK" });
            jihiSinId.Add(96);
        }

        var sinRps = NoTrackingDataContext.SinRpInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startSinYm &&
                p.SinYm <= endSinYm &&
                santeiKbnls.Contains(p.SanteiKbn) &&
                (p.SinId <= maxSinId || jihiSinId.Contains(p.SinId))
            );

        var sinKouis = NoTrackingDataContext.SinKouis.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startSinYm &&
        p.SinYm <= endSinYm &&
                cdKbnls.Contains(p.CdKbn)
            );

        var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinYm >= startSinYm &&
                p.SinYm <= endSinYm
            );

        var sinKouiCountGroups = (
                from sinKouiCount in sinKouiCounts
                group sinKouiCount by
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.SinDay, sinKouiCount.RpNo, sinKouiCount.SeqNo } into A
                select new
                {
                    A.Key.HpId,
                    A.Key.PtId,
                    A.Key.SinYm,
                    A.Key.SinDay,
                    A.Key.RpNo,
                    A.Key.SeqNo,
                    Count = A.Sum(p => p.Count)
                }
            );

        var join = (
                from sinRp in sinRps
                join sinKoui in sinKouis on
                    new { sinRp.HpId, sinRp.PtId, sinRp.SinYm, sinRp.RpNo } equals
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo }
                join sinKouiCount in sinKouiCountGroups on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                orderby
                    sinKoui.HokenId, sinKouiCount.SinYm, sinKouiCount.SinDay, sinRp.SanteiKbn, sinKoui.CdKbn, sinRp.CdNo, sinKoui.RpNo, sinKoui.SeqNo
                select new
                {
                    sinRp.HpId,
                    sinRp.PtId,
                    SinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay,
                    sinKoui.RpNo,
                    sinKoui.SeqNo,
                    sinRp.SinId,
                    sinKoui.SyukeiSaki,
                    sinKoui.Ten,
                    sinKouiCount.Count,
                    sinKoui.EntenKbn,
                    sinRp.SanteiKbn,
                    sinKoui.CdKbn,
                    sinRp.CdNo,
                    sinKoui.HokenId
                }
            );

        var entities = join.AsEnumerable().Select(
            data =>
                new CoSinKouiModel(
                    data.PtId,
                    data.SinDate,
                    data.RpNo,
                    data.SeqNo,
                    data.SinId,
                    data.SyukeiSaki,
                    data.Ten,
                    data.Count,
                    data.EntenKbn,
                    data.SanteiKbn,
                    data.CdKbn,
                    data.CdNo,
                    data.HokenId
                )
            )
            .ToList();
        List<CoSinKouiModel> results = new List<CoSinKouiModel>();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoSinKouiModel(
                    entity.PtId,
                    entity.SinDate,
                    entity.RpNo,
                    entity.SeqNo,
                    entity.SinId,
                    entity.SyukeiSaki,
                    entity.Ten,
                    entity.Count,
                    entity.EntenKbn,
                    entity.SanteiKbn,
                    entity.CdKbn,
                    entity.CdNo,
                    entity.HokenId
                ));

        }
        );

        return results;
    }

    /// <summary>
    /// 会計情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">検索開始日</param>
    /// <param name="endDate">検索終了日</param>
    /// <returns></returns>
    ///         
    public List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int startYm, int endYm, int hokenId)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startYm * 100 + 1 &&
            p.SinDate <= endYm * 100 + 31 &&
            p.HokenId == hokenId &&
            p.PtId == ptId
        );
        var join = (
            from kaikeiInf in kaikeiInfs
            group kaikeiInf by
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.HokenId } into A
            select new
            {
                A.Key.HpId,
                A.Key.PtId,
                A.Key.SinDate,
                A.Key.HokenId,
                Tensu = A.Sum(p => p.Tensu),
                PtFutan = A.Sum(p => p.PtFutan),
                JihiFutan = A.Sum(p => p.JihiFutan),
                Outtax = A.Sum(p => p.JihiOuttax),
                TotalPtFutan = A.Sum(p => p.TotalPtFutan)
            }
            ).ToList();

        var entities = join.AsEnumerable().Select(
            data =>
                new CoKaikeiInfModel(
                    data.HpId,
                    data.PtId,
                    data.SinDate,
                    data.HokenId,
                    data.Tensu,
                    data.PtFutan,
                    data.JihiFutan,
                    data.Outtax,
                    data.TotalPtFutan
                )
            )
            .ToList();
        List<CoKaikeiInfModel> results = new();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoKaikeiInfModel(
                    entity.HpId,
                    entity.PtId,
                    entity.SinDate,
                    entity.HokenId,
                    entity.Tensu,
                    entity.PtFutan,
                    entity.JihiFutan,
                    entity.Outtax,
                    entity.TotalPtFutan
                ));

        }
        );

        return results;
    }
    /// <summary>
    /// 会計情報を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startDate">検索開始日</param>
    /// <param name="endDate">検索終了日</param>
    /// <returns></returns>
    ///         
    public List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int startYm, int endYm, List<int> hokenIds)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startYm * 100 + 1 &&
            p.SinDate <= endYm * 100 + 31 &&
            hokenIds.Contains(p.HokenId) &&
            p.PtId == ptId
        );
        var join = (
            from kaikeiInf in kaikeiInfs
            group kaikeiInf by
                new { kaikeiInf.HpId, kaikeiInf.PtId, kaikeiInf.SinDate } into A
            select new
            {
                A.Key.HpId,
                A.Key.PtId,
                A.Key.SinDate,
                HokenId = A.Max(p => p.HokenId),
                Tensu = A.Sum(p => p.Tensu),
                PtFutan = A.Sum(p => p.PtFutan),
                JihiFutan = A.Sum(p => p.JihiFutan),
                Outtax = A.Sum(p => p.JihiOuttax),
                TotalPtFutan = A.Sum(p => p.TotalPtFutan)
            }
            ).ToList();

        var entities = join.AsEnumerable().Select(
            data =>
                new CoKaikeiInfModel(
                    data.HpId,
                    data.PtId,
                    data.SinDate,
                    data.HokenId,
                    data.Tensu,
                    data.PtFutan,
                    data.JihiFutan,
                    data.Outtax,
                    data.TotalPtFutan
                )
            )
            .ToList();
        List<CoKaikeiInfModel> results = new();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoKaikeiInfModel(
                    entity.HpId,
                    entity.PtId,
                    entity.SinDate,
                    entity.HokenId,
                    entity.Tensu,
                    entity.PtFutan,
                    entity.JihiFutan,
                    entity.Outtax,
                    entity.TotalPtFutan
                ));

        }
        );

        return results;
    }
    /// <summary>
    /// 指定期間に使用されている公費の種類を取得する
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="startYm">検索開始年月</param>
    /// <param name="endYm">検索終了年月</param>
    /// <param name="hokenId">保険ID</param>
    /// <returns></returns>
    public HashSet<string> FindKohiInf(int hpId, long ptId, int startYm, int endYm, int hokenId)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= startYm * 100 + 1 &&
            p.SinDate <= endYm * 100 + 31 &&
        p.HokenId == hokenId &&
            p.PtId == ptId
        );
        var ptKohis = NoTrackingDataContext.PtKohis.Where(p =>
        p.HpId == hpId &&
            p.PtId == ptId
        );
        var hokenMsts = NoTrackingDataContext.HokenMsts.Where(p =>
            p.HpId == hpId &&
        new int[] { 6, 7 }.Contains(p.HokenSbtKbn)
            );
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.AsQueryable(
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


        var k1 = (
            from kaikeiInf in kaikeiInfs
            join ptKohi in ptKohis on
                new { kaikeiInf.HpId, kaikeiInf.PtId, HokenId = kaikeiInf.Kohi1Id } equals
                new { ptKohi.HpId, ptKohi.PtId, ptKohi.HokenId }
            join houbetuMst in houbetuMsts on
                new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo, houbetuMst.hokenMst.PrefNo }
            group houbetuMst by
                new { houbetuMst.hokenMst.Houbetu } into A
            select new
            {
                KohiHoubetu = A.Key.Houbetu
            }
            ).ToList();
        var k2 = (
            from kaikeiInf in kaikeiInfs
            join ptKohi in ptKohis on
                new { kaikeiInf.HpId, kaikeiInf.PtId, HokenId = kaikeiInf.Kohi2Id } equals
                new { ptKohi.HpId, ptKohi.PtId, ptKohi.HokenId }
            join houbetuMst in houbetuMsts on
                new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo, houbetuMst.hokenMst.PrefNo }
            group houbetuMst by
                new { houbetuMst.hokenMst.Houbetu } into A
            select new
            {
                KohiHoubetu = A.Key.Houbetu
            }
            ).ToList();
        var k3 = (
            from kaikeiInf in kaikeiInfs
            join ptKohi in ptKohis on
                new { kaikeiInf.HpId, kaikeiInf.PtId, HokenId = kaikeiInf.Kohi3Id } equals
                new { ptKohi.HpId, ptKohi.PtId, ptKohi.HokenId }
            join houbetuMst in houbetuMsts on
                new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo, houbetuMst.hokenMst.PrefNo }
            group houbetuMst by
                new { houbetuMst.hokenMst.Houbetu } into A
            select new
            {
                KohiHoubetu = A.Key.Houbetu
            }
            ).ToList();
        var k4 = (
            from kaikeiInf in kaikeiInfs
            join ptKohi in ptKohis on
                new { kaikeiInf.HpId, kaikeiInf.PtId, HokenId = kaikeiInf.Kohi4Id } equals
                new { ptKohi.HpId, ptKohi.PtId, ptKohi.HokenId }
            join houbetuMst in houbetuMsts on
                new { ptKohi.HpId, ptKohi.HokenNo, ptKohi.HokenEdaNo, ptKohi.PrefNo } equals
                new { houbetuMst.hokenMst.HpId, houbetuMst.hokenMst.HokenNo, houbetuMst.hokenMst.HokenEdaNo, houbetuMst.hokenMst.PrefNo }
            group houbetuMst by
                new { houbetuMst.hokenMst.Houbetu } into A
            select new
            {
                KohiHoubetu = A.Key.Houbetu
            }
            ).ToList();

        HashSet<string> results = new HashSet<string>();

        k1?.ForEach(entity =>
        {
            results.Add(
                    (entity.KohiHoubetu)
                    );
        }
        );

        k2?.ForEach(entity =>
        {
            results.Add(
                    (entity.KohiHoubetu)
                    );
        }
        );

        k3?.ForEach(entity =>
        {
            results.Add(
                    (entity.KohiHoubetu)
                    );
        }
        );

        k4?.ForEach(entity =>
        {
            results.Add(
                   (entity.KohiHoubetu)
                   );
        }
        );

        return results;
    }
}
