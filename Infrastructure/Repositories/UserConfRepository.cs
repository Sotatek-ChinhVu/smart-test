using Domain.Models.UserConf;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UserConfRepository : IUserConfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public UserConfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<UserConfModel> GetList(int userId, int fromGrpCd, int toGrpCd)
    {
        return _tenantDataContext.UserConfs
            .Where(u => u.UserId == userId && u.GrpCd >= fromGrpCd && u.GrpCd <= toGrpCd)
            .AsEnumerable().Select(u => ToModel(u)).ToList();
    }

    private UserConfModel ToModel(UserConf u)
    {
        return new UserConfModel(u.UserId, u.GrpCd,
            u.GrpItemCd, u.GrpItemEdaNo, u.Val, u.Param ?? String.Empty);
    }
}
