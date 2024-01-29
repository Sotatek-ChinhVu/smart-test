using Infrastructure.Base;
using Infrastructure.Interfaces;
using CalculateService.Constants;
using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.HikariDisk.DB;

public class CoHikariDiskFinder : RepositoryBase, ICoHikariDiskFinder
{
    private readonly ICoHpInfFinder _hpInfFinder;
    public CoHikariDiskFinder(ITenantProvider tenantProvider, ICoHpInfFinder coHpInfFinder) : base(tenantProvider)
    {
        _hpInfFinder = coHpInfFinder;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
        _hpInfFinder.ReleaseResource();
    }

    public CoHpInfModel GetHpInf(int hpId, int seikyuYm)
    {
        return _hpInfFinder.GetHpInf(hpId, seikyuYm);
    }

    public List<CoReceInfModel> GetReceInf(int hpId, int seikyuYm, int hokenKbn)
    {
        var receInfs = NoTrackingDataContext.ReceInfs;
        var receStatuses = NoTrackingDataContext.ReceStatuses;

        var joinQuery = (
            from receInf in receInfs
            join wrkStatus in receStatuses on
                new { receInf.HpId, receInf.PtId, receInf.SeikyuYm, receInf.HokenId, receInf.SinYm } equals
                new { wrkStatus.HpId, wrkStatus.PtId, wrkStatus.SeikyuYm, wrkStatus.HokenId, wrkStatus.SinYm } into statusJoin
            from receStatus in statusJoin.DefaultIfEmpty()
            where
                receInf.HpId == hpId &&
                receInf.SeikyuYm == seikyuYm &&
                receInf.HokenKbn == hokenKbn &&
                (receInf.SeikyuKbn == SeikyuKbn.Normal || receInf.SeikyuKbn == SeikyuKbn.Delay) &&
                receInf.IsTester == 0
            select new
            {
                receInf,
                IsPaperRece = receStatus == null ? 0 : receStatus.IsPaperRece,
            }
        );

        joinQuery = joinQuery.Where(r => r.IsPaperRece == 0);

        var result = joinQuery.AsEnumerable().Select(
            data => new CoReceInfModel(data.receInf, null, null, null, null, null, HokensyaNoKbn.NoSum, 0)
        ).ToList();

        return result;
    }
}
