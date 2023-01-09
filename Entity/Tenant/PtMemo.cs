using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者メモ
    /// </summary>
    [Table(name: "PT_MEMO")]
    [Index(nameof(HpId), nameof(PtId), nameof(IsDeleted), Name = "PT_MEMO_IDX01")]
    public class PtMemo : EmrCloneable<PtMemo>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号						
        /// </summary>
        
        [Column(name: "PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column(name: "SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        [Column(name: "MEMO")]
        public string? Memo { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// </summary>
        [Column(name: "IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}