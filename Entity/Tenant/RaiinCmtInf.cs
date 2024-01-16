using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院コメント情報
    /// </summary>
    [Table(name: "raiin_cmt_inf")]
    [Index(nameof(HpId), nameof(RaiinNo), nameof(CmtKbn), nameof(IsDelete), Name = "raiin_cmt_inf_idx01")]
    public class RaiinCmtInf : EmrCloneable<RaiinCmtInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号						
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        ///		yyyymmdd	
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        
        [Column("raiin_no", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// コメント区分
        ///		1:来院コメント 
        ///		9:備考			
        /// </summary>
        
        [Column("cmt_kbn", Order = 3)]
        public int CmtKbn { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// </summary>
        [MaxLength(200)]
        [Column("text")]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_delete")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}