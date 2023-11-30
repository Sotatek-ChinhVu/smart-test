using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Statistics.Model;

namespace Reporting.Statistics.DB;

public class CoHpInfFinder : RepositoryBase, ICoHpInfFinder
{
    public CoHpInfFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        int wrkDate = sinDate;
        if (wrkDate.ToString().Length == 6)
        {
            wrkDate = wrkDate * 100 + 31;
        }

        HpInf hpInf = NoTrackingDataContext.HpInfs.Where(h =>
            h.HpId == hpId &&
            h.StartDate <= wrkDate
        ).OrderByDescending(h => h.StartDate).FirstOrDefault() ?? new();

        return new CoHpInfModel(hpInf);
    }
}
