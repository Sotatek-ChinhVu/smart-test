using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "system_change_log")]
    public class SystemChangeLog : EmrCloneable<SystemChangeLog>
    {
        /// <summary>
        /// セットID
        /// 
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        [Column(name: "file_name")]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// Version
        /// </summary>
        [Column(name: "version")]
        public string? Version { get; set; } = string.Empty;

        /// <summary>
        /// IS_PG
        /// </summary>
        [Column(name: "is_pg")]
        public int IsPg { get; set; }

        /// <summary>
        /// IS_DB
        /// </summary>
        [Column(name: "is_db")]
        public int IsDb { get; set; }

        /// <summary>
        /// IS_MASTER
        /// </summary>
        [Column(name: "is_master")]
        public int IsMaster { get; set; }

        /// <summary>
        /// IS_RUN
        /// </summary>
        [Column(name: "is_run")]
        public int IsRun { get; set; }

        /// <summary>
        /// IS_NOTE
        /// </summary>
        [Column(name: "is_note")]
        public int IsNote { get; set; }

        /// <summary>
        /// IS_DRUG_PHOTO
        /// </summary>
        [Column(name: "is_drug_photo")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDrugPhoto { get; set; }

        /// <summary>
        /// STATUS
        /// </summary>
        [Column(name: "status")]
        public int Status { get; set; }

        /// <summary>
        /// ERR MESSAGE
        /// </summary>
        [Column(name: "err_message")]
        public string? ErrMessage { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
