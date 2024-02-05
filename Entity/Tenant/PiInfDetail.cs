using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "pi_inf_detail")]
    public class PiInfDetail : EmrCloneable<PiInfDetail>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 添付文書ID
        /// 
        /// </summary>

        [Column("pi_id", Order = 1)]
        [MaxLength(6)]
        public string PiId { get; set; } = string.Empty;

        /// <summary>
        /// 枝番
        /// "規格毎の情報の場合、「001」からの連番になる
        /// 添付文書単位の情報の場合、「999」をセット"
        /// </summary>
        
        [Column("branch", Order = 2)]
        [MaxLength(3)]
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// JPNコード
        /// 
        /// </summary>
        
        [Column("jpn", Order = 3)]
        [MaxLength(6)]
        public string Jpn { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 4)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 階層
        /// 
        /// </summary>
        [Column("level")]
        public int Level { get; set; }

        /// <summary>
        /// 内容
        /// 
        /// </summary>
        [Column("text")]
        public string? Text { get; set; } = string.Empty;
    }
}
