using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// セット区分マスタ
	/// </summary>
    [Table(name: "set_kbn_mst")]
    public class SetKbnMst : EmrCloneable<SetKbnMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セット区分
        /// 
        /// </summary>
        
        [Column("set_kbn", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbn { get; set; }

        /// <summary>
        /// セット区分枝番
        /// 
        /// </summary>
        
        [Column("set_kbn_eda_no", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbnEdaNo { get; set; }

        /// <summary>
        /// 世代ID
        /// 
        /// </summary>
        
        [Column("generation_id", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// セット区分名称
        ///    
        /// </summary>
        [Column("set_kbn_name")]
        [MaxLength(60)]
        public string? SetKbnName { get; set; } = string.Empty;

        /// <summary>
        /// 診療科コード
        ///     0: 共通
        /// </summary>
        [Column("ka_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int KaCd { get; set; }

        /// <summary>
        /// 医師コード
        ///      0: 共通
        /// </summary>
        [Column("doc_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int DocCd { get; set; }

        /// <summary>
        /// 削除区分
        ///     1: 削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
