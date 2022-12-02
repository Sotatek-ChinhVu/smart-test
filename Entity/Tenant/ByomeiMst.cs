using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 病名マスタ
    /// </summary>
    [Table(name: "BYOMEI_MST")]
    public class ByomeiMst : EmrCloneable<ByomeiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 傷病名コード
        ///     修飾語ごとに重複しない番号を設定する。  
        ///     8000～8999：傷病名の接尾語に使用 
        ///     0001～7999：傷病名の接頭語に使用 （0001～1000は現在なし） 
        ///     9000～9999：歯科部位コード（現在なし）  
        ///     「ＩＣＤ１０対応標準病名マスター」における「修飾語テーブル」の「レセ電算修飾語コード」項目と同一内容である。 
        /// </summary>
        
        [Column(name: "BYOMEI_CD", Order = 2)]
        [MaxLength(7)]
        public string ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 病名
        /// </summary>
        [Column(name: "BYOMEI")]
        [MaxLength(200)]
        public string? Byomei { get; set; } = string.Empty;

        /// <summary>
        /// 省略病名    
        ///     全角20文字以内に収めた病名
        /// </summary>
        [Column("SBYOMEI")]
        [MaxLength(200)]
        public string? Sbyomei { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称１
        /// </summary>
        [Column("KANA_NAME1")]
        [MaxLength(200)]
        public string? KanaName1 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称２
        /// </summary>
        [Column("KANA_NAME2")]
        [MaxLength(200)]
        public string? KanaName2 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称３
        /// </summary>
        [Column("KANA_NAME3")]
        [MaxLength(200)]
        public string? KanaName3 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称４
        /// </summary>
        [Column("KANA_NAME4")]
        [MaxLength(200)]
        public string? KanaName4 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称５
        /// </summary>
        [Column("KANA_NAME5")]
        [MaxLength(200)]
        public string? KanaName5 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称６
        /// </summary>
        [Column("KANA_NAME6")]
        [MaxLength(200)]
        public string? KanaName6 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称７
        /// </summary>
        [Column("KANA_NAME7")]
        [MaxLength(200)]
        public string? KanaName7 { get; set; } = string.Empty;

        /// <summary>
        /// 移行先コード
        ///     傷病名コードの廃止に伴い、傷病名コードの移行先がある場合に、移行先の傷病名コードを表す。
        /// </summary>
        [Column("IKO_CD")]
        [MaxLength(7)]
        public string? IkoCd { get; set; } = string.Empty;

        /// <summary>
        /// 特定疾患コード
        ///     特定疾患療養管理料等の算定対象となる傷病名であるか否かを表す。  
        ///     00: 算定対象外 
        ///     03: 皮膚科特定疾患指導管理料（Ⅰ）算定対象 
        ///     04: 皮膚科特定疾患指導管理料（Ⅱ）算定対象 
        ///     05: 特定疾患療養管理料算定対象 
        ///     07: てんかん指導料算定対象 
        ///     08: 特定疾患療養管理料又はてんかん指導料算定対象
        /// </summary>
        [Column("SIKKAN_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int SikkanCd { get; set; }

        /// <summary>
        /// 単独使用禁止区分
        ///     当該病名を単独でレセプト表示等は禁止することを表す。
        ///     00: 下記以外
        ///     01: 部位等修飾語との組合せが必須の病名
        /// </summary>
        [Column("TANDOKU_KINSI")]
        [CustomAttribute.DefaultValue(0)]
        public int TandokuKinsi { get; set; }

        /// <summary>
        /// 保険請求外区分
        ///     当該病名を単独でレセプト表示等は保険請求外の扱いとなることを表す。
        ///     0: 下記以外
        ///     1: 保険請求対象外の病名
        /// </summary>
        [Column("HOKEN_GAI")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenGai { get; set; }

        /// <summary>
        /// 病名管理番号
        ///     「ＩＣＤ１０対応標準病名マスター」の「病名基本テーブル」の収載項目と同一内容である。
        /// </summary>
        [Column("BYOMEI_KANRI")]
        [MaxLength(8)]
        public string? ByomeiKanri { get; set; } = string.Empty;

        /// <summary>
        /// 採択区分
        ///     「ＩＣＤ１０対応標準病名マスター」の「病名基本テーブル」の収載項目と同一内容である。
        /// </summary>
        [Column("SAITAKU_KBN")]
        [MaxLength(1)]
        public string? SaitakuKbn { get; set; } = string.Empty;

        /// <summary>
        /// 病名交換用コード
        ///     「ＩＣＤ１０対応標準病名マスター」の「病名基本テーブル」の収載項目と同一内容である。
        /// </summary>
        [Column("KOUKAN_CD")]
        [MaxLength(4)]
        public string? KoukanCd { get; set; } = string.Empty;

        /// <summary>
        /// 収載年月日
        /// </summary>
        [Column("SYUSAI_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int SyusaiDate { get; set; }

        /// <summary>
        /// 変更年月日
        /// </summary>
        [Column("UPD_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdDate { get; set; }

        /// <summary>
        /// 廃止年月日
        /// </summary>
        [Column("DEL_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int DelDate { get; set; }

        /// <summary>
        /// 難病外来コード
        ///     当該傷病名が難病外来指導管理料の算定対象であるか否かを表す。
        ///     00: 算定対象外
        ///     09: 難病外来指導管理料算定対象
        /// </summary>
        [Column("NANBYO_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int NanbyoCd { get; set; }

        /// <summary>
        /// ＩＣＤ１０－１
        /// </summary>
        [Column("ICD10_1")]
        [MaxLength(5)]
        public string? Icd101 { get; set; } = string.Empty;

        /// <summary>
        /// ＩＣＤ１０－２
        /// </summary>
        [Column("ICD10_2")]
        [MaxLength(5)]
        public string? Icd102 { get; set; } = string.Empty;

        /// <summary>
        /// ＩＣＤ１０－１（２０１３）
        /// </summary>
        [Column("ICD10_1_2013")]
        [MaxLength(5)]
        public string? Icd1012013 { get; set; } = string.Empty;

        /// <summary>
        /// ＩＣＤ１０－２（２０１３）
        /// </summary>
        [Column("ICD10_2_2013")]
        [MaxLength(5)]
        public string? Icd1022013 { get; set; } = string.Empty;

        /// <summary>
        /// 採用区分
        ///     0: 未採用
        ///     1: 採用
        /// </summary>
        [Column("IS_ADOPTED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsAdopted { get; set; }

        /// <summary>
        /// 修飾語区分
        ///     項番７「修飾語名称」を次の区分により、先頭２桁
        ///     目で分類する。 
        ///     *1******：部位（頭部、頸部等） 
        ///     *2******：位置（左、右等） 
        ///     *3******：病因（外傷性、感染症等） 
        ///     *4******：経過表現（急性、慢性等） 
        ///     *5******：状態表現（悪性、良性等） 
        ///     *6******：患者帰属（胎児、老人性等） 
        ///     *7******：その他（高度、生理的等） 
        ///     *8******：接尾語
        ///     *9******：歯科用（未収録） 
        /// </summary>
        [Column("SYUSYOKU_KBN")]
        [MaxLength(8)]
        public string? SyusyokuKbn { get; set; } = string.Empty;

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
        public string? UpdateMachine { get; set; } = string.Empty ;
    }
}