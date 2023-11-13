using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.SuperAdmin
{
    [Table("ADMIN")]
    public class Admin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NAME")]
        [MaxLength(100)]
        public string? Name { get; set; } = string.Empty;

        [Column("FULL_NAME")]
        [MaxLength(100)]
        public string? FullName { get; set; } = string.Empty;

        [Column("ROLE")]
        [CustomAttribute.DefaultValue(0)]
        public int Role { get; set; }

        [Column("LOGIN_ID")]
        public int LoginId { get; set; }

        [Column("PASSWORD")]
        [MaxLength(100)]
        public string? PassWord { get; set; } = string.Empty;

        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
