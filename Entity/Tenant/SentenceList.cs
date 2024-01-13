using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 文章リスト
    /// </summary>
    [Table(name: "sentence_list")]
    public class SentenceList : EmrCloneable<SentenceList>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("sentence_list_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// </summary>
        
        [Column("sentence_cd", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetCd { get; set; }

        /// <summary>
        /// セット区分
        ///     0:上
        ///     1:下
        /// </summary>
        [Column("set_kbn")]
        //[Index("sentence_list_idx01", 2)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbn { get; set; }

        /// <summary>
        /// カルテ区分
        ///     >=1:KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        [Column("karte_kbn")]
        //[Index("sentence_list_idx01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// レベル１
        /// </summary>
        [Column("level1")]
        //[Index("sentence_list_idx01", 4)]
        [CustomAttribute.DefaultValue(0)]
        public long Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// </summary>
        [Column("level2")]
        //[Index("sentence_list_idx01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public long Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// </summary>
        [Column("level3")]
        //[Index("sentence_list_idx01", 6)]
        [CustomAttribute.DefaultValue(0)]
        public long Level3 { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        [Column("sentence")]
        [MaxLength(400)]
        public string? Sentence { get; set; } = string.Empty;

        /// <summary>
        /// 選択種別
        ///      1: 上位選択
        /// </summary>
        [Column("select_type")]
        [CustomAttribute.DefaultValue(0)]
        public int SelectType { get; set; }

        /// <summary>
        /// 改行
        ///         1: 改行付き
        /// </summary>
        [Column("new_line")]
        [CustomAttribute.DefaultValue(0)]
        public int NewLine { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("create_machine")]
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
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
