using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    /// <summary>
    /// 高額療養費限度額
    /// </summary>
    [Table(name: "KOGAKU_LIMIT")]
    public class KogakuLimit : EmrCloneable<KogakuLimit>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }
        /// <summary>
        /// 年齢区分
        /// </summary>
        [Key]
        [Column("AGE_KBN", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int AgeKbn { get; set; }
        /// <summary>
        /// 高額療養費区分
        /// </summary>
        [Key]
        [Column("KOGAKU_KBN", Order = 3)]
        public int KogakuKbn { get; set; }
        /// <summary>
        /// 所得区分
        /// </summary>
        [Column("INCOME_KBN")]
        [MaxLength(20)]
        public string IncomeKbn { get; set; }
        /// <summary>
        /// 開始日
        /// </summary>
        [Key]
        [Column("START_DATE", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }
        /// <summary>
        /// 終了日
        /// </summary>
        [Column("END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }
        /// <summary>
        /// 基準額
        /// </summary>
        [Column("BASE_LIMIT")]
        public int BaseLimit { get; set; }
        /// <summary>
        /// 調整額
        /// </summary>
        [Column("ADJUST_LIMIT")]
        public int AdjustLimit { get; set; }
        /// <summary>
        /// 多数該当
        /// </summary>
        [Column("TASU_LIMIT")]
        public int TasuLimit { get; set; }
        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 作成者
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }
        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }
        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 更新者
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }
        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }
    }
}
