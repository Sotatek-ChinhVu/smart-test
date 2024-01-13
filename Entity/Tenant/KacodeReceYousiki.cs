using Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emr.DatabaseEntity
{
    [Table(name: "kacode_rece_yousiki")]
    public class KacodeReceYousiki : EmrCloneable<KacodeReceYousiki>
    {
        /// <summary>
        /// レセ診療科コード
        /// </summary>
        [Key]
        [Column("rece_ka_cd", Order = 1)]
        [MaxLength(2)]
        public string ReceKaCd { get; set; } = string.Empty;

        /// <summary>
        /// 様式1診療科コード
        /// </summary>
        [Key]
        [Column("yousiki_ka_cd", Order = 2)]
        [MaxLength(3)]
        public string YousikiKaCd { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; } = string.Empty;
    }
}
