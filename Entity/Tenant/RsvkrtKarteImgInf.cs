using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rsvkrt_karte_img_inf")]
    public class RsvkrtKarteImgInf : EmrCloneable<RsvkrtKarteImgInf>
    {
        /// <summary>
        /// ID
        /// 
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        [Column("rsvkrt_no")]
        public long RsvkrtNo { get; set; }

        /// <summary>
        /// カルテ区分
        /// KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        [Column("karte_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("seq_no")]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 表示位置
        /// 
        /// </summary>
        [Column("position")]
        [CustomAttribute.DefaultValue(0)]
        public long Position { get; set; }

        /// <summary>
        /// ファイル名
        /// 
        /// </summary>
        [Column("file_name")]
        [MaxLength(100)]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ
        /// 
        /// </summary>
        [Column("message")]
        [MaxLength(2000)]
        public string? Message { get; set; } = string.Empty;
    }
}