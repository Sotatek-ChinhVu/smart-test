using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rece_inf_jd")]
    public class ReceInfJd : EmrCloneable<ReceInfJd>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        
        [Column("seikyu_ym", Order = 2)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        
        [Column("hoken_id", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費保険ID
        /// 
        /// </summary>
        
        [Column("kohi_id", Order = 6)]
        public int KohiId { get; set; }

        /// <summary>
        /// 負担者種別コード
        ///     1:保険 2:公1 3:公2 4:公3 5:公4
        /// </summary>
        [Column("futan_sbt_cd")]
        public int FutanSbtCd { get; set; }

        /// <summary>
        /// 受診等区分コード(1日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu1")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu1 { get; set; }

        /// <summary>
        /// 　受診等区分コード(2日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu2")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu2 { get; set; }

        /// <summary>
        /// 　受診等区分コード(3日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu3")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu3 { get; set; }

        /// <summary>
        /// 　受診等区分コード(4日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu4")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu4 { get; set; }

        /// <summary>
        /// 　受診等区分コード(5日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu5")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu5 { get; set; }

        /// <summary>
        /// 　受診等区分コード(6日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu6")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu6 { get; set; }

        /// <summary>
        /// 　受診等区分コード(7日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu7")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu7 { get; set; }

        /// <summary>
        /// 　受診等区分コード(8日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu8")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu8 { get; set; }

        /// <summary>
        /// 　受診等区分コード(9日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu9")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu9 { get; set; }

        /// <summary>
        /// 　受診等区分コード(10日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu10")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu10 { get; set; }

        /// <summary>
        /// 　受診等区分コード(11日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu11")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu11 { get; set; }

        /// <summary>
        /// 　受診等区分コード(12日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu12")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu12 { get; set; }

        /// <summary>
        /// 　受診等区分コード(13日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu13")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu13 { get; set; }

        /// <summary>
        /// 　受診等区分コード(14日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu14")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu14 { get; set; }

        /// <summary>
        /// 　受診等区分コード(15日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu15")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu15 { get; set; }

        /// <summary>
        /// 　受診等区分コード(16日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu16")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu16 { get; set; }

        /// <summary>
        /// 　受診等区分コード(17日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu17")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu17 { get; set; }

        /// <summary>
        /// 　受診等区分コード(18日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu18")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu18 { get; set; }

        /// <summary>
        /// 　受診等区分コード(19日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu19")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu19 { get; set; }

        /// <summary>
        /// 　受診等区分コード(20日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu20")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu20 { get; set; }

        /// <summary>
        /// 　受診等区分コード(21日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu21")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu21 { get; set; }

        /// <summary>
        /// 　受診等区分コード(22日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu22")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu22 { get; set; }

        /// <summary>
        /// 　受診等区分コード(23日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu23")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu23 { get; set; }

        /// <summary>
        /// 　受診等区分コード(24日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu24")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu24 { get; set; }

        /// <summary>
        /// 　受診等区分コード(25日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu25")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu25 { get; set; }

        /// <summary>
        /// 　受診等区分コード(26日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu26")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu26 { get; set; }

        /// <summary>
        /// 　受診等区分コード(27日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu27")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu27 { get; set; }

        /// <summary>
        /// 　受診等区分コード(28日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu28")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu28 { get; set; }

        /// <summary>
        /// 　受診等区分コード(29日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu29")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu29 { get; set; }

        /// <summary>
        /// 　受診等区分コード(30日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu30")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu30 { get; set; }

        /// <summary>
        /// 　受診等区分コード(31日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        [Column("nissu31")]
        [CustomAttribute.DefaultValue(2)]
        public int Nissu31 { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;
    }
}
