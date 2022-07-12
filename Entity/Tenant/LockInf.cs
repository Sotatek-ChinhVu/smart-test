using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "LOCK_INF")]
    public class LockInf : EmrCloneable<LockInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 機能コード
        /// 
        /// </summary>
        //[Key]
        [Column("FUNCTION_CD", Order = 3)]
        [MaxLength(8)]
        public string FunctionCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        //[Key]
        [Column("SIN_DATE", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public long SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 5)]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 親来院番号
        /// 
        /// </summary>
        //[Key]
        [Column("OYA_RAIIN_NO", Order = 6)]
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 端末名
        /// 
        /// </summary>
        [Column("MACHINE")]
        public string Machine { get; set; } = string.Empty;

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        [Column("USER_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UserId { get; set; }

        /// <summary>
        /// ロック日時
        /// 
        /// </summary>
        [Column("LOCK_DATE")]
        public DateTime LockDate { get; set; }
    }
}
