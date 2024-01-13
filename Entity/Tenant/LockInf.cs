using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "lock_inf")]
    public class LockInf : EmrCloneable<LockInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>

        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 機能コード
        /// 
        /// </summary>

        [Column("function_cd", Order = 3)]
        [MaxLength(8)]
        public string FunctionCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療日
        /// 
        /// </summary>

        [Column("sin_date", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public long SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>

        [Column("raiin_no", Order = 5)]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 親来院番号
        /// 
        /// </summary>

        [Column("oya_raiin_no", Order = 6)]
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 端末名
        /// 
        /// </summary>
        [Column("machine")]
        public string? Machine { get; set; } = string.Empty;

        /// <summary>
        /// 端末名
        /// 
        /// </summary>
        [Column("loginkey")]
        public string? LoginKey { get; set; } = string.Empty;

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        [Column("user_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UserId { get; set; }

        /// <summary>
        /// ロック日時
        /// 
        /// </summary>
        [Column("lock_date")]
        public DateTime LockDate { get; set; }
    }
}
