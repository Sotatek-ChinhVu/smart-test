using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_EX_INGRDT_MAIN")]
    public class M56ExIngrdtMain : EmrCloneable<M56ExIngrdtMain>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        [Key]
        [Column("YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; }

        /// <summary>
        /// 薬剤区分コード
        /// 01:内用薬 04:注射薬 06:外用薬
        /// </summary>
        [Column("DRUG_KBN")]
        [MaxLength(2)]
        public string DrugKbn { get; set; }

        /// <summary>
        /// 用法コード
        /// 
        /// </summary>
        [Column("YOHO_CD")]
        [MaxLength(6)]
        public string YohoCd { get; set; }

        /// <summary>
        /// 配合剤フラグ
        /// 1:該当
        /// </summary>
        [Column("HAIGOU_FLG")]
        [MaxLength(1)]
        public string HaigouFlg { get; set; }

        /// <summary>
        /// 輸液フラグ
        /// 1:該当
        /// </summary>
        [Column("YUEKI_FLG")]
        [MaxLength(1)]
        public string YuekiFlg { get; set; }

        /// <summary>
        /// 漢方フラグ
        /// 1:該当
        /// </summary>
        [Column("KANPO_FLG")]
        [MaxLength(1)]
        public string KanpoFlg { get; set; }

        /// <summary>
        /// 全身作用フラグ
        /// 1:該当
        /// </summary>
        [Column("ZENSINSAYO_FLG")]
        [MaxLength(1)]
        public string ZensinsayoFlg { get; set; }
    }
}
