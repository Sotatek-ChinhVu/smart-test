using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PI_INF_DETAIL")]
    public class PiInfDetail : EmrCloneable<PiInfDetail>
    {
        /// <summary>
        /// 添付文書ID
        /// 
        /// </summary>
        [Key]
        [Column("PI_ID", Order = 1)]
        [MaxLength(6)]
        public string PiId { get; set; } = string.Empty;

        /// <summary>
        /// 枝番
        /// "規格毎の情報の場合、「001」からの連番になる
        /// 添付文書単位の情報の場合、「999」をセット"
        /// </summary>
        [Key]
        [Column("BRANCH", Order = 2)]
        [MaxLength(3)]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// JPNコード
        /// 
        /// </summary>
        [Key]
        [Column("JPN", Order = 3)]
        [MaxLength(6)]
        public string Jpn { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 4)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 階層
        /// 
        /// </summary>
        [Column("LEVEL")]
        public int Level { get; set; }

        /// <summary>
        /// 内容
        /// 
        /// </summary>
        [Column("TEXT")]
        public string? Text { get; set; } = string.Empty;
    }
}
