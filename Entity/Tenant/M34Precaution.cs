﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_PRECAUTIONS")]
    public class M34Precaution : EmrCloneable<M34Precaution>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        
        [Column("YJ_CD", Order = 1)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("SEQ_NO", Order = 2)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 注意事項コード
        /// 
        /// </summary>
        [Column("PRECAUTION_CD")]
        public string? PrecautionCd { get; set; } = string.Empty;

    }
}
