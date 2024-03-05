using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "uketuke_sbt_day_inf")]
    public class UketukeSbtDayInf : EmrCloneable<UketukeSbtDayInf>
    {
        /// <summary>
        /// 医療機関識別ID 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療日 
        /// </summary>
        
        [Column("sin_date", Order = 2)]
        public int SinDate { get; set; }

        /// <summary>
        /// 連番 
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 受付種別 
        /// </summary>
        [Column("uketuke_sbt")]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 作成日時 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者 
        /// </summary>
        [Column("create_id")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;
    }
}
