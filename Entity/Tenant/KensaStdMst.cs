using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KENSA_STD_MST")]
    public class KensaStdMst : EmrCloneable<KensaStdMst>
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
        /// 検査項目コード
        /// 
        /// </summary>
        //[Key]
        [Column("KENSA_ITEM_CD", Order = 2)]
        [MaxLength(10)]
        public string KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        //[Key]
        [Column("START_DATE", Order = 3)]
        public int StartDate { get; set; }

        /// <summary>
        /// 男性基準値
        /// 
        /// </summary>
        [Column("MALE_STD")]
        [MaxLength(60)]
        public string MaleStd { get; set; } = string.Empty;

        /// <summary>
        /// 男性基準値下限
        /// 
        /// </summary>
        [Column("MALE_STD_LOW")]
        [MaxLength(60)]
        public string MaleStdLow { get; set; } = string.Empty;

        /// <summary>
        /// 男性基準値上限
        /// 
        /// </summary>
        [Column("MALE_STD_HIGH")]
        [MaxLength(60)]
        public string MaleStdHigh { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値
        /// 
        /// </summary>
        [Column("FEMALE_STD")]
        [MaxLength(60)]
        public string FemaleStd { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値下限
        /// 
        /// </summary>
        [Column("FEMALE_STD_LOW")]
        [MaxLength(60)]
        public string FemaleStdLow { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値上限
        /// 
        /// </summary>
        [Column("FEMALE_STD_HIGH")]
        [MaxLength(60)]
        public string FemaleStdHigh { get; set; } = string.Empty;

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
