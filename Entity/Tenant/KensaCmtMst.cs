using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name:"kensa_cmt_mst")]
    [Serializable]
    [Index(nameof(HpId), nameof(CmtCd), nameof(CmtSeqNo), nameof(IsDeleted), Name = "kensa_cmt_mst_skey1")]
    public class KensaCmtMst
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        [MaxLength(2)]
        public int HpId { get; set; }

        /// <summary>
        /// コメントコード
        /// 
        /// </summary>

        [MaxLength(3)]
        [Column("cmt_cd", Order = 2)]
        public string? CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cmt_seq_no", Order = 3)]
        [MaxLength(9)]
        public int CmtSeqNo { get; set; }

        /// <summary>
        /// 結果コメント
        /// 
        /// </summary>
        [Column("cmt")]
        [MaxLength(100)]
        public string? CMT { get; set; }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        [Column("center_cd")]
        [MaxLength(10)]
        public string? CenterCd { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(1)]
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
        [MaxLength(8)]
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
        [MaxLength(8)]
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
