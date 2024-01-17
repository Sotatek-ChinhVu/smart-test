using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "limit_cnt_list_inf")]
    public class LimitCntListInf : EmrCloneable<LimitCntListInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 公費ID
        /// PT_KOHI.KOHI_ID
        /// </summary>
        
        [Column("kohi_id", Order = 3)]
        public int KohiId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("sin_date", Order = 4)]
        public int SinDate { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 0:他院 1以上:自院
        /// </summary>
        [Column("hoken_pid")]
        public int HokenPid { get; set; }

        /// <summary>
        /// 計算順番
        /// 自院:診察日診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        [Column("sort_key")]
        [MaxLength(61)]
        public string? SortKey { get; set; } = string.Empty;

        /// <summary>
        /// 来院番号
        /// 0:他院 1以上:自院
        /// </summary>
        [Column("oya_raiin_no")]
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("biko")]
        [MaxLength(200)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
