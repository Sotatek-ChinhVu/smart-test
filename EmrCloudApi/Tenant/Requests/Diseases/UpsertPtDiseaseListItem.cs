using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.Diseases

{
    public class UpsertPtDiseaseListItem
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long PtId { get; set; }

        [Required]
        public int SortNo { get; set; }

        [MaxLength(7)]
        public string SyusyokuCd1 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd2 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd3 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd4 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd5 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd6 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd7 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd8 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd9 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd10 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd11 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd12 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd13 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd14 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd15 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd16 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd17 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd18 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd19 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd20 { get; set; } = string.Empty;

        [MaxLength(7)]
        public string SyusyokuCd21 { get; set; } = string.Empty;

        [MaxLength(160)]
        public string Byomei { get; set; } = string.Empty;

        [Required]
        public int StartDate { get; set; }

        [Required]
        public int TenkiKbn { get; set; }

        [Required]
        public int TenkiDate { get; set; }

        public int SyubyoKbn { get; set; }

        [Required]
        public int SikkanKbn { get; set; }

        [Required]
        public int NanByoCd { get; set; }

        [MaxLength(80)]
        public string HosokuCmt { get; set; } = string.Empty;

        [Required]
        public int HokenPid { get; set; }

        [Required]
        public int IsNodspRece { get; set; }

        [Required]
        public int IsNodspKarte { get; set; }

        [Required]
        public long SeqNo { get; set; }

        [Required]
        public int IsImportant { get; set; }

        [Required]
        public int IsDeleted { get; set; }
    }
}
