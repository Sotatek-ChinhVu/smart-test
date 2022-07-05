using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "BUI_ODR_MST")]
    public class BuiOdrMst : EmrCloneable<BuiOdrMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 部位ID
        /// </summary>
        [Key]
        [Column("BUI_ID", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuiId { get; set; }

        /// <summary>
        /// オーダー部位
        /// オーダー入力された部位名称
        /// </summary>
        [Column("ODR_BUI")]
        [MaxLength(100)]
        public string OdrBui { get; set; }

        /// <summary>
        /// 左右区分
        /// 1: 左・右をチェックする
        /// </summary>
        [Column("LR_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int LrKbn { get; set; }

        /// <summary>
        /// 左右必須区分
        /// 1: LR_KBN=1 AND BOTH_KBN=1の場合は左・右・両のいずれか必須とする					
        /// LR_KBN=1 AND BOTH_KBN = 0の場合は左・右のいずれか必須とする
        /// LR_KBN = 0 AND BOTH_KBN = 1の場合は両を必須とする
        /// </summary>
        [Column("MUST_LR_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int MustLrKbn { get; set; }

        /// <summary>
        /// 両区分
        /// 1: 両（左右・右左）をチェックする
        /// </summary>
        [Column("BOTH_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int BothKbn { get; set; }

        /// <summary>
        /// 行為区分(注射)
        /// 1:チェックあり
        /// </summary>
        [Column("KOUI_30")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui30 { get; set; }

        /// <summary>
        /// 行為区分(処置)
        /// 1:チェックあり
        /// </summary>
        [Column("KOUI_40")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui40 { get; set; }

        /// <summary>
        /// 行為区分(手術)
        /// 1:チェックあり
        /// </summary>
        [Column("KOUI_50")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui50 { get; set; }

        /// <summary>
        /// 行為区分(検査)
        /// 1:チェックあり
        /// </summary>
        [Column("KOUI_60")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui60 { get; set; }

        /// <summary>
        /// 行為区分(画像)
        /// 1:チェックあり
        /// </summary>
        [Column("KOUI_70")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui70 { get; set; }

        /// <summary>
        /// 行為区分(その他)
        /// 1:チェックあり
        /// </summary>
        [Column("KOUI_80")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui80 { get; set; }

        /// <summary>
        /// 更新日時
        /// 
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
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }
    }
}
