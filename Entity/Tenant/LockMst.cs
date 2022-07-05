using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "LOCK_MST")]
    public class LockMst : EmrCloneable<LockMst>
    {
        /// <summary>
        /// 機能コードA
        /// 開く画面
        /// </summary>
        [Key]
        [Column("FUNCTION_CD_A", Order = 1)]
        [MaxLength(8)]
        public string FunctionCdA { get; set; }

        /// <summary>
        /// 機能コードB
        /// 排他制御の対象となる画面
        /// </summary>
        [Key]
        [Column("FUNCTION_CD_B", Order = 2)]
        [MaxLength(8)]
        public string FunctionCdB { get; set; }

        /// <summary>
        /// ロック範囲
        /// 0:すべて 1:同来院 2:同親来院 3:同日
        /// </summary>
        [Column("LOCK_RANGE")]
        [CustomAttribute.DefaultValue(0)]
        public int LockRange { get; set; }

        /// <summary>
        /// ロックレベル
        /// 0:ロック 1:警告
        /// </summary>
        [Column("LOCK_LEVEL")]
        [CustomAttribute.DefaultValue(0)]
        public int LockLevel { get; set; }

        /// <summary>
        /// 有効区分
        /// 0:有効、1:無効
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

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
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }
    }
}
