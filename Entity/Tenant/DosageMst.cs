using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "dosage_mst")]
    public class DosageMst : EmrCloneable<DosageMst>
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>

        [Column("item_cd", Order = 3)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>

        [Column("seq_no", Order = 4)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 一回量最小値
        /// Y 薬価単位を基準にした一回当たりの最小投与量
        /// </summary>
        [Column("once_min")]
        public double OnceMin { get; set; }

        /// <summary>
        /// 一回量最大値
        /// Y 薬価単位を基準にした一回当たりの最大投与量
        /// </summary>
        [Column("once_max")]
        public double OnceMax { get; set; }

        /// <summary>
        /// 一回量上限値
        /// Y 薬価単位を基準にした一回当たりの上限投与量
        /// </summary>
        [Column("once_limit")]
        public double OnceLimit { get; set; }

        /// <summary>
        /// 一回量単位
        /// 0:体重(体表面積)条件なし 1:/kg 2:/m2
        /// </summary>
        [Column("once_unit")]
        public int OnceUnit { get; set; }

        /// <summary>
        /// 一日量最小値
        /// Y 薬価単位を基準にした一日当たりの最小投与量
        /// </summary>
        [Column("day_min")]
        public double DayMin { get; set; }

        /// <summary>
        /// 一日量最大値
        /// Y 薬価単位を基準にした一日当たりの最大投与量
        /// </summary>
        [Column("day_max")]
        public double DayMax { get; set; }

        /// <summary>
        /// 一日量上限値
        /// Y 薬価単位を基準にした一日当たりの上限投与量
        /// </summary>
        [Column("day_limit")]
        public double DayLimit { get; set; }

        /// <summary>
        /// 一日量単位
        /// 0:体重(体表面積)条件なし 1:/kg 2:/m2
        /// </summary>
        [Column("day_unit")]
        public int DayUnit { get; set; }

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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
