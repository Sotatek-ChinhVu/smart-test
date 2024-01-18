using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 受付種別マスタ
    /// </summary>
    [Table(name: "uketuke_sbt_mst")]
    public class UketukeSbtMst : EmrCloneable<UketukeSbtMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        //[Index("uketuke_sbt_mst_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 区分ID
        /// </summary>
        
        [Column("kbn_id", Order = 2)]
        //[Index("uketuke_sbt_mst_idx01", 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KbnId { get; set; }

        /// <summary>
        /// 区分名称
        /// </summary>
        [Column(name: "kbn_name")]
        [MaxLength(20)]
        [Required]
        public string? KbnName { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("uketuke_sbt_mst_idx01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

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