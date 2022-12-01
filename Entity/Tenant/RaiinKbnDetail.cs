using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院区分詳細マスタ
    /// </summary>
    [Table(name: "RAIIN_KBN_DETAIL")]
    [Index(nameof(HpId), nameof(GrpCd), nameof(KbnCd), nameof(IsDeleted), Name = "RAIIN_KBN_DETAIL_IDX01")]
    public class RaiinKbnDetail : EmrCloneable<RaiinKbnDetail>
    {
        /// <summary>
        ///医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        [Key]
        [Column("GRP_ID", Order = 2)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 区分コード
        /// </summary>
        [Key]
        [Column("KBN_CD", Order = 3)]
        public int KbnCd { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 区分名称
        /// </summary>
        [Column("KBN_NAME")]
        [MaxLength(20)]
        public string? KbnName { get; set; } = string.Empty;

        /// <summary>
        /// 配色
        /// </summary>
        [Column("COLOR_CD")]
        [MaxLength(8)]
        public string? ColorCd { get; set; } = string.Empty;

        /// <summary>
        /// 変更確認
        ///		0:なし 
        ///		1:あり			
        /// </summary>
        [Column("IS_CONFIRMED")]
        public int IsConfirmed { get; set; }

        /// <summary>
        /// 自動設定
        ///		0:なし            
        ///		1:今回オーダー 
        ///		2:予約オーダー     
        ///		3:すべて
        /// </summary>
        [Column("IS_AUTO")]
        public int IsAuto { get; set; }

        /// <summary>
        /// 自動削除
        ///		0:なし 
        ///		1:あり
        /// </summary>
        [Column("IS_AUTO_DELETE")]
        public int IsAutoDelete { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("IS_DELETED")]
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}