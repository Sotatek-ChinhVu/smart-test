using Domain.Models.VisitingListSetting;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VisitingListSettingRepository : IVisitingListSettingRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public VisitingListSettingRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public VisitingListSettingModel Get(int userId)
        {
             var userConfigList = _tenantDataContext.UserConfs
            .Where(u => u.UserId == userId && u.GrpCd >= 2001 && u.GrpCd <= 2005)
            .AsEnumerable().Select(u => UserConfToModel(u)).ToList();

            var systemConfigList = _tenantDataContext.SystemConfs
            .Where(s => s.GrpCd >= 5002 && s.GrpCd <= 5003)
            .AsEnumerable().Select(s => SystemConfToModel(s)).ToList();

            return new VisitingListSettingModel(userConfigList, systemConfigList);
        }

        private ConfigModel UserConfToModel(UserConf u)
        {
            return new ConfigModel(u.GrpCd, u.Param, u.Val, u.GrpItemEdaNo);
        }

        private ConfigModel SystemConfToModel(SystemConf u)
        {
            return new ConfigModel(u.GrpCd, u.Param, u.Val, u.GrpEdaNo);
        }
    }
}
