using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 特記事項マスタ
    /// </summary>
    [Table(name: "tokki_mst")]
    public class TokkiMst : EmrCloneable<TokkiMst>
    {
        /// <summary>
        /// 特記事項コード
        /// </summary>

        [Column(name: "tokki_cd")]
        //[Index("tokki_mst_idx01", 2)]
        [MaxLength(2)]
        public string TokkiCd { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項名
        /// </summary>
        [Column(name: "tokki_name")]
        [MaxLength(20)]
        [Required]
        public string? TokkiName { get; set; } = string.Empty;

        /// <summary>
        /// 使用開始日
        /// </summary>
        [Column(name: "start_date")]
        //[Index("tokki_mst_idx01", 3)]
        public int StartDate { get; set; }

        /// <summary>
        /// 使用終了日
        /// </summary>
        [Column(name: "end_date")]
        //[Index("tokki_mst_idx01", 4)]
        public int EndDate { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}