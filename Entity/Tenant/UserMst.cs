using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// ユーザーマスタ
    ///		ユーザー権限は別テーブルで管理予定						
    /// </summary>
    [Table(name: "USER_MST")]
    public class UserMst : EmrCloneable<UserMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("HP_ID", Order = 1)]
        //[Index("USER_MST_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// </summary>
        [Column("USER_ID")]
        //[Index("USER_MST_IDX01", 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 医師区分
        ///		JOB_MST.JOB_CD	
        /// </summary>
        [Column("JOB_CD")]
        public int JobCd { get; set; }

        /// <summary>
        /// 管理者区分
        ///		0:一般 
        ///		1:管理者 
        ///		9:システム管理者
        /// </summary>
        [Column("MANAGER_KBN")]
        public int ManagerKbn { get; set; }

        /// <summary>
        /// 診療科ID
        ///		KA_MST.KA_ID		
        /// </summary>
        [Column("KA_ID")]
        public int KaId { get; set; }

        /// <summary>
        /// カナ氏名
        /// </summary>
        [Column(name: "KANA_NAME")]
        [MaxLength(40)]
        public string? KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column(name: "NAME")]
        [MaxLength(40)]
        [Required]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 略氏名
        /// </summary>
        [Column(name: "SNAME")]
        [MaxLength(20)]
        [Required]
        public string? Sname { get; set; } = string.Empty;

        /// <summary>
        /// 保険医氏名
        /// </summary>
        [Column(name: "DR_NAME")]
        [MaxLength(40)]
        public string? DrName { get; set; } = string.Empty;

        /// <summary>
        /// ログインID
        /// </summary>
        [Column(name: "LOGIN_ID")]
        [MaxLength(20)]
        [Required]
        public string? LoginId { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        [Column(name: "LOGIN_PASS")]
        [MaxLength(20)]
        [Required]
        public string? LoginPass { get; set; } = string.Empty;

        /// <summary>
        /// 麻薬使用者免許No.
        /// </summary>
        [Column(name: "MAYAKU_LICENSE_NO")]
        [MaxLength(20)]
        public string? MayakuLicenseNo { get; set; } = string.Empty;

        /// <summary>
        /// 在籍開始日
        /// </summary>
        [Column(name: "START_DATE")]
        //[Index("USER_MST_IDX01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 在籍終了日
        /// </summary>
        [Column(name: "END_DATE")]
        //[Index("USER_MST_IDX01", 4)]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 並び順
        ///		担当医メニューの表示順などに使用					
        /// </summary>
        [Column(name: "SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 連携コード１
        /// </summary>
        [Column(name: "RENKEI_CD1")]
        [MaxLength(14)]
        public string? RenkeiCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("IS_DELETED")]
        //[Index("USER_MST_IDX01", 5)]
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

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("ID", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}