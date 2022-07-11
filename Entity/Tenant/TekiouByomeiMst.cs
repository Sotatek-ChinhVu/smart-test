using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "TEKIOU_BYOMEI_MST")]
    public class TekiouByomeiMst : EmrCloneable<TekiouByomeiMst>
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
        /// 項目コード
        /// 
        /// </summary>
        //[Key]
        [Column("ITEM_CD", Order = 2)]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 病名コード
        /// 
        /// </summary>
        //[Key]
        [Column("BYOMEI_CD", Order = 3)]
        [MaxLength(7)]
        public string ByomeiCd { get; set; }

        /// <summary>
        /// システム管理データ
        /// 1:配信したマスタ
        /// </summary>
        //[Key]
        [Column("SYSTEM_DATA", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int SystemData { get; set; }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        [Column("START_YM")]
        [CustomAttribute.DefaultValue(0)]
        public int StartYM { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("END_YM")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndYM { get; set; }

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 特処無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("IS_INVALID_TOKUSYO")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalidTokusyo { get; set; }

        /// <summary>
        /// 編集区分
        /// 1:ユーザーが編集したデータ
        /// </summary>
        [Column("EDIT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int EditKbn { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
