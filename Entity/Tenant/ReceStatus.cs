using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RECE_STATUS")]
    public class ReceStatus : EmrCloneable<ReceStatus>
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
        /// 
        /// </summary>
        [Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        [Key]
        [Column("SEIKYU_YM", Order = 3)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        [Key]
        [Column("HOKEN_ID", Order = 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        [Key]
        [Column("SIN_YM", Order = 5)]
        public int SinYm { get; set; }

        /// <summary>
        /// 付箋区分
        /// 
        /// </summary>
        [Column("FUSEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int FusenKbn { get; set; }

        /// <summary>
        /// 紙レセフラグ
        /// 1:紙レセプト
        /// </summary>
        [Column("IS_PAPER_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsPaperRece { get; set; }

        /// <summary>
        /// 1:仮確認済
        /// </summary>
        [Column("IS_PRECHECKED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsPrechecked { get; set; }

        /// <summary>
        /// 出力フラグ
        /// 1:出力済み
        /// </summary>
        [Column("OUTPUT")]
        [CustomAttribute.DefaultValue(0)]
        public int Output { get; set; }

        /// <summary>
        /// 状態区分
        /// 0:未確認 1:システム保留 2:保留1 3:保留2 4:保留3 8:仮確認 9:確認済
        /// </summary>
        [Column("STATUS_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int StatusKbn { get; set; }

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
        /// 作成者ID
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
        /// 更新者ID
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

    }
}
