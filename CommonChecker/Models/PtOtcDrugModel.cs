namespace CommonChecker.Models
{
    public class PtOtcDrugModel
    {
        public PtOtcDrugModel(int hpId, long ptId, long seqNo, int sortNo, int serialNum, string tradeName, int startDate, int endDate, string cmt, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            SerialNum = serialNum;
            TradeName = tradeName;
            StartDate = startDate;
            EndDate = endDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public int SerialNum { get; private set; }

        public string TradeName { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string Cmt { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
