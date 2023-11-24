using Domain.SuperAdminModels.MigrationTenantHistory;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.SuperAdminRepositories
{
    public class MigrationTenantHistoryRepository : SuperAdminRepositoryBase, IMigrationTenantHistoryRepository
    {
        public MigrationTenantHistoryRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }
        public List<string> GetMigration(int tenantId)
        {
            var data = NoTrackingDataContext.MigrationTenantHistories.Where(i => i.TenantId == tenantId).Select(i => i.MigrationId).ToList();
            return data;
        }
    }
}
