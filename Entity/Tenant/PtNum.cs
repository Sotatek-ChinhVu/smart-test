using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PT_NUM")]
    public class PtNum : EmrCloneable<PtNum>
    {
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        [Column("PT_NUM")]
        public long PtNumber { get; set; }

    }
}