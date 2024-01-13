using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "tekiou_byomei_mst")]
    public class TekiouByomeiMst : EmrCloneable<TekiouByomeiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        
        [Column("item_cd", Order = 2)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 病名コード
        /// 
        /// </summary>
        
        [Column("byomei_cd", Order = 3)]
        [MaxLength(7)]
        public string ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// システム管理データ
        /// 1:配信したマスタ
        /// </summary>
        
        [Column("system_data", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int SystemData { get; set; }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        [Column("start_ym")]
        [CustomAttribute.DefaultValue(0)]
        public int StartYM { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("end_ym")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndYM { get; set; }

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("is_invalid")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 特処無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("is_invalid_tokusyo")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalidTokusyo { get; set; }

        /// <summary>
        /// 編集区分
        /// 1:ユーザーが編集したデータ
        /// </summary>
        [Column("edit_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int EditKbn { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
