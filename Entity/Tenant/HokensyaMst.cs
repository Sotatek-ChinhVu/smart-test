using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    // <summary>
    /// 保険者マスタ
    /// </summary>
    [Table(name: "HOKENSYA_MST")]
    public class HokensyaMst : EmrCloneable<HokensyaMst>
    {
        /// <summary>
		/// 医療機関識別ID
		/// </summary>
		[Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(name: "NAME")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// カナ名称
        /// </summary>
        [Column(name: "KANA_NAME")]
        [MaxLength(100)]
        public string KanaName { get; set; }

        /// <summary>
        /// 法別区分
        /// </summary>
        [Column(name: "HOUBETU_KBN")]
        [MaxLength(2)]
        public string HoubetuKbn { get; set; }

        /// <summary>
        /// 法別番号
        /// </summary>
        [Column(name: "HOUBETU")]
        [MaxLength(3)]
        public string Houbetu { get; set; }

        /// <summary>
        /// 保険区分
        ///     1:社保
        ///     2:国保
        ///     3:公費
        ///     4:その他
        /// </summary>
        [Column(name: "HOKEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 都道府県番号
        /// </summary>
        [Column(name: "PREF_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int PrefNo { get; set; }

        /// <summary>
        /// 保険者番号
        /// </summary>
        //[Key]
        [Column(name: "HOKENSYA_NO", Order = 2)]
        [MaxLength(8)]
        public string HokensyaNo { get; set; }

        /// <summary>
        /// 記号
        /// </summary>
        [Column(name: "KIGO")]
        [MaxLength(80)]
        public string Kigo { get; set; }

        /// <summary>
        /// 番号
        /// </summary>
        [Column(name: "BANGO")]
        [MaxLength(80)]
        public string Bango { get; set; }

        /// <summary>
        /// 1:保険登録時に記号なしで登録可
        /// </summary>
        [Column(name: "IS_KIGO_NA")]
        [CustomAttribute.DefaultValue(0)]
        public int IsKigoNa { get; set; }

        /// <summary>
        /// 給付割合本人
        /// </summary>
        [Column(name: "RATE_HONNIN")]
        public int RateHonnin { get; set; }

        /// <summary>
        /// 給付割合家族
        /// </summary>
        [Column(name: "RATE_KAZOKU")]
        public int RateKazoku { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        [Column(name: "POST_CODE")]
        [MaxLength(7)]
        public string PostCode { get; set; }

        /// <summary>
        /// 住所１
        /// </summary>
        [Column(name: "ADDRESS1")]
        [MaxLength(200)]
        public string Address1 { get; set; }

        /// <summary>
        /// 住所２
        /// </summary>
        [Column(name: "ADDRESS2")]
        [MaxLength(200)]
        public string Address2 { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Column(name: "TEL1")]
        [MaxLength(15)]
        public string Tel1 { get; set; }

        /// <summary>
        /// 削除日
        /// </summary>
        [Column(name: "DELETE_DATE")]
        public int DeleteDate { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除・廃止・消滅・解散
        /// </summary>
        [Column(name: "IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

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
