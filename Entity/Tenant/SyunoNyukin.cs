using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "syuno_nyukin")]
    public class SyunoNyukin : EmrCloneable<SyunoNyukin>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("syuno_nyukin_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        //[Index("syuno_nyukin_idx01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
        //[Index("syuno_nyukin_idx01", 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("raiin_no", Order = 2)]
        //[Index("syuno_nyukin_idx01", 4)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 入金順番
        /// 同一来院に対して分割入金した場合の入金の順番
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 調整額
        /// 
        /// </summary>
        [Column("adjust_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutan { get; set; }

        /// <summary>
        /// 入金額
        /// 
        /// </summary>
        [Column("nyukin_gaku")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinGaku { get; set; }

        /// <summary>
        /// 支払方法コード
        /// PAYMENT_METHOD_MST.PAYMENT_METHOD_CD
        /// </summary>
        [Column("payment_method_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int PaymentMethodCd { get; set; }

        /// <summary>
        /// 入金日
        /// 
        /// </summary>
        [Column("nyukin_date")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinDate { get; set; }

        /// <summary>
        /// 受付種別
        /// UKETUKE_SBT_MST.KBN_ID（入金時の受付種別）
        /// </summary>
        [Column("uketuke_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 入金コメント
        /// 
        /// </summary>
        [Column("nyukin_cmt")]
        [MaxLength(100)]
        public string? NyukinCmt { get; set; } = string.Empty;

        /// <summary>
        /// 入金時請求点数
        /// 入金時の診療点数
        /// </summary>
        [Column("nyukinji_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinjiTensu { get; set; }

        /// <summary>
        /// 入金時請求額
        /// 入金時の請求額
        /// </summary>
        [Column("nyukinji_seikyu")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinjiSeikyu { get; set; }

        /// <summary>
        /// 入金時請求詳細
        /// 入金時の診療明細
        /// </summary>
        [Column("nyukinji_detail")]
        public string? NyukinjiDetail { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("syuno_nyukin_idx01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
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
        [CustomAttribute.DefaultValue(0)]
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
