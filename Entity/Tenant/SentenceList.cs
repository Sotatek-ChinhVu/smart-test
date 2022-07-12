using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    /// <summary>
    /// 文章リスト
    /// </summary>
    [Table(name: "SENTENCE_LIST")]
    public class SentenceList : EmrCloneable<SentenceList>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("SENTENCE_LIST_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// </summary>
        //[Key]
        [Column("SENTENCE_CD", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetCd { get; set; }

        /// <summary>
        /// セット区分
        ///     0:上
        ///     1:下
        /// </summary>
        [Column("SET_KBN")]
        //[Index("SENTENCE_LIST_IDX01", 2)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbn { get; set; }

        /// <summary>
        /// カルテ区分
        ///     >=1:KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        [Column("KARTE_KBN")]
        //[Index("SENTENCE_LIST_IDX01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// レベル１
        /// </summary>
        [Column("LEVEL1")]
        //[Index("SENTENCE_LIST_IDX01", 4)]
        [CustomAttribute.DefaultValue(0)]
        public long Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// </summary>
        [Column("LEVEL2")]
        //[Index("SENTENCE_LIST_IDX01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public long Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// </summary>
        [Column("LEVEL3")]
        //[Index("SENTENCE_LIST_IDX01", 6)]
        [CustomAttribute.DefaultValue(0)]
        public long Level3 { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        [Column("SENTENCE")]
        [MaxLength(400)]
        public string Sentence { get; set; } = string.Empty;

        /// <summary>
        /// 選択種別
        ///      1: 上位選択
        /// </summary>
        [Column("SELECT_TYPE")]
        [CustomAttribute.DefaultValue(0)]
        public int SelectType { get; set; }

        /// <summary>
        /// 改行
        ///         1: 改行付き
        /// </summary>
        [Column("NEW_LINE")]
        [CustomAttribute.DefaultValue(0)]
        public int NewLine { get; set; }

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
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
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
        public string UpdateMachine { get; set; }  = string.Empty;
    }
}
