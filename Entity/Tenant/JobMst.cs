using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 職種マスタ
    /// 
    /// </summary>
    [Table(name: "JOB_MST")]
    public class JobMst : EmrCloneable<JobMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 職種コード
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("JOB_CD", Order = 2)]
        public int JobCd { get; set; }

        /// <summary>
        /// 職種名
        /// </summary>
        [Column("JOB_NAME")]
        [MaxLength(10)]
        public string? JobName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("CREATE_ID")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("UPDATE_ID")]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
