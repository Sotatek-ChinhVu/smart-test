using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    [Table(name: "z_rece_seikyu")]
    public class ZReceSeikyu : EmrCloneable<ZReceSeikyu>
    {
        
        [Column("op_id", Order = 1)]
        public long OpId { get; set; }

        [Column("op_type")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("op_time")]
        public DateTime OpTime { get; set; }

        [Column("op_addr")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("op_hostname")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        [Column("sin_ym")]
        public int SinYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        [Column("hoken_id")]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        [Column("seikyu_ym")]
        [CustomAttribute.DefaultValue(999999)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        [Column("seikyu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuKbn { get; set; }

        /// <summary>
        /// 前回請求保険ID
        /// 
        /// </summary>
        [Column("pre_hoken_id")]
        [CustomAttribute.DefaultValue(0)]
        public int PreHokenId { get; set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("cmt")]
        [MaxLength(60)]
        public string? Cmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
