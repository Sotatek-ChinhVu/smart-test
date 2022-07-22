using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KENSA_IRAI_LOG")]
    public class KensaIraiLog : EmrCloneable<KensaIraiLog>
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
        /// 依頼日
        /// 
        /// </summary>
        [Column("IRAI_DATE")]
        public int IraiDate { get; set; }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        //[Key]
        [Column("CENTER_CD", Order = 2)]
        [MaxLength(10)]
        public string CenterCd { get; set; } = string.Empty;

        /// <summary>
        /// 作成対象日FROM
        /// 
        /// </summary>
        [Column("FROM_DATE")]
        public int FromDate { get; set; }

        /// <summary>
        /// 作成対象日TO
        /// 
        /// </summary>
        [Column("TO_DATE")]
        public int ToDate { get; set; }

        ///<summary>
        ///依頼ファイル
        /// 
        /// </summary>
        [Column("IRAI_FILE")]
        public string IraiFile { get; set; } = string.Empty;

        ///<summary>
        ///依頼リスト		
        /// 
        /// </summary>
        [Column("IRAI_LIST")]
        public byte[] IraiList { get; set; } = default!;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        //[Key]
        [Column("CREATE_DATE", Order = 3)]
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
