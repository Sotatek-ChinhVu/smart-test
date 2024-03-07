using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rece_cmt")]
    [Index(nameof(HpId), nameof(PtId), nameof(SinYm), nameof(HokenId), nameof(IsDeleted), Name = "rece_cmt_idx01")]
    public class ReceCmt : EmrCloneable<ReceCmt>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 3)]
        public int SinYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        
        [Column("hoken_id", Order = 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// コメント区分
        /// 1:ヘッダー 2:フッター
        /// </summary>
        
        [Column("cmt_kbn", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public int CmtKbn { get; set; }

        /// <summary>
        /// コメント種別
        /// 0:コメント文（ITEM_CDあり）、1:フリーコメント
        /// </summary>
        
        [Column("cmt_sbt", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int CmtSbt { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("seq_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// コメントコード
        /// フリーコメントはNULL
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("cmt")]
        public string? Cmt { get; set; } = string.Empty;

        /// <summary>
        /// コメントデータ
        /// コメントマスターの定型文に組み合わせる文字情報
        /// </summary>
        [Column("cmt_data")]
        [MaxLength(38)]
        public string? CmtData { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
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
        /// 作成者ID
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
        /// 更新者ID
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
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// ID
        /// </summary>
        
        [Column("id", Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
