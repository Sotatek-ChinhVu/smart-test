using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.Diseases

{
    public class UpsertPtDiseaseListItem
    {

        public long Id { get; set; }
        public int HpId { get; set; }

        public long PtId { get; set; }

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

        [MaxLength(7)]
        public string Byomei { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int TenkiKbn { get; set; }

        public int TenkiDate { get; set; }

        public int SyubyoKbn { get; set; }

        public int SikkanKbn { get; set; }

        public int NanByoCd { get; set; }

        [MaxLength(80)]
        public string HosokuCmt { get; set; } = string.Empty;

        public int HokenPid { get; set; }

        public int IsNodspRece { get; set; }

        public int IsNodspKarte { get; set; }

        public long SeqNo { get; set; }

        public int IsImportant { get; set; }

        public int IsDeleted { get; set; }
    }
}
