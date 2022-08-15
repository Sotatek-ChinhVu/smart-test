using Domain.Models.UserConfig;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class UserConfigRepository : IUserConfigRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public UserConfigRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<UserConfigModel> GetList(int groupCd, int grpItemCd)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd && x.GrpItemCd == grpItemCd).AsEnumerable().Select(x => ToModel(x));
        return userConfigs.ToList();
    }

    public List<UserConfigModel> GetList(int hpId, int groupCd, int grpItemCd, int userId)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd && x.GrpItemCd == grpItemCd && x.HpId == hpId && x.UserId == userId).AsEnumerable().Select(x => ToModel(x));
        return userConfigs.ToList();
    }

    public List<UserConfigModel> GetList(int hpId, int groupCd, int userId)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd && x.HpId == hpId && x.UserId == userId).AsEnumerable().Select(x => ToModel(x));
        return userConfigs.ToList();
    }

    public List<UserConfigModel> GetList(int groupCd)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd).AsEnumerable().Select(x => ToModel(x));
        return userConfigs.ToList();
    }

    public List<UserConfigModel> GetListFT(int userId, int fromGrpCd, int toGrpCd)
    {
        return _tenantDataContext.UserConfs
            .Where(u => u.UserId == userId && u.GrpCd >= fromGrpCd && u.GrpCd <= toGrpCd)
            .AsEnumerable().Select(u => ToModel(u)).ToList();
    }

    private UserConfigModel ToModel(UserConf u)
    {
        return new UserConfigModel(u.HpId, u.UserId, u.GrpCd,
            u.GrpItemCd, u.GrpItemEdaNo, u.Val, u.Param ?? String.Empty);
    }

    public UserConfigModel? Get(int userId, int groupCd, int grpItemCd)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd && x.GrpItemCd == grpItemCd && x.UserId == userId).AsEnumerable().Select(x => ToModel(x));
        return userConfigs.FirstOrDefault();
    }
}
