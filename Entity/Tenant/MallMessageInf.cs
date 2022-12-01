using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "MALL_MESSAGE_INF")]
    [Index(nameof(SinDate), Name = "MALL_MESSAGE_INF_IDX01")]
    public class MallMessageInf : EmrCloneable<MallMessageInf>
    {
        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 受信データ番号
        /// </summary>
        [Column("RECEIVE_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceiveNo { get; set; }

        /// <summary>
        /// 送信データ番号
        /// 診療日内で連番
        /// </summary>
        [Column("SEND_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SendNo { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        [Column("MESSAGE")]
        [MaxLength(400)]
        public string? Message { get; set; } = string.Empty;

        /// <summary>
        /// 日付
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
    }
}
