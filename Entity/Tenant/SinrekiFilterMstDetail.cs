using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sinreki_filter_mst_detail")]
    public class SinrekiFilterMstDetail : EmrCloneable<SinrekiFilterMstDetail>
    {
        /// <summary>
        /// Id
        /// </summary>
        
        [Column(name: "id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類コード
        /// 
        /// </summary>
        
        [Column("grp_cd", Order = 3)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 順番
        /// 
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_exclude")]
        [CustomAttribute.DefaultValue(0)]
        public int IsExclude { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

    }
}
