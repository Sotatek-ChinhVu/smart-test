using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "filing_inf")]
    [Index(nameof(PtId), nameof(GetDate), nameof(FileNo), nameof(CategoryCd), Name = "filing_inf_idx01")]
    public class FilingInf : EmrCloneable<FilingInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ファイルID
        /// </summary>
        
        [Column("file_id", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        /// <summary>
        /// 患者番号
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 取得日
        /// </summary>
        
        [Column("get_date", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int GetDate { get; set; }

        /// <summary>
        /// ファイル番号
        /// </summary>
        
        [Column("file_no", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public int FileNo { get; set; }

        /// <summary>
        /// カテゴリコード
        ///     FILING_CATEGORY_MST.CATEGORY_CD
        /// </summary>
        [Column("category_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int CategoryCd { get; set; }


        /// <summary>
        /// ファイル名
        ///     0 実ファイル名
        /// </summary>
        [Column("file_name")]
        [MaxLength(300)]
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// 表示ファイル名
        ///     表示用ファイル名
        /// </summary>
        [Column("dsp_file_name")]
        [MaxLength(1024)]
        public string? DspFileName { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("is_deleted")]
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
