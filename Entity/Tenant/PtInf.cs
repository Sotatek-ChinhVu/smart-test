using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者情報
    /// </summary>
    [Table("pt_inf")]
    [Index(nameof(HpId), nameof(PtNum), Name = "pt_inf_idx01")]
    [Index(nameof(HpId), nameof(PtId), nameof(IsDelete), Name = "pt_inf_idx02")]
    public class PtInf : EmrCloneable<PtInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号							
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 患者番号
        ///		医療機関が患者特定するための番号
        /// </summary>
        [Column("pt_num")]
        public string PtNum { get; set; } = string.Empty;

        /// <summary>
        /// カナ氏名
        /// </summary>
        [Column("kana_name")]
        [MaxLength(100)]
        public string? KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column("name")]
        [MaxLength(100)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 性別
        ///		1:男 
        ///		2:女
        /// </summary>
        [Column("sex")]
        [CustomAttribute.DefaultValue(0)]
        public int Sex { get; set; }

        /// <summary>
        /// 生年月日
        ///		yyyymmdd	
        /// </summary>
        [Column("birthday")]
        [CustomAttribute.DefaultValue(0)]
        public int Birthday { get; set; }

        /// <summary>
        /// 死亡区分
        ///		0:生存 
        ///		1:死亡 
        ///		2:消息不明
        /// </summary>
        [Column("is_dead")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDead { get; set; }

        /// <summary>
        /// 死亡日
        ///		yyyymmdd		
        /// </summary>
        [Column("death_date")]
        [CustomAttribute.DefaultValue(0)]
        public int DeathDate { get; set; }

        /// <summary>
        /// 自宅郵便番号
        ///		区切り文字("-") を除く			
        /// </summary>
        [Column("home_post")]
        [MaxLength(7)]
        public string? HomePost { get; set; } = string.Empty;

        /// <summary>
        /// 自宅住所１
        /// </summary>
        [Column("home_address1")]
        [MaxLength(100)]
        public string? HomeAddress1 { get; set; } = string.Empty;

        /// <summary>
        /// 自宅住所２
        /// </summary>
        [Column("home_address2")]
        [MaxLength(100)]
        public string? HomeAddress2 { get; set; } = string.Empty;

        /// <summary>
        /// 電話番号１
        /// </summary>
        [Column("tel1")]
        [MaxLength(15)]
        public string? Tel1 { get; set; } = string.Empty;

        /// <summary>
        /// 電話番号２
        /// </summary>
        [Column("tel2")]
        [MaxLength(15)]
        public string? Tel2 { get; set; } = string.Empty;

        /// <summary>
        /// E-Mailアドレス
        /// </summary>
        [Column("mail")]
        [MaxLength(100)]
        public string? Mail { get; set; } = string.Empty;

        /// <summary>
        /// 世帯主名
        /// </summary>
        [Column("setainusi")]
        [MaxLength(100)]
        public string? Setanusi { get; set; } = string.Empty;

        /// <summary>
        /// 続柄
        /// </summary>
        [Column("zokugara")]
        [MaxLength(20)]
        public string? Zokugara { get; set; } = string.Empty;

        /// <summary>
        /// 職業
        /// </summary>
        [Column("job")]
        [MaxLength(40)]
        public string? Job { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先名称
        /// </summary>
        [Column("renraku_name")]
        [MaxLength(100)]
        public string? RenrakuName { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先郵便番号
        /// </summary>
        [Column("renraku_post")]
        [MaxLength(7)]
        public string? RenrakuPost { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先住所１
        /// </summary>
        [Column("renraku_address1")]
        [MaxLength(100)]
        public string? RenrakuAddress1 { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先住所２
        /// </summary>
        [Column("renraku_address2")]
        [MaxLength(100)]
        public string? RenrakuAddress2 { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先電話番号
        /// </summary>
        [Column("renraku_tel")]
        [MaxLength(15)]
        public string? RenrakuTel { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先電話番号
        /// </summary>
        [Column("renraku_memo")]
        [MaxLength(100)]
        public string? RenrakuMemo { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先名称
        /// </summary>
        [Column("office_name")]
        [MaxLength(100)]
        public string? OfficeName { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先郵便番号
        /// </summary>
        [Column("office_post")]
        [MaxLength(7)]
        public string? OfficePost { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先住所１
        /// </summary>
        [Column("office_address1")]
        [MaxLength(100)]
        public string? OfficeAddress1 { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先住所２
        /// </summary>
        [Column("office_address2")]
        [MaxLength(100)]
        public string? OfficeAddress2 { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先電話番号
        /// </summary>
        [Column("office_tel")]
        [MaxLength(15)]
        public string? OfficeTel { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先備考
        /// </summary>
        [Column("office_memo")]
        [MaxLength(100)]
        public string? OfficeMemo { get; set; } = string.Empty;

        /// <summary>
        /// 領収証明細発行区分
        ///		0:不要 
        ///		1:要
        /// </summary>
        [Column("is_ryosyo_detail")]
        [CustomAttribute.DefaultValue(1)]
        public int IsRyosyoDetail { get; set; }

        /// <summary>
        /// 主治医コード
        /// </summary>
        [Column("primary_doctor")]
        public int PrimaryDoctor { get; set; }

        /// <summary>
        /// テスト患者区分
        ///		1:テスト患者
        /// </summary>
        [Column("is_tester")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTester { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_delete")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

        /// <summary>
        /// MAIN_HOKEN_PID
        /// </summary>
        [Column("main_hoken_pid")]
        [CustomAttribute.DefaultValue(0)]
        public int MainHokenPid { get; set; }

        /// <summary>
        /// REFERENCE_NO
        /// </summary>
        [Column("reference_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReferenceNo { get; set; }

        /// <summary>
        /// LIMIT_CONS_FLG
        /// </summary>
        [Column("limit_cons_flg")]
        [CustomAttribute.DefaultValue(0)]
        public int LimitConsFlg { get; set; }

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
    }
}