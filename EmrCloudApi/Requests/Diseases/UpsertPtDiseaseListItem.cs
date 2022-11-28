namespace EmrCloudApi.Requests.Diseases

{
    public class UpsertPtDiseaseListItem
    {
        public long Id { get; set; }
        public long PtId { get; set; }
        public int HpId { get; set; }
        public int SortNo { get; set; }
        public List<PrefixSuffixRequest> PrefixList { get; set; } = new List<PrefixSuffixRequest>();
        public List<PrefixSuffixRequest> SuffixList { get; set; } = new List<PrefixSuffixRequest>();
        public string Byomei { get; set; } = string.Empty;
        public string ByomeiCd { get; set; } = string.Empty;
        public int StartDate { get; set; }
        public int TenkiKbn { get; set; }
        public int TenkiDate { get; set; }
        public int SyubyoKbn { get; set; }
        public int SikkanKbn { get; set; }
        public int NanByoCd { get; set; }
        public string HosokuCmt { get; set; } = string.Empty;
        public int HokenPid { get; set; }
        public int IsNodspRece { get; set; }
        public int IsNodspKarte { get; set; }
        public long SeqNo { get; set; }
        public int IsImportant { get; set; }
        public int IsDeleted { get; set; }
    }

    public class PrefixSuffixRequest
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
