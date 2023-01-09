using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RAIIN_FILTER_KBN")]
    public class RaiinFilterKbn : EmrCloneable<RaiinFilterKbn>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// フィルターID
        /// 
        /// </summary>
        
        [Column("FILTER_ID", Order = 2)]
        public int FilterId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 分類ID
        /// RAIIN_KBN_INF.GRP_ID
        /// </summary>
        [Column("GRP_ID")]
        public int GrpId { get; set; }

        /// <summary>
        /// 区分コード
        /// RAIIN_KBN_INF.KBN_CD
        /// </summary>
        [Column("KBN_CD")]
        public int KbnCd { get; set; }

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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
