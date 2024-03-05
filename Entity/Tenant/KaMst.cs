using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 診療科マスタ
    /// </summary>
    [Table(name: "ka_mst")]
    [Index(nameof(KaId), Name = "pt_ka_mst_idx01")]
    public class KaMst : EmrCloneable<KaMst>
    {
        /// <summary>
        /// Id
        /// </summary>
        
        [Column(name: "id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療科ID
        /// </summary>

        [Column("ka_id")]
        public int KaId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// レセ診療科コード
        /// </summary>
        [Column(name: "rece_ka_cd")]
        [MaxLength(2)]
        public string? ReceKaCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療科略称
        /// </summary>
        [Column(name: "ka_sname")]
        [MaxLength(20)]
        public string? KaSname { get; set; } = string.Empty;

        /// <summary>
        /// 診療科名称
        /// </summary>
        [Column(name: "ka_name")]
        [MaxLength(40)]
        public string? KaName { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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

        /// <summary>
        /// 様式診療科コード
        /// </summary>
        [Column(name: "yousiki_ka_cd")]
        [MaxLength(3)]
        public string? YousikiKaCd { get; set; } = string.Empty;
    }
}