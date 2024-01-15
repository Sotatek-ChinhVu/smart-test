using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 保険証スキャン情報
    /// </summary>
    [Table("pt_hoken_scan")]
    [Index(nameof(HpId), nameof(PtId), nameof(HokenGrp), nameof(HokenId), Name = "pt_hoken_scan_idx01")]
    [Index(nameof(HpId), nameof(PtId), nameof(HokenGrp), nameof(HokenId), nameof(SeqNo), nameof(IsDeleted), Name = "pt_hoken_scan_pkey")]
    public class PtHokenScan : EmrCloneable<PtHokenScan>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号			
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>			
        /// 保険グループ
        ///     1：主保険・労災・自賠 
        ///     2：公費
        /// </summary>
        
        [Column("hoken_grp", Order = 3)]
        public int HokenGrp { get; set; }

        /// <summary>
        /// 保険グループ
        ///     患者別に保険情報を識別するための固有の番号
        /// </summary
        
        [Column("hoken_id", Order = 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番 
        /// </summary
        
        [Column("seq_no", Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary
        [Column("file_name")]
        [MaxLength(100)]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary
        [Column("update_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        ///更新端末
        /// </summary
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
