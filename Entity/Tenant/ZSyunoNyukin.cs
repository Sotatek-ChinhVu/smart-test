using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "z_syuno_nyukin")]
    [Index(nameof(HpId), nameof(PtId), nameof(SinDate), nameof(RaiinNo), nameof(IsDeleted), Name = "syuno_nyukin_idx01")]
    public class ZSyunoNyukin : EmrCloneable<ZSyunoNyukin>
    {
        
        [Column("op_id", Order = 1)]
        public long OpId { get; set; }

        [Column("op_type")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("op_time")]
        public DateTime OpTime { get; set; }

        [Column("op_addr")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("op_hostname")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("raiin_no")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

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
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
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
        public string? UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 入金日
        /// 
        /// </summary>
        [Column("nyukin_date")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinDate { get; set; }

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
    }
}
