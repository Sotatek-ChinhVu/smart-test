using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "bui_odr_byomei_mst")]
    public class BuiOdrByomeiMst : EmrCloneable<BuiOdrByomeiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// BUI_ODR_MST.HP_ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 部位ID
        /// BUI_ODR_MST.BUI_ID
        /// </summary>
        
        [Column("bui_id", Order = 2)]
        public int BuiId { get; set; }

        /// <summary>
        /// 病名部位
        /// 病名に登録された部位
        /// </summary>
        
        [Column("byomei_bui", Order = 3)]
        [MaxLength(100)]
        public string ByomeiBui { get; set; } = string.Empty;

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
