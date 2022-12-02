using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "BUI_ODR_ITEM_BYOMEI_MST")]
    public class BuiOdrItemByomeiMst : EmrCloneable<BuiOdrItemByomeiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// BUI_ODR_ITEM_MST.HP_ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療行為コード
        /// BUI_ODR_ITEM_MST.ITEM_CD
        /// </summary>
        
        [Column("ITEM_CD", Order = 2)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 病名部位
        /// 病名に登録された部位
        /// </summary>
        
        [Column("BYOMEI_BUI", Order = 3)]
        [MaxLength(100)]
        public string ByomeiBui { get; set; } = string.Empty;

        /// <summary>
        /// 左右区分
        /// 1: 左・右をチェックする
        /// </summary>
        [Column("LR_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int LrKbn { get; set; }

        /// <summary>
        /// 両区分
        /// 1: 両（左右・右左）をチェックする
        /// </summary>
        [Column("BOTH_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int BothKbn { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
