using Domain.Models.SystemConf;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class SystemConfRepository : RepositoryBase, ISystemConfRepository
{
    public SystemConfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {

    }

    public List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd)
    {
        return NoTrackingDataContext.SystemConfs
            .Where(s => s.GrpCd >= fromGrpCd && s.GrpCd <= toGrpCd)
            .AsEnumerable().Select(s => ToModel(s)).ToList();
    }
    public SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo)
    {
        var data = NoTrackingDataContext.SystemConfs
            .FirstOrDefault(s => s.HpId == hpId && s.GrpCd == grpCd && s.GrpEdaNo == grpEdaNo);
        if (data == null) return new SystemConfModel();
        return new SystemConfModel(data.GrpCd, data.GrpEdaNo, data.Val, data?.Param ?? string.Empty, data?.Biko ?? string.Empty);
    }
    public double GetSettingValue(int groupCd, int grpEdaNo, int hpId)
    {
        var systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo && p.HpId == hpId);
        return systemConf != null ? systemConf.Val : 0;
    }

    public string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "")
    {

        var systemConf = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo && p.HpId == hpId);

        //Fix comment 894 (duong.vu)
        //Return value in DB if and only if Param is not null or white space
        if (systemConf != null && !string.IsNullOrWhiteSpace(systemConf.Param))
        {
            return systemConf.Param;
        }

        return defaultParam;
    }

    private SystemConfModel ToModel(SystemConf s)
    {
        return new SystemConfModel(s.GrpCd, s.GrpEdaNo, s.Val, s?.Param ?? string.Empty, s?.Biko ?? string.Empty);
    }
}
