using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M42_CONTRAINDI_DIS_CLASS")]
    public class M42ContraindiDisClass : EmrCloneable<M42ContraindiDisClass>
    {
        /// <summary>
        /// 病態分類コード
        /// BC で始まり2桁の数字が続く
        /// </summary>
        [Key]
        [Column("BYOTAI_CLASS_CD", Order = 1)]
        [MaxLength(4)]
        public string ByotaiClassCd { get; set; } = string.Empty;

        /// <summary>
        /// 病態分類
        /// 循環器疾患、消化器疾患などの分類
        /// </summary>
        [Column("BYOTAI")]
        [MaxLength(50)]
        public string? Byotai { get; set; } = string.Empty;

    }
}
