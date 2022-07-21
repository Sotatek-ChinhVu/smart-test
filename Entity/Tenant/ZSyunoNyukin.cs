using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_SYUNO_NYUKIN")]
    [Index(nameof(HpId), nameof(PtId), nameof(SinDate), nameof(RaiinNo), nameof(IsDeleted), Name = "SYUNO_NYUKIN_IDX01")]
    public class ZSyunoNyukin : EmrCloneable<ZSyunoNyukin>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("RAIIN_NO")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 入金順番
        /// 同一来院に対して分割入金した場合の入金の順番
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 調整額
        /// 
        /// </summary>
        [Column("ADJUST_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutan { get; set; }

        /// <summary>
        /// 入金額
        /// 
        /// </summary>
        [Column("NYUKIN_GAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinGaku { get; set; }

        /// <summary>
        /// 支払方法コード
        /// PAYMENT_METHOD_MST.PAYMENT_METHOD_CD
        /// </summary>
        [Column("PAYMENT_METHOD_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int PaymentMethodCd { get; set; }

        /// <summary>
        /// 受付種別
        /// UKETUKE_SBT_MST.KBN_ID（入金時の受付種別）
        /// </summary>
        [Column("UKETUKE_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 入金コメント
        /// 
        /// </summary>
        [Column("NYUKIN_CMT")]
        [MaxLength(100)]
        public string NyukinCmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

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
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 入金日
        /// 
        /// </summary>
        [Column("NYUKIN_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinDate { get; set; }

        /// <summary>
        /// 入金時請求点数
        /// 入金時の診療点数
        /// </summary>
        [Column("NYUKINJI_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinjiTensu { get; set; }

        /// <summary>
        /// 入金時請求額
        /// 入金時の請求額
        /// </summary>
        [Column("NYUKINJI_SEIKYU")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinjiSeikyu { get; set; }

        /// <summary>
        /// 入金時請求詳細
        /// 入金時の診療明細
        /// </summary>
        [Column("NYUKINJI_DETAIL")]
        public string NyukinjiDetail { get; set; } = string.Empty;
    }
}
