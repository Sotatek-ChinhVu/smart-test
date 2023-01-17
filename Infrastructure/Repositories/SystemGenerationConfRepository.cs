using Domain.Models.SystemGenerationConf;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class SystemGenerationConfRepository : RepositoryBase, ISystemGenerationConfRepository
    {
        public SystemGenerationConfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public (int, string) GetSettingValue(int hpId, int groupCd, int grpEdaNo = 0, int presentDate = 0, int defaultValue = 0, string defaultParam = "", bool fromLastestDb = false)
        {
            SystemGenerationConf? systemConf;
            if (!fromLastestDb)
            {
                systemConf = NoTrackingDataContext.SystemGenerationConfs.FirstOrDefault(p => p.HpId == hpId
                && p.GrpCd == groupCd
                && p.GrpEdaNo == grpEdaNo
                && p.StartDate <= presentDate
                && p.EndDate >= presentDate);
            }
            else
            {
                systemConf = NoTrackingDataContext.SystemGenerationConfs.Where(p => p.HpId == hpId
                && p.GrpCd == groupCd
                && p.GrpEdaNo == grpEdaNo
                && p.StartDate <= presentDate
                && p.EndDate >= presentDate)?.FirstOrDefault();
            }
            return systemConf != null ? (systemConf.Val, systemConf.Param ?? string.Empty) : (defaultValue, defaultParam);
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
