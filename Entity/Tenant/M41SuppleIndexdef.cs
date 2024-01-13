using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m41_supple_indexdef")]
    public class M41SuppleIndexdef : EmrCloneable<M41SuppleIndexdef>
    {
        /// <summary>
        /// 索引語コード
        /// Iで始まる、6桁の数字
        /// </summary>
        
        [Column("seibun_cd", Order = 1)]
        [MaxLength(7)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 索引語
        /// サプリメント成分・素材の代表名の他、別名や類似名、その成分を多く含む食品や特定保健用食品等
        /// </summary>
        [Column("index_word")]
        [MaxLength(200)]
        public string? IndexWord { get; set; } = string.Empty;

        /// <summary>
        /// 特保フラグ
        /// 1：索引語が特定保健用食品等の商品名の場合
        /// </summary>
        [Column("tokuho_flg")]
        [MaxLength(1)]
        public string? TokuhoFlg { get; set; } = string.Empty;
    }
}
