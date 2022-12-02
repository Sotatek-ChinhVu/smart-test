using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 家族情報
    /// </summary>
    [Table(name: "Z_PT_FAMILY")]
    public class ZPtFamily : EmrCloneable<ZPtFamily>
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
        /// 家族ID
        ///     患者の家族を識別するための番号
        /// </summary>
        [Column("FAMILY_ID")]
        //[Index("PT_FAMILY_IDX01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FamilyId { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        //[Index("PT_FAMILY_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 続柄コード
        /// </summary>
        [Column("ZOKUGARA_CD")]
        [Required]
        [MaxLength(10)]
        public string? ZokugaraCd { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 親ID
        ///     孫の親の家族ID
        /// </summary>
        [Column("PARENT_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int ParentId { get; set; }

        /// <summary>
        /// 家族患者ID
        /// </summary>
        [Column("FAMILY_PT_ID")]
        //[Index("PT_FAMILY_IDX01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public long FamilyPtId { get; set; }

        /// <summary>
        /// カナ氏名
        /// </summary>
        [Column("KANA_NAME")]
        [MaxLength(100)]
        public string? KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column("NAME")]
        [MaxLength(100)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 性別
        ///     1:男
        ///     2:女
        /// </summary>
        [Column("SEX")]
        public int Sex { get; set; }

        /// <summary>
        /// 生年月日
        ///     yyyymmdd
        /// </summary>
        [Column("BIRTHDAY")]
        public int Birthday { get; set; }

        /// <summary>
        /// 死亡区分
        ///     0:生存
        ///     1:死亡
        ///     2:消息不明
        /// </summary>
        [Column("IS_DEAD")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDead { get; set; }

        /// <summary>
        /// 別居フラグ
        ///     1:別居
        /// </summary>
        [Column("IS_SEPARATED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsSeparated { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("BIKO")]
        [MaxLength(120)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///     1:削除
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
