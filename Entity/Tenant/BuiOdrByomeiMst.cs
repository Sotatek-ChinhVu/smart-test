using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "BUI_ODR_BYOMEI_MST")]
    public class BuiOdrByomeiMst : EmrCloneable<BuiOdrByomeiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// BUI_ODR_MST.HP_ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 部位ID
        /// BUI_ODR_MST.BUI_ID
        /// </summary>
        //[Key]
        [Column("BUI_ID", Order = 2)]
        public int BuiId { get; set; }

        /// <summary>
        /// 病名部位
        /// 病名に登録された部位
        /// </summary>
        //[Key]
        [Column("BYOMEI_BUI", Order = 3)]
        [MaxLength(100)]
        public string ByomeiBui { get; set; }

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
        public string UpdateMachine { get; set; }
    }
}
