using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者算定設定テーブルの変更履歴管理テーブル
    /// </summary>
    [Table(name: "pt_santei_conf_history")]
    [Index(nameof(HpId), nameof(PtId), nameof(KbnNo), nameof(EdaNo), nameof(SeqNo), Name = "pt_calc_conf_pkey")]
    [Index(nameof(HpId), nameof(PtId), nameof(KbnNo), nameof(EdaNo), nameof(StartDate), nameof(EndDate), nameof(IsDeleted), Name = "pt_calc_conf_idx01")]
    public class PtSanteiConfHistory : EmrCloneable<PtSanteiConfHistory>
    {
        /// <summary>
        /// 履歴番号
        ///     変更していく旅に増えていく
        /// </summary>
        
        [Column(name: "revision", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Revision { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column(name: "pt_id", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 区分番号
        ///     1: 調整額
        ///     2: 調整率
        ///     3: 自動算定
        /// </summary>
        [Column(name: "kbn_no")]
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
        public int EdaNo { get; set; }

        /// <summary>
        /// 連番
        ///     区分番号枝番の枝番
        /// </summary>
        
        [Column(name: "seq_no", Order = 4)]
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
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日	
        /// </summary>
        [Column(name: "end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 削除区分	
        ///     1:削除
        /// </summary>
        [Column(name: "is_deleted")]
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
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// Update type: 
        /// Insert: 挿入
        /// Update: 更新
        /// Delete: 削除
        /// </summary>
        [Column(name: "update_type")]
        [MaxLength(6)]
        public string? UpdateType { get; set; } = string.Empty;
    }
}
