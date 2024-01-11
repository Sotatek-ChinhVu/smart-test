using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "mall_message_inf")]
    [Index(nameof(SinDate), Name = "mall_message_inf_idx01")]
    public class MallMessageInf : EmrCloneable<MallMessageInf>
    {
        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 受信データ番号
        /// </summary>
        [Column("receive_no")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceiveNo { get; set; }

        /// <summary>
        /// 送信データ番号
        /// 診療日内で連番
        /// </summary>
        [Column("send_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SendNo { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        [Column("message")]
        [MaxLength(400)]
        public string? Message { get; set; } = string.Empty;

        /// <summary>
        /// 日付
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
    }
}
