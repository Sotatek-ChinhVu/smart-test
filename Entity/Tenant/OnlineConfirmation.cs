using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("ONLINE_CONFIRMATION")]
    public class OnlineConfirmation : EmrCloneable<OnlineConfirmation>
    {
        /// <summary>
        /// 受付番号
        /// オンライン資格確認システムが払い出す受付番号
        /// </summary>
        
        [Column("RECEPTION_NO")]
        public string ReceptionNo { get; set; } = string.Empty;

        /// <summary>
        /// 受付日時
        /// オンライン資格確認システムが払い出す受付日時
        /// </summary>
        [Column("RECEPTION_DATETIME")]
        public DateTime ReceptionDateTime { get; set; }

        /// <summary>
        /// 予約日			
        /// </summary>
        [Column("YOYAKU_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int YoyakuDate { get; set; }

        /// <summary>
        /// 処理結果区分	
        /// 0 or 空:未処理、1:正常終了、2:処理中、9:異常終了
        /// </summary>
        [Column("SEGMENT_OF_RESULT")]
        [MaxLength(1)]
        public string? SegmentOfResult { get; set; } = string.Empty;

        /// <summary>
        /// エラーメッセージ			
        /// </summary>
        [Column("ERROR_MESSAGE")]
        [MaxLength(60)]
        public string? ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
