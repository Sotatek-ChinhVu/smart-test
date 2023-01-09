using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者公費情報
    /// </summary>
    [Table("PT_KOHI")]
    public class PtKohi : EmrCloneable<PtKohi>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        ///		患者別に保険情報を識別するための固有の番号
        /// </summary>
        
        [Column("HOKEN_ID", Order = 3)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("SEQ_NO", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 都道府県番号
        ///		保険マスタの都道府県番号
        /// </summary>
        [Column("PREF_NO")]
        public int PrefNo { get; set; }

        /// <summary>
        /// 保険番号
        ///		保険マスタに登録された保険番号
        /// </summary>
        [Column("HOKEN_NO")]
        public int HokenNo { get; set; }

        /// <summary>
        /// 保険番号枝番
        ///		保険マスタに登録された保険番号枝番
        /// </summary>
        [Column("HOKEN_EDA_NO")]
        public int HokenEdaNo { get; set; }

        /// <summary>
        /// 負担者番号
        /// </summary>
        [Column("FUTANSYA_NO")]
        [MaxLength(8)]
        public string? FutansyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 受給者番号
        /// </summary>
        [Column("JYUKYUSYA_NO")]
        [MaxLength(7)]
        public string? JyukyusyaNo { get; set; } = string.Empty;


        /// <summary>
        /// 保険種別区分
        ///		2:マル長
        ///		5:生活保護 
        ///		6:分点公費
        ///		7:一般公費  
        /// </summary>
        [Column(name: "HOKEN_SBT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenSbtKbn { get; set; }


        /// <summary>
        /// 法別番号
        /// </summary>
        [Column(name: "HOUBETU")]
        [MaxLength(3)]
        public string? Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 特殊受給者番号
        /// </summary>
        [Column("TOKUSYU_NO")]
        [MaxLength(20)]
        public string? TokusyuNo { get; set; } = string.Empty;

        /// <summary>
        /// 資格取得日
        ///		yyyymmdd	
        /// </summary>
        [Column("SIKAKU_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int SikakuDate { get; set; }

        /// <summary>
        /// 交付日
        ///		yyyymmdd
        /// </summary>
        [Column("KOFU_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int KofuDate { get; set; }

        /// <summary>
        /// 適用開始日
        ///		yyyymmdd
        /// </summary>
        [Column("START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        ///		yyyymmdd
        /// </summary>
        [Column("END_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int EndDate { get; set; }

        /// <summary>
        /// 負担率
        ///		yyyymmdd
        /// </summary>
        [Column("RATE")]
        [CustomAttribute.DefaultValue(0)]
        public int Rate { get; set; }

        /// <summary>
        /// 一部負担限度額
        ///		yyyymmdd
        /// </summary>
        [Column("GENDOGAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int GendoGaku { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}