namespace Domain.SuperAdminModels.MigrationTenantHistory
{
    public class MigrationTenantHistoryModel
    {
        public MigrationTenantHistoryModel(int tenantId, string migrationId)
        {
            TenantId = tenantId;
            MigrationId = migrationId;
        }

        public int Id { get; private set; }

        public int TenantId { get; private set; }
        public string MigrationId { get; private set; }
    }

}
