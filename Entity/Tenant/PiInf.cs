using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PI_INF")]
    public class PiInf : EmrCloneable<PiInf>
    {
        /// <summary>
        /// 添付文書ID
        /// 
        /// </summary>
        [Key]
        [Column("PI_ID", Order = 1)]
        [MaxLength(6)]
        public string PiId { get; set; } = string.Empty;

        /// <summary>
        /// 最終作業日
        /// 
        /// </summary>
        [Column("W_DATE")]
        public int WDate { get; set; }

        /// <summary>
        /// 添付文書標題
        /// 
        /// </summary>
        [Column("TITLE")]
        [MaxLength(60)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 改定年月
        /// 
        /// </summary>
        [Column("R_DATE")]
        public int RDate { get; set; }

        /// <summary>
        /// 版数
        /// 
        /// </summary>
        [Column("REVISION")]
        [MaxLength(100)]
        public string Revision { get; set; } = string.Empty;

        /// <summary>
        /// 改訂種別
        /// 
        /// </summary>
        [Column("R_TYPE")]
        [MaxLength(20)]
        public string RType { get; set; } = string.Empty;

        /// <summary>
        /// 改訂理由
        /// 
        /// </summary>
        [Column("R_REASON")]
        [MaxLength(200)]
        public string RReason { get; set; } = string.Empty;

        /// <summary>
        /// 商品分類番号
        /// 
        /// </summary>
        [Column("SCCJNO")]
        [MaxLength(200)]
        public string Sccjno { get; set; } = string.Empty;

        /// <summary>
        /// 薬効分類名
        /// 
        /// </summary>
        [Column("THERAPEUTICCLASSIFICATION")]
        [MaxLength(200)]
        public string Therapeuticclassification { get; set; } = string.Empty;

        /// <summary>
        /// 製剤名
        /// 
        /// </summary>
        [Column("PREPARATION_NAME")]
        [MaxLength(200)]
        public string PreparationName { get; set; } = string.Empty;

        /// <summary>
        /// ハイライト
        /// 
        /// </summary>
        [Column("HIGHLIGHT")]
        [MaxLength(200)]
        public string Highlight { get; set; } = string.Empty;

        /// <summary>
        /// 製剤の特徴
        /// 
        /// </summary>
        [Column("FEATURE")]
        [MaxLength(200)]
        public string Feature { get; set; } = string.Empty;

        /// <summary>
        /// 関連事項
        /// 
        /// </summary>
        [Column("RELATEDMATTER")]
        [MaxLength(200)]
        public string Relatedmatter { get; set; } = string.Empty;

        /// <summary>
        /// 総称名
        /// 
        /// </summary>
        [Column("COMMONNAME")]
        [MaxLength(200)]
        public string Commonname { get; set; } = string.Empty;

        /// <summary>
        /// 一般的名称
        /// 
        /// </summary>
        [Column("GENERICNAME")]
        public string Genericname { get; set; } = string.Empty;

    }
}
