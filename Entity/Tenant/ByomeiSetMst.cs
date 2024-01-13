using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "byomei_set_mst")]
    public class ByomeiSetMst : EmrCloneable<ByomeiSetMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 世代ID
        /// BYOMEI_SET_GENERATION_MST.GENERATION_ID
        /// </summary>
        
        [Column("generation_id", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// レベル１
        /// 
        /// </summary>
        [Column("level1")]
        public int Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// 
        /// </summary>
        [Column("level2")]
        public int Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// 
        /// </summary>
        [Column("level3")]
        public int Level3 { get; set; }

        /// <summary>
        /// レベル４
        /// 
        /// </summary>
        [Column("level4")]
        public int Level4 { get; set; }

        /// <summary>
        /// レベル５
        /// 
        /// </summary>
        [Column("level5")]
        public int Level5 { get; set; }

        /// <summary>
        /// 傷病名コード
        /// 
        /// </summary>
        [Column("byomei_cd")]
        [MaxLength(7)]
        public string? ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// 
        /// </summary>
        [Column("set_name")]
        [MaxLength(60)]
        public string? SetName { get; set; } = string.Empty;

        /// <summary>
        /// タイトル
        /// 1:タイトル
        /// </summary>
        [Column("is_title")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTitle { get; set; }

        /// <summary>
        /// 選択方式
        /// "0:選択不可 
        /// 1:選択可能 2:親も選択 3:子も選択 
        /// 4:親子両方選択"
        /// </summary>
        [Column("select_type")]
        public int SelectType { get; set; }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
