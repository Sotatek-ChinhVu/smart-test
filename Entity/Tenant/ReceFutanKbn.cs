using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RECE_FUTAN_KBN")]
    public class ReceFutanKbn : EmrCloneable<ReceFutanKbn>
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
        /// 請求年月
        /// 
        /// </summary>
        //[Key]
        [Column("SEIKYU_YM", Order = 2)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        //[Key]
        [Column("SIN_YM", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        //[Key]
        [Column("HOKEN_ID", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 患者別に保険情報を識別するための固有の番号
        /// </summary>
        //[Key]
        [Column("HOKEN_PID", Order = 6)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 負担区分コード
        /// 
        /// </summary>
        [Column("FUTAN_KBN_CD")]
        [MaxLength(1)]
        public string FutanKbnCd { get; set; } = string.Empty;

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

    }
}
