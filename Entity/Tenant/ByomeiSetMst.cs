using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "BYOMEI_SET_MST")]
    public class ByomeiSetMst : EmrCloneable<ByomeiSetMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 世代ID
        /// BYOMEI_SET_GENERATION_MST.GENERATION_ID
        /// </summary>
        //[Key]
        [Column("GENERATION_ID", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// レベル１
        /// 
        /// </summary>
        [Column("LEVEL1")]
        public int Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// 
        /// </summary>
        [Column("LEVEL2")]
        public int Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// 
        /// </summary>
        [Column("LEVEL3")]
        public int Level3 { get; set; }

        /// <summary>
        /// レベル４
        /// 
        /// </summary>
        [Column("LEVEL4")]
        public int Level4 { get; set; }

        /// <summary>
        /// レベル５
        /// 
        /// </summary>
        [Column("LEVEL5")]
        public int Level5 { get; set; }

        /// <summary>
        /// 傷病名コード
        /// 
        /// </summary>
        [Column("BYOMEI_CD")]
        [MaxLength(7)]
        public string ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// 
        /// </summary>
        [Column("SET_NAME")]
        [MaxLength(60)]
        public string SetName { get; set; } = string.Empty;

        /// <summary>
        /// タイトル
        /// 1:タイトル
        /// </summary>
        [Column("IS_TITLE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTitle { get; set; }

        /// <summary>
        /// 選択方式
        /// "0:選択不可 
        /// 1:選択可能 2:親も選択 3:子も選択 
        /// 4:親子両方選択"
        /// </summary>
        [Column("SELECT_TYPE")]
        public int SelectType { get; set; }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }  = string.Empty;

    }
}
