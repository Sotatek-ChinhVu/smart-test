using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rece_status")]
    public class ReceStatus : EmrCloneable<ReceStatus>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        
        [Column("seikyu_ym", Order = 3)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        
        [Column("hoken_id", Order = 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 5)]
        public int SinYm { get; set; }

        /// <summary>
        /// 付箋区分
        /// 
        /// </summary>
        [Column("fusen_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int FusenKbn { get; set; }

        /// <summary>
        /// 紙レセフラグ
        /// 1:紙レセプト
        /// </summary>
        [Column("is_paper_rece")]
        [CustomAttribute.DefaultValue(0)]
        public int IsPaperRece { get; set; }

        /// <summary>
        /// 1:仮確認済
        /// </summary>
        [Column("is_prechecked")]
        [CustomAttribute.DefaultValue(0)]
        public int IsPrechecked { get; set; }

        /// <summary>
        /// 出力フラグ
        /// 1:出力済み
        /// </summary>
        [Column("output")]
        [CustomAttribute.DefaultValue(0)]
        public int Output { get; set; }

        /// <summary>
        /// 状態区分
        /// 0:未確認 1:システム保留 2:保留1 3:保留2 4:保留3 8:仮確認 9:確認済
        /// </summary>
        [Column("status_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int StatusKbn { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
