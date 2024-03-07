using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("pt_jibkar")]
    [Index(nameof(HpId), nameof(WebId), nameof(PtId), Name = "pt_jibkar_idx01")]
    public class PtJibkar : EmrCloneable<PtJibkar>
    {
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        
        [Column("web_id", Order = 2)]
        [MaxLength(16)]
        public string WebId { get; set; } = string.Empty;

        [Column("pt_id")]
        public long PtId { get; set; }

        [Column("odr_kaiji")]
        [CustomAttribute.DefaultValue(0)]
        public int OdrKaiji { get; set; }

        [Column("odr_update_date")]
        public DateTime OdrUpdateDate { get; set; }

        [Column("karte_kaiji")]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKaiji { get; set; }

        [Column("karte_update_date")]
        public DateTime KarteUpdateDate { get; set; }

        [Column("kensa_kaiji")]
        [CustomAttribute.DefaultValue(0)]
        public int KensaKaiji { get; set; }

        [Column("kensa_update_date")]
        public DateTime KensaUpdateDate { get; set; }

        [Column("byomei_kaiji")]
        [CustomAttribute.DefaultValue(0)]
        public int ByomeiKaiji { get; set; }

        [Column("byomei_update_date")]
        public DateTime ByomeiUpdateDate { get; set; }

        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
