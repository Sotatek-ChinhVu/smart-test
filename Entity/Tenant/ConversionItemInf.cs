using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 項目変換情報
    /// </summary>
    [Table(name: "CONVERSION_ITEM_INF")]
    [Index(nameof(HpId), nameof(SourceItemCd), Name = "CONVERSION_ITEM_INF_IDX01")]
    public class ConversionItemInf : EmrCloneable<ConversionItemInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 変換元診療行為コード
        /// </summary>
        
        [Column("SOURCE_ITEM_CD", Order = 2)]
        [MaxLength(10)]
        public string SourceItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 変換先診療行為コード
        /// </summary>
        
        [Column("DEST_ITEM_CD", Order = 3)]
        [MaxLength(10)]
        public string DestItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
