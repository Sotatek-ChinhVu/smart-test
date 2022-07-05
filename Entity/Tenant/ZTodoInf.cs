﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_TODO_INF")]
    public class ZTodoInf : EmrCloneable<ZTodoInf>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string OpType { get; set; }

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string OpAddr { get; set; }

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string OpHostName { get; set; }

        /// <summary>
        /// 医療機関識別ID 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// TODO番号 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TODO_NO")]
        public int TodoNo { get; set; }

        /// <summary>
        /// TODO枝番 
        /// </summary>
        [Column("TODO_EDA_NO")]
        public int TodoEdaNo { get; set; }

        /// <summary>
        /// 患者ID 
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日 
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 予約番号 
        /// </summary>
        [Column("RAIIN_NO")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// TODO区分番号 
        /// </summary>
        [Column("TODO_KBN_NO")]
        public int TodoKbnNo { get; set; }

        /// <summary>
        /// TODO分類番号 
        /// </summary>
        [Column("TODO_GRP_NO")]
        public int TodoGrpNo { get; set; }

        /// <summary>
        /// 担当 
        /// </summary>
        [Column("TANTO")]
        [CustomAttribute.DefaultValue(0)]
        public int Tanto { get; set; }

        /// <summary>
        /// 期限 
        /// </summary>
        [Column("TERM")]
        public int Term { get; set; }

        /// <summary>
        /// コメント１ 
        /// </summary>
        [Column("CMT1")]
        public string Cmt1 { get; set; }

        /// <summary>
        /// コメント２ 
        /// </summary>
        [Column("CMT2")]
        public string Cmt2 { get; set; }

        /// <summary>
        /// 済
        /// 1: 済み
        /// </summary>
        [Column("IS_DONE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDone { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }

    }
}
