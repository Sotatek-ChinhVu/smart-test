using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PT_JIBAI_DOC")]
    public class PtJibaiDoc : EmrCloneable<PtJibaiDoc>
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
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        /// PT_HOKEN_INF.HOKEN_ID
        /// </summary>
        //[Key]
        [Column("HOKEN_ID", Order = 3)]
        public int HokenId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        //[Key]
        [Column("SIN_YM", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 診断書料
        /// 
        /// </summary>
        [Column("SINDAN_COST")]
        [CustomAttribute.DefaultValue(0)]
        public int SindanCost { get; set; }

        /// <summary>
        /// 診断書枚数
        /// 
        /// </summary>
        [Column("SINDAN_NUM")]
        [CustomAttribute.DefaultValue(0)]
        public int SindanNum { get; set; }

        /// <summary>
        /// 明細書料
        /// 
        /// </summary>
        [Column("MEISAI_COST")]
        [CustomAttribute.DefaultValue(0)]
        public int MeisaiCost { get; set; }

        /// <summary>
        /// 明細書枚数
        /// 
        /// </summary>
        [Column("MEISAI_NUM")]
        [CustomAttribute.DefaultValue(0)]
        public int MeisaiNum { get; set; }

        /// <summary>
        /// その他
        /// 
        /// </summary>
        [Column("ELSE_COST")]
        [CustomAttribute.DefaultValue(0)]
        public int ElseCost { get; set; }

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