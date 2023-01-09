using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RECE_CHECK_ERR")]
    public class ReceCheckErr : EmrCloneable<ReceCheckErr>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        
        [Column("HOKEN_ID", Order = 3)]
        public int HokenId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("SIN_YM", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// エラーコード
        /// 
        /// </summary>
        
        [Column("ERR_CD", Order = 5)]
        [MaxLength(5)]
        public string ErrCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("SIN_DATE", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// Aコード
        /// 
        /// </summary>
        
        [Column("A_CD", Order = 7)]
        [MaxLength(100)]
        public string ACd { get; set; } = string.Empty;

        /// <summary>
        /// Bコード
        /// 
        /// </summary>
        
        [Column("B_CD", Order = 8)]
        [MaxLength(100)]
        public string BCd { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ１
        /// 
        /// </summary>
        [Column("MESSAGE_1")]
        [MaxLength(100)]
        public string? Message1 { get; set; } = string.Empty;

        /// <summary>
        /// メッセージ２
        /// 
        /// </summary>
        [Column("MESSAGE_2")]
        [MaxLength(100)]
        public string? Message2 { get; set; } = string.Empty;

        /// <summary>
        /// チェックフラグ
        /// 1:確認済み
        /// </summary>
        [Column("IS_CHECKED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsChecked { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
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
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
