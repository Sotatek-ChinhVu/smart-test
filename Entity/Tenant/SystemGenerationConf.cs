using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SYSTEM_GENERATION_CONF")]
    public class SystemGenerationConf : EmrCloneable<SystemGenerationConf>
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
        /// 分類コード
        /// 
        /// </summary>
        //[Key]
        [Column("GRP_CD", Order = 3)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類枝番
        /// 
        /// </summary>
        //[Key]
        [Column("GRP_EDA_NO", Order = 4)]
        public int GrpEdaNo { get; set; }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        [Column("START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 設定値
        /// 
        /// </summary>
        [Column("VAL")]
        [CustomAttribute.DefaultValue(0)]
        public int Val { get; set; }

        /// <summary>
        /// パラメーター
        /// 
        /// </summary>
        [Column("PARAM")]
        [MaxLength(300)]
        public string? Param { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("BIKO")]
        [MaxLength(200)]
        public string? Biko { get; set; } = string.Empty;

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
        public string? CreateMachine { get; set; } = string.Empty;

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
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
