using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "ipn_name_mst")]
    [Index(nameof(IpnNameCd), Name = "ipn_name_mst_idx01")]

    public class IpnNameMst : EmrCloneable<IpnNameMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>

        [Column("ipn_name_cd", Order = 2)]
        [MaxLength(12)]
        public string IpnNameCd { get; set; } = string.Empty;

        /// <summary>
        /// 開始日
        /// 
        /// </summary>

        [Column("start_date", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 一般名
        /// 
        /// </summary>
        [Column("ipn_name")]
        [MaxLength(100)]
        public string? IpnName { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 同一一般名コード、開始日内の連番
        /// </summary>

        [Column("seq_no", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
