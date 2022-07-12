using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("Z_PT_INF")]
    public class ZPtInf : EmrCloneable<ZPtInf>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///        患者を識別するためのシステム固有の番号                            
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
        public long SeqNo { get; set; }

        /// <summary>
        /// 患者番号
        ///        医療機関が患者特定するための番号
        /// </summary>
        [Column("PT_NUM")]
        public long PtNum { get; set; }

        /// <summary>
        /// カナ氏名
        /// </summary>
        [Column("KANA_NAME")]
        [MaxLength(100)]
        public string KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column("NAME")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 性別
        ///        1:男 
        ///        2:女
        /// </summary>
        [Column("SEX")]
        [CustomAttribute.DefaultValue(0)]
        public int Sex { get; set; }

        /// <summary>
        /// 生年月日
        ///        yyyymmdd    
        /// </summary>
        [Column("BIRTHDAY")]
        [CustomAttribute.DefaultValue(0)]
        public int Birthday { get; set; }

        /// <summary>
        /// 死亡区分
        ///        0:生存 
        ///        1:死亡 
        ///        2:消息不明
        /// </summary>
        [Column("IS_DEAD")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDead { get; set; }

        /// <summary>
        /// 死亡日
        ///        yyyymmdd        
        /// </summary>
        [Column("DEATH_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int DeathDate { get; set; }

        /// <summary>
        /// 自宅郵便番号
        ///        区切り文字("-") を除く            
        /// </summary>
        [Column("HOME_POST")]
        [MaxLength(7)]
        public string HomePost { get; set; } = string.Empty;

        /// <summary>
        /// 自宅住所１
        /// </summary>
        [Column("HOME_ADDRESS1")]
        [MaxLength(100)]
        public string HomeAddress1 { get; set; } = string.Empty;

        /// <summary>
        /// 自宅住所２
        /// </summary>
        [Column("HOME_ADDRESS2")]
        [MaxLength(100)]
        public string HomeAddress2 { get; set; } = string.Empty;

        /// <summary>
        /// 電話番号１
        /// </summary>
        [Column("TEL1")]
        [MaxLength(15)]
        public string Tel1 { get; set; } = string.Empty;

        /// <summary>
        /// 電話番号２
        /// </summary>
        [Column("TEL2")]
        [MaxLength(15)]
        public string Tel2 { get; set; } = string.Empty;

        /// <summary>
        /// E-Mailアドレス
        /// </summary>
        [Column("MAIL")]
        [MaxLength(100)]
        public string Mail { get; set; } = string.Empty;

        /// <summary>
        /// 世帯主名
        /// </summary>
        [Column("SETAINUSI")]
        [MaxLength(100)]
        public string Setanusi { get; set; } = string.Empty;

        /// <summary>
        /// 続柄
        /// </summary>
        [Column("ZOKUGARA")]
        [MaxLength(20)]
        public string Zokugara { get; set; } = string.Empty;

        /// <summary>
        /// 職業
        /// </summary>
        [Column("JOB")]
        [MaxLength(40)]
        public string Job { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先名称
        /// </summary>
        [Column("RENRAKU_NAME")]
        [MaxLength(100)]
        public string RenrakuName { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先郵便番号
        /// </summary>
        [Column("RENRAKU_POST")]
        [MaxLength(7)]
        public string RenrakuPost { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先住所１
        /// </summary>
        [Column("RENRAKU_ADDRESS1")]
        [MaxLength(100)]
        public string RenrakuAddress1 { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先住所２
        /// </summary>
        [Column("RENRAKU_ADDRESS2")]
        [MaxLength(100)]
        public string RenrakuAddress2 { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先電話番号
        /// </summary>
        [Column("RENRAKU_TEL")]
        [MaxLength(15)]
        public string RenrakuTel { get; set; } = string.Empty;

        /// <summary>
        /// 連絡先電話番号
        /// </summary>
        [Column("RENRAKU_MEMO")]
        [MaxLength(100)]
        public string RenrakuMemo { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先名称
        /// </summary>
        [Column("OFFICE_NAME")]
        [MaxLength(100)]
        public string OfficeName { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先郵便番号
        /// </summary>
        [Column("OFFICE_POST")]
        [MaxLength(7)]
        public string OfficePost { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先住所１
        /// </summary>
        [Column("OFFICE_ADDRESS1")]
        [MaxLength(100)]
        public string OfficeAddress1 { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先住所２
        /// </summary>
        [Column("OFFICE_ADDRESS2")]
        [MaxLength(100)]
        public string OfficeAddress2 { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先電話番号
        /// </summary>
        [Column("OFFICE_TEL")]
        [MaxLength(15)]
        public string OfficeTel { get; set; } = string.Empty;

        /// <summary>
        /// 勤務先備考
        /// </summary>
        [Column("OFFICE_MEMO")]
        [MaxLength(100)]
        public string OfficeMemo { get; set; } = string.Empty;

        /// <summary>
        /// 領収証明細発行区分
        ///        0:不要 
        ///        1:要
        /// </summary>
        [Column("IS_RYOSYO_DETAIL")]
        [CustomAttribute.DefaultValue(1)]
        public int IsRyosyoDetail { get; set; }

        /// <summary>
        /// 主治医コード
        /// </summary>
        [Column("PRIMARY_DOCTOR")]
        public int PrimaryDoctor { get; set; }

        /// <summary>
        /// テスト患者区分
        ///        1:テスト患者
        /// </summary>
        [Column("IS_TESTER")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTester { get; set; }

        /// <summary>
        /// 削除区分
        ///        1:削除
        /// </summary>
        [Column("IS_DELETE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }


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
        public string CreateMachine { get; set; } = string.Empty;

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
        public string UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// MAIN_HOKEN_PID
        /// </summary>
        [Column("MAIN_HOKEN_PID")]
        [CustomAttribute.DefaultValue(0)]
        public int MainHokenPid { get; set; }

        /// <summary>
        /// REFERENCE_NO
        /// </summary>
        [Column("REFERENCE_NO")]
        [CustomAttribute.DefaultValue(0)]
        public long ReferenceNo { get; set; }

        /// <summary>
        /// LIMIT_CONS_FLG
        /// </summary>
        [Column("LIMIT_CONS_FLG")]
        [CustomAttribute.DefaultValue(0)]
        public int LimitConsFlg { get; set; }

    }
}
