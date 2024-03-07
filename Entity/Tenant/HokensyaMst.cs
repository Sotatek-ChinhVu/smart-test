using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    // <summary>
    /// 保険者マスタ
    /// </summary>
    [Table(name: "hokensya_mst")]
    public class HokensyaMst : EmrCloneable<HokensyaMst>
    {
        /// <summary>
		/// 医療機関識別ID
		/// </summary>
		
        [Column(name: "hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(name: "name")]
        [MaxLength(100)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称
        /// </summary>
        [Column(name: "kana_name")]
        [MaxLength(100)]
        public string? KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 法別区分
        /// </summary>
        [Column(name: "houbetu_kbn")]
        [MaxLength(2)]
        public string? HoubetuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 法別番号
        /// </summary>
        [Column(name: "houbetu")]
        [MaxLength(3)]
        public string? Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 保険区分
        ///     1:社保
        ///     2:国保
        ///     3:公費
        ///     4:その他
        /// </summary>
        [Column(name: "hoken_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 都道府県番号
        /// </summary>
        [Column(name: "pref_no")]
        [CustomAttribute.DefaultValue(0)]
        public int PrefNo { get; set; }

        /// <summary>
        /// 保険者番号
        /// </summary>
        
        [Column(name: "hokensya_no", Order = 2)]
        [MaxLength(8)]
        public string? HokensyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 記号
        /// </summary>
        [Column(name: "kigo")]
        [MaxLength(80)]
        public string? Kigo { get; set; } = string.Empty;

        /// <summary>
        /// 番号
        /// </summary>
        [Column(name: "bango")]
        [MaxLength(80)]
        public string? Bango { get; set; } = string.Empty;

        /// <summary>
        /// 1:保険登録時に記号なしで登録可
        /// </summary>
        [Column(name: "is_kigo_na")]
        [CustomAttribute.DefaultValue(0)]
        public int IsKigoNa { get; set; }

        /// <summary>
        /// 給付割合本人
        /// </summary>
        [Column(name: "rate_honnin")]
        public int RateHonnin { get; set; }

        /// <summary>
        /// 給付割合家族
        /// </summary>
        [Column(name: "rate_kazoku")]
        public int RateKazoku { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        [Column(name: "post_code")]
        [MaxLength(7)]
        public string? PostCode { get; set; } = string.Empty;

        /// <summary>
        /// 住所１
        /// </summary>
        [Column(name: "address1")]
        [MaxLength(200)]
        public string? Address1 { get; set; } = string.Empty;

        /// <summary>
        /// 住所２
        /// </summary>
        [Column(name: "address2")]
        [MaxLength(200)]
        public string? Address2 { get; set; } = string.Empty;

        /// <summary>
        /// 電話番号
        /// </summary>
        [Column(name: "tel1")]
        [MaxLength(15)]
        public string? Tel1 { get; set; } = string.Empty;

        /// <summary>
        /// 削除日
        /// </summary>
        [Column(name: "delete_date")]
        public int DeleteDate { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除・廃止・消滅・解散
        /// </summary>
        [Column(name: "is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

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
