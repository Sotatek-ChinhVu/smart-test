using Domain.Models.Document;
using Domain.Models.RaiinListSetting;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class RaiinListSettingRepository : RepositoryBase, IRaiinListSettingRepository
    {
        public RaiinListSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
