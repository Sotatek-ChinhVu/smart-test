using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.SuperAdmin
{
    [Table("TENANT")]
    public class Tenant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TENANT_ID")]
        public int TenantId { get; set; }

        [Column("HOSPITAL")]
        [MaxLength(100)]
        public string Hospital { get; set; } = string.Empty;

        [Column("STATUS")]
        public byte Status { get; set; }

        [Column("ADMIN_ID")]
        public int AdminId { get; set; }

        [Column("PASSWORD")]
        public string Password { get; set; } = string.Empty;

        [Column("SUB_DOMAIN")]
        public string SubDomain { get; set; } = string.Empty;

        [Column("DB")]
        public string Db { get; set; } = string.Empty;

        [Column("SIZE")]
        public int Size { get; set; }

        [Column("TYPE")]
        public byte Type { get; set; }

        [Column("END_POINT_DB")]
        public string EndPointDb { get; set; } = string.Empty;

        [Column("END_SUB_DOMAIN")]
        public string EndSubDomain { get; set; } = string.Empty;

        [Column("ACTION")]
        public int Action { get; set; }

        [Column("SCHEDULE_DATE")]
        public int ScheduleDate { get; set; }

        [Column("SCHEDULE_TIME")]
        public int ScheduleTime { get; set; }

        [Column("IS_DELETED")]
        public int IsDeleted { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
