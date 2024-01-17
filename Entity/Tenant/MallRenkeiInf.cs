using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "mall_renkei_inf")]
    public class MallRenkeiInf : EmrCloneable<MallRenkeiInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        
        [Column("raiin_no", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Column("pt_id")]
        [CustomAttribute.DefaultValue(0)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        [Column("sin_date")]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 診察番号
        /// </summary>
        [Column("sinsatu_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SinsatuNo { get; set; }

        /// <summary>
        /// 会計番号
        /// </summary>
        [Column("kaikei_no")]
        [CustomAttribute.DefaultValue(0)]
        public int KaikeiNo { get; set; }

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
        /// 会計情報送信フラグ
        /// 0:未送信、1:送信済み
        /// </summary>
        [Column("send_flg")]
        [CustomAttribute.DefaultValue(0)]
        public int SendFlg { get; set; }

        /// <summary>
        /// クリニックコード
        /// </summary>
        [Column("clinic_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int ClinicCd { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
