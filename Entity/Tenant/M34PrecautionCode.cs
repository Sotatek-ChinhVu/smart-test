using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_PRECAUTION_CODE")]
    public class M34PrecautionCode : EmrCloneable<M34PrecautionCode>
    {
        /// <summary>
        /// 注意事項コード
        /// 
        /// </summary>
        [Key]
        [Column("PRECAUTION_CD", Order = 1)]
        public string PrecautionCd { get; set; } = string.Empty;

        /// <summary>
        /// 拡張コード
        /// 
        /// </summary>
        //[Key]
        [Column("EXTEND_CD", Order = 2)]
        public string ExtendCd { get; set; } = string.Empty;

        /// <summary>
        /// 注意事項コメント
        /// 
        /// </summary>
        [Column("PRECAUTION_CMT")]
        [MaxLength(200)]
        public string? PrecautionCmt { get; set; } = string.Empty;

        /// <summary>
        /// 属性コード
        /// 
        /// </summary>
        [Column("PROPERTY_CD")]
        public int PropertyCd { get; set; }

        /// <summary>
        /// 制御年齢_上限
        /// 
        /// </summary>
        [Column("AGE_MAX")]
        public int AgeMax { get; set; }

        /// <summary>
        /// 制御年齢_下限
        /// 
        /// </summary>
        [Column("AGE_MIN")]
        public int AgeMin { get; set; }

        /// <summary>
        /// 性別制御コード
        /// 1:男性 2:女性
        /// </summary>
        [Column("SEX_CD")]
        public string? SexCd { get; set; } = string.Empty;
    }
}
