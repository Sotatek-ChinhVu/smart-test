using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m42_contraindi_dis_bc")]
    public class M42ContraindiDisBc : EmrCloneable<M42ContraindiDisBc>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 病態コード
        /// BY で始まり5桁の数字が続く
        /// </summary>

        [Column("byotai_cd", Order = 1)]
        [MaxLength(7)]
        public string ByotaiCd { get; set; } = string.Empty;

        /// <summary>
        /// 病態分類コード
        /// BC で始まり2桁の数字が続く
        /// </summary>
        
        [Column("byotai_class_cd", Order = 2)]
        [MaxLength(4)]
        public string ByotaiClassCd { get; set; } = string.Empty;
    }
}
