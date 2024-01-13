using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("online_confirmation_history")]
    [Index(nameof(ID), Name = "online_confirmation_history_idx01")]
    public class OnlineConfirmationHistory : EmrCloneable<OnlineConfirmationHistory>
    {
        /// <summary>
        /// 資格確認履歴番号
        /// </summary>

        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        /// <summary>
        /// 患者番号
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 資格確認日時	
        /// </summary>
        [Column("online_confirmation_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime OnlineConfirmationDate { get; set; }

        [Column("info_cons_flg")]
        [MaxLength(10)]
        public string? InfoConsFlg { get; set; }

        /// <summary>
        /// 資格確認種別
        ///     1 資格確認結果
        ///     2 特定健診情報閲覧結果
        ///     3 薬剤情報閲覧結果"										
        /// </summary>
        [Column("confirmation_type")]
        public int ConfirmationType { get; set; }

        /// <summary>
        /// PRESCRIPTION_ISSUE_TYPE
        /// </summary>
        [Column("prescription_issue_type")]
        [CustomAttribute.DefaultValue(0)]
        public int PrescriptionIssueType { get; set; }

        /// <summary>
        /// 資格確認結果
        /// </summary>
        [Column("confirmation_result")]
        public string? ConfirmationResult { get; set; } = string.Empty;

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 登録者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 登録端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// UKETUKE_STATUS
        /// </summary>
        [Column("uketuke_status")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeStatus { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("update_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 登録者		
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 登録端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }
    }
}