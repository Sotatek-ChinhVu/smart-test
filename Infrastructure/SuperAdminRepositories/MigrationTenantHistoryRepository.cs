using Domain.SuperAdminModels.MigrationTenantHistory;
using Entity.SuperAdmin;
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

        public bool AddMigrationHistory(int tenantId, string migrationId)
        {
            var migrationHistory = new MigrationTenantHistory();
            migrationHistory.MigrationId = migrationId;
            migrationHistory.TenantId = tenantId;
            TrackingDataContext.MigrationTenantHistories.Add(migrationHistory);
            return TrackingDataContext.SaveChanges() > 0;
        }

        public List<MigrationTenantHistoryModel> GetMigrationByTenants(List<int> tenantIds)
        {
            var data = NoTrackingDataContext.MigrationTenantHistories.Where(i => tenantIds.Contains(i.TenantId)).Select(x => new MigrationTenantHistoryModel(x.TenantId, x.MigrationId)).ToList();
            return data;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
