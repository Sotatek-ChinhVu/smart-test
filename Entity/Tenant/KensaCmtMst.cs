using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name:"KENSA_CMT_MST")]
    [Serializable]
    [Index(nameof(HpId), nameof(CmtCd), nameof(CmtSeqNo), nameof(IsDeleted), Name = "KENSA_CMT_MST_SKEY1")]
    public class KensaCmtMst
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        [MaxLength(2)]
        public int HpId { get; set; }

        /// <summary>
        /// コメントコード
        /// 
        /// </summary>

        [MaxLength(3)]
        [Column("CMT_CD", Order = 2)]
        public string? CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CMT_SEQ_NO", Order = 3)]
        [MaxLength(9)]
        public int CmtSeqNo { get; set; }

        /// <summary>
        /// 結果コメント
        /// 
        /// </summary>
        [Column("CMT")]
        [MaxLength(100)]
        public string? CMT { get; set; }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        [Column("CENTER_CD")]
        [MaxLength(10)]
        public string? CenterCd { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(1)]
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
        [MaxLength(8)]
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
        [MaxLength(8)]
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
