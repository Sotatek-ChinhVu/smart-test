using Domain.Models.PtCmtInf;
using Domain.Models.PtInfection;
using Domain.Models.UserConfig;
using Entity.Tenant;
using Helper.Constants;
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
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd && x.GrpItemCd == grpItemCd).Select(x => new UserConfigModel(
              x.HpId,
              x.UserId,
              x.GrpCd,
              x.GrpItemCd,
              x.GrpItemEdaNo,
              x.Val,
              x.Param ?? String.Empty
            ));
        return userConfigs.ToList();
    }

    public List<UserConfigModel> GetList(int hpId, int groupCd, int grpItemCd, int userId)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd && x.GrpItemCd == grpItemCd && x.HpId == hpId && x.UserId == userId).Select(x => new UserConfigModel(
                  x.HpId,
                  x.UserId,
                  x.GrpCd,
                  x.GrpItemCd,
                  x.GrpItemEdaNo,
                  x.Val,
                  x.Param ?? String.Empty
                ));
        return userConfigs.ToList();
    }

    public List<UserConfigModel> GetList(int hpId, int groupCd, int userId)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd &&  x.HpId == hpId && x.UserId == userId).Select(x => new UserConfigModel(
                  x.HpId,
                  x.UserId,
                  x.GrpCd,
                  x.GrpItemCd,
                  x.GrpItemEdaNo,
                  x.Val,
                  x.Param ?? String.Empty
                ));
        return userConfigs.ToList();
    }

    public List<UserConfigModel> GetList(int groupCd)
    {
        var userConfigs = _tenantDataContext.UserConfs.Where(x => x.GrpCd == groupCd).Select(x => new UserConfigModel(
                  x.HpId,
                  x.UserId,
                  x.GrpCd,
                  x.GrpItemCd,
                  x.GrpItemEdaNo,
                  x.Val,
                  x.Param ?? String.Empty
                ));
        return userConfigs.ToList();
    }
}
