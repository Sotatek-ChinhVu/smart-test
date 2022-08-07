using EmrCalculateApi.Interface;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace EmrCalculateApi.Implementation
{
    public class SystemConfigProvider : ISystemConfigProvider
    {
        private readonly List<SystemConf> _systemConfigs;
        public SystemConfigProvider(ITenantProvider tenantProvider)
        {
            _systemConfigs = tenantProvider.GetDataContext().SystemConfs.ToList();
        }

        public int GetChokiDateRange()
        {
            return (int)GetSettingValue(3006, 3, 0);
        }

        public int GetChokiFutan()
        {
            return (int)GetSettingValue(3006, 2, 0);
        }

        public int GetJibaiJunkyo()
        {
            return (int)GetSettingValue(3001, 0);
        }

        public double GetJibaiRousaiRate()
        {
            return GetSettingValue(3001, 1);
        }

        public int GetRoundKogakuPtFutan()
        {
            return (int)GetSettingValue(3016, 0);
        }

        private double GetSettingValue(int groupCd, int grpEdaNo = 0, int defaultValue = 0)
        {
            SystemConf? systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
            return systemConf != null ? systemConf.Val : defaultValue;
        }
    }
}
