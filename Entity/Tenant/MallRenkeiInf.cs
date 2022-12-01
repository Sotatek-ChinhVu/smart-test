using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "MALL_RENKEI_INF")]
    public class MallRenkeiInf : EmrCloneable<MallRenkeiInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Column("PT_ID")]
        [CustomAttribute.DefaultValue(0)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        [Column("SIN_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 診察番号
        /// </summary>
        [Column("SINSATU_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SinsatuNo { get; set; }

        /// <summary>
        /// 会計番号
        /// </summary>
        [Column("KAIKEI_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int KaikeiNo { get; set; }

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
        /// 会計情報送信フラグ
        /// 0:未送信、1:送信済み
        /// </summary>
        [Column("SEND_FLG")]
        [CustomAttribute.DefaultValue(0)]
        public int SendFlg { get; set; }

        /// <summary>
        /// クリニックコード
        /// </summary>
        [Column("CLINIC_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int ClinicCd { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
