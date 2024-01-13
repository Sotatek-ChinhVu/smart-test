using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "except_hokensya")]
    public class ExceptHokensya : EmrCloneable<ExceptHokensya>
    {
        /// <summary>
        /// Id
        /// </summary>
        
        [Column(name: "id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 都道府県番号
        /// 
        /// </summary>
        
        [Column("pref_no", Order = 3)]
        public int PrefNo { get; set; }

        /// <summary>
        /// 保険番号
        /// 
        /// </summary>
        
        [Column("hoken_no", Order = 4)]
        public int HokenNo { get; set; }

        /// <summary>
        /// 保険番号枝番
        /// 
        /// </summary>
        
        [Column("hoken_eda_no", Order = 5)]
        public int HokenEdaNo { get; set; }

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        
        [Column("start_date", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 保険者番号
        /// 
        /// </summary>
        [Column("hokensya_no")]
        [MaxLength(8)]
        public string? HokensyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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

