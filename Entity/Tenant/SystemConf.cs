using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// システム設定
    /// </summary>
    [Table("system_conf")]
    public class SystemConf : EmrCloneable<SystemConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類コード
        /// </summary>
        
        [Column("grp_cd", Order = 2)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類枝番
        /// </summary>
        
        [Column("grp_eda_no", Order = 3)]
        public int GrpEdaNo { get; set; }

        /// <summary>
        /// 設定値
        /// </summary>
        [Column("val")]
        public double Val { get; set; }

        /// <summary>
        /// パラメーター
        /// </summary>
        [Column("param")]
        [MaxLength(300)]
        public string? Param { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        [Column("biko")]
        [MaxLength(200)]
        public string? Biko { get; set; } = string.Empty;

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