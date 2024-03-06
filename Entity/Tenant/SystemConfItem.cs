using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "system_conf_item")]
    public class SystemConfItem : EmrCloneable<SystemConfItem>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// メニューID
        /// 
        /// </summary>
        
        [Column("menu_id", Order = 2)]
        public int MenuId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 項目名
        /// プルダウンメニュー項目
        /// </summary>
        [Column("item_name")]
        [MaxLength(100)]
        public string? ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 設定値
        /// プルダウンメニュー設定値
        /// </summary>
        [Column("val")]
        [CustomAttribute.DefaultValue(0)]
        public int Val { get; set; }

        /// <summary>
        /// パラメータ－最小値
        /// PARAM_MIN=0 and PARAM_MAX=0の場合はチェックしない
        /// </summary>
        [Column("param_min")]
        [CustomAttribute.DefaultValue(0)]
        public int ParamMin { get; set; }

        /// <summary>
        /// パラメータ－最大値
        /// PARAM_MIN=0 and PARAM_MAX=0の場合はチェックしない
        /// </summary>
        [Column("param_max")]
        [CustomAttribute.DefaultValue(0)]
        public int ParamMax { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        /// 更新者
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
