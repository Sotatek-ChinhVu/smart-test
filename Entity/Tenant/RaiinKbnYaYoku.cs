﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// （来院区分コード予約枠条件設定）
	/// </summary>
	[Table(name: "RAIIN_KBN_YOYAKU")]
    [Index(nameof(HpId), nameof(GrpId), nameof(KbnCd), nameof(IsDeleted), Name = "RAIIN_KBN_YOYAKU_IDX01")]
    public class RaiinKbnYayoku : EmrCloneable<RaiinKbnYayoku>
    {
        /// <summary>
        ///医療機関識別ID
        /// </summary>
        
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        
        [Column("GRP_ID", Order = 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        
        [Column("KBN_CD", Order = 3)]
        public int KbnCd { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column(name: "SEQ_NO", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }



        /// <summary>
        /// 予約枠コード
        /// </summary>
        [Column(name: "YOYAKU_CD")]
        public int YoyakuCd { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
		/// 作成日時	
		/// </summary>
		[Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
