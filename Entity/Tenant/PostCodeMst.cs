using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 郵便番号マスタ
    /// </summary>
    [Table(name: "post_code_mst")]
    [Index(nameof(HpId), nameof(PostCd), nameof(IsDeleted), Name = "pt_post_code_mst_idx01")]
    public class PostCodeMst : EmrCloneable<PostCodeMst>
    {
        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 郵便番号
        ///		1つの郵便番号で2以上の町域を表す場合あり
        /// </summary>
        [Column(name: "post_cd")]
        [MaxLength(7)]
        public string? PostCd { get; set; } = string.Empty;

        /// <summary>
        /// 都道府県名カナ
        /// </summary>
        [Column(name: "pref_kana")]
        [MaxLength(60)]
        public string? PrefKana { get; set; } = string.Empty;

        /// <summary>
        /// 市区町村名カナ
        /// </summary>
        [Column(name: "city_kana")]
        [MaxLength(60)]
        public string? CityKana { get; set; } = string.Empty;

        /// <summary>
        /// 町域名カナ
        /// </summary>
        [Column(name: "postal_term_kana")]
        [MaxLength(150)]
        public string? PostalTermKana { get; set; } = string.Empty;

        /// <summary>
        /// 都道府県名
        /// </summary>
        [Column(name: "pref_name")]
        [MaxLength(40)]
        public string? PrefName { get; set; } = string.Empty;


        /// <summary>
        /// 市区町村名
        /// </summary>
        [Column(name: "city_name")]
        [MaxLength(40)]
        public string? CityName { get; set; } = string.Empty;

        /// <summary>
        /// 町域名
        /// </summary>
        [Column(name: "banti")]
        [MaxLength(100)]
        public string? Banti { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除	
        /// </summary>
        [Column(name: "is_deleted")]
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
    }
}