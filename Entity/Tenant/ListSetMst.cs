using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    [Table(name: "LIST_SET_MST")]
    public class ListSetMst : EmrCloneable<ListSetMst>
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
        /// 
        /// </summary>
        [Key]
        [Column("GENERATION_ID", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// セット区分
        /// ○
        /// </summary>
        [Column("SET_KBN")]
        public int SetKbn { get; set; }

        /// <summary>
        /// セットID
        /// 
        /// </summary>
        [Key]
        [Column("SET_ID", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetId { get; set; }

        /// <summary>
        /// レベル１
        /// ○
        /// </summary>
        [Column("LEVEL1")]
        public int Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// ○
        /// </summary>
        [Column("LEVEL2")]
        public int Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// ○
        /// </summary>
        [Column("LEVEL3")]
        public int Level3 { get; set; }

        /// <summary>
        /// レベル４
        /// ○
        /// </summary>
        [Column("LEVEL4")]
        public int Level4 { get; set; }

        /// <summary>
        /// レベル５
        /// ○
        /// </summary>
        [Column("LEVEL5")]
        public int Level5 { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 名称
        /// 
        /// </summary>
        [Column("SET_NAME")]
        [MaxLength(240)]
        public string SetName { get; set; }

        /// <summary>
        /// タイトル
        /// 
        /// </summary>
        [Column("IS_TITLE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTitle { get; set; }

        /// <summary>
        /// 選択方式
        /// 
        /// </summary>
        [Column("SELECT_TYPE")]
        public int SelectType { get; set; }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        [Column("SURYO")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo { get; set; }

        /// <summary>
        /// 単位種別
        /// 
        /// </summary>
        [Column("UNIT_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int UnitSbt { get; set; }

        /// <summary>
        /// 至急区分
        /// 
        /// </summary>
        [Column("SIKYU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SikyuKbn { get; set; }

        /// <summary>
        /// コメント名称
        /// 
        /// </summary>
        [Column("CMT_NAME")]
        [MaxLength(240)]
        public string CmtName { get; set; }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        [Column("CMT_OPT")]
        [MaxLength(38)]
        public string CmtOpt { get; set; }

        /// <summary>
        /// 削除区分
        /// 
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
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }

    }
}
