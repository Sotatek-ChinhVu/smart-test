using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "kensa_std_mst")]
    public class KensaStdMst : EmrCloneable<KensaStdMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 検査項目コード
        /// 
        /// </summary>
        
        [Column("kensa_item_cd", Order = 2)]
        [MaxLength(10)]
        public string KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        
        [Column("start_date", Order = 3)]
        public int StartDate { get; set; }

        /// <summary>
        /// 男性基準値
        /// 
        /// </summary>
        [Column("male_std")]
        [MaxLength(60)]
        public string? MaleStd { get; set; } = string.Empty;

        /// <summary>
        /// 男性基準値下限
        /// 
        /// </summary>
        [Column("male_std_low")]
        [MaxLength(60)]
        public string? MaleStdLow { get; set; } = string.Empty;

        /// <summary>
        /// 男性基準値上限
        /// 
        /// </summary>
        [Column("male_std_high")]
        [MaxLength(60)]
        public string? MaleStdHigh { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値
        /// 
        /// </summary>
        [Column("female_std")]
        [MaxLength(60)]
        public string? FemaleStd { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値下限
        /// 
        /// </summary>
        [Column("female_std_low")]
        [MaxLength(60)]
        public string? FemaleStdLow { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値上限
        /// 
        /// </summary>
        [Column("female_std_high")]
        [MaxLength(60)]
        public string? FemaleStdHigh { get; set; } = string.Empty;

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
