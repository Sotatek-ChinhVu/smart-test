using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m10_day_limit")]
    public class M10DayLimit : EmrCloneable<M10DayLimit>
    {
        /// <summary>
        /// 個別医薬品コード
        /// YJコード
        /// </summary>
        
        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 1..99
        /// </summary>
        
        [Column("seq_no", Order = 2)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 制限日数
        /// 1..999: 制限日数 (Null or 999 は制限日数なし)
        /// </summary>
        [Column("limit_day")]
        public int LimitDay { get; set; }

        /// <summary>
        /// 適用開始日
        /// Null: 制限日数なし
        /// </summary>
        [Column("st_date")]
        [MaxLength(8)]
        public string? StDate { get; set; } = string.Empty;

        /// <summary>
        /// 適用終了日
        /// Null: 連番が最大レコードの場合
        /// </summary>
        [Column("ed_date")]
        [MaxLength(8)]
        public string? EdDate { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// 特記事項があるときのみ記載
        /// </summary>
        [Column("cmt")]
        [MaxLength(400)]
        public string? Cmt { get; set; } = string.Empty;

    }
}
