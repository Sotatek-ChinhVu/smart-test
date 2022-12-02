using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_FILING_INF")]
    public class ZFilingInf : EmrCloneable<ZFilingInf>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者番号
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        //[Index("FILING_INF_IDX01", 1)]
        public long PtId { get; set; }

        /// <summary>
        /// 取得日
        /// </summary>
        [Column("GET_DATE")]
        //[Index("FILING_INF_IDX01", 2)]
        [CustomAttribute.DefaultValue(0)]
        public int GetDate { get; set; }

        /// <summary>
        /// カテゴリコード
        ///     FILING_CATEGORY_MST.CATEGORY_CD
        /// </summary>
        [Column("CATEGORY_CD")]
        //[Index("FILING_INF_IDX01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int CategoryCd { get; set; }

        /// <summary>
        /// ファイル番号
        /// </summary>
        [Column("FILE_NO")]
        //[Index("FILING_INF_IDX01", 4)]
        [CustomAttribute.DefaultValue(1)]
        public int FileNo { get; set; }


        /// <summary>
        /// ファイル名
        ///     0 実ファイル名
        /// </summary>
        [Column("FILE_NAME")]
        [MaxLength(300)]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// 表示ファイル名
        ///     表示用ファイル名
        /// </summary>
        [Column("DSP_FILE_NAME")]
        [MaxLength(1024)]
        public string? DspFileName { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("IS_DELETED")]
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
        public string? CreateMachine { get; set; } = string.Empty;

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
        public string? UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// ファイルID
        /// </summary>
        [Column("FILE_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }
    }
}
