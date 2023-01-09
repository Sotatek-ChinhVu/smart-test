using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KARTE_FILTER_MST")]
    public class KarteFilterMst : EmrCloneable<KarteFilterMst>
    {
        /// <summary>
        /// 病院コード
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// USER_MST.USER_ID
        /// </summary>
        
        [Column("USER_ID", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// フィルタID
        /// USER_ID内で連番
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("FILTER_ID", Order = 3)]
        public long FilterId { get; set; }

        /// <summary>
        /// フィルタ名
        /// 
        /// </summary>
        [Column("FILTER_NAME")]
        [MaxLength(20)]
        public string? FilterName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// カルテ起動時に適用
        /// "0:適用なし 1:適用
        /// ※同一USER_ID内で1レコードだけ1に設定可"
        /// </summary>
        [Column("AUTO_APPLY")]
        [CustomAttribute.DefaultValue(0)]
        public int AutoApply { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
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
