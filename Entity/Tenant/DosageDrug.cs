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
    [Table(name: "M46_DOSAGE_DRUG")]
    public class DosageDrug : EmrCloneable<DosageDrug>
    {
        /// <summary>
        /// 医薬品コード
        /// </summary>
        [Key]
        [Column(name: "YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; }

        /// <summary>
        /// ＤＯＥＩコード
        /// </summary>
        [Key]
        [Column(name: "DOEI_CD", Order = 2)]
        [MaxLength(8)]
        public string DoeiCd { get; set; }

        /// <summary>
        /// 薬剤区分コード
        /// </summary>
        [Column("DRUG_KBN")]
        [MaxLength(1)]
        public string DgurKbn { get; set; }

        /// <summary>
        /// 規格単位
        /// </summary>
        [Column("KIKAKU_UNIT")]
        [MaxLength(100)]
        public string KikakiUnit { get; set; }

        /// <summary>
        /// 薬価単位
        /// </summary>
        [Column("YAKKA_UNIT")]
        [MaxLength(20)]
        public string YakkaiUnit { get; set; }

        /// <summary>
        /// 力価係数
        /// </summary>
        [Column("RIKIKA_RATE")]
        public decimal RikikaRate { get; set; }

        /// <summary>
        /// 力価単位
        /// </summary>
        [Column("RIKIKA_UNIT")]
        [MaxLength(30)]
        public string RikikaUnit { get; set; }


        /// <summary>
        /// 溶解液添付コード
        /// </summary>
        [Column("YOUKAIEKI_CD")]
        [MaxLength(1)]
        public string YoukaiekiCd { get; set; }
        
    }
}
