using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// セット区分マスタ
	/// </summary>
    [Table(name: "SET_KBN_MST")]
    public class SetKbnMst : EmrCloneable<SetKbnMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セット区分
        /// 
        /// </summary>
        //[Key]
        [Column("SET_KBN", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbn { get; set; }

        /// <summary>
        /// セット区分枝番
        /// 
        /// </summary>
        //[Key]
        [Column("SET_KBN_EDA_NO", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SetKbnEdaNo { get; set; }

        /// <summary>
        /// 世代ID
        /// 
        /// </summary>
        //[Key]
        [Column("GENERATION_ID", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// セット区分名称
        ///    
        /// </summary>
        [Column("SET_KBN_NAME")]
        [MaxLength(60)]
        public string SetKbnName { get; set; } = string.Empty;

        /// <summary>
        /// 診療科コード
        ///     0: 共通
        /// </summary>
        [Column("KA_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int KaCd { get; set; }

        /// <summary>
        /// 医師コード
        ///      0: 共通
        /// </summary>
        [Column("DOC_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int DocCd { get; set; }

        /// <summary>
        /// 削除区分
        ///     1: 削除
        /// </summary>
        [Column("IS_DELETED")]
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
        public string CreateMachine { get; set; } = string.Empty;

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
        public string UpdateMachine { get; set; }  = string.Empty;

    }
}
