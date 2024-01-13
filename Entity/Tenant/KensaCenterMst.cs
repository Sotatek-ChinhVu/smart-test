using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "kensa_center_mst")]
    public class KensaCenterMst : EmrCloneable<KensaCenterMst>
    {
        /// <summary>
        /// Id
        /// </summary>
        
        [Column(name: "id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// センターコード
        /// 依頼ファイルに記録するセンターコード
        /// </summary>
        [Column("center_cd")]
        [MaxLength(10)]
        public string? CenterCd { get; set; } = string.Empty;

        /// <summary>
        /// センター名称
        /// 
        /// </summary>
        [Column("center_name")]
        [MaxLength(120)]
        public string? CenterName { get; set; } = string.Empty;

        /// <summary>
        /// 主区分
        /// "0: 優先以外のセンター
        /// 1: 優先するセンター
        ///  （センターコード未設定時のセンター）"
        /// </summary>
        [Column("primary_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int PrimaryKbn { get; set; }

        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
