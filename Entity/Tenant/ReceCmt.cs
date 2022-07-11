using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RECE_CMT")]
    [Index(nameof(HpId), nameof(PtId), nameof(SinYm), nameof(HokenId), nameof(IsDeleted), Name = "RECE_CMT_IDX01")]
    public class ReceCmt : EmrCloneable<ReceCmt>
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
        /// 
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        //[Key]
        [Column("SIN_YM", Order = 3)]
        public int SinYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        //[Key]
        [Column("HOKEN_ID", Order = 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// コメント区分
        /// 1:ヘッダー 2:フッター
        /// </summary>
        //[Key]
        [Column("CMT_KBN", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public int CmtKbn { get; set; }

        /// <summary>
        /// コメント種別
        /// 0:コメント文（ITEM_CDあり）、1:フリーコメント
        /// </summary>
        //[Key]
        [Column("CMT_SBT", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int CmtSbt { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// コメントコード
        /// フリーコメントはNULL
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("CMT")]
        public string Cmt { get; set; }

        /// <summary>
        /// コメントデータ
        /// コメントマスターの定型文に組み合わせる文字情報
        /// </summary>
        [Column("CMT_DATA")]
        [MaxLength(38)]
        public string CmtData { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
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
        /// 作成者ID
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
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
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
        public string UpdateMachine { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        //[Key]
        [Column("ID", Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
