using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.Diseases

{
    public class UpsertPtDiseaseListItem
    {
        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public long Id { get; set; }

        [Required]
        public long PtId { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
        public int SortNo { get; set; }

        public List<PrefixSuffixRequest> PrefixList { get; set; } = new List<PrefixSuffixRequest>();

        public List<PrefixSuffixRequest> SuffixList { get; set; } = new List<PrefixSuffixRequest>();

        [Required]
        [MaxLength(160)]
        public string Byomei { get; set; } = string.Empty;

        [MaxLength(7)]
        public string ByomeiCd { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public int StartDate { get; set; }

        [Required]
        public int TenkiKbn { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public int TenkiDate { get; set; }

        [Required]
        [Range(0, 1)]
        public int SyubyoKbn { get; set; }

        [Required]
        public int SikkanKbn { get; set; }

        [Required]
        public int NanByoCd { get; set; }

        [MaxLength(80)]
        public string HosokuCmt { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public int HokenPid { get; set; }

        [Required]
        [Range(0, 1)]
        public int IsNodspRece { get; set; }

        [Required]
        [Range(0, 1)]
        public int IsNodspKarte { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
        public long SeqNo { get; set; }

        [Required]
        [Range(0, 1)]
        public int IsImportant { get; set; }

        [Required]
        [Range(0, 1)]
        public int IsDeleted { get; set; }
    }

    public class PrefixSuffixRequest
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
