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

    public List<SystemConfModel> GetListByGrpCd(int hpId, List<SystemConfModel> grpItemList)
    {
        List<SystemConfModel> result = new();
        var grpCdList = grpItemList.Select(item => item.GrpCd).ToList();
        var grpEdaNoList = grpItemList.Select(item => item.GrpEdaNo).ToList();
        var systemConfigList = NoTrackingDataContext.SystemConfs.Where(item => item.HpId == hpId && grpCdList.Contains(item.GrpCd) && grpEdaNoList.Contains(item.GrpEdaNo)).ToList();
        foreach (var grp in grpItemList)
        {
            var systemConfigItem = systemConfigList.FirstOrDefault(item => item.GrpCd == grp.GrpCd && item.GrpEdaNo == grp.GrpEdaNo);
            if (systemConfigItem != null)
            {
                result.Add(new SystemConfModel(systemConfigItem.GrpCd, systemConfigItem.GrpEdaNo, systemConfigItem.Val, systemConfigItem?.Param ?? string.Empty, systemConfigItem?.Biko ?? string.Empty));
            }
        }
        return result;
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

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

}
