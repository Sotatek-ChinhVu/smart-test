﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SANTEI_INF_DETAIL")]
    public class SanteiInfDetail : EmrCloneable<SanteiInfDetail>
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
        /// 項目コード
        /// 
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO")]
        public int SeqNo { get; set; }

        /// <summary>
        /// 有効期限
        /// 
        /// </summary>
        [Column("END_DATE")]
        public int EndDate { get; set; }

        /// <summary>
        /// 起算種別
        /// 1: 初回算定 2:発症 3:急性憎悪 4:治療開始 5:手術 6:初回診断
        /// </summary>
        [Column("KISAN_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int KisanSbt { get; set; }

        /// <summary>
        /// 起算日
        /// 
        /// </summary>
        [Column("KISAN_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int KisanDate { get; set; }

        /// <summary>
        /// 病名
        /// 
        /// </summary>
        [Column("BYOMEI")]
        [MaxLength(160)]
        public string Byomei { get; set; }

        /// <summary>
        /// 補足コメント
        /// 
        /// </summary>
        [Column("HOSOKU_COMMENT")]
        [MaxLength(80)]
        public string HosokuComment { get; set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("COMMENT")]
        public string Comment { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
