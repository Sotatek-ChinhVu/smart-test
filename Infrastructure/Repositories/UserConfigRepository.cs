using Domain.Models.UserConf;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UserConfigRepository : IUserConfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public UserConfigRepository(ITenantProvider tenantProvider)
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

    public UserConfModel? Get(int userId, int groupCd, int grpItemCd)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd && x.GrpItemCd == grpItemCd && x.UserId == userId).AsEnumerable().Select(x => ToModel(x));
        return userConfigs.FirstOrDefault();
    }
}
