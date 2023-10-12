﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RAIIN_LIST_INF")]
    [Index(nameof(GrpId), nameof(KbnCd), nameof(RaiinListKbn), Name = "RAIIN_LIST_INF_IDX01")]
    [Index(nameof(HpId), nameof(PtId), Name = "RAIIN_LIST_INF_IDX02")]


    public class RaiinListInf : EmrCloneable<RaiinListInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("RAIIN_NO")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 分類ID
        /// 
        /// </summary>
        
        [Column("GRP_ID")]
        public int GrpId { get; set; }

        /// <summary>
        /// 区分コード
        /// 
        /// </summary>
        [Column("KBN_CD")]
        public int KbnCd { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 来院リスト区分
        ///		1: 行為
        ///		2: 項目
        ///		3: 文書
        ///		4: ファイル
        /// </summary>
        
        [Column("RAIIN_LIST_KBN", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int RaiinListKbn { get; set; }

        /// <summary>
        /// ID
        /// </summary>

        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
