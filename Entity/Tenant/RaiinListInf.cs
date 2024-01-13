using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "raiin_list_inf")]
    [Index(nameof(GrpId), nameof(KbnCd), nameof(RaiinListKbn), Name = "raiin_list_inf_idx01")]
    [Index(nameof(HpId), nameof(PtId), Name = "raiin_list_inf_idx02")]


    public class RaiinListInf : EmrCloneable<RaiinListInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("raiin_no")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 分類ID
        /// 
        /// </summary>
        
        [Column("grp_id")]
        public int GrpId { get; set; }

        /// <summary>
        /// 区分コード
        /// 
        /// </summary>
        [Column("kbn_cd")]
        public int KbnCd { get; set; }

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

        /// <summary>
        /// 来院リスト区分
        ///		1: 行為
        ///		2: 項目
        ///		3: 文書
        ///		4: ファイル
        /// </summary>
        
        [Column("raiin_list_kbn", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int RaiinListKbn { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [Column("id", Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
