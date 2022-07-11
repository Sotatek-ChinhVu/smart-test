using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PERMISSION_MST")]
    [Index(nameof(FunctionCd), nameof(Permission), Name = "PERMISSION_MST_PKEY")]
    public class PermissionMst : EmrCloneable<PermissionMst>
    {
        /// <summary>
        /// 機能コード
        /// FUNCTION_MST.FUNCTION_CD
        /// </summary>
        [Key]
        [Column("FUNCTION_CD", Order = 1)]
        [MaxLength(8)]
        public string FunctionCd { get; set; }

        /// <summary>
        /// 許可区分
        /// "0: 制限なし(既定値)
        /// 1: 参照権限
        /// 99:使用不可"
        /// </summary>
        //[Key]
        [Column("PERMISSION", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int Permission { get; set; }

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
