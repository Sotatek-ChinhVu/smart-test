using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Entity.Tenant
{
    [Table(name: "kensa_inf_detail")]
    [Index(nameof(PtId), nameof(IsDeleted),nameof(KensaItemCd), Name = "kensa_inf_detail_pt_id_idx")]
    public class KensaInfDetail : EmrCloneable<KensaInfDetail>
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
        /// 
        /// </summary>
        
        [Column("irai_cd", Order = 3)]
        public long IraiCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 検査項目コード
        /// 
        /// </summary>
        [Column("kensa_item_cd")]
        [MaxLength(10)]
        public string? KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 結果値
        /// 
        /// </summary>
        [Column("result_val")]
        [MaxLength(10)]
        public string? ResultVal { get; set; } = string.Empty;

        /// <summary>
        /// 検査値形態
        /// "E: 以下
        /// L: 未満
        /// H: 以上"
        /// </summary>
        [Column("result_type")]
        [MaxLength(1)]
        public string? ResultType { get; set; } = string.Empty;

        /// <summary>
        /// 異常値区分
        /// "L: 基準値未満
        /// H: 基準値以上"
        /// </summary>
        [Column("abnormal_kbn")]
        [MaxLength(1)]
        public string? AbnormalKbn { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 検査結果コメント１
        /// 
        /// </summary>
        [Column("cmt_cd1")]
        [MaxLength(3)]
        public string? CmtCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 検査結果コメント２
        /// 
        /// </summary>
        [Column("cmt_cd2")]
        [MaxLength(3)]
        public string? CmtCd2 { get; set; } = string.Empty;

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

        [Column("seq_parent_no")]
        [CustomAttribute.DefaultValue(0)]
        public long SeqParentNo { get; set; }

        [Column("seq_group_no")]
        [CustomAttribute.DefaultValue(0)]
        public long SeqGroupNo { get; set; }
    }
}
