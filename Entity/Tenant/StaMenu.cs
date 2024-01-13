using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sta_menu")]
    public class StaMenu : EmrCloneable<StaMenu>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        //[Index("sta_menu_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// メニューID
        /// 
        /// </summary>
        
        [Column("menu_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }

        /// <summary>
        /// グループID
        /// 1:日報 2:月報 3:その他
        /// </summary>
        [Column("grp_id")]
        //[Index("sta_menu_idx01", 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 帳票ID
        /// STA_MST.REPORT_ID
        /// </summary>
        [Column("report_id")]
        public int ReportId { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// メニュー名称
        /// 
        /// </summary>
        [Column("menu_name")]
        [MaxLength(130)]
        public string? MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 印刷区分
        /// 0:チェックなし 1:チェックあり
        /// </summary>
        [Column("is_print")]
        [CustomAttribute.DefaultValue(1)]
        public int IsPrint { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("sta_menu_idx01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
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
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
