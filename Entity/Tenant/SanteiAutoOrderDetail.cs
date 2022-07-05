using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SANTEI_AUTO_ORDER_DETAIL")]
    public class SanteiAutoOrderDetail : EmrCloneable<SanteiAutoOrderDetail>
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// グループコード
        /// SANTEI_GRP_MST.SANTEI_GRP_CD
        /// </summary>
        [Key]
        [Column("SANTEI_GRP_CD", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiGrpCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Key]
        [Column("ITEM_CD", Order = 5)]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        [Column("SURYO")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo { get; set; }

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
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }
    }
}
