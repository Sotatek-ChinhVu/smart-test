using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "pt_jibai_doc")]
    public class PtJibaiDoc : EmrCloneable<PtJibaiDoc>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        /// PT_HOKEN_INF.HOKEN_ID
        /// </summary>
        
        [Column("hoken_id", Order = 3)]
        public int HokenId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 診断書料
        /// 
        /// </summary>
        [Column("sindan_cost")]
        [CustomAttribute.DefaultValue(0)]
        public int SindanCost { get; set; }

        /// <summary>
        /// 診断書枚数
        /// 
        /// </summary>
        [Column("sindan_num")]
        [CustomAttribute.DefaultValue(0)]
        public int SindanNum { get; set; }

        /// <summary>
        /// 明細書料
        /// 
        /// </summary>
        [Column("meisai_cost")]
        [CustomAttribute.DefaultValue(0)]
        public int MeisaiCost { get; set; }

        /// <summary>
        /// 明細書枚数
        /// 
        /// </summary>
        [Column("meisai_num")]
        [CustomAttribute.DefaultValue(0)]
        public int MeisaiNum { get; set; }

        /// <summary>
        /// その他
        /// 
        /// </summary>
        [Column("else_cost")]
        [CustomAttribute.DefaultValue(0)]
        public int ElseCost { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}