using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M10_DAY_LIMIT")]
    public class M10DayLimit : EmrCloneable<M10DayLimit>
    {
        /// <summary>
        /// 個別医薬品コード
        /// YJコード
        /// </summary>
        [Key]
        [Column("YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; }

        /// <summary>
        /// 連番
        /// 1..99
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 2)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 制限日数
        /// 1..999: 制限日数 (Null or 999 は制限日数なし)
        /// </summary>
        [Column("LIMIT_DAY")]
        public int LimitDay { get; set; }

        /// <summary>
        /// 適用開始日
        /// Null: 制限日数なし
        /// </summary>
        [Column("ST_DATE")]
        [MaxLength(8)]
        public string StDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// Null: 連番が最大レコードの場合
        /// </summary>
        [Column("ED_DATE")]
        [MaxLength(8)]
        public string EdDate { get; set; }

        /// <summary>
        /// コメント
        /// 特記事項があるときのみ記載
        /// </summary>
        [Column("CMT")]
        [MaxLength(400)]
        public string Cmt { get; set; }

    }
}
