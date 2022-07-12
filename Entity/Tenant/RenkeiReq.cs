using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RENKEI_REQ")]
    public class RenkeiReq : EmrCloneable<RenkeiReq>
    {
        /// <summary>
        /// 要求ID
        /// 
        /// </summary>
        [Key]
        [Column("REQ_ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReqId { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("RAIIN_NO")]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 要求種別
        /// 1:患者情報　2:来院情報　3:クリア　4:診察中　5:診察済み
        /// </summary>
        [Column("REQ_SBT")]
        public int ReqSbt { get; set; }

        /// <summary>
        /// 更新タイプ
        /// I:追加　U:更新　D:削除
        /// </summary>
        [Column("REQ_TYPE")]
        [MaxLength(2)]
        public string ReqType { get; set; } = string.Empty;

        /// <summary>
        /// ステータス
        /// 0:未処理　1:処理中　8:エラー　9:正常終了
        /// </summary>
        [Column("STATUS")]
        [CustomAttribute.DefaultValue(0)]
        public int Status { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// 
        /// </summary>
        [Column("ERR_MST")]
        [MaxLength(120)]
        public string ErrMst { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }  = string.Empty;

    }
}
