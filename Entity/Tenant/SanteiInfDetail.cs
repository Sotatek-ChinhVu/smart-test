using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "santei_inf_detail")]
    public class SanteiInfDetail : EmrCloneable<SanteiInfDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("seq_no")]
        public int SeqNo { get; set; }

        /// <summary>
        /// 有効期限
        /// 
        /// </summary>
        [Column("end_date")]
        public int EndDate { get; set; }

        /// <summary>
        /// 起算種別
        /// 1: 初回算定 2:発症 3:急性憎悪 4:治療開始 5:手術 6:初回診断
        /// </summary>
        [Column("kisan_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int KisanSbt { get; set; }

        /// <summary>
        /// 起算日
        /// 
        /// </summary>
        [Column("kisan_date")]
        [CustomAttribute.DefaultValue(0)]
        public int KisanDate { get; set; }

        /// <summary>
        /// 病名
        /// 
        /// </summary>
        [Column("byomei")]
        [MaxLength(160)]
        public string? Byomei { get; set; } = string.Empty;

        /// <summary>
        /// 補足コメント
        /// 
        /// </summary>
        [Column("hosoku_comment")]
        [MaxLength(80)]
        public string? HosokuComment { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("comment")]
        public string? Comment { get; set; } = string.Empty;

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
        /// 作成者
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
        /// 更新者
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

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
