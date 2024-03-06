using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Byomei.Model;

namespace Reporting.Byomei.DB;

public class CoPtByomeiFinder : RepositoryBase, ICoPtByomeiFinder
{
    public CoPtByomeiFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public PtInf FindPtInf(int hpId, long ptId)
    {
        return NoTrackingDataContext.PtInfs.Where(p =>
                 p.HpId == hpId &&
                 p.PtId == ptId &&
                 p.IsDelete == DeleteStatus.None
            ).FirstOrDefault();
    }

    public List<PtByomei> GetPtByomei(int hpId, long ptId, int fromDay, int toDay,
        bool tenkiIn, List<int> hokenIds)
    {
        // 共通病名は常に
        List<int> tgtHokenIds = new List<int>();
        tgtHokenIds.AddRange(hokenIds);
        tgtHokenIds.Add(0);

        var byoMeiList = NoTrackingDataContext.PtByomeis.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            (tenkiIn || p.TenkiKbn <= TenkiKbnConst.Continued) &&
            //(tenkiOut == false ? p.StartDate < toDay && (p.TenkiKbn <= 1 || p.TenkiDate > fromDay) : true) &&
            (p.StartDate <= toDay && (p.TenkiKbn <= TenkiKbnConst.Continued || p.TenkiDate >= fromDay)) &&
            tgtHokenIds.Contains(p.HokenPid) &&
            p.IsDeleted == DeleteTypes.None
            ).OrderBy(p => p.StartDate).ThenBy(p => p.TenkiDate).ThenBy(p => p.Byomei).ThenBy(p => p.SyubyoKbn).ToList();

        //CoPtByomeiModel coPtByomeiModel = new CoPtByomeiModel(fromDay, toDay, ptInf, ptHokenInf, byoMeiList);

        return byoMeiList;
    }

    public List<CoPtHokenInfModel> GetPtHokenInf(int hpId, long ptId, List<int> hokenIds, int sinDate)
    {
        var hokenMsts = NoTrackingDataContext.HokenMsts.Where(p => p.HpId == hpId && p.PrefNo == 0 && new int[] { 0, 1, 3, 4, 8 }.Contains(p.HokenSbtKbn));
        //診療日基準で保険番号マスタのキー情報を取得
        var hokenMstKeys = NoTrackingDataContext.HokenMsts.Where(
            h => h.HpId == hpId && h.StartDate <= sinDate
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
                hokenIds.Contains(p.HokenId));
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
        return entities;
    }
}
