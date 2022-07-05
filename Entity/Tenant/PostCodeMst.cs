using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 郵便番号マスタ
    /// </summary>
    [Table(name: "POST_CODE_MST")]
    [Index(nameof(HpId), nameof(PostCd), nameof(IsDeleted), Name = "PT_POST_CODE_MST_IDX01")]
    public class PostCodeMst : EmrCloneable<PostCodeMst>
    {
        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column(name: "HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 郵便番号
        ///		1つの郵便番号で2以上の町域を表す場合あり
        /// </summary>
        [Column(name: "POST_CD")]
        [MaxLength(7)]
        public string PostCd { get; set; }

        /// <summary>
        /// 都道府県名カナ
        /// </summary>
        [Column(name: "PREF_KANA")]
        [MaxLength(60)]
        public string PrefKana { get; set; }

        /// <summary>
        /// 市区町村名カナ
        /// </summary>
        [Column(name: "CITY_KANA")]
        [MaxLength(60)]
        public string CityKana { get; set; }

        /// <summary>
        /// 町域名カナ
        /// </summary>
        [Column(name: "POSTAL_TERM_KANA")]
        [MaxLength(150)]
        public string PostalTermKana { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        [Column(name: "PREF_NAME")]
        [MaxLength(40)]
        public string PrefName { get; set; }


        /// <summary>
        /// 市区町村名
        /// </summary>
        [Column(name: "CITY_NAME")]
        [MaxLength(40)]
        public string CityName { get; set; }

        /// <summary>
        /// 町域名
        /// </summary>
        [Column(name: "BANTI")]
        [MaxLength(100)]
        public string Banti { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除	
        /// </summary>
        [Column(name: "IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }
    }
}