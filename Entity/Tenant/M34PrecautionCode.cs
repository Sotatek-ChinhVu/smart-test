using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_precaution_code")]
    /// <summary>
    /// create index to speed up performance
    /// </summary>
    [Index(nameof(AgeMin), nameof(AgeMax), nameof(SexCd), Name = "m34_precaution_code_age_min_idx")]

    public class M34PrecautionCode : EmrCloneable<M34PrecautionCode>
    {
        /// <summary>
        /// 注意事項コード
        /// 
        /// </summary>
        
        [Column("precaution_cd", Order = 1)]
        public string PrecautionCd { get; set; } = string.Empty;

        /// <summary>
        /// 拡張コード
        /// 
        /// </summary>
        
        [Column("extend_cd", Order = 2)]
        public string ExtendCd { get; set; } = string.Empty;

        /// <summary>
        /// 注意事項コメント
        /// 
        /// </summary>
        [Column("precaution_cmt")]
        [MaxLength(200)]
        public string? PrecautionCmt { get; set; } = string.Empty;

        /// <summary>
        /// 属性コード
        /// 
        /// </summary>
        [Column("property_cd")]
        public int PropertyCd { get; set; }

        /// <summary>
        /// 制御年齢_上限
        /// 
        /// </summary>
        [Column("age_max")]
        public int AgeMax { get; set; }

        /// <summary>
        /// 制御年齢_下限
        /// 
        /// </summary>
        [Column("age_min")]
        public int AgeMin { get; set; }

        /// <summary>
        /// 性別制御コード
        /// 1:男性 2:女性
        /// </summary>
        [Column("sex_cd")]
        public string? SexCd { get; set; } = string.Empty;
    }
}
