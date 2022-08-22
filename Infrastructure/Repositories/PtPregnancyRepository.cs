using Domain.Models.PtPregnancy;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtPregnancyRepository : IPtPregnancyRepository
{
    private readonly TenantNoTrackingDataContext _tenantDataContext;

    public PtPregnancyRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<PtPregnancyModel> GetList(long ptId, int hpId)
    {
        var ptPregnancys = _tenantDataContext.PtPregnancies.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0).Select(x => new PtPregnancyModel(
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
                x.IsDeleted,
                x.UpdateDate,
                x.UpdateId,
                x.UpdateMachine ?? String.Empty
            ));
        return ptPregnancys.ToList();
    }
}
