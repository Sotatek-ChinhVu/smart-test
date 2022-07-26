using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    [Table(name: "RECE_SEIKYU")]
    public class ReceSeikyu : EmrCloneable<ReceSeikyu>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        //[Key]
        [Column("SIN_YM", Order = 2)]
        public int SinYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        [Column("HOKEN_ID")]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        [Column("SEIKYU_YM")]
        [CustomAttribute.DefaultValue(999999)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        [Column("SEIKYU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuKbn { get; set; }

        /// <summary>
        /// 前回請求保険ID
        /// 
        /// </summary>
        [Column("PRE_HOKEN_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int PreHokenId { get; set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("CMT")]
        [MaxLength(60)]
        public string Cmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
