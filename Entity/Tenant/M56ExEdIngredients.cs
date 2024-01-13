using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_ex_ed_ingredients")]
    public class M56ExEdIngredients : EmrCloneable<M56ExEdIngredients>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        
        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 2)]
        [MaxLength(3)]
        public string SeqNo { get; set; } = string.Empty;

        /// <summary>
        /// 成分コード
        /// 
        /// </summary>
        [Column("seibun_cd")]
        [MaxLength(9)]
        public string? SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分索引コード
        /// 
        /// </summary>
        [Column("seibun_index_cd")]
        [MaxLength(3)]
        public string? SeibunIndexCd { get; set; } = string.Empty;

        /// <summary>
        /// 種別
        /// 1:主成分 2:添加物
        /// </summary>
        [Column("sbt")]
        public int Sbt { get; set; }

        /// <summary>
        /// プロドラッグチェック対象フラグ
        /// 1:該当
        /// </summary>
        [Column("prodrug_check")]
        [MaxLength(1)]
        public string? ProdrugCheck { get; set; } = string.Empty;

        /// <summary>
        /// 類似成分チェック対象フラグ
        /// 1:該当
        /// </summary>
        [Column("analogue_check")]
        [MaxLength(1)]
        public string? AnalogueCheck { get; set; } = string.Empty;

        /// <summary>
        /// 溶解液チェック対象フラグ
        /// 1:該当
        /// </summary>
        [Column("yokaieki_check")]
        [MaxLength(1)]
        public string? YokaiekiCheck { get; set; } = string.Empty;

        /// <summary>
        /// 添加物チェック対象フラグ
        /// 1:該当
        /// </summary>
        [Column("tenkabutu_check")]
        [MaxLength(1)]
        public string? TenkabutuCheck { get; set; } = string.Empty;
    }
}
