using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 項目変換情報
    /// </summary>
    [Table(name: "conversion_item_inf")]
    [Index(nameof(HpId), nameof(SourceItemCd), Name = "conversion_item_inf_idx01")]
    public class ConversionItemInf : EmrCloneable<ConversionItemInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>

        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 変換元診療行為コード
        /// </summary>

        [Column("source_item_cd", Order = 2)]
        [MaxLength(10)]
        public string SourceItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 変換先診療行為コード
        /// </summary>

        [Column("dest_item_cd", Order = 3)]
        [MaxLength(10)]
        public string DestItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

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
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
