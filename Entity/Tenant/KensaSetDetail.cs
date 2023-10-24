using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KENSA_SET_DETAIL")]
    [Serializable]
    [Index(nameof(HpId), nameof(SetId), nameof(SetEdaNo) , Name = "KENSA_SET_DETAIL_PKEY")]
    public class KensaSetDetail
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        [MaxLength(2)]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("SET_ID", Order = 2)]
        [MaxLength(9)]
        public int SetId { get; set; }

        /// <summary>
        /// セット名称
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SET_EDA_NO", Order = 3)]
        [MaxLength(9)]
        public int SetEdaNo { get; set; }


        [Column("SEQ_PARENT_NO")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(9)]
        public int SetEdaParentNo { get; set; }

        /// <summary>
        /// 検査項目コード
        /// 
        /// </summary>
        [Column("KENSA_ITEM_CD")]
        [MaxLength(10)]
        public string KensaItemCd { get; set; }

        /// <summary>
        /// 検査項目コード
        /// 
        /// </summary>
        [Column("KENSA_ITEM_SEQ_NO")]
        [MaxLength(2)]
        public int KensaItemSeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [MaxLength(9)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(1)]
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
        [MaxLength(8)]
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
        [MaxLength(8)]
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
