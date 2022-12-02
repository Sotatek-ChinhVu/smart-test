using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者算定設定
    ///     患者毎の自動算定項目等を設定するテーブル。
    /// </summary>
    [Table(name: "Z_PT_SANTEI_CONF")]
    public class ZPtSanteiConf : EmrCloneable<ZPtSanteiConf>
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
        [Column(name: "HP_ID")]
        //[Index("PT_CALC_CONF_PKEY", 1)]
        //[Index("PT_CALC_CONF_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Column(name: "PT_ID")]
        //[Index("PT_CALC_CONF_PKEY", 2)]
        //[Index("PT_CALC_CONF_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 区分番号
        ///     1: 調整額
        ///     2: 調整率
        ///     3: 自動算定
        /// </summary>
        [Column(name: "KBN_NO")]
        //[Index("PT_CALC_CONF_PKEY", 3)]
        //[Index("PT_CALC_CONF_IDX01", 3)]
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
        [Column(name: "EDA_NO")]
        //[Index("PT_CALC_CONF_PKEY", 4)]
        //[Index("PT_CALC_CONF_IDX01", 4)]
        public int EdaNo { get; set; }

        /// <summary>
        /// 連番
        ///     区分番号枝番の枝番
        /// </summary>
        [Column(name: "SEQ_NO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("PT_CALC_CONF_PKEY", 5)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 区分値	
        /// </summary>
        [Column(name: "KBN_VAL")]
        public int KbnVal { get; set; }

        /// <summary>
        /// 並び順	
        /// </summary>
        [Column(name: "SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 開始日	
        /// </summary>
        [Column(name: "START_DATE")]
        //[Index("PT_CALC_CONF_IDX01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日	
        /// </summary>
        [Column(name: "END_DATE")]
        //[Index("PT_CALC_CONF_IDX01", 6)]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 削除区分	
        ///     1:削除
        /// </summary>
        [Column(name: "IS_DELETED")]
        //[Index("PT_CALC_CONF_IDX01", 7)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string? CreateMachine { get; set; } = string.Empty;

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
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}