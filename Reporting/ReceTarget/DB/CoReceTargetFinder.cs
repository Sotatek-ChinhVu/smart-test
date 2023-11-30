using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.ReceTarget.Model;

namespace Reporting.ReceTarget.DB;

public class CoReceTargetFinder : RepositoryBase, ICoReceTargetFinder
{
    public CoReceTargetFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public CoReceTargetModel FindReceInf(int hpId, int seikyuYm)
    {
        var receInfs = NoTrackingDataContext.ReceInfs.Where(item =>
            item.HpId == hpId &&
            item.SeikyuYm == seikyuYm
        );
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p =>
            p.HpId == hpId &&
            p.IsDelete == DeleteStatus.None
        );

        var join = (
                from receInf in receInfs
                join ptInf in ptInfs on
                    new { receInf.HpId, receInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                select new
                {
                    receInf,
                    ptInf
                }
            ).ToList();

        var entities = join.Select(
            data =>
                new CoReceInfModel(data.receInf, data.ptInf)
            )
            .ToList();

        List<CoReceInfModel> receInfModels = new();

        entities?.ForEach(entity => {
            receInfModels.Add(new CoReceInfModel(entity.ReceInf, entity.PtInf));
        });

        return new CoReceTargetModel(receInfModels);
    }
}
