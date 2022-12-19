using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RSVKRT_ODR_INF")]
    public class RsvkrtOdrInf : EmrCloneable<RsvkrtOdrInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        [Column("RSV_DATE")]
        public int RsvDate { get; set; }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        
        [Column("RSVKRT_NO", Order = 3)]
        public long RsvkrtNo { get; set; }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        
        [Column("RP_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public long RpNo { get; set; }

        /// <summary>
        /// 剤枝番
        /// 剤に変更があった場合、カウントアップ
        /// </summary>
        
        [Column("RP_EDA_NO", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public long RpEdaNo { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        
        [Column("ID", Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 保険組合せID
        /// </summary>
        [Column("HOKEN_PID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// オーダー行為区分
        /// 
        /// </summary>
        [Column("ODR_KOUI_KBN")]
        public int OdrKouiKbn { get; set; }

        /// <summary>
        /// 剤名称
        /// 
        /// </summary>
        [Column("RP_NAME")]
        [MaxLength(240)]
        public string? RpName { get; set; } = string.Empty;

        /// <summary>
        /// 院内院外区分
        /// "0: 院内
        /// 1: 院外"
        /// </summary>
        [Column("INOUT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int InoutKbn { get; set; }

        /// <summary>
        /// 至急区分
        /// "0:通常 
        /// 1:至急"
        /// </summary>
        [Column("SIKYU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SikyuKbn { get; set; }

        /// <summary>
        /// 処方種別
        /// "0: 日数判断
        /// 1: 臨時
        /// 2: 常態"
        /// </summary>
        [Column("SYOHO_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int SyohoSbt { get; set; }

        /// <summary>
        /// 算定区分
        /// "1: 算定外
        /// 2: 自費算定"
        /// </summary>
        [Column("SANTEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiKbn { get; set; }

        /// <summary>
        /// 透析区分
        /// "0: 透析以外
        /// 1: 透析前
        /// 2: 透析後"
        /// </summary>
        [Column("TOSEKI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TosekiKbn { get; set; }

        /// <summary>
        /// 日数回数
        /// 処方日数
        /// </summary>
        [Column("DAYS_CNT")]
        [CustomAttribute.DefaultValue(0)]
        public int DaysCnt { get; set; }

        /// <summary>
        /// 削除区分
        /// "1: 削除
        /// 2: 実施"
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
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
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
