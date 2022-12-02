using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "UNIT_MST")]
    public class UnitMst : EmrCloneable<UnitMst>
    {
        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        [Key]
        [Column("UNIT_CD", Order = 1)]
        public int UnitCd { get; set; }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        [Column("UNIT_NAME")]
        [MaxLength(40)]
        public string? UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 使用開始日
        /// 
        /// </summary>
        [Column("START_DATE")]
        public int StartDate { get; set; }

        /// <summary>
        /// 使用終了日
        /// 
        /// </summary>
        [Column("END_DATE")]
        public int EndDate { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
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
        /// 更新者ID
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
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
