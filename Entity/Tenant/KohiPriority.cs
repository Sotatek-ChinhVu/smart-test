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
    /// 公費優先順位
    /// </summary>
    [Table(name: "KOHI_PRIORITY")]
    public class KohiPriority : EmrCloneable<KohiPriority>
    {
        /// <summary>
        /// 都道府県番号
        /// </summary>
        [Key]
        [Column("PREF_NO", Order = 1)]
        public int PrefNo { get; set; }
        /// <summary>
        /// 法別番号
        /// </summary>
        //[Key]
        [Column("HOUBETU", Order = 2)]
        [MaxLength(3)]
        public string Houbetu { get; set; }
        /// <summary>
        /// 優先順位
        /// </summary>
        //[Key]
        [Column("PRIORITY_NO", Order = 3)]
        [MaxLength(5)]
        public string PriorityNo { get; set; }
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
