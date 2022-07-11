using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KARTE_FILTER_DETAIL")]
    public class KarteFilterDetail : EmrCloneable<KarteFilterDetail>
    {
        /// <summary>
        /// 病院コード
        /// KARTE_FILTER_MST.HP_ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// KARTE_FILTER_MST.USER_ID
        /// </summary>
        //[Key]
        [Column("USER_ID", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// フィルタID
        /// KARTE_FILTER_MST.FILTER_ID
        /// </summary>
        //[Key]
        [Column("FILTER_ID", Order = 3)]
        public long FilterId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        //[Key]
        [Column("FILTER_ITEM_CD", Order = 4)]
        public int FilterItemCd { get; set; }

        /// <summary>
        /// 枝番
        /// 
        /// </summary>
        //[Key]
        [Column("FILTER_EDA_NO", Order = 5)]
        public int FilterEdaNo { get; set; }

        /// <summary>
        /// 設定値
        /// 
        /// </summary>
        [Column("VAL")]
        public int Val { get; set; }

        /// <summary>
        /// パラメータ
        /// 
        /// </summary>
        [Column("PARAM")]
        [MaxLength(300)]
        public string Param { get; set; }

    }
}