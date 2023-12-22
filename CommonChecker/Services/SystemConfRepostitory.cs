﻿using CommonChecker.DB;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace CommonChecker.Services
{
    public class SystemConfRepostitory : ISystemConfigRepository
    {
        private readonly TenantDataContext _tenantDataContext;

        public SystemConfRepostitory(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
        }
        public int GetSettingValue(int hpId, int groupCd, int grpEdaNo = 0, int presentDate = 0, int defaultValue = 0, bool fromLastestDb = false)
        {
            SystemGenerationConf? systemConf;
            if (!fromLastestDb)
            {
                systemConf = _tenantDataContext.SystemGenerationConfs.FirstOrDefault(p => p.HpId == hpId
                && p.GrpCd == groupCd
                && p.GrpEdaNo == grpEdaNo
                && p.StartDate <= presentDate
                && p.EndDate >= presentDate);
            }
            else
            {
                systemConf = _tenantDataContext.SystemGenerationConfs.Where(p => p.HpId == hpId
                && p.GrpCd == groupCd
                && p.GrpEdaNo == grpEdaNo
                && p.StartDate <= presentDate
                && p.EndDate >= presentDate)?.FirstOrDefault();
            }
            return systemConf != null ? systemConf.Val : defaultValue;
        }

        /// <summary>
        /// Get SystemGenerationConf
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="presentDate"></param>
        /// <returns></returns>
        public int RefillSetting(int hpId,int presentDate)
        {
            return GetSettingValue(hpId, 2002, 0, presentDate: presentDate, defaultValue: 999);
        }
    }
}
