using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KARTE_IMG_INF")]
    public class KarteImgInf : EmrCloneable<KarteImgInf>
    {
        /// <summary>
        /// ID
        /// 
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("RAIIN_NO")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// カルテ区分
        /// KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        [Column("KARTE_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 表示位置
        /// 
        /// </summary>
        [Column("POSITION")]
        [CustomAttribute.DefaultValue(0)]
        public long Position { get; set; }

        /// <summary>
        /// ファイル名
        /// 
        /// </summary>
        [Column("FILE_NAME")]
        [MaxLength(100)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ
        /// 
        /// </summary>
        [Column("MESSAGE")]
        [MaxLength(2000)]
        public string Message { get; set; } = string.Empty;

    }
}
