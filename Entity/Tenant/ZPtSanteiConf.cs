using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者算定設定
    ///     患者毎の自動算定項目等を設定するテーブル。
    /// </summary>
    [Table(name: "z_pt_santei_conf")]
    public class ZPtSanteiConf : EmrCloneable<ZPtSanteiConf>
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
        /// 医療機関識別ID
        /// </summary>
        [Column(name: "hp_id")]
        //[Index("pt_calc_conf_pkey", 1)]
        //[Index("pt_calc_conf_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Column(name: "pt_id")]
        //[Index("pt_calc_conf_pkey", 2)]
        //[Index("pt_calc_conf_idx01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 区分番号
        ///     1: 調整額
        ///     2: 調整率
        ///     3: 自動算定
        /// </summary>
        [Column(name: "kbn_no")]
        //[Index("pt_calc_conf_pkey", 3)]
        //[Index("pt_calc_conf_idx01", 3)]
        public int KbnNo { get; set; }

        /// <summary>
        /// 区分番号枝番
        ///     KBN_NO: 1: 調整額
        ///             (   0: すべて
        ///                 1: 自費除く	
        ///                 2: 自費のみ  )
        ///     KBN_NO: 2: 調整率
        ///             (   0: すべて
        ///                 1: 自費除く	
        ///                 2: 自費のみ  )
        ///     KBN_NO: 3: 自動算定
        ///             (   1: 地域包括診療料                 
        ///                 2: 認知症地域包括診療料	)
        /// </summary>
        [Column(name: "eda_no")]
        //[Index("pt_calc_conf_pkey", 4)]
        //[Index("pt_calc_conf_idx01", 4)]
        public int EdaNo { get; set; }

        /// <summary>
        /// 連番
        ///     区分番号枝番の枝番
        /// </summary>
        [Column(name: "seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("pt_calc_conf_pkey", 5)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 区分値	
        /// </summary>
        [Column(name: "kbn_val")]
        public int KbnVal { get; set; }

        /// <summary>
        /// 並び順	
        /// </summary>
        [Column(name: "sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 開始日	
        /// </summary>
        [Column(name: "start_date")]
        //[Index("pt_calc_conf_idx01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日	
        /// </summary>
        [Column(name: "end_date")]
        //[Index("pt_calc_conf_idx01", 6)]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 削除区分	
        ///     1:削除
        /// </summary>
        [Column(name: "is_deleted")]
        //[Index("pt_calc_conf_idx01", 7)]
        [CustomAttribute.DefaultValue(0)]
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