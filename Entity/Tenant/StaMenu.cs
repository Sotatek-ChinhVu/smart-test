using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "STA_MENU")]
    public class StaMenu : EmrCloneable<StaMenu>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        //[Index("STA_MENU_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// メニューID
        /// 
        /// </summary>
        [Key]
        [Column("MENU_ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }

        /// <summary>
        /// グループID
        /// 1:日報 2:月報 3:その他
        /// </summary>
        [Column("GRP_ID")]
        //[Index("STA_MENU_IDX01", 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 帳票ID
        /// STA_MST.REPORT_ID
        /// </summary>
        [Column("REPORT_ID")]
        public int ReportId { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// メニュー名称
        /// 
        /// </summary>
        [Column("MENU_NAME")]
        [MaxLength(130)]
        public string MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 印刷区分
        /// 0:チェックなし 1:チェックあり
        /// </summary>
        [Column("IS_PRINT")]
        [CustomAttribute.DefaultValue(1)]
        public int IsPrint { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        //[Index("STA_MENU_IDX01", 3)]
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
