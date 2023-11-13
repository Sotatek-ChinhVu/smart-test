using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.SuperAdmin
{
    [Table("SCRIPTION")]
    public class Scription
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("TENANT_ID")]
        public int TenantId { get; set; }

        [Column("HOSPITAL")]
        public string? HOSPITAL { get; set; } = string.Empty;

        [Column("ScriptString")]
        public string ScriptString { get; set; } = string.Empty;

        [Column("IS_DELETED")]
        public int IsDeleted { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
