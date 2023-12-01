using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.NameLabel.Models;

namespace Reporting.NameLabel.DB;

public class CoNameLabelFinder : RepositoryBase, ICoNameLabelFinder
{
    public CoNameLabelFinder(ITenantProvider tenantProvider) : base(tenantProvider) { }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public CoPtInfModel FindPtInf(long ptId)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == 1 &&
            p.PtId == ptId &&
            p.IsDelete == 0
        );

        CoPtInfModel result = new();

        if (ptInfs != null && ptInfs.Any())
        {
            result = new CoPtInfModel(ptInfs.First());
        }

        return result;
    }
}
