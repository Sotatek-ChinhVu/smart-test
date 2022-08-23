using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// サマリ情報
    ///     サマリ情報を管理する
    /// </summary>
    [Table(name: "SUMMARY_INF")]
    public class SummaryInf : EmrCloneable<SummaryInf>
    {
        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        //[Index("SUMMARY_INF_IDX01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 病院コード
        /// </summary>
        [Column("HP_ID")]
        //[Index("SUMMARY_INF_IDX01", 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者番号
        /// </summary>
        [Column("PT_ID")]
        //[Index("SUMMARY_INF_IDX01", 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
        //[Index("SUMMARY_INF_IDX01", 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// </summary>
        [Column("TEXT")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// リッチテキスト
        /// </summary>
        [Column("RTEXT")]
        public byte[] Rtext { get; set; } = default!;

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
