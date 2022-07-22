using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RSV_RENKEI_INF_TK")]
    public class RsvRenkeiInfTk : EmrCloneable<RsvRenkeiInfTk>
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
        /// 来院番号
        /// 
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 訪問番号
        /// 
        /// </summary>
        [Column("OTHER_SEQ_NO")]
        [CustomAttribute.DefaultValue(0)]
        public long OtherSeqNo { get; set; }

        /// <summary>
        /// 訪問番号予備
        /// 
        /// </summary>
        [Column("OTHER_SEQ_NO2")]
        [CustomAttribute.DefaultValue(0)]
        public long OtherSeqNo2 { get; set; }

        /// <summary>
        /// チームカルテ患者番号
        /// 
        /// </summary>
        [Column("OTHER_PT_ID")]
        [CustomAttribute.DefaultValue(0)]
        public long OtherPtId { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
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
        /// 更新者ID
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
