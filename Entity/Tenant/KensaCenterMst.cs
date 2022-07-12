using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KENSA_CENTER_MST")]
    public class KensaCenterMst : EmrCloneable<KensaCenterMst>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Column(name: "ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// センターコード
        /// 依頼ファイルに記録するセンターコード
        /// </summary>
        [Column("CENTER_CD")]
        [MaxLength(10)]
        public string CenterCd { get; set; } = string.Empty;

        /// <summary>
        /// センター名称
        /// 
        /// </summary>
        [Column("CENTER_NAME")]
        [MaxLength(120)]
        public string CenterName { get; set; } = string.Empty;

        /// <summary>
        /// 主区分
        /// "0: 優先以外のセンター
        /// 1: 優先するセンター
        ///  （センターコード未設定時のセンター）"
        /// </summary>
        [Column("PRIMARY_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int PrimaryKbn { get; set; }

        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

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
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
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
