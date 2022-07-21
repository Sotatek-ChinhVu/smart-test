using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// セットマスタ情報
	/// </summary>
    [Table(name: "SET_MST")]
    public class SetMst : EmrCloneable<SetMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        //[Index("SET_MST_UI001", 1)]
        //[Index("SET_MST_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// 
        /// </summary>
        //[Key]
        [Column("SET_CD", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetCd { get; set; }

        /// <summary>
        /// セット区分
        /// 
        /// </summary>
        [Column("SET_KBN")]
        //[Index("SET_MST_UI001", 2)]
        //[Index("SET_MST_IDX01", 2)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbn { get; set; }

        /// <summary>
        /// セット区分枝番
        /// 
        /// </summary>
        [Column("SET_KBN_EDA_NO")]
        //[Index("SET_MST_UI001", 3)]
        //[Index("SET_MST_IDX01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbnEdaNo { get; set; }

        /// <summary>
        /// 世代ID
        /// 
        /// </summary>
        [Column("GENERATION_ID")]
        //[Index("SET_MST_IDX01", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// レベル１
        /// 
        /// </summary>
        [Column("LEVEL1")]
        //[Index("SET_MST_UI001", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// 
        /// </summary>
        [Column("LEVEL2")]
        //[Index("SET_MST_UI001", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// 
        /// </summary>
        [Column("LEVEL3")]
        //[Index("SET_MST_UI001", 6)]
        [CustomAttribute.DefaultValue(0)]
        public int Level3 { get; set; }

        /// <summary>
        /// セット名称
        /// 
        /// </summary>
        [Column("SET_NAME")]
        [MaxLength(60)]
        public string SetName { get; set; } = string.Empty;

        /// <summary>
        /// 体重別区分
        /// 
        /// </summary>
        [Column("WEIGHT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int WeightKbn { get; set; }

        /// <summary>
        /// 色
        /// 
        /// </summary>
        [Column("COLOR")]
        [CustomAttribute.DefaultValue(0)]
        public int Color { get; set; }

        [Column("IS_GROUP")]
        [CustomAttribute.DefaultValue(0)]
        public int IsGroup { get; set; }

        /// <summary>
        /// 削除区分
        ///     1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        //[Index("SET_MST_IDX01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
