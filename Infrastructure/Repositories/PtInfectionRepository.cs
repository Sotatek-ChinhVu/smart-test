using Domain.Models.PtInfection;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtInfectionRepository : IPtInfectionRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtInfectionRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtInfectionModel> GetList(long ptId)
    {
        var ptInfections = _tenantDataContext.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(x => x.SortNo).Select(x => new PtInfectionModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd ?? String.Empty,
               x.ByotaiCd ?? String.Empty,
               x.Byomei ?? String.Empty,
               x.StartDate,
               x.Cmt ?? String.Empty,
               x.IsDeleted
            ));
        return ptInfections.ToList();
    }
}
