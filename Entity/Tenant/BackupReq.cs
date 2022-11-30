using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "BACKUP_REQ")]
    public class BackupReq : EmrCloneable<BackupReq>
    {
        /// <summary>
        /// ID
        /// 
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 出力帳票区分
        /// 0-カルテ2号紙
        /// </summary>
        [Column("OUTPUT_TYPE")]
        public int OutputType { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 出力対象診療日FROM
        /// yyyyMMdd
        /// </summary>
        [Column("FROM_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int FromDate { get; set; }

        /// <summary>
        /// 出力対象診療日TO
        /// yyyyMMdd
        /// </summary>
        [Column("TO_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int ToDate { get; set; }

        /// <summary>
        /// ステータス
        /// 0-未処理、1-処理中、8-異常終了、9-正常終了
        /// </summary>
        [Column("STATUS")]
        [CustomAttribute.DefaultValue(0)]
        public int Status { get; set; }

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
