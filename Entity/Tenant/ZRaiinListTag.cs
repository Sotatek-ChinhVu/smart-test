using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_RAIIN_LIST_TAG")]
    public class ZRaiinListTag : EmrCloneable<ZRaiinListTag>
    {
        
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
        //[Index("RAIIN_LIST_CMT_UKEY01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// yyyymmdd
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("RAIIN_NO")]
        //[Index("RAIIN_LIST_CMT_UKEY01", 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        public int SeqNo { get; set; }

        /// <summary>
        /// タグNo
        /// 
        /// </summary>
        [Column("TAG_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int TagNo { get; set; }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        //[Index("RAIIN_LIST_CMT_UKEY01", 3)]
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
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
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
