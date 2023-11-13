using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.SuperAdmin
{
    [Table("NOTIFICATION")]
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("STATUS")]
        public byte Status { get; set; }

        [Column("MESSAGE")]
        public string? ENDPOINTDB { get; set; } = string.Empty;

        [Column("IS_DELETED")]
        public int IsDeleted { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
