using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 休日設定マスタ
    /// </summary>
    [Table(name: "HOLIDAY_MST")]
    [Index(nameof(HpId), nameof(SinDate), nameof(IsDeleted), Name = "HOLIDAY_MST_UKEY01")]
    public class HolidayMst : EmrCloneable<HolidayMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        //[Key]
        [Column(name: "SIN_DATE", Order = 2)]
        public int SinDate { get; set; }

        /// <summary>
        /// 連番
        ///     診療日内の連番
        /// </summary>
        //[Key]
        [Column(name: "SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 休日区分
        /// </summary>
        [Column(name: "HOLIDAY_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HolidayKbn { get; set; }

        /// <summary>
        /// 休診区分
        /// </summary>
        [Column(name: "KYUSIN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KyusinKbn { get; set; }

        /// <summary>
        /// 休日名
        /// </summary>
        [Column(name: "HOLIDAY_NAME")]
        [MaxLength(20)]
        public string HolidayName { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
