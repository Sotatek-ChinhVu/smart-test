using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "online_consent")]
    public class OnlineConsent
    {
        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 1)]
        public long PtId { get; set; }

        /// <summary>
        /// 1:薬剤情報 2:特定健診情報 3:診療情報
        /// </summary>
        
        [Column("cons_kbn", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int ConsKbn { get; set; }

        /// <summary>
        /// 同意日時
        /// </summary>
        [Column("cons_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime ConsDate { get; set; }

        /// <summary>
        /// 有効期限
        /// </summary>
        [Column("limit_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime LimitDate { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
