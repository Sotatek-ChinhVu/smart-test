using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_drug_info_main")]
    public class M34DrugInfoMain : EmrCloneable<M34DrugInfoMain>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>

        [Column("yj_cd", Order = 1)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 剤形コード
        /// 
        /// </summary>
        [Column("form_cd")]
        public string? FormCd { get; set; } = string.Empty;

        /// <summary>
        /// 色調
        /// 
        /// </summary>
        [Column("color")]
        [MaxLength(20)]
        public string? Color { get; set; } = string.Empty;

        /// <summary>
        /// 本体記号
        /// 
        /// </summary>
        [Column("mark")]
        [MaxLength(20)]
        public string? Mark { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果コード
        /// 
        /// </summary>
        [Column("kono_cd")]
        public string? KonoCd { get; set; } = string.Empty;

        /// <summary>
        /// 副作用コード
        /// 
        /// </summary>
        [Column("fukusayo_cd")]
        public string? FukusayoCd { get; set; } = string.Empty;

        /// <summary>
        /// 副作用初期症状コード
        /// 
        /// </summary>
        [Column("fukusayo_init_cd")]
        public string? FukusayoInitCd { get; set; } = string.Empty;

    }
}
