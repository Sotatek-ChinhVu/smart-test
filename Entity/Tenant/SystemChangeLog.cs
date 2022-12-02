using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SYSTEM_CHANGE_LOG")]
    public class SystemChangeLog : EmrCloneable<SystemChangeLog>
    {
        /// <summary>
        /// セットID
        /// 
        /// </summary>
        
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        [Column(name: "FILE_NAME")]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// Version
        /// </summary>
        [Column(name: "VERSION")]
        public string? Version { get; set; } = string.Empty;

        /// <summary>
        /// IS_PG
        /// </summary>
        [Column(name: "IS_PG")]
        public int IsPg { get; set; }

        /// <summary>
        /// IS_DB
        /// </summary>
        [Column(name: "IS_DB")]
        public int IsDb { get; set; }

        /// <summary>
        /// IS_MASTER
        /// </summary>
        [Column(name: "IS_MASTER")]
        public int IsMaster { get; set; }

        /// <summary>
        /// IS_RUN
        /// </summary>
        [Column(name: "IS_RUN")]
        public int IsRun { get; set; }

        /// <summary>
        /// IS_NOTE
        /// </summary>
        [Column(name: "IS_NOTE")]
        public int IsNote { get; set; }

        /// <summary>
        /// IS_DRUG_PHOTO
        /// </summary>
        [Column(name: "IS_DRUG_PHOTO")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDrugPhoto { get; set; }

        /// <summary>
        /// STATUS
        /// </summary>
        [Column(name: "STATUS")]
        public int Status { get; set; }

        /// <summary>
        /// ERR MESSAGE
        /// </summary>
        [Column(name: "ERR_MESSAGE")]
        public string? ErrMessage { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
