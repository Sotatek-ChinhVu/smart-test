using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "ONLINE_CONSENT")]
    public class OnlineConsent
    {
        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("PT_ID", Order = 1)]
        public long PtId { get; set; }

        /// <summary>
        /// 1:薬剤情報 2:特定健診情報 3:診療情報
        /// </summary>
        
        [Column("CONS_KBN", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int ConsKbn { get; set; }

        /// <summary>
        /// 同意日時
        /// </summary>
        [Column("CONS_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime ConsDate { get; set; }

        /// <summary>
        /// 有効期限
        /// </summary>
        [Column("LIMIT_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime LimitDate { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
