using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "IPN_MIN_YAKKA_MST")]
    [Serializable]
    public class IpnMinYakkaMst : EmrCloneable<IpnMinYakkaMst>
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 1)]
        public int Id { get; set; }
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        
        [Column("IPN_NAME_CD", Order = 3)]
        [MaxLength(12)]
        public string IpnNameCd { get; set; } = string.Empty;

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        [Column("START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 最低薬価
        /// 
        /// </summary>
        [Column("YAKKA")]
        public double Yakka { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("SEQ_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 削除区分
        /// 同一一般名コード、開始日内の連番
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
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
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
