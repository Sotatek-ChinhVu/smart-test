using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "karte_filter_detail")]
    public class KarteFilterDetail : EmrCloneable<KarteFilterDetail>
    {
        /// <summary>
        /// 病院コード
        /// KARTE_FILTER_MST.HP_ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// KARTE_FILTER_MST.USER_ID
        /// </summary>
        
        [Column("user_id", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// フィルタID
        /// KARTE_FILTER_MST.FILTER_ID
        /// </summary>
        
        [Column("filter_id", Order = 3)]
        public long FilterId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        
        [Column("filter_item_cd", Order = 4)]
        public int FilterItemCd { get; set; }

        /// <summary>
        /// 枝番
        /// 
        /// </summary>
        
        [Column("filter_eda_no", Order = 5)]
        public int FilterEdaNo { get; set; }

        /// <summary>
        /// 設定値
        /// 
        /// </summary>
        [Column("val")]
        public int Val { get; set; }

        /// <summary>
        /// パラメータ
        /// 
        /// </summary>
        [Column("param")]
        [MaxLength(300)]
        public string? Param { get; set; } = string.Empty;
    }
}