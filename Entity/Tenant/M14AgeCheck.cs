using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M14_AGE_CHECK")]
    public class M14AgeCheck : EmrCloneable<M14AgeCheck>
    {
        /// <summary>
        /// YJコード
        /// 
        /// </summary>
        
        [Column("YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 注意コメントコード
        /// TNC で始まり4桁の数字が続く
        /// </summary>
        
        [Column("ATTENTION_CMT_CD", Order = 2)]
        [MaxLength(7)]
        public string AttentionCmtCd { get; set; } = string.Empty;

        /// <summary>
        /// 作用機序
        /// 改行を全角セミコロン「；」へ置換
        /// </summary>
        [Column("WORKING_MECHANISM")]
        [MaxLength(1000)]
        public string? WorkingMechanism { get; set; } = string.Empty;

        /// <summary>
        /// 添付文書レベル
        /// 01: 投与禁忌
        ///                     02: 原則投与禁忌          
        ///                     03: 投与禁忌が望ましい          
        ///                     04: 原則投与禁忌が望ましい          
        ///                     05: 投与回避 [が望ましい]          
        ///                     06: 有益性投与          
        ///                     07: 慎重投与 [が望ましい]          
        ///                     10: 安全性未確立          
        ///                     00: 小児、高齢者への投与に関する禁忌等の情報なし          
        ///                     99: 小児、高齢者等への投与に関する情報を収集又は作成中          
        /// </summary>
        [Column("TENPU_LEVEL")]
        [MaxLength(2)]
        public string? TenpuLevel { get; set; } = string.Empty;

        /// <summary>
        /// 年齢区分フラグ
        /// 1:年齢情報を有する　Null:年齢情報を有しない場合
        /// </summary>
        [Column("AGE_KBN")]
        [MaxLength(1)]
        public string? AgeKbn { get; set; } = string.Empty;

        /// <summary>
        /// 体重区分フラグ
        /// 1:体重情報を有する　Null:体重情報を有しない場合
        /// </summary>
        [Column("WEIGHT_KBN")]
        [MaxLength(1)]
        public string? WeightKbn { get; set; } = string.Empty;

        /// <summary>
        /// 性別区分フラグ
        /// 1:男性 2:女性（ただし、年齢区分、又は体重区分と併用）
        ///                     Null:性別情報を有しない場合          
        /// </summary>
        [Column("SEX_KBN")]
        [MaxLength(1)]
        public string? SexKbn { get; set; } = string.Empty;

        /// <summary>
        /// 年齢条件下限値
        /// Y チェック年齢下限値を記載。設定値「以上」を表す。（年齢換算値で記載）
        /// </summary>
        [Column("AGE_MIN")]
        public double AgeMin { get; set; }

        /// <summary>
        /// 年齢条件上限値
        /// Y チェック年齢上限値を記載。設定値「未満」を表す。（年齢換算値で記載）
        /// </summary>
        [Column("AGE_MAX")]
        public double AgeMax { get; set; }

        /// <summary>
        /// 体重条件下限値
        /// Y チェック体重下限値を記載。設定値「以上」を表す。（kg換算値で記載）
        /// </summary>
        [Column("WEIGHT_MIN")]
        public double WeightMin { get; set; }

        /// <summary>
        /// 体重条件上限値
        /// Y チェック体重上限値を記載。設定値「未満」を表す。（kg換算値で記載）
        /// </summary>
        [Column("WEIGHT_MAX")]
        public double WeightMax { get; set; }
    }
}
