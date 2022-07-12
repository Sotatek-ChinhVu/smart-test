﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RSVKRT_ODR_INF_CMT")]
    public class RsvkrtOdrInfCmt : EmrCloneable<RsvkrtOdrInfCmt>
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
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        [Column("RSV_DATE")]
        public int RsvDate { get; set; }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        //[Key]
        [Column("RSVKRT_NO", Order = 3)]
        public long RsvkrtNo { get; set; }

        /// <summary>
        /// 剤番号
        /// ODR_INF_DETAIL.RP_NO
        /// </summary>
        //[Key]
        [Column("RP_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public long RpNo { get; set; }

        /// <summary>
        /// 剤枝番
        /// 
        /// </summary>
        //[Key]
        [Column("RP_EDA_NO", Order = 5)]
        public long RpEdaNo { get; set; }

        /// <summary>
        /// 行番号
        /// ODR_INF_DETAIL.ROW_NO
        /// </summary>
        //[Key]
        [Column("ROW_NO", Order = 6)]
        [CustomAttribute.DefaultValue(1)]
        public int RowNo { get; set; }

        /// <summary>
        /// 枝番
        /// ※2018/11/29現在、1項目につき、最大3つまで
        /// </summary>
        //[Key]
        [Column("EDA_NO", Order = 7)]
        [CustomAttribute.DefaultValue(1)]
        public int EdaNo { get; set; }

        /// <summary>
        /// 文字色
        /// 
        /// </summary>
        [Column("FONT_COLOR")]
        public int FontColor { get; set; }

        /// <summary>
        /// コメントコード
        /// 当該診療行為に対するコメントコード
        /// </summary>
        [Column("CMT_CD")]
        [MaxLength(10)]
        public string CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// コメント名称
        /// コメントコードの名称
        /// </summary>
        [Column("CMT_NAME")]
        [MaxLength(32)]
        public string CmtName { get; set; } = string.Empty;

        /// <summary>
        /// コメント文
        /// コメントコードの定型文に組み合わせる文字情報
        /// </summary>
        [Column("CMT_OPT")]
        [MaxLength(38)]
        public string CmtOpt { get; set; } = string.Empty;

    }
}
