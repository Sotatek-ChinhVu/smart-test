using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "kensa_irai_log")]
    public class KensaIraiLog : EmrCloneable<KensaIraiLog>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 依頼日
        /// 
        /// </summary>
        [Column("irai_date")]
        public int IraiDate { get; set; }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        
        [Column("center_cd", Order = 2)]
        [MaxLength(10)]
        public string CenterCd { get; set; } = string.Empty;

        /// <summary>
        /// 作成対象日FROM
        /// 
        /// </summary>
        [Column("from_date")]
        public int FromDate { get; set; }

        /// <summary>
        /// 作成対象日TO
        /// 
        /// </summary>
        [Column("to_date")]
        public int ToDate { get; set; }

        ///<summary>
        ///依頼ファイル
        /// 
        /// </summary>
        [Column("irai_file")]
        public string? IraiFile { get; set; } = string.Empty;

        ///<summary>
        ///依頼リスト		
        /// 
        /// </summary>
        [Column("irai_list")]
        public byte[]? IraiList { get; set; } = default!;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        
        [Column("create_date", Order = 3)]
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
