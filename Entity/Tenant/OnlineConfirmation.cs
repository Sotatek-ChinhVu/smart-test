using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("online_confirmation")]
    public class OnlineConfirmation : EmrCloneable<OnlineConfirmation>
    {
        /// <summary>
        /// 受付番号
        /// オンライン資格確認システムが払い出す受付番号
        /// </summary>
        
        [Column("reception_no")]
        public string ReceptionNo { get; set; } = string.Empty;

        /// <summary>
        /// 受付日時
        /// オンライン資格確認システムが払い出す受付日時
        /// </summary>
        [Column("reception_datetime")]
        public DateTime ReceptionDateTime { get; set; }

        /// <summary>
        /// 予約日			
        /// </summary>
        [Column("yoyaku_date")]
        [CustomAttribute.DefaultValue(0)]
        public int YoyakuDate { get; set; }

        /// <summary>
        /// 処理結果区分	
        /// 0 or 空:未処理、1:正常終了、2:処理中、9:異常終了
        /// </summary>
        [Column("segment_of_result")]
        [MaxLength(1)]
        public string? SegmentOfResult { get; set; } = string.Empty;

        /// <summary>
        /// エラーメッセージ			
        /// </summary>
        [Column("error_message")]
        [MaxLength(60)]
        public string? ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
