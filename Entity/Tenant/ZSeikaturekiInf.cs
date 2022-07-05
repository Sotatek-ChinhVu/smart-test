using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// サマリ情報
    ///     生活歴情報を管理する
    /// </summary>
    [Table(name: "Z_SEIKATUREKI_INF")]
    public class ZSeikaturekiInf : EmrCloneable<ZSeikaturekiInf>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string OpType { get; set; }

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string OpAddr { get; set; }

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string OpHostName { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("ID")]
        //[Index("SEIKATUREKI_INF_IDX01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        //[Index("SEIKATUREKI_INF_IDX01", 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Column("PT_ID")]
        //[Index("SEIKATUREKI_INF_IDX01", 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
        //[Index("SEIKATUREKI_INF_IDX01", 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト 
        /// </summary>
        [Column("TEXT")]
        public string Text { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }
    }
}
