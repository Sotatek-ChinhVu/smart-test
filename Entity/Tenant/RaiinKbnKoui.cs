using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院区分行為設定
    /// </summary>
    [Table(name: "RAIIN_KBN_KOUI")]
    [Index(nameof(HpId), nameof(GrpId), nameof(KbnCd), nameof(IsDeleted), Name = "RAIIN_KBN_KOUI_IDX01")]
    public class RaiinKbnKoui
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        [Key]
        [Column("GRP_ID", Order = 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 区分コード
        /// </summary>
        [Key]
        [Column("KBN_CD", Order = 3)]
        public int KbnCd { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 行為区分ID
        ///     KOUI_KBN_MST.KOUI_KBN_ID
        /// </summary>
        [Required]
        [Column("KOUI_KBN_ID")]
        public int KouiKbnId { get; set; }

        /// <summary>
        /// 削除区分
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
