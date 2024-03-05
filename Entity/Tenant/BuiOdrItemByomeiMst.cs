using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "bui_odr_item_byomei_mst")]
    public class BuiOdrItemByomeiMst : EmrCloneable<BuiOdrItemByomeiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// BUI_ODR_ITEM_MST.HP_ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療行為コード
        /// BUI_ODR_ITEM_MST.ITEM_CD
        /// </summary>
        
        [Column("item_cd", Order = 2)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 病名部位
        /// 病名に登録された部位
        /// </summary>
        
        [Column("byomei_bui", Order = 3)]
        [MaxLength(100)]
        public string ByomeiBui { get; set; } = string.Empty;

        /// <summary>
        /// 左右区分
        /// 1: 左・右をチェックする
        /// </summary>
        [Column("lr_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int LrKbn { get; set; }

        /// <summary>
        /// 両区分
        /// 1: 両（左右・右左）をチェックする
        /// </summary>
        [Column("both_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int BothKbn { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
