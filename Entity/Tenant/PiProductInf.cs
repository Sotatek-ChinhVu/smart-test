using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PI_PRODUCT_INF")]
    public class PiProductInf : EmrCloneable<PiProductInf>
    {
        /// <summary>
        /// 添付文書
        /// N ○ 添付文書ＩＤ
        /// </summary>
        [Column("PI_ID_FULL")]
        public string? PiIdFull { get; set; } = string.Empty;

        /// <summary>
        /// 添付文書
        /// N ○ 添付文書ＩＤの下6桁
        /// </summary>
        [Key]
        [Column(name: "PI_ID", Order = 1)]
        public string PiId { get; set; } = string.Empty;

        /// <summary>
        /// 添付文書
        /// N ○ 枝番号
        /// </summary>
        [Key]
        [Column(name: "BRANCH", Order = 2)]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// JPN
        /// N ○ ＪＰＮコード
        /// </summary>
        [Key]
        [Column(name: "JPN", Order = 3)]
        public string Jpn { get; set; } = string.Empty;

        /// <summary>
        /// 商品名
        /// 商品名
        /// </summary>
        [Column("PRODUCT_NAME")]
        [MaxLength(120)]
        public string? ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 規格単位
        /// 規格単位
        /// </summary>
        [Column("UNIT")]
        [MaxLength(100)]
        public string? Unit { get; set; } = string.Empty;

        /// <summary>
        /// 製造_輸入会社
        /// 製造_輸入会社名
        /// </summary>
        [Column("MAKER")]
        [MaxLength(256)]
        public string? Maker { get; set; } = string.Empty;

        /// <summary>
        /// 販売会社
        /// 販売会社名
        /// </summary>
        [Column("VENDER")]
        [MaxLength(256)]
        public string? Vender { get; set; } = string.Empty;

        /// <summary>
        /// 発売会社
        /// 発売会社名
        /// </summary>
        [Column("MARKETER")]
        [MaxLength(256)]
        public string? Marketer { get; set; } = string.Empty;

        /// <summary>
        /// その他の会社
        /// その他の会社名
        /// </summary>
        [Column("OTHER")]
        [MaxLength(256)]
        public string? Other { get; set; } = string.Empty;

        /// <summary>
        /// YJコード
        /// ＹＪコード
        /// </summary>
        [Column("YJ_CD")]
        public string? YjCd { get; set; } = string.Empty;

        /// <summary>
        /// HOT番号
        /// ＨＯＴ番号
        /// </summary>
        [Column("HOT_CD")]
        public string? HotCd { get; set; } = string.Empty;

        /// <summary>
        /// 総称名
        /// 総称名
        /// </summary>
        [Column("SOSYO_NAME")]
        [MaxLength(80)]
        public string? SosyoName { get; set; } = string.Empty;

        /// <summary>
        /// 一般名
        /// 一般名
        /// </summary>
        [Column("GENERIC_NAME")]
        [MaxLength(120)]
        public string? GenericName { get; set; } = string.Empty;

        /// <summary>
        /// 一般名(欧名)
        /// 一般名_欧名
        /// </summary>
        [Column("GENERIC_ENG_NAME")]
        [MaxLength(120)]
        public string? GenericEngName { get; set; } = string.Empty;

        /// <summary>
        /// 日本標準商品分類番号
        /// 日本標準商品分類番号
        /// </summary>
        [Column("GENERAL_NO")]
        [MaxLength(50)]
        public string? GeneralNo { get; set; } = string.Empty;

        /// <summary>
        /// 改訂年月
        /// 改訂年月
        /// </summary>
        [Column("VER_DATE")]
        public string? VerDate { get; set; } = string.Empty;

        /// <summary>
        /// 薬価収載日
        /// 薬価収載日
        /// </summary>
        [Column("YAKKA_REG")]
        public string? YakkaReg { get; set; } = string.Empty;

        /// <summary>
        /// 薬価削除日
        /// 薬価削除日
        /// </summary>
        [Column("YAKKA_DEL")]
        public string? YakkaDel { get; set; } = string.Empty;

        /// <summary>
        /// 製造中止フラグ
        /// 製造中止フラグ
        /// </summary>
        [Column("IS_STOPED")]
        public string? IsStoped { get; set; } = string.Empty;
        
        /// <summary>
        /// 製造中止日または経過措置日
        /// 製造中止_経過措置日
        /// </summary>
        [Column("STOP_DATE")]
        public string? StopDate { get; set; } = string.Empty;

        /// <summary>
        /// 添付文書状態
        /// 添付文書状態
        /// </summary>
        [Column("PI_STATE")]
        public string? PiState { get; set; } = string.Empty;

        /// <summary>
        /// 添付文書種別
        /// 添付文書種別
        /// </summary>
        [Column("PI_SBT")]
        public string? PiSbt { get; set; } = string.Empty;

        /// <summary>
        /// 備考（添付文書単位）
        /// 備考_添付文書単位
        /// </summary>
        [Column("BIKO_PI_UNIT")]
        [MaxLength(512)]
        public string? BikoPiUnit { get; set; } = string.Empty;

        /// <summary>
        /// 備考（添付文書枝番単位）
        /// 備考_添付文書枝番
        /// </summary>
        [Column("BIKO_PI_BRANCH")]
        [MaxLength(256)]
        public string? BikoPiBranch { get; set; } = string.Empty;

        /// <summary>
        /// レコード更新日時（イメージ）
        /// 更新日時_イメージ
        /// </summary>
        [Column("UPD_DATE_IMG")]
        public DateTime? UpdDateImg { get; set; }

        /// <summary>
        /// レコード更新日時（添付文書情報）
        /// 更新日時_添付文書
        /// </summary>
        [Column("UPD_DATE_PI")]
        public DateTime? UpdDatePi { get; set; }

        /// <summary>
        /// レコード更新日時（商品情報）
        /// 更新日時_商品情報
        /// </summary>
        [Column("UPD_DATE_PRODUCT")]
        public DateTime? UpdDateProduct { get; set; }

        /// <summary>
        /// レコード更新日時（XML）
        /// 更新日時_ＸＭＬ
        /// </summary>
        [Column("UPD_DATE_XML")]
        public DateTime? UpdDateXml { get; set; }
    }
}
