using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m38_ingredients")]
    public class M38Ingredients : EmrCloneable<M38Ingredients>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// シリアルナンバー
        /// 管理用通し番号　1~9999999
        /// </summary>

        [Column("serial_num", Order = 1)]
        public int SerialNum { get; set; }

        /// <summary>
        /// 成分コード
        /// 英数字7桁
        /// </summary>
        
        [Column("seibun_cd", Order = 2)]
        [MaxLength(7)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 種別
        /// 1:成分　2:添加物
        /// </summary>
        
        [Column("sbt", Order = 3)]
        public int Sbt { get; set; }
    }
}
