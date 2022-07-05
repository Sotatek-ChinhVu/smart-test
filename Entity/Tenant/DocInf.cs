using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 文書情報
    /// </summary>
    [Table(name: "DOC_INF")]
    public class DocInf : EmrCloneable<DocInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        [Key]
        [Column("SIN_DATE", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        [Key]
        [Column("RAIIN_NO", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// カテゴリコード
        ///     DOC_CATEGORY_MST.CATEGORY_CD
        /// </summary>
        [Column("CATEGORY_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int CategoryCd { get; set; }

        /// <summary>
        /// ファイル名
        ///     実ファイル名
        /// </summary>
        [Column("FILE_NAME")]
        [MaxLength(300)]
        public string FileName { get; set; }

        /// <summary>
        /// 表示用ファイル名
        ///     表示用ファイル名
        /// </summary>
        [Column("DSP_FILE_NAME")]
        [MaxLength(300)]
        public string DspFileName { get; set; }

        /// <summary>
        /// ロック区分
        ///     1:編集中
        /// </summary>
        [Column("IS_LOCKED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsLocked { get; set; }

        /// <summary>
        /// ロック日時
        /// </summary>
        [Column("LOCK_DATE")]
        public Nullable<DateTime> LockDate { get; set; }

        /// <summary>
        /// ロックID
        /// </summary>
        [Column("LOCK_ID")]
        public int LockId { get; set; }

        /// <summary>
        /// ロック端末
        /// </summary>
        [Column("LOCK_MACHINE")]
        [MaxLength(60)]
        public string LockMachine { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }
    }
}
