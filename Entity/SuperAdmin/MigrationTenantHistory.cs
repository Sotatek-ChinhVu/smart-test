using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.SuperAdmin
{
    [Table("MIGRATION_TENANT_HISTORY")]
    public class MigrationTenantHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("TENANT_ID")]
        public int TenantId { get; set; }

        [Column("MIGRATION_ID")]
        public string MigrationId { get; set; } = string.Empty;

    }
}
