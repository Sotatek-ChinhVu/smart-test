using Domain.Models.PtCmtInf;
using Domain.Models.PtPregnancy;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtPregnancyRepository : IPtPregnancyRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtPregnancyRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtPregnancyModel> GetList(long ptId, int hpId, int sinDate)
    {
        var ptPregnancys = _tenantDataContext.PtPregnancies.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0 && x.StartDate <= sinDate && x.EndDate >= sinDate).Select(x => new PtPregnancyModel(
                x.Id,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.StartDate,
                x.EndDate,
                x.PeriodDate,
                x.PeriodDueDate,
                x.OvulationDate,
                x.OvulationDueDate,
                x.IsDeleted
            ));
        return ptPregnancys.ToList();
    }
}
