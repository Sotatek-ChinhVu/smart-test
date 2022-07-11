using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M38_INGREDIENTS")]
    public class M38Ingredients : EmrCloneable<M38Ingredients>
    {
        /// <summary>
        /// シリアルナンバー
        /// 管理用通し番号　1~9999999
        /// </summary>
        [Key]
        [Column("SERIAL_NUM", Order = 1)]
        public int SerialNum { get; set; }

        /// <summary>
        /// 成分コード
        /// 英数字7桁
        /// </summary>
        //[Key]
        [Column("SEIBUN_CD", Order = 2)]
        [MaxLength(7)]
        public string SeibunCd { get; set; }

        /// <summary>
        /// 種別
        /// 1:成分　2:添加物
        /// </summary>
        //[Key]
        [Column("SBT", Order = 3)]
        public int Sbt { get; set; }
    }
}
