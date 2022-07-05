﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M01_KINKI_CMT")]
    public class M01KinkiCmt : EmrCloneable<M01KinkiCmt>
    {
        /// <summary>
        /// コメントコード
        /// 
        /// </summary>
        [Key]
        [Column("CMT_CD", Order = 1)]
        [MaxLength(6)]
        public string CmtCd { get; set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("CMT")]
        [MaxLength(200)]
        public string Cmt { get; set; }

    }
}
