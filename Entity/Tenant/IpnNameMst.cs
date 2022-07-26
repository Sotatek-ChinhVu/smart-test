using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "IPN_NAME_MST")]
    public class IpnNameMst : EmrCloneable<IpnNameMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        //[Key]
        [Column("IPN_NAME_CD", Order = 2)]
        [MaxLength(12)]
        public string IpnNameCd { get; set; } = string.Empty;

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        //[Key]
        [Column("START_DATE", Order = 3)]
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
        /// 一般名
        /// 
        /// </summary>
        [Column("IPN_NAME")]
        [MaxLength(100)]
        public string IpnName { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 同一一般名コード、開始日内の連番
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
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
