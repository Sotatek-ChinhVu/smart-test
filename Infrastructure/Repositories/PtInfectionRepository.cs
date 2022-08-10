using Domain.Models.PtCmtInf;
using Domain.Models.PtInfection;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtInfectionRepository : IPtInfectionRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtInfectionRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtInfectionModel> GetList(long ptId)
    {
        var ptInfections = _tenantDataContext.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtInfectionModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd,
               x.ByotaiCd,
               x.Byomei,
               x.StartDate,
               x.Cmt,
               x.IsDeleted
            )).OrderBy(x => x.SortNo);
        return ptInfections.ToList();
    }
}
