using Entity.Tenant;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "MONSHIN_INF")]
    public class MonshinInfo : EmrCloneable<MonshinInfo>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        ///		yyyymmdd	
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 3)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// </summary>
        [Column("TEXT")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// リッチテキスト
        /// </summary>
        [Column("RTEXT")]
        public string Rtext { get; set; } = string.Empty;

        /// <summary>
        /// 取込区分
        ///     1:取込済
        /// </summary>
        [Column("GET_KBN")]
        public int GetKbn { get; set; }

        /// <summary>
        /// 削除区分
        ///		1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
		/// 作成日時	
		/// </summary>
		[Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
		/// 作成端末			
		/// </summary>
		[Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
