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
    [Table(name: "kohi_priority")]
    public class KohiPriority : EmrCloneable<KohiPriority>
    {
        /// <summary>
        /// 都道府県番号
        /// </summary>
        
        [Column("pref_no", Order = 1)]
        public int PrefNo { get; set; }
        /// <summary>
        /// 法別番号
        /// </summary>
        
        [Column("houbetu", Order = 2)]
        [MaxLength(3)]
        public string Houbetu { get; set; } = string.Empty;
        /// <summary>
        /// 優先順位
        /// </summary>
        
        [Column("priority_no", Order = 3)]
        [MaxLength(5)]
        public string PriorityNo { get; set; } = string.Empty;
        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string? CreateMachine { get; set; } = string.Empty;
        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
