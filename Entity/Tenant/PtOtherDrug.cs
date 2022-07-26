using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PT_OTHER_DRUG")]
    public class PtOtherDrug : EmrCloneable<PtOtherDrug>
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
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 医薬品名称
        /// 
        /// </summary>
        [Column("DRUG_NAME")]
        [MaxLength(100)]
        public string DrugName { get; set; } = string.Empty;

        /// <summary>
        /// 開始日
        /// yyyymmdd
        /// </summary>
        [Column("START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// yyyymmdd
        /// </summary>
        [Column("END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("CMT")]
        [MaxLength(100)]
        public string Cmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        //// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
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
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
