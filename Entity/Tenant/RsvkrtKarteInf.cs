using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RSVKRT_KARTE_INF")]
    public class RsvkrtKarteInf : EmrCloneable<RsvkrtKarteInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        [Column("RSV_DATE")]
        public int RsvDate { get; set; }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        //[Key]
        [Column("RSVKRT_NO", Order = 3)]
        public long RsvkrtNo { get; set; }

        /// <summary>
        /// カルテ区分
        /// KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        //[Key]
        [Column("KARTE_KBN", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// 
        /// </summary>
        [Column("TEXT")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// リッチテキスト
        /// 
        /// </summary>
        [Column("RICH_TEXT")]
        public byte[] RichText { get; set; } = default!;

        /// <summary>
        /// 削除区分
        /// "1: 削除
        /// 2: 実施"
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}