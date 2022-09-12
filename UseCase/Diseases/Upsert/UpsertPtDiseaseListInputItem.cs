using Domain.Models.Diseases;

namespace UseCase.Diseases.Upsert
{
    public class UpsertPtDiseaseListInputItem
    {
        public UpsertPtDiseaseListInputItem(long id, long ptId, int sortNo, List<PrefixSuffixModel> prefixList, List<PrefixSuffixModel> suffixList, string byomei, int startDate, int tenkiKbn, int tenkiDate, int syubyoKbn, int sikkanKbn, int nanByoCd, string hosokuCmt, int hokenPid, int isNodspRece, int isNodspKarte, long seqNo, int isImportant, int isDeleted, string byomeiCd)
        {
            Id = id;
            PtId = ptId;
            SortNo = sortNo;
            PrefixList = prefixList;
            SuffixList = suffixList;
            Byomei = byomei;
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanByoCd = nanByoCd;
            HosokuCmt = hosokuCmt;
            HokenPid = hokenPid;
            IsNodspRece = isNodspRece;
            IsNodspKarte = isNodspKarte;
            SeqNo = seqNo;
            IsImportant = isImportant;
            IsDeleted = isDeleted;
            ByomeiCd = byomeiCd;
        }

        public long Id { get; private set; }
        public long PtId { get; private set; }
        public int SortNo { get; private set; }

        public List<PrefixSuffixModel> PrefixList { get; private set; }

        public List<PrefixSuffixModel> SuffixList { get; private set; }

        public string Byomei { get; private set; }
        public int StartDate { get; private set; }
        public int TenkiKbn { get; private set; }
        public int TenkiDate { get; private set; }
        public int SyubyoKbn { get; private set; }
        public int SikkanKbn { get; private set; }
        public int NanByoCd { get; private set; }
        public string HosokuCmt { get; private set; }
        public int HokenPid { get; private set; }
        public int IsNodspRece { get; private set; }
        public int IsNodspKarte { get; private set; }
        public long SeqNo { get; private set; }
        public int IsImportant { get; private set; }
        public int IsDeleted { get; private set; }
        public string ByomeiCd { get; private set; }

    }
}
