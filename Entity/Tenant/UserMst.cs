using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// ユーザーマスタ
    ///		ユーザー権限は別テーブルで管理予定						
    /// </summary>
    [Table(name: "user_mst")]
    public class UserMst : EmrCloneable<UserMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>

        [Column("hp_id", Order = 1)]
        //[Index("user_mst_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// </summary>
        [Column("user_id")]
        //[Index("user_mst_idx01", 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 医師区分
        ///		JOB_MST.JOB_CD	
        /// </summary>
        [Column("job_cd")]
        public int JobCd { get; set; }

        /// <summary>
        /// 管理者区分
        ///		0:一般 
        ///		1:管理者 
        ///		9:システム管理者
        /// </summary>
        [Column("manager_kbn")]
        public int ManagerKbn { get; set; }

        /// <summary>
        /// 診療科ID
        ///		KA_MST.KA_ID		
        /// </summary>
        [Column("ka_id")]
        public int KaId { get; set; }

        /// <summary>
        /// カナ氏名
        /// </summary>
        [Column(name: "kana_name")]
        [MaxLength(40)]
        public string? KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column(name: "name")]
        [MaxLength(40)]
        [Required]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 略氏名
        /// </summary>
        [Column(name: "sname")]
        [MaxLength(20)]
        [Required]
        public string? Sname { get; set; } = string.Empty;

        /// <summary>
        /// 保険医氏名
        /// </summary>
        [Column(name: "dr_name")]
        [MaxLength(40)]
        public string? DrName { get; set; } = string.Empty;

        /// <summary>
        /// ログインID
        /// </summary>
        [Column(name: "login_id")]
        [MaxLength(30)]
        [Required]
        public string? LoginId { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        //[Column(name: "login_pass")]
        //[MaxLength(20)]
        //[Required]
        //public string? LoginPass { get; set; } = string.Empty;

        /// <summary>
        /// 麻薬使用者免許No.
        /// </summary>
        [Column(name: "mayaku_license_no")]
        [MaxLength(20)]
        public string? MayakuLicenseNo { get; set; } = string.Empty;

        /// <summary>
        /// 在籍開始日
        /// </summary>
        [Column(name: "start_date")]
        //[Index("user_mst_idx01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 在籍終了日
        /// </summary>
        [Column(name: "end_date")]
        //[Index("user_mst_idx01", 4)]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 並び順
        ///		担当医メニューの表示順などに使用					
        /// </summary>
        [Column(name: "sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 連携コード１
        /// </summary>
        [Column(name: "renkei_cd1")]
        [MaxLength(14)]
        public string? RenkeiCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("user_mst_idx01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// </summary>

        [Column("id", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("login_type")]
        [CustomAttribute.DefaultValue(0)]
        public int LoginType { get; set; }

        [Column("hpki_sn")]
        [MaxLength(100)]
        public string? HpkiSn { get; set; } = string.Empty;

        [Column("hpki_issuer_dn")]
        [MaxLength(100)]
        public string? HpkiIssuerDn { get; set; } = string.Empty;

        [Column(name: "hash_password")]
        public string? HashPassword { get; set; } = string.Empty;

        /// <summary>
        /// 連携コード１
        /// </summary>
        [Column(name: "salt")]
        [MaxLength(14)]
        public string? Salt { get; set; } = string.Empty;

        [Column(name: "email")]
        [MaxLength(300)]
        public string? Email { get; set; } = string.Empty;

        [Column(name: "is_init_password")]
        public int IsInitPassword { get; set; }

        [Column(name: "miss_login_count")]
        public int MissLoginCount { get; set; }

        [Column(name: "status")]
        public int Status { get; set; }
    }
}