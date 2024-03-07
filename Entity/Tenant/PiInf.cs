using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "pi_inf")]
    public class PiInf : EmrCloneable<PiInf>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 添付文書ID
        /// 
        /// </summary>

        [Column("pi_id", Order = 1)]
        [MaxLength(6)]
        public string PiId { get; set; } = string.Empty;

        /// <summary>
        /// 最終作業日
        /// 
        /// </summary>
        [Column("w_date")]
        public int WDate { get; set; }

        /// <summary>
        /// 添付文書標題
        /// 
        /// </summary>
        [Column("title")]
        [MaxLength(60)]
        public string? Title { get; set; } = string.Empty;

        /// <summary>
        /// 改定年月
        /// 
        /// </summary>
        [Column("r_date")]
        public int RDate { get; set; }

        /// <summary>
        /// 版数
        /// 
        /// </summary>
        [Column("revision")]
        [MaxLength(100)]
        public string? Revision { get; set; } = string.Empty;

        /// <summary>
        /// 改訂種別
        /// 
        /// </summary>
        [Column("r_type")]
        [MaxLength(20)]
        public string? RType { get; set; } = string.Empty;

        /// <summary>
        /// 改訂理由
        /// 
        /// </summary>
        [Column("r_reason")]
        [MaxLength(200)]
        public string? RReason { get; set; } = string.Empty;

        /// <summary>
        /// 商品分類番号
        /// 
        /// </summary>
        [Column("sccjno")]
        [MaxLength(200)]
        public string? Sccjno { get; set; } = string.Empty;

        /// <summary>
        /// 薬効分類名
        /// 
        /// </summary>
        [Column("therapeuticclassification")]
        [MaxLength(200)]
        public string? Therapeuticclassification { get; set; } = string.Empty;

        /// <summary>
        /// 製剤名
        /// 
        /// </summary>
        [Column("preparation_name")]
        [MaxLength(200)]
        public string? PreparationName { get; set; } = string.Empty;

        /// <summary>
        /// ハイライト
        /// 
        /// </summary>
        [Column("highlight")]
        [MaxLength(200)]
        public string? Highlight { get; set; } = string.Empty;

        /// <summary>
        /// 製剤の特徴
        /// 
        /// </summary>
        [Column("feature")]
        [MaxLength(200)]
        public string? Feature { get; set; } = string.Empty;

        /// <summary>
        /// 関連事項
        /// 
        /// </summary>
        [Column("relatedmatter")]
        [MaxLength(200)]
        public string? Relatedmatter { get; set; } = string.Empty;

        /// <summary>
        /// 総称名
        /// 
        /// </summary>
        [Column("commonname")]
        [MaxLength(200)]
        public string? Commonname { get; set; } = string.Empty;

        /// <summary>
        /// 一般的名称
        /// 
        /// </summary>
        [Column("genericname")]
        public string? Genericname { get; set; } = string.Empty;
    }
}
