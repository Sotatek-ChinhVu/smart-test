using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M42_CONTRAINDI_DIS_BC")]
    public class M42ContraindiDisBc : EmrCloneable<M42ContraindiDisBc>
    {
        /// <summary>
        /// 病態コード
        /// BY で始まり5桁の数字が続く
        /// </summary>
        
        [Column("BYOTAI_CD", Order = 1)]
        [MaxLength(7)]
        public string ByotaiCd { get; set; } = string.Empty;

        /// <summary>
        /// 病態分類コード
        /// BC で始まり2桁の数字が続く
        /// </summary>
        
        [Column("BYOTAI_CLASS_CD", Order = 2)]
        [MaxLength(4)]
        public string ByotaiClassCd { get; set; } = string.Empty;
    }
}
