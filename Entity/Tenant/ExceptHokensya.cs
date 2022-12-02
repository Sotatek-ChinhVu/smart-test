using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "EXCEPT_HOKENSYA")]
    public class ExceptHokensya : EmrCloneable<ExceptHokensya>
    {
        /// <summary>
        /// Id
        /// </summary>
        
        [Column(name: "ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 都道府県番号
        /// 
        /// </summary>
        
        [Column("PREF_NO", Order = 3)]
        public int PrefNo { get; set; }

        /// <summary>
        /// 保険番号
        /// 
        /// </summary>
        
        [Column("HOKEN_NO", Order = 4)]
        public int HokenNo { get; set; }

        /// <summary>
        /// 保険番号枝番
        /// 
        /// </summary>
        
        [Column("HOKEN_EDA_NO", Order = 5)]
        public int HokenEdaNo { get; set; }

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        
        [Column("START_DATE", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 保険者番号
        /// 
        /// </summary>
        [Column("HOKENSYA_NO")]
        [MaxLength(8)]
        public string? HokensyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}

