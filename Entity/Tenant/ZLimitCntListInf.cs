using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_LIMIT_CNT_LIST_INF")]
    public class ZLimitCntListInf : EmrCloneable<ZLimitCntListInf>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 公費ID
        /// PT_KOHI.KOHI_ID
        /// </summary>
        [Column("KOHI_ID")]
        public int KohiId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        public int SeqNo { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 0:他院 1以上:自院
        /// </summary>
        [Column("HOKEN_PID")]
        public int HokenPid { get; set; }

        /// <summary>
        /// 計算順番
        /// 自院:診察日診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        [Column("SORT_KEY")]
        [MaxLength(61)]
        public string? SortKey { get; set; } = string.Empty;

        /// <summary>
        /// 来院番号
        /// 0:他院 1以上:自院
        /// </summary>
        [Column("OYA_RAIIN_NO")]
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("BIKO")]
        [MaxLength(200)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
