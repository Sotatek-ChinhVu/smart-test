using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// サマリ情報
    ///     サマリ情報を管理する
    /// </summary>
    [Table(name: "summary_inf")]
    public class SummaryInf : EmrCloneable<SummaryInf>
    {
        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("id", Order = 1)]
        //[Index("summary_inf_idx01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 病院コード
        /// </summary>
        [Column("hp_id")]
        //[Index("summary_inf_idx01", 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者番号
        /// </summary>
        [Column("pt_id")]
        //[Index("summary_inf_idx01", 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("seq_no")]
        //[Index("summary_inf_idx01", 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// </summary>
        [Column("text")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// リッチテキスト
        /// </summary>
        [Column("rtext")]
        public byte[]? Rtext { get; set; } = default!;

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
