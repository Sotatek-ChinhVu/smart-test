using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 文書情報
    /// </summary>
    [Table(name: "doc_inf")]
    public class DocInf : EmrCloneable<DocInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        
        [Column("sin_date", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        
        [Column("raiin_no", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// カテゴリコード
        ///     DOC_CATEGORY_MST.CATEGORY_CD
        /// </summary>
        [Column("category_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int CategoryCd { get; set; }

        /// <summary>
        /// ファイル名
        ///     実ファイル名
        /// </summary>
        [Column("file_name")]
        [MaxLength(300)]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// 表示用ファイル名
        ///     表示用ファイル名
        /// </summary>
        [Column("dsp_file_name")]
        [MaxLength(300)]
        public string? DspFileName { get; set; } = string.Empty ;

        /// <summary>
        /// ロック区分
        ///     1:編集中
        /// </summary>
        [Column("is_locked")]
        [CustomAttribute.DefaultValue(0)]
        public int IsLocked { get; set; }

        /// <summary>
        /// ロック日時
        /// </summary>
        [Column("lock_date")]
        public Nullable<DateTime> LockDate { get; set; }

        /// <summary>
        /// ロックID
        /// </summary>
        [Column("lock_id")]
        public int LockId { get; set; }

        /// <summary>
        /// ロック端末
        /// </summary>
        [Column("lock_machine")]
        [MaxLength(60)]
        public string? LockMachine { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
