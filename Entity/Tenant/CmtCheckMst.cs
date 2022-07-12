using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "CMT_CHECK_MST   ")]
    public class CmtCheckMst : EmrCloneable<CmtCheckMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        //[Key]
        [Column("ITEM_CD", Order = 2)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SEQ_NO", Order = 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 順番
        /// 
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// カルテ区分
        /// KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        [Column("KARTE_KBN")]
        public int KarteKbn { get; set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("CMT")]
        [MaxLength(250)]
        public string Cmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
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
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
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
        public string UpdateMachine { get; set; }  = string.Empty;
    }
}
