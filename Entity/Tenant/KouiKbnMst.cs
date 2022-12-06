using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 行為区分マスタ
    /// </summary>
    [Table(name: "KOUI_KBN_MST")]
    public class KouiKbnMst
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 行為区分ID
        /// </summary>
        
        [Column("KOUI_KBN_ID", Order = 2)]
        public int KouiKbnId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Required]
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 行為区分１
        /// </summary>
        [Required]
        [Column("KOUI_KBN1")]
        public int KouiKbn1 { get; set; }

        /// <summary>
        /// 行為区分２
        /// </summary>
        [Required]
        [Column("KOUI_KBN2")]
        public int KouiKbn2 { get; set; }

        /// <summary>
        /// 行為グループ名
        /// </summary>
        [Column("KOUI_GRP_NAME")]
        [MaxLength(20)]
        public string? KouiGrpName { get; set; } = string.Empty;

        /// <summary>
        /// 行為グループ名
        /// </summary>
        [Required]
        [Column("KOUI_NAME")]
        [MaxLength(20)]
        public string? KouiName { get; set; } = string.Empty;

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
    }
}