using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.OrdInfs
{
    public class OdrInfItem
    {
        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public long Id { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int HpId { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long RaiinNo { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long RpNo { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long RpEdaNo { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public long PtId { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int SinDate { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int HokenPid { get; set; }

        [Required]
        public int OdrKouiKbn { get; set; }

        [MaxLength (240)]
        public string RpName { get; set; } = string.Empty;

        [Required]
        [Range(0,1)]
        public int InoutKbn { get; set; }

        [Required]
        [Range(0, 1)]
        public int SikyuKbn { get; set; }

        [Required]
        [Range(0, 2)]
        public int SyohoSbt { get; set; }

        [Required]
        [Range(0, 2)]
        public int SanteiKbn { get; set; }

        [Required]
        [Range(0, 2)]
        public int TosekiKbn { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public int DaysCnt { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int SortNo { get; set; }

        [Required]
        [Range(0, 2)]
        public int IsDeleted { get; set; }

        [Required]
        public List<OdrInfDetailItem> OdrDetails { get; set; } = new();
    }
}
