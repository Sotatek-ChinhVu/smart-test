using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "UKETUKE_SBT_DAY_INF")]
    public class UketukeSbtDayInf : EmrCloneable<UketukeSbtDayInf>
    {
        /// <summary>
        /// 医療機関識別ID 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療日 
        /// </summary>
        //[Key]
        [Column("SIN_DATE", Order = 2)]
        public int SinDate { get; set; }

        /// <summary>
        /// 連番 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 受付種別 
        /// </summary>
        [Column("UKETUKE_SBT")]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 作成日時 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者 
        /// </summary>
        [Column("CREATE_ID")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

    }
}
