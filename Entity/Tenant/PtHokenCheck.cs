using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("pt_hoken_check")]
    public class PtHokenCheck : EmrCloneable<PtHokenCheck>
    {
        /// <summary>
        /// 病院コード
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtID { get; set; }

        /// <summary>
        /// 保険グループ
        ///     1:主保険・労災・自賠
        ///     2:公費
        /// </summary>
        
        [Column("hoken_grp", Order = 3)]
        public int HokenGrp { get; set; }

        /// <summary>
        /// 保険ID
        /// 患者別に保険情報を識別するための固有の番号
        /// </summary>
        
        [Column("hoken_id", Order = 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 確認日時
        /// </summary>
        [Column("check_date")]
        public DateTime CheckDate { get; set; }

        /// <summary>
        /// 確認者コード
        /// </summary>
        [Column("check_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CheckId { get; set; }

        /// <summary>
        /// 確認端末
        /// </summary>
        [Column("check_machine")]
        [MaxLength(60)]
        public string? CheckMachine { get; set; } = string.Empty;

        /// <summary>
        /// 確認コメント
        /// </summary>
        [Column("check_cmt")]
        [MaxLength(100)]
        public string? CheckCmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}