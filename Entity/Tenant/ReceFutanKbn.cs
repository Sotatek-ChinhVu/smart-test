using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rece_futan_kbn")]
    public class ReceFutanKbn : EmrCloneable<ReceFutanKbn>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        
        [Column("seikyu_ym", Order = 2)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        
        [Column("hoken_id", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 患者別に保険情報を識別するための固有の番号
        /// </summary>
        
        [Column("hoken_pid", Order = 6)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 負担区分コード
        /// 
        /// </summary>
        [Column("futan_kbn_cd")]
        [MaxLength(1)]
        public string? FutanKbnCd { get; set; } = string.Empty;

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
    }
}
