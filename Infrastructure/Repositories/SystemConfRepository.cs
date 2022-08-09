using Domain.Models.SystemConf;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class SystemConfRepository : ISystemConfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public SystemConfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd)
    {
        return _tenantDataContext.SystemConfs
            .Where(s => s.GrpCd >= fromGrpCd && s.GrpCd <= toGrpCd)
            .AsEnumerable().Select(s => ToModel(s)).ToList();
    }

    private SystemConfModel ToModel(SystemConf s)
    {
        return new SystemConfModel(s.GrpCd, s.GrpEdaNo, s.Val, s.Param, s.Biko ?? string.Empty);
    }
}
