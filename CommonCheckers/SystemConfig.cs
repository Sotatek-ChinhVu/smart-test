using Entity.Tenant;

namespace CommonCheckers
{
    public class SystemConfig
    {
        private List<SystemConf> _systemConfigs = new List<SystemConf>();
        private static readonly object _threadsafelock = new object();
        public double GetSettingValue(int groupCd, int grpEdaNo = 0, int defaultValue = 0, bool fromLastestDb = false)
        {
            lock (_threadsafelock)
            {
                SystemConf systemConf = new SystemConf();
                if (!fromLastestDb)
                {
                    systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
                }
                else
                {
                    systemConf = dbService.SystemConfigRepository.FindListQueryableNoTrack(p =>
                        p.HpId == Session.HospitalID && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo).FirstOrDefault();
                }
                return systemConf != null ? systemConf.Val : defaultValue;
            }
        }
    }
}
