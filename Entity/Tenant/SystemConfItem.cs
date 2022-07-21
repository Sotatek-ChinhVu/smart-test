using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SYSTEM_CONF_ITEM")]
    public class SystemConfItem : EmrCloneable<SystemConfItem>
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
        /// メニューID
        /// 
        /// </summary>
        //[Key]
        [Column("MENU_ID", Order = 2)]
        public int MenuId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 項目名
        /// プルダウンメニュー項目
        /// </summary>
        [Column("ITEM_NAME")]
        [MaxLength(100)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 設定値
        /// プルダウンメニュー設定値
        /// </summary>
        [Column("VAL")]
        [CustomAttribute.DefaultValue(0)]
        public int Val { get; set; }

        /// <summary>
        /// パラメータ－最小値
        /// PARAM_MIN=0 and PARAM_MAX=0の場合はチェックしない
        /// </summary>
        [Column("PARAM_MIN")]
        [CustomAttribute.DefaultValue(0)]
        public int ParamMin { get; set; }

        /// <summary>
        /// パラメータ－最大値
        /// PARAM_MIN=0 and PARAM_MAX=0の場合はチェックしない
        /// </summary>
        [Column("PARAM_MAX")]
        [CustomAttribute.DefaultValue(0)]
        public int ParamMax { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
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
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
