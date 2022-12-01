using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("PT_JIBKAR")]
    [Index(nameof(HpId), nameof(WebId), nameof(PtId), Name = "PT_JIBKAR_IDX01")]
    public class PtJibkar : EmrCloneable<PtJibkar>
    {
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        //[Key]
        [Column("WEB_ID", Order = 2)]
        [MaxLength(16)]
        public string WebId { get; set; } = string.Empty;

        [Column("PT_ID")]
        public long PtId { get; set; }

        [Column("ODR_KAIJI")]
        [CustomAttribute.DefaultValue(0)]
        public int OdrKaiji { get; set; }

        [Column("ODR_UPDATE_DATE")]
        public DateTime OdrUpdateDate { get; set; }

        [Column("KARTE_KAIJI")]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKaiji { get; set; }

        [Column("KARTE_UPDATE_DATE")]
        public DateTime KarteUpdateDate { get; set; }

        [Column("KENSA_KAIJI")]
        [CustomAttribute.DefaultValue(0)]
        public int KensaKaiji { get; set; }

        [Column("KENSA_UPDATE_DATE")]
        public DateTime KensaUpdateDate { get; set; }

        [Column("BYOMEI_KAIJI")]
        [CustomAttribute.DefaultValue(0)]
        public int ByomeiKaiji { get; set; }

        [Column("BYOMEI_UPDATE_DATE")]
        public DateTime ByomeiUpdateDate { get; set; }

        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
