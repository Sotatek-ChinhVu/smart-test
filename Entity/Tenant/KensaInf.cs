using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "kensa_inf")]
    public class KensaInf : EmrCloneable<KensaInf>
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
        /// 依頼日
        /// 
        /// </summary>
        [Column("irai_date")]
        public int IraiDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("raiin_no")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 検査依頼コード
        /// SEQUENCE
        /// </summary>
        
        [Column("irai_cd", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IraiCd { get; set; }

        /// <summary>
        /// 院内院外区分
        /// "0: 院内
        /// 1: 院外"
        /// </summary>
        [Column("inout_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int InoutKbn { get; set; }

        /// <summary>
        /// 実施状況
        /// "0: 依頼中
        /// 1: 検査中
        /// 2: 検査完了"
        /// </summary>
        [Column("status")]
        [CustomAttribute.DefaultValue(0)]
        public int Status { get; set; }

        /// <summary>
        /// 透析前後区分
        /// "0: 透析以外
        /// 1: 透析前
        /// 2: 透析後"
        /// </summary>
        [Column("toseki_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int TosekiKbn { get; set; }

        /// <summary>
        /// 至急区分
        /// "0: 通常
        /// 1: 至急"
        /// </summary>
        [Column("sikyu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SikyuKbn { get; set; }

        /// <summary>
        /// 結果確認
        /// "0: 未確認
        /// 1: 確認済み"
        /// </summary>
        [Column("result_check")]
        [CustomAttribute.DefaultValue(0)]
        public int ResultCheck { get; set; }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        [Column("center_cd")]
        [MaxLength(10)]
        public string? CenterCd { get; set; } = string.Empty;

        /// <summary>
        /// 乳び
        /// 
        /// </summary>
        [Column("nyubi")]
        [MaxLength(3)]
        public string? Nyubi { get; set; } = string.Empty;

        /// <summary>
        /// 溶血
        /// 
        /// </summary>
        [Column("yoketu")]
        [MaxLength(3)]
        public string? Yoketu { get; set; } = string.Empty;

        /// <summary>
        /// ビリルビン
        /// 
        /// </summary>
        [Column("bilirubin")]
        [MaxLength(3)]
        public string? Bilirubin { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
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
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
