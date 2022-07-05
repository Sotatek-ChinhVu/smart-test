using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "DOSAGE_MST")]
    public class DosageMst : EmrCloneable<DosageMst>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Key]
        [Column("ITEM_CD", Order = 3)]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 4)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 一回量最小値
        /// Y 薬価単位を基準にした一回当たりの最小投与量
        /// </summary>
        [Column("ONCE_MIN")]
        public double OnceMin { get; set; }

        /// <summary>
        /// 一回量最大値
        /// Y 薬価単位を基準にした一回当たりの最大投与量
        /// </summary>
        [Column("ONCE_MAX")]
        public double OnceMax { get; set; }

        /// <summary>
        /// 一回量上限値
        /// Y 薬価単位を基準にした一回当たりの上限投与量
        /// </summary>
        [Column("ONCE_LIMIT")]
        public double OnceLimit { get; set; }

        /// <summary>
        /// 一回量単位
        /// 0:体重(体表面積)条件なし 1:/kg 2:/m2
        /// </summary>
        [Column("ONCE_UNIT")]
        public int OnceUnit { get; set; }

        /// <summary>
        /// 一日量最小値
        /// Y 薬価単位を基準にした一日当たりの最小投与量
        /// </summary>
        [Column("DAY_MIN")]
        public double DayMin { get; set; }

        /// <summary>
        /// 一日量最大値
        /// Y 薬価単位を基準にした一日当たりの最大投与量
        /// </summary>
        [Column("DAY_MAX")]
        public double DayMax { get; set; }

        /// <summary>
        /// 一日量上限値
        /// Y 薬価単位を基準にした一日当たりの上限投与量
        /// </summary>
        [Column("DAY_LIMIT")]
        public double DayLimit { get; set; }

        /// <summary>
        /// 一日量単位
        /// 0:体重(体表面積)条件なし 1:/kg 2:/m2
        /// </summary>
        [Column("DAY_UNIT")]
        public int DayUnit { get; set; }

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
