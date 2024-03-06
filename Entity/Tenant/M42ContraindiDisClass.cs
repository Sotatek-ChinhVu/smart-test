using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m42_contraindi_dis_class")]
    public class M42ContraindiDisClass : EmrCloneable<M42ContraindiDisClass>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 病態分類コード
        /// BC で始まり2桁の数字が続く
        /// </summary>

        [Column("byotai_class_cd", Order = 1)]
        [MaxLength(4)]
        public string ByotaiClassCd { get; set; } = string.Empty;

        /// <summary>
        /// 病態分類
        /// 循環器疾患、消化器疾患などの分類
        /// </summary>
        [Column("byotai")]
        [MaxLength(50)]
        public string? Byotai { get; set; } = string.Empty;

    }
}
