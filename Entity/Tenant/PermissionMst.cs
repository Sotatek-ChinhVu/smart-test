using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "permission_mst")]
    [Index(nameof(FunctionCd), nameof(Permission), Name = "permission_mst_pkey")]
    public class PermissionMst : EmrCloneable<PermissionMst>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 機能コード
        /// FUNCTION_MST.FUNCTION_CD
        /// </summary>

        [Column("function_cd", Order = 1)]
        [MaxLength(8)]
        public string FunctionCd { get; set; } = string.Empty;

        /// <summary>
        /// 許可区分
        /// "0: 制限なし(既定値)
        /// 1: 参照権限
        /// 99:使用不可"
        /// </summary>

        [Column("permission", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int Permission { get; set; }

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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
