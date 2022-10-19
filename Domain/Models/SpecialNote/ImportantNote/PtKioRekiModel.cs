using Helper.Extension;

namespace Domain.Models.SpecialNote.ImportantNote
{
    public class PtKioRekiModel
    {
        public PtKioRekiModel(int hpId, long ptId, int seqNo, int sortNo, string byomeiCd, string byotaiCd, string byomei, int startDate, string cmt, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            ByomeiCd = byomeiCd;
            ByotaiCd = byotaiCd;
            Byomei = byomei;
            StartDate = startDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SeqNo { get; private set; }

        public int SortNo { get; private set; }

        public string ByomeiCd { get; private set; }

        public string ByotaiCd { get; private set; }

        public string Byomei { get; private set; }

        public int StartDate { get; private set; }

        public string Cmt { get; private set; }

        public int IsDeleted { get; private set; }
        public int FullStartDate
        {
            get
            {
                if (StartDate == 0) return 0;

                int startDateLength = StartDate.AsString().Length;
                if (startDateLength == 8)
                {
                    //Format of StartDate is yyyymmdd
                    return StartDate;
                }
                else if (startDateLength == 6)
                {
                    //Format of StartDate is yyyymm
                    //Need to convert to yyyymm01
                    return StartDate * 100 + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
