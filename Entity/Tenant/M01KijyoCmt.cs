using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m01_kijyo_cmt")]
    public class M01KijyoCmt : EmrCloneable<M01KijyoCmt>
    {

        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// コメントコード
        /// 
        /// </summary>

        [Column("cmt_cd", Order = 1)]
        [MaxLength(6)]
        public string CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("cmt")]
        [MaxLength(200)]
        public string? Cmt { get; set; } = string.Empty;

    }
}
