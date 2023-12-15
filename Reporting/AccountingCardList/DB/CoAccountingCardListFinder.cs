using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.AccountingCardList.Model;

namespace Reporting.AccountingCardList.DB;

public class CoAccountingCardListFinder : RepositoryBase, ICoAccountingCardListFinder
{
    public CoAccountingCardListFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    { }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    /// <summary>
    /// 患者情報取得
    /// </summary>
    /// <param name="ptId">患者ID</param>
    /// <param name="sinDate">診療日</param>
    /// <returns></returns>
    public CoPtInfModel FindPtInf(int hpId, long ptId, int sinDate)
    {
        var ptInf = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.IsDelete == DeleteStatus.None
        ).FirstOrDefault();

        if (ptInf == null)
        {
            return new();
        }
        return new CoPtInfModel(ptInf, sinDate);
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
    public List<CoKaikeiInfModel> FindKaikeiInf(int hpId, long ptId, int sinYm, int hokenId)
    {
        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p =>
            p.HpId == hpId &&
            p.SinDate >= sinYm * 100 + 1 &&
            p.SinDate <= sinYm * 100 + 31 &&
            p.HokenId == hokenId &&
            p.PtId == ptId
        ).ToList();

        var entities = kaikeiInfs.AsEnumerable().Select(
            data =>
                new CoKaikeiInfModel(
                    data,
                    FindKaikeiDetail(hpId, ptId, data.SinDate, data.RaiinNo)
                )
            )
            .ToList();
        List<CoKaikeiInfModel> results = new List<CoKaikeiInfModel>();

        entities?.ForEach(entity =>
        {

            results.Add(
                new CoKaikeiInfModel(
                    entity.KaikeiInf,
                    entity.KaikeiDtls
                ));

        }
        );

        return results;
    }

    public List<KaikeiDetail> FindKaikeiDetail(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return NoTrackingDataContext.KaikeiDetails.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.SinDate == sinDate &&
            p.RaiinNo == raiinNo
        ).ToList();
    }

    /// <summary>
    /// 患者病名情報を取得する
    /// </summary>        
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public List<CoPtByomeiModel> FindPtByomei(int hpId, long ptId, int startDate, int endDate, int hokenId)
    {
        var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p =>
            p.HpId == hpId &&
            p.PtId == ptId &&
            p.StartDate <= endDate &&
            ((p.TenkiKbn == TenkiKbnConst.Continued && p.TenkiDate == 0) || p.TenkiDate >= startDate) &&
            p.IsDeleted == DeleteStatus.None &&
            (p.HokenPid == hokenId || p.HokenPid == 0)
            )
            .OrderBy(p => p.StartDate)
            .ThenBy(p => p.SortNo)
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
}
