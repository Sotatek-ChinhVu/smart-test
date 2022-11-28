using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RAIIN_LIST_CMT")]
    [Index(nameof(HpId), nameof(RaiinNo), nameof(CmtKbn), nameof(IsDeleted), Name = "RAIIN_LIST_CMT_UKEY01")]
    public class RaiinListCmt : EmrCloneable<RaiinListCmt>
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
        /// 診療日
        /// yyyymmdd
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Key]
        [Column("RAIIN_NO", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// コメント区分
        /// 1:SOAP欄の1行目 9:備考
        /// </summary>
        [Key]
        [Column("CMT_KBN", Order = 3)]
        public int CmtKbn { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// 
        /// </summary>
        [Column("TEXT")]
        [MaxLength(300)]
        public string? Text { get; set; } = string.Empty;

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
        public string? UpdateMachine { get; set; } = string.Empty;

    }
}
