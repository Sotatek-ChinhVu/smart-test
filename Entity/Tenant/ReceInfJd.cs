using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RECE_INF_JD")]
    public class ReceInfJd : EmrCloneable<ReceInfJd>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        
        [Column("SEIKYU_YM", Order = 2)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("PT_ID", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("SIN_YM", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        
        [Column("HOKEN_ID", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費保険ID
        /// 
        /// </summary>
        
        [Column("KOHI_ID", Order = 6)]
        public int KohiId { get; set; }

        /// <summary>
        /// 負担者種別コード
        ///     1:保険 2:公1 3:公2 4:公3 5:公4
        /// </summary>
        [Column("FUTAN_SBT_CD")]
        public int FutanSbtCd { get; set; }

        /// <summary>
        /// 受診等区分コード(1日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU1")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu1 { get; set; }

        /// <summary>
        /// 　受診等区分コード(2日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU2")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu2 { get; set; }

        /// <summary>
        /// 　受診等区分コード(3日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU3")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu3 { get; set; }

        /// <summary>
        /// 　受診等区分コード(4日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU4")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu4 { get; set; }

        /// <summary>
        /// 　受診等区分コード(5日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU5")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu5 { get; set; }

        /// <summary>
        /// 　受診等区分コード(6日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU6")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu6 { get; set; }

        /// <summary>
        /// 　受診等区分コード(7日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU7")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu7 { get; set; }

        /// <summary>
        /// 　受診等区分コード(8日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU8")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu8 { get; set; }

        /// <summary>
        /// 　受診等区分コード(9日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU9")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu9 { get; set; }

        /// <summary>
        /// 　受診等区分コード(10日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU10")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu10 { get; set; }

        /// <summary>
        /// 　受診等区分コード(11日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU11")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu11 { get; set; }

        /// <summary>
        /// 　受診等区分コード(12日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU12")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu12 { get; set; }

        /// <summary>
        /// 　受診等区分コード(13日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU13")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu13 { get; set; }

        /// <summary>
        /// 　受診等区分コード(14日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU14")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu14 { get; set; }

        /// <summary>
        /// 　受診等区分コード(15日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU15")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu15 { get; set; }

        /// <summary>
        /// 　受診等区分コード(16日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU16")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu16 { get; set; }

        /// <summary>
        /// 　受診等区分コード(17日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU17")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu17 { get; set; }

        /// <summary>
        /// 　受診等区分コード(18日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU18")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu18 { get; set; }

        /// <summary>
        /// 　受診等区分コード(19日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU19")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu19 { get; set; }

        /// <summary>
        /// 　受診等区分コード(20日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU20")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu20 { get; set; }

        /// <summary>
        /// 　受診等区分コード(21日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU21")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu21 { get; set; }

        /// <summary>
        /// 　受診等区分コード(22日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU22")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu22 { get; set; }

        /// <summary>
        /// 　受診等区分コード(23日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU23")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu23 { get; set; }

        /// <summary>
        /// 　受診等区分コード(24日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU24")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu24 { get; set; }

        /// <summary>
        /// 　受診等区分コード(25日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU25")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu25 { get; set; }

        /// <summary>
        /// 　受診等区分コード(26日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU26")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu26 { get; set; }

        /// <summary>
        /// 　受診等区分コード(27日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU27")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu27 { get; set; }

        /// <summary>
        /// 　受診等区分コード(28日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU28")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu28 { get; set; }

        /// <summary>
        /// 　受診等区分コード(29日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU29")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu29 { get; set; }

        /// <summary>
        /// 　受診等区分コード(30日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU30")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu30 { get; set; }

        /// <summary>
        /// 　受診等区分コード(31日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("NISSU31")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu31 { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
    }
}
