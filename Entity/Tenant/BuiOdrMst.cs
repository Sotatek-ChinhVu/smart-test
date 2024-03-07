using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "bui_odr_mst")]
    public class BuiOdrMst : EmrCloneable<BuiOdrMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 部位ID
        /// </summary>
        
        [Column("bui_id", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuiId { get; set; }

        /// <summary>
        /// オーダー部位
        /// オーダー入力された部位名称
        /// </summary>
        [Column("odr_bui")]
        [MaxLength(100)]
        public string? OdrBui { get; set; } = string.Empty;

        /// <summary>
        /// 左右区分
        /// 1: 左・右をチェックする
        /// </summary>
        [Column("lr_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int LrKbn { get; set; }

        /// <summary>
        /// 左右必須区分
        /// 1: LR_KBN=1 AND BOTH_KBN=1の場合は左・右・両のいずれか必須とする					
        /// LR_KBN=1 AND BOTH_KBN = 0の場合は左・右のいずれか必須とする
        /// LR_KBN = 0 AND BOTH_KBN = 1の場合は両を必須とする
        /// </summary>
        [Column("must_lr_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int MustLrKbn { get; set; }

        /// <summary>
        /// 両区分
        /// 1: 両（左右・右左）をチェックする
        /// </summary>
        [Column("both_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int BothKbn { get; set; }

        /// <summary>
        /// 行為区分(注射)
        /// 1:チェックあり
        /// </summary>
        [Column("koui_30")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui30 { get; set; }

        /// <summary>
        /// 行為区分(処置)
        /// 1:チェックあり
        /// </summary>
        [Column("koui_40")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui40 { get; set; }

        /// <summary>
        /// 行為区分(手術)
        /// 1:チェックあり
        /// </summary>
        [Column("koui_50")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui50 { get; set; }

        /// <summary>
        /// 行為区分(検査)
        /// 1:チェックあり
        /// </summary>
        [Column("koui_60")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui60 { get; set; }

        /// <summary>
        /// 行為区分(画像)
        /// 1:チェックあり
        /// </summary>
        [Column("koui_70")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui70 { get; set; }

        /// <summary>
        /// 行為区分(その他)
        /// 1:チェックあり
        /// </summary>
        [Column("koui_80")]
        [CustomAttribute.DefaultValue(0)]
        public int Koui80 { get; set; }

        /// <summary>
        /// 更新日時
        /// 
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
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
