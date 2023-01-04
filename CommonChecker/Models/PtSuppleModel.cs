namespace CommonChecker.Models
{
    public class PtSuppleModel
    {
        public PtSuppleModel(int hpId, long ptId, int seqNo, int sortNo, string indexCd, string indexWord, int startDate, int endDate, string cmt, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            IndexCd = indexCd;
            IndexWord = indexWord;
            StartDate = startDate;
            EndDate = endDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public string IndexCd { get; private set; }

        public string IndexWord { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string Cmt { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
