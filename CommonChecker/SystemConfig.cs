using CommonChecker;
using Entity.Tenant;
using PostgreDataContext;

namespace CommonCheckers
{
    public class SystemConfig : ISystemConfig
    {
        private List<SystemConf> _systemConfigs = new();
        private static readonly object _threadsafelock = new object();
        private const int HpId = 1;


        public TenantNoTrackingDataContext NoTrackingDataContext { get; private set; }
        public SystemConfig(TenantNoTrackingDataContext noTrackingDataContext)
        {
            NoTrackingDataContext = noTrackingDataContext;
            RefreshData();
        }

        public void RefreshData()
        {
            _systemConfigs = NoTrackingDataContext.SystemConfs.Where(p => p.HpId == 1).ToList();
        }

        public double GetSettingValue(int groupCd, int grpEdaNo = 0, int defaultValue = 0, bool fromLastestDb = false)
        {
            lock (_threadsafelock)
            {
                SystemConf? systemConf;
                if (!fromLastestDb)
                {
                    systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
                }
                else
                {
                    systemConf = NoTrackingDataContext.SystemConfs.Where(p =>
                        p.HpId == HpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo).FirstOrDefault();
                }
                return systemConf != null ? systemConf.Val : defaultValue;
            }
        }

        public string GetSettingParam(int groupCd, int grpEdaNo = 0, string defaultParam = "", bool fromLastestDb = false)
        {
            lock (_threadsafelock)
            {
                SystemConf? systemConf;
                if (!fromLastestDb)
                {
                    systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
                }
                else
                {
                    systemConf = NoTrackingDataContext.SystemConfs.Where(p =>
                        p.HpId == HpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo).FirstOrDefault();
                }
                //Fix comment 894 (duong.vu)
                //Return value in DB if and only if Param is not null or white space
                if (systemConf != null && !string.IsNullOrWhiteSpace(systemConf.Param))
                {
                    return systemConf.Param;
                }

                return defaultParam;
            }
        }

        public List<SystemConf> GetListGroupCd(int groupCd, bool fromLastestDb = false)
        {
            lock (_threadsafelock)
            {
                List<SystemConf> systemConfs;
                if (!fromLastestDb)
                {
                    systemConfs = _systemConfigs.FindAll(p => p.GrpCd == groupCd);
                }
                else
                {
                    systemConfs = NoTrackingDataContext.SystemConfs.Where(p =>
                        p.HpId == HpId && p.GrpCd == groupCd).ToList();
                }
                return systemConfs != null ? systemConfs : new List<SystemConf>();
            }
        }


        public SystemConf CreateNewSystemConf(int grpCd, int grpEdaNo = 0, int value = 0, string param = "")
        {
            SystemConf systemConf = new SystemConf();
            systemConf.HpId = HpId;
            systemConf.GrpCd = grpCd;
            systemConf.GrpEdaNo = grpEdaNo;
            systemConf.Val = value;
            systemConf.Param = param;

            return systemConf;
        }
    }
}
