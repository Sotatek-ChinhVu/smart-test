using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "drug_unit_conv")]
    public class DrugUnitConv : EmrCloneable<DrugUnitConv>
    {
        
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>

        [Column("item_cd", Order = 1)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        
        [Column("start_date", Order = 2)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// 
        /// </summary>
        [Column("end_date")]
        public int EndDate { get; set; }

        /// <summary>
        /// 換算係数
        /// 
        /// </summary>
        [Column("cnv_val")]
        public double CnvVal { get; set; }

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
        public string? UpdateMachine { get; set; } = string.Empty;

    }
}

