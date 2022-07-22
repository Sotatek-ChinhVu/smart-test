using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_RECE_CMT")]
    public class ZReceCmt : EmrCloneable<ZReceCmt>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID")]
        //[Index("RECE_CMT_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        //[Index("RECE_CMT_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        [Column("SIN_YM")]
        //[Index("RECE_CMT_IDX01", 3)]
        public int SinYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        [Column("HOKEN_ID")]
        //[Index("RECE_CMT_IDX01", 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// コメント区分
        /// 1:ヘッダー 2:フッター
        /// </summary>
        [Column("CMT_KBN")]
        [CustomAttribute.DefaultValue(1)]
        public int CmtKbn { get; set; }

        /// <summary>
        /// コメント種別
        /// 0:コメント文（ITEM_CDあり）、1:フリーコメント
        /// </summary>
        [Column("CMT_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtSbt { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [Column("ID")]
        [CustomAttribute.DefaultValue(0)]
        public long Id { get; set; }

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
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("CMT")]
        public string Cmt { get; set; } = string.Empty;

        /// <summary>
        /// コメントデータ
        /// コメントマスターの定型文に組み合わせる文字情報
        /// </summary>
        [Column("CMT_DATA")]
        [MaxLength(38)]
        public string CmtData { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("RECE_CMT_IDX01", 5)]
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
        public string? CreateMachine { get; set; } = string.Empty;

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
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
