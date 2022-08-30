using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.OrdInfs
{
    public class OdrInfDetailItem
    {
        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int HpId { get;  set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long RaiinNo { get;  set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long RpNo { get;  set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long RpEdaNo { get;  set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int RowNo { get;  set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long PtId { get;  set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int SinDate { get;  set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public int SinKouiKbn { get;  set; }

        [MaxLength(10)]
        public string ItemCd { get;  set; } = string.Empty;

        [MaxLength(10)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public double Suryo { get;  set; }

        [MaxLength(24)]
        public string UnitName { get; set; } = string.Empty;

        [Required]
        [Range(0,2)]
        public int UnitSbt { get;  set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public double TermVal { get;  set; }

        [Required]
        public int KohatuKbn { get;  set; }

        [Required]
        [Range(0,3)]
        public int SyohoKbn { get;  set; }

        [Required]
        [Range(0, 3)]
        public int SyohoLimitKbn { get;  set; }

        [Required]
        public int DrugKbn { get;  set; }

        [Required]
        [Range(0,2)]
        public int YohoKbn { get;  set; }

        public string Kokuji1 { get; set; } = string.Empty;

        public string Kokuji2 { get; set; } = string.Empty;

        [Required]
        public int IsNodspRece { get;  set; }

        [MaxLength(12)]
        public string IpnCd { get; set; } = string.Empty;

        [MaxLength(120)]
        public string IpnName { get; set; } = string.Empty;

        [Required]
        [Range(0,1)]
        public int JissiKbn { get;  set; }

        public DateTime? JissiDate { get;  set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int JissiId { get;  set; }

        [MaxLength(60)]
        public string JissiMachine { get; set; } = string.Empty;

        [MaxLength(10)]
        public string ReqCd { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Bunkatu { get; set; } = string.Empty;

        [MaxLength(240)]
        public string CmtName { get; set; } = string.Empty;

        [MaxLength(38)]
        public string CmtOpt { get; set; } = string.Empty;

        [MaxLength(8)]
        public string FontColor { get; set; } = string.Empty;

        [Required]
        [Range(0, 1)]
        public int CommentNewline { get;  set; }
    }
}
