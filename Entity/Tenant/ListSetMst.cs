using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    [Table(name: "list_set_mst")]
    public class ListSetMst : EmrCloneable<ListSetMst>
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
        /// 
        /// </summary>
        
        [Column("generation_id", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int GenerationId { get; set; }

        /// <summary>
        /// セット区分
        /// ○
        /// </summary>
        [Column("set_kbn")]
        public int SetKbn { get; set; }

        /// <summary>
        /// セットID
        /// 
        /// </summary>
        
        [Column("set_id", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetId { get; set; }

        /// <summary>
        /// レベル１
        /// ○
        /// </summary>
        [Column("level1")]
        public int Level1 { get; set; }

        /// <summary>
        /// レベル２
        /// ○
        /// </summary>
        [Column("level2")]
        public int Level2 { get; set; }

        /// <summary>
        /// レベル３
        /// ○
        /// </summary>
        [Column("level3")]
        public int Level3 { get; set; }

        /// <summary>
        /// レベル４
        /// ○
        /// </summary>
        [Column("level4")]
        public int Level4 { get; set; }

        /// <summary>
        /// レベル５
        /// ○
        /// </summary>
        [Column("level5")]
        public int Level5 { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// 
        /// </summary>
        [Column("set_name")]
        [MaxLength(240)]
        public string? SetName { get; set; } = string.Empty;

        /// <summary>
        /// タイトル
        /// 
        /// </summary>
        [Column("is_title")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTitle { get; set; }

        /// <summary>
        /// 選択方式
        /// 
        /// </summary>
        [Column("select_type")]
        public int SelectType { get; set; }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        [Column("suryo")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo { get; set; }

        /// <summary>
        /// 単位種別
        /// 
        /// </summary>
        [Column("unit_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int UnitSbt { get; set; }

        /// <summary>
        /// 至急区分
        /// 
        /// </summary>
        [Column("sikyu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SikyuKbn { get; set; }

        /// <summary>
        /// コメント名称
        /// 
        /// </summary>
        [Column("cmt_name")]
        [MaxLength(240)]
        public string? CmtName { get; set; } = string.Empty;

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        [Column("cmt_opt")]
        [MaxLength(38)]
        public string? CmtOpt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 
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
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
