using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者算定設定
    ///     患者毎の自動算定項目等を設定するテーブル。
    /// </summary>
    [Table(name: "PT_SANTEI_CONF")]
    [Index(nameof(HpId), nameof(PtId), nameof(KbnNo), nameof(EdaNo), nameof(SeqNo), Name = "PT_CALC_CONF_PKEY")]
    [Index(nameof(HpId), nameof(PtId), nameof(KbnNo), nameof(EdaNo), nameof(StartDate), nameof(EndDate), nameof(IsDeleted), Name = "PT_CALC_CONF_IDX01")]
    public class PtSanteiConf : EmrCloneable<PtSanteiConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Key]
        [Column(name: "PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 区分番号
        ///     1: 調整額
        ///     2: 調整率
        ///     3: 自動算定
        /// </summary>
        [Column(name: "KBN_NO")]
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
        public int EdaNo { get; set; }

        /// <summary>
        /// 連番
        ///     区分番号枝番の枝番
        /// </summary>
        [Key]
        [Column(name: "SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日	
        /// </summary>
        [Column(name: "END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 削除区分	
        ///     1:削除
        /// </summary>
        [Column(name: "IS_DELETED")]
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