using Entity.Tenant;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// アフターケア病名マスタ
    ///     労災アフターケアの病名マスタ
    /// </summary>
    [Table(name: "byomei_mst_aftercare")]
    public class ByomeiMstAftercare : EmrCloneable<ByomeiMstAftercare>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 傷病名コード
        /// </summary>

        [Column(name: "byomei_cd", Order = 1)]
        [MaxLength(2)]
        public string ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 病名
        /// </summary>
        
        [Column(name: "byomei", Order = 2)]
        [MaxLength(200)]
        public string Byomei { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        /// </summary>
        
        [Column(name: "start_date", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// </summary>
        [Column(name: "end_date")]
        [CustomAttribute.DefaultValue(0)]
        public int EndDate { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
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
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
