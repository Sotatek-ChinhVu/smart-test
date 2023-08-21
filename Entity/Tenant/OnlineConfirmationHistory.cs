﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("ONLINE_CONFIRMATION_HISTORY")]
    [Index(nameof(ID), Name = "ONLINE_CONFIRMATION_HISTORY_IDX01")]
    public class OnlineConfirmationHistory : EmrCloneable<OnlineConfirmationHistory>
    {
        /// <summary>
        /// 資格確認履歴番号
        /// </summary>

        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        /// <summary>
        /// 患者番号
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 資格確認日時	
        /// </summary>
        [Column("ONLINE_CONFIRMATION_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime OnlineConfirmationDate { get; set; }

        [Column("INFO_CONS_FLG")]
        [MaxLength(10)]
        public string? InfoConsFlg { get; set; }

        /// <summary>
        /// 資格確認種別
        ///     1 資格確認結果
        ///     2 特定健診情報閲覧結果
        ///     3 薬剤情報閲覧結果"										
        /// </summary>
        [Column("CONFIRMATION_TYPE")]
        public int ConfirmationType { get; set; }

        /// <summary>
        /// PRESCRIPTION_ISSUE_TYPE
        /// </summary>
        [Column("PRESCRIPTION_ISSUE_TYPE")]
        [CustomAttribute.DefaultValue(0)]
        public int PrescriptionIssueType { get; set; }

        /// <summary>
        /// 資格確認結果
        /// </summary>
        [Column("CONFIRMATION_RESULT")]
        public string? ConfirmationResult { get; set; } = string.Empty;

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 登録者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 登録端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// UKETUKE_STATUS
        /// </summary>
        [Column("UKETUKE_STATUS")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeStatus { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("UPDATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 登録者		
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 登録端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }
    }
}