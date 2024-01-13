using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rece_check_err")]
    public class ReceCheckErr : EmrCloneable<ReceCheckErr>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        
        [Column("hoken_id", Order = 3)]
        public int HokenId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// エラーコード
        /// 
        /// </summary>
        
        [Column("err_cd", Order = 5)]
        [MaxLength(5)]
        public string ErrCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("sin_date", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// Aコード
        /// 
        /// </summary>
        
        [Column("a_cd", Order = 7)]
        [MaxLength(100)]
        public string ACd { get; set; } = string.Empty;

        /// <summary>
        /// Bコード
        /// 
        /// </summary>
        
        [Column("b_cd", Order = 8)]
        [MaxLength(100)]
        public string BCd { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ１
        /// 
        /// </summary>
        [Column("message_1")]
        [MaxLength(100)]
        public string? Message1 { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ２
        /// 
        /// </summary>
        [Column("message_2")]
        [MaxLength(100)]
        public string? Message2 { get; set; } = string.Empty;

        /// <summary>
        /// チェックフラグ
        /// 1:確認済み
        /// </summary>
        [Column("is_checked")]
        [CustomAttribute.DefaultValue(0)]
        public int IsChecked { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
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
