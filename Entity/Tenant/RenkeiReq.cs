using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "renkei_req")]
    public class RenkeiReq : EmrCloneable<RenkeiReq>
    {
        /// <summary>
        /// 要求ID
        /// 
        /// </summary>
        
        [Column("req_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReqId { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("raiin_no")]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 要求種別
        /// 1:患者情報　2:来院情報　3:クリア　4:診察中　5:診察済み
        /// </summary>
        [Column("req_sbt")]
        public int ReqSbt { get; set; }

        /// <summary>
        /// 更新タイプ
        /// I:追加　U:更新　D:削除
        /// </summary>
        [Column("req_type")]
        [MaxLength(2)]
        public string? ReqType { get; set; } = string.Empty;

        /// <summary>
        /// ステータス
        /// 0:未処理　1:処理中　8:エラー　9:正常終了
        /// </summary>
        [Column("status")]
        [CustomAttribute.DefaultValue(0)]
        public int Status { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// 
        /// </summary>
        [Column("err_mst")]
        [MaxLength(120)]
        public string? ErrMst { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
