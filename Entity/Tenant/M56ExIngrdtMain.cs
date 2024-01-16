using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_ex_ingrdt_main")]
    public class M56ExIngrdtMain : EmrCloneable<M56ExIngrdtMain>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        
        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 薬剤区分コード
        /// 01:内用薬 04:注射薬 06:外用薬
        /// </summary>
        [Column("drug_kbn")]
        [MaxLength(2)]
        public string? DrugKbn { get; set; } = string.Empty;

        /// <summary>
        /// 用法コード
        /// 
        /// </summary>
        [Column("yoho_cd")]
        [MaxLength(6)]
        public string? YohoCd { get; set; } = string.Empty;

        /// <summary>
        /// 配合剤フラグ
        /// 1:該当
        /// </summary>
        [Column("haigou_flg")]
        [MaxLength(1)]
        public string? HaigouFlg { get; set; } = string.Empty;

        /// <summary>
        /// 輸液フラグ
        /// 1:該当
        /// </summary>
        [Column("yueki_flg")]
        [MaxLength(1)]
        public string? YuekiFlg { get; set; } = string.Empty;

        /// <summary>
        /// 漢方フラグ
        /// 1:該当
        /// </summary>
        [Column("kanpo_flg")]
        [MaxLength(1)]
        public string? KanpoFlg { get; set; } = string.Empty;

        /// <summary>
        /// 全身作用フラグ
        /// 1:該当
        /// </summary>
        [Column("zensinsayo_flg")]
        [MaxLength(1)]
        public string? ZensinsayoFlg { get; set; } = string.Empty;
    }
}
