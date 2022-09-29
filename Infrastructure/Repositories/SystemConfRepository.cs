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
    public SystemConfModel GetByGrpCd(int hpId, int grpCd)
    {
        var data =  _tenantDataContext.SystemConfs
            .FirstOrDefault(s => s.HpId == hpId && s.GrpCd == grpCd);
        if (data == null) return new SystemConfModel();
        return new SystemConfModel(data.GrpCd, data.GrpEdaNo, data.Val, data.Param, data.Biko ?? String.Empty);
    }
    public double GetSettingValue(int groupCd, int grpEdaNo)
    {
        var systemConf = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
        return systemConf != null ? systemConf.Val : 0;
    }

    public string GetSettingParams(int groupCd, int grpEdaNo)
    {

        var systemConf = _tenantDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);

        //Fix comment 894 (duong.vu)
        //Return value in DB if and only if Param is not null or white space
        if (systemConf != null && !string.IsNullOrWhiteSpace(systemConf.Param))
        {
            return systemConf.Param;
        }

        return string.Empty;
    }

    private SystemConfModel ToModel(SystemConf s)
    {
        return new SystemConfModel(s.GrpCd, s.GrpEdaNo, s.Val, s.Param, s.Biko ?? string.Empty);
    }
}
