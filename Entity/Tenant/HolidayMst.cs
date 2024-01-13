using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 休日設定マスタ
    /// </summary>
    [Table(name: "holiday_mst")]
    [Index(nameof(HpId), nameof(SinDate), nameof(IsDeleted), Name = "holiday_mst_ukey01")]
    public class HolidayMst : EmrCloneable<HolidayMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        
        [Column(name: "sin_date", Order = 2)]
        public int SinDate { get; set; }

        /// <summary>
        /// 連番
        ///     診療日内の連番
        /// </summary>
        
        [Column(name: "seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 休日区分
        /// </summary>
        [Column(name: "holiday_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HolidayKbn { get; set; }

        /// <summary>
        /// 休診区分
        /// </summary>
        [Column(name: "kyusin_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KyusinKbn { get; set; }

        /// <summary>
        /// 休日名
        /// </summary>
        [Column(name: "holiday_name")]
        [MaxLength(20)]
        public string? HolidayName { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
