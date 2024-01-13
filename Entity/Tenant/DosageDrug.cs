using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    /// <summary>
    /// 医薬品テーブル
    /// </summary>
    [Table(name: "m46_dosage_drug")]
    public class DosageDrug : EmrCloneable<DosageDrug>
    {
        /// <summary>
        /// 医薬品コード
        /// </summary>
        
        [Column(name: "yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// ＤＯＥＩコード
        /// </summary>
        
        [Column(name: "doei_cd", Order = 2)]
        [MaxLength(8)]
        public string DoeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 薬剤区分コード
        /// </summary>
        [Column("drug_kbn")]
        [MaxLength(1)]
        public string? DgurKbn { get; set; } = string.Empty;

        /// <summary>
        /// 規格単位
        /// </summary>
        [Column("kikaku_unit")]
        [MaxLength(100)]
        public string? KikakiUnit { get; set; } = string.Empty;

        /// <summary>
        /// 薬価単位
        /// </summary>
        [Column("yakka_unit")]
        [MaxLength(20)]
        public string? YakkaiUnit { get; set; } = string.Empty;

        /// <summary>
        /// 力価係数
        /// </summary>
        [Column("rikika_rate")]
        public decimal RikikaRate { get; set; }

        /// <summary>
        /// 力価単位
        /// </summary>
        [Column("rikika_unit")]
        [MaxLength(30)]
        public string? RikikaUnit { get; set; } = string.Empty;


        /// <summary>
        /// 溶解液添付コード
        /// </summary>
        [Column("youkaieki_cd")]
        [MaxLength(1)]
        public string? YoukaiekiCd { get; set; } = string.Empty;
    }
}
