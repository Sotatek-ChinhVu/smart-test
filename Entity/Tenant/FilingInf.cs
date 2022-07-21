using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "FILING_INF")]
    [Index(nameof(PtId), nameof(GetDate), nameof(FileNo), nameof(CategoryCd), Name = "FILING_INF_IDX01")]
    public class FilingInf : EmrCloneable<FilingInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ファイルID
        /// </summary>
        //[Key]
        [Column("FILE_ID", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        /// <summary>
        /// 患者番号
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 取得日
        /// </summary>
        //[Key]
        [Column("GET_DATE", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int GetDate { get; set; }

        /// <summary>
        /// ファイル番号
        /// </summary>
        //[Key]
        [Column("FILE_NO", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public int FileNo { get; set; }

        /// <summary>
        /// カテゴリコード
        ///     FILING_CATEGORY_MST.CATEGORY_CD
        /// </summary>
        [Column("CATEGORY_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int CategoryCd { get; set; }


        /// <summary>
        /// ファイル名
        ///     0 実ファイル名
        /// </summary>
        [Column("FILE_NAME")]
        [MaxLength(300)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 表示ファイル名
        ///     表示用ファイル名
        /// </summary>
        [Column("DSP_FILE_NAME")]
        [MaxLength(1024)]
        public string DspFileName { get; set; } = string.Empty;

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
    }
}
