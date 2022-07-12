using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 保険証スキャン情報
    /// </summary>
    [Table("PT_HOKEN_SCAN")]
    [Index(nameof(HpId), nameof(PtId), nameof(HokenGrp), nameof(HokenId), Name = "PT_HOKEN_SCAN_IDX01")]
    [Index(nameof(HpId), nameof(PtId), nameof(HokenGrp), nameof(HokenId), nameof(SeqNo), nameof(IsDeleted), Name = "PT_HOKEN_SCAN_PKEY")]
    public class PtHokenScan : EmrCloneable<PtHokenScan>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号			
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>			
        /// 保険グループ
        ///     1：主保険・労災・自賠 
        ///     2：公費
        /// </summary>
        //[Key]
        [Column("HOKEN_GRP", Order = 3)]
        public int HokenGrp { get; set; }

        /// <summary>
        /// 保険グループ
        ///     患者別に保険情報を識別するための固有の番号
        /// </summary
        //[Key]
        [Column("HOKEN_ID", Order = 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番 
        /// </summary
        //[Key]
        [Column("SEQ_NO", Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary
        [Column("FILE_NAME")]
        [MaxLength(100)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary
        [Column("UPDATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        ///更新端末
        /// </summary
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }  = string.Empty;
    }
}
