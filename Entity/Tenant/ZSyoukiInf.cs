using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_SYOUKI_INF")]
    public class ZSyoukiInf : EmrCloneable<ZSyoukiInf>
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
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID")]
        //[Index("SYOUKI_INF_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        //[Index("SYOUKI_INF_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        [Column("SIN_YM")]
        //[Index("SYOUKI_INF_IDX01", 3)]
        public int SinYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        [Column("HOKEN_ID")]
        //[Index("SYOUKI_INF_IDX01", 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SEQ_NO")]
        public int SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 症状詳記区分
        /// SYOUKI_KBN_MST.SYOUKI_KBN
        /// </summary>
        [Column("SYOUKI_KBN")]
        public int SyoukiKbn { get; set; }

        /// <summary>
        /// 症状詳記
        /// 
        /// </summary>
        [Column("SYOUKI")]
        public string? Syouki { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("SYOUKI_INF_IDX01", 5)]
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
