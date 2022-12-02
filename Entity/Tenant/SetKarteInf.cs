using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// セットカルテ情報
    /// </summary>
    [Table(name: "SET_KARTE_INF")]
    public class SetKarteInf : EmrCloneable<SetKarteInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// </summary>
        //[Key]
        [Column("SET_CD", Order = 2)]
        public int SetCd { get; set; }

        /// <summary>
        /// カルテ区分
        ///    KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        //[Key]
        [Column("KARTE_KBN", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// </summary>
        [Column("TEXT")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// リッチテキスト
        /// </summary>
        [Column("RICH_TEXT")]
        public byte[]? RichText { get; set; } = default!;

        /// <summary>
        /// 削除区分
        ///    1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
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
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
