﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_KENSA_INF_DETAIL")]
    public class ZKensaInfDetail : EmrCloneable<ZKensaInfDetail>
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
        /// 検査依頼コード
        /// 
        /// </summary>
        [Column("IRAI_CD")]
        public long IraiCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 依頼日
        /// 
        /// </summary>
        [Column("IRAI_DATE")]
        public int IraiDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("RAIIN_NO")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 検査項目コード
        /// 
        /// </summary>
        [Column("KENSA_ITEM_CD")]
        [MaxLength(10)]
        public string KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 結果値
        /// 
        /// </summary>
        [Column("RESULT_VAL")]
        [MaxLength(10)]
        public string ResultVal { get; set; } = string.Empty;

        /// <summary>
        /// 検査値形態
        /// "E: 以下
        /// L: 未満
        /// H: 以上"
        /// </summary>
        [Column("RESULT_TYPE")]
        [MaxLength(1)]
        public string ResultType { get; set; } = string.Empty;

        /// <summary>
        /// 異常値区分
        /// "L: 基準値未満
        /// H: 基準値以上"
        /// </summary>
        [Column("ABNORMAL_KBN")]
        [MaxLength(1)]
        public string AbnormalKbn { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 検査結果コメント１
        /// 
        /// </summary>
        [Column("CMT_CD1")]
        [MaxLength(3)]
        public string CmtCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 検査結果コメント２
        /// 
        /// </summary>
        [Column("CMT_CD2")]
        [MaxLength(3)]
        public string CmtCd2 { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

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
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}

