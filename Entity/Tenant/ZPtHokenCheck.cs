﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("Z_PT_HOKEN_CHECK")]
    public class ZPtHokenCheck : EmrCloneable<ZPtHokenCheck>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 病院コード
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Column("PT_ID")]
        public long PtID { get; set; }

        /// <summary>
        /// 保険グループ
        ///     1:主保険・労災・自賠
        ///     2:公費
        /// </summary>
        [Column("HOKEN_GRP")]
        public int HokenGrp { get; set; }

        /// <summary>
        /// 保険ID
        /// 患者別に保険情報を識別するための固有の番号
        /// </summary>
        [Column("HOKEN_ID")]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 確認日時
        /// </summary>
        [Column("CHECK_DATE")]
        public DateTime CheckDate { get; set; }

        /// <summary>
        /// 確認者コード
        /// </summary>
        [Column("CHECK_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CheckId { get; set; }

        /// <summary>
        /// 確認端末
        /// </summary>
        [Column("CHECK_MACHINE")]
        [MaxLength(60)]
        public string CheckMachine { get; set; } = string.Empty;

        /// <summary>
        /// 確認コメント
        /// </summary>
        [Column("CHECK_CMT")]
        [MaxLength(100)]
        public string CheckCmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
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
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("CREATE_MACHINE")]
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
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}