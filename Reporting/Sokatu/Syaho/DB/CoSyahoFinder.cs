using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Calculate.Constants;
using Reporting.CommonMasters.Constants;
using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.Common.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.Syaho.DB;

public class CoSyahoFinder : RepositoryBase, ICoSyahoFinder
{
    private readonly ICoHpInfFinder _hpInfFinder;

    public CoSyahoFinder(ITenantProvider tenantProvider, ICoHpInfFinder hpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = hpInfFinder;
    }

    public void ReleaseResource()
    {
        _hpInfFinder.ReleaseResource();
        DisposeDataContext();
    }

    public CoHpInfModel GetHpInf(int hpId, int seikyuYm)
    {
        return _hpInfFinder.GetHpInf(hpId, seikyuYm);
    }

    public List<CoReceInfModel> GetReceInf(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        var receInfs = NoTrackingDataContext.ReceInfs;
        var receStatuses = NoTrackingDataContext.ReceStatuses;
        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(
            p => p.IsDeleted == DeleteStatus.None
        );

        var joinQuery = (
            from receInf in receInfs
            join wrkStatus in receStatuses on
                new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                new { wrkStatus.HpId, wrkStatus.PtId, wrkStatus.SeikyuYm, wrkStatus.HokenId, wrkStatus.SinYm } into statusJoin
            from receStatus in statusJoin.DefaultIfEmpty()
            join ptHokenInf in ptHokenInfs on
                new { receInf.HpId, receInf.PtId, receInf.HokenId } equals
                new { ptHokenInf.HpId, ptHokenInf.PtId, ptHokenInf.HokenId }
            where
                receInf.HpId == hpId &&
                receInf.SeikyuYm == seikyuYm &&
                receInf.HokenKbn == HokenKbn.Syaho &&
                receInf.IsTester == 0
            select new
            {
                receInf,
                ptHokenInf,
                IsPaperRece = receStatus == null ? 0 : receStatus.IsPaperRece,
            }
        );

        //請求区分
        List<int> Codes = new();
        if (seikyuType.IsNormal) Codes.Add(SeikyuKbn.Normal);
        if (seikyuType.IsDelay) Codes.Add(SeikyuKbn.Delay);
        if (seikyuType.IsHenrei) Codes.Add(SeikyuKbn.Henrei);
        if (seikyuType.IsOnline) Codes.Add(SeikyuKbn.Online);

        if (seikyuType.IsPaper)
        {
            joinQuery = joinQuery.Where(r => r.IsPaperRece == 1 || Codes.Contains(r.receInf.SeikyuKbn));
        }
        else
        {
            joinQuery = joinQuery.Where(r => r.IsPaperRece == 0 && Codes.Contains(r.receInf.SeikyuKbn));
        }

        var result = joinQuery.AsEnumerable().Select(
            data => new CoReceInfModel(
                data.receInf, data.ptHokenInf, null, null, null, null, HokensyaNoKbn.NoSum, 0
            )
        ).ToList();

        return result;
    }
}
