using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// セットマスタ情報
	/// </summary>
    [Table(name: "set_mst")]
    public class SetMst : EmrCloneable<SetMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id", Order = 1)]
        //[Index("set_mst_ui001", 1)]
        //[Index("set_mst_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// 
        /// </summary>
        
        [Column("set_cd", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetCd { get; set; }

        /// <summary>
        /// セット区分
        /// 
        /// </summary>
        [Column("set_kbn")]
        //[Index("set_mst_ui001", 2)]
        //[Index("set_mst_idx01", 2)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbn { get; set; }

        /// <summary>
        /// セット区分枝番
        /// 
        /// </summary>
        [Column("set_kbn_eda_no")]
        //[Index("set_mst_ui001", 3)]
        //[Index("set_mst_idx01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbnEdaNo { get; set; }

        /// <summary>
        /// 世代ID
        /// 
        /// </summary>
        [Column("generation_id")]
        //[Index("set_mst_idx01", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// レベル１
        /// 
        /// </summary>
        [Column("level1")]
        //[Index("set_mst_ui001", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// 
        /// </summary>
        [Column("level2")]
        //[Index("set_mst_ui001", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// 
        /// </summary>
        [Column("level3")]
        //[Index("set_mst_ui001", 6)]
        [CustomAttribute.DefaultValue(0)]
        public int Level3 { get; set; }

        /// <summary>
        /// セット名称
        /// 
        /// </summary>
        [Column("set_name")]
        [MaxLength(60)]
        public string? SetName { get; set; } = string.Empty;

        /// <summary>
        /// 体重別区分
        /// 
        /// </summary>
        [Column("weight_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int WeightKbn { get; set; }

        /// <summary>
        /// 色
        /// 
        /// </summary>
        [Column("color")]
        [CustomAttribute.DefaultValue(0)]
        public int Color { get; set; }

        [Column("is_group")]
        [CustomAttribute.DefaultValue(0)]
        public int IsGroup { get; set; }

        /// <summary>
        /// 削除区分
        ///     1: 削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("set_mst_idx01", 5)]
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
