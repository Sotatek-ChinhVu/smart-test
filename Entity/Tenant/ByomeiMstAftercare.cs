using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// アフターケア病名マスタ
    ///     労災アフターケアの病名マスタ
    /// </summary>
    [Table(name: "BYOMEI_MST_AFTERCARE")]
    public class ByomeiMstAftercare : EmrCloneable<ByomeiMstAftercare>
    {
        /// <summary>
        /// 傷病名コード
        /// </summary>
        [Key]
        [Column(name: "BYOMEI_CD", Order = 1)]
        [MaxLength(2)]
        public string ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 病名
        /// </summary>
        [Key]
        [Column(name: "BYOMEI", Order = 2)]
        [MaxLength(200)]
        public string Byomei { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        /// </summary>
        [Key]
        [Column(name: "START_DATE", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// </summary>
        [Column(name: "END_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int EndDate { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
