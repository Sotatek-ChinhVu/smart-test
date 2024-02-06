using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rsvkrt_mst")]
    public class RsvkrtMst : EmrCloneable<RsvkrtMst>
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
        /// 予約カルテ区分
        /// "0: 予約カルテ
        /// 1: 定期カルテ"
        /// </summary>
        [Column("rsvkrt_kbn")]
        public int RsvkrtKbn { get; set; }

        /// <summary>
        /// 予約日
        /// "yyyymmdd
        /// 予約日未指定、定期カルテは0"
        /// </summary>
        [Column("rsv_date")]
        public int RsvDate { get; set; }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        
        [Column("rsvkrt_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RsvkrtNo { get; set; }

        /// <summary>
        /// 予約名
        /// 予約名（定期カルテ用）
        /// </summary>
        [Column("rsv_name")]
        [MaxLength(120)]
        public string? RsvName { get; set; } = string.Empty;

        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        /// "1: 削除
        /// 2: 実施"
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
