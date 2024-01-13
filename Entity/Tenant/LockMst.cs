using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "lock_mst")]
    public class LockMst : EmrCloneable<LockMst>
    {
        /// <summary>
        /// 機能コードA
        /// 開く画面
        /// </summary>
        
        [Column("function_cd_a", Order = 1)]
        [MaxLength(8)]
        public string FunctionCdA { get; set; } = string.Empty;

        /// <summary>
        /// 機能コードB
        /// 排他制御の対象となる画面
        /// </summary>
        
        [Column("function_cd_b", Order = 2)]
        [MaxLength(8)]
        public string FunctionCdB { get; set; } = string.Empty;

        /// <summary>
        /// ロック範囲
        /// 0:すべて 1:同来院 2:同親来院 3:同日
        /// </summary>
        [Column("lock_range")]
        [CustomAttribute.DefaultValue(0)]
        public int LockRange { get; set; }

        /// <summary>
        /// ロックレベル
        /// 0:ロック 1:警告
        /// </summary>
        [Column("lock_level")]
        [CustomAttribute.DefaultValue(0)]
        public int LockLevel { get; set; }

        /// <summary>
        /// 有効区分
        /// 0:有効、1:無効
        /// </summary>
        [Column("is_invalid")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
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
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
