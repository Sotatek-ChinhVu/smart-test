using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// サマリ情報
    ///     生活歴情報を管理する
    /// </summary>
    [Table(name: "z_seikatureki_inf")]
    public class ZSeikaturekiInf : EmrCloneable<ZSeikaturekiInf>
    {
        
        [Column("op_id", Order = 1)]
        public long OpId { get; set; }

        [Column("op_type")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("op_time")]
        public DateTime OpTime { get; set; }

        [Column("op_addr")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("op_hostname")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// </summary>
        [Column("id")]
        //[Index("seikatureki_inf_idx01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("hp_id")]
        //[Index("seikatureki_inf_idx01", 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Column("pt_id")]
        //[Index("seikatureki_inf_idx01", 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("seq_no")]
        //[Index("seikatureki_inf_idx01", 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト 
        /// </summary>
        [Column("text")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column(name: "create_machine")]
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
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
