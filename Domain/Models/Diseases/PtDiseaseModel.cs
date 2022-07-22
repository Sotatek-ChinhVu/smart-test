namespace Domain.Models.Diseases
{
    public class PtDiseaseModel
    {
        public PtDiseaseModel(int hpId, long ptId, long seqNo, string byomeiCd, int sortNo, 
            string syusyokuCd1, string syusyokuCd2, string syusyokuCd3, string syusyokuCd4, 
            string syusyokuCd5, string syusyokuCd6, string syusyokuCd7, string syusyokuCd8, 
            string syusyokuCd9, string syusyokuCd10, string syusyokuCd11, string syusyokuCd12, 
            string syusyokuCd13, string syusyokuCd14, string syusyokuCd15, string syusyokuCd16, 
            string syusyokuCd17, string syusyokuCd18, string syusyokuCd19, string syusyokuCd20, 
            string syusyokuCd21, string byomei, int startDate, int tenkiKbn, int tenkiDate, 
            int syubyoKbn, int sikkanKbn, int nanbyoCd, int isNodspRece, int isNodspKarte, 
            int isDeleted, long id, int isImportant, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            ByomeiCd = byomeiCd;
            SortNo = sortNo;
            SyusyokuCd = new Dictionary<string, string>()
            {
                {"SyusyokuCd1" , syusyokuCd1 != null ? syusyokuCd1 : "" },
                {"SyusyokuCd2" , syusyokuCd2 != null ? syusyokuCd2 : "" },
                {"SyusyokuCd3" , syusyokuCd3 != null ? syusyokuCd3 : "" },
                {"SyusyokuCd4" , syusyokuCd4 != null ? syusyokuCd4 : "" },
                {"SyusyokuCd5" , syusyokuCd5 != null ? syusyokuCd5 : "" },
                {"SyusyokuCd6" , syusyokuCd6 != null ? syusyokuCd6 : "" },
                {"SyusyokuCd7" , syusyokuCd7 != null ? syusyokuCd7 : "" },
                {"SyusyokuCd8" , syusyokuCd8 != null ? syusyokuCd8 : "" },
                {"SyusyokuCd9" , syusyokuCd9 != null ? syusyokuCd9 : "" },
                {"SyusyokuCd10" , syusyokuCd10 != null ? syusyokuCd10 : "" },
                {"SyusyokuCd11" , syusyokuCd11 != null ? syusyokuCd11 : "" },
                {"SyusyokuCd12" , syusyokuCd12 != null ? syusyokuCd12 : "" },
                {"SyusyokuCd13" , syusyokuCd13 != null ? syusyokuCd13 : "" },
                {"SyusyokuCd14" , syusyokuCd14 != null ? syusyokuCd14 : "" },
                {"SyusyokuCd15" , syusyokuCd15 != null ? syusyokuCd15 : "" },
                {"SyusyokuCd16" , syusyokuCd16 != null ? syusyokuCd16 : "" },
                {"SyusyokuCd17" , syusyokuCd17 != null ? syusyokuCd17 : "" },
                {"SyusyokuCd18" , syusyokuCd18 != null ? syusyokuCd18 : "" },
                {"SyusyokuCd19" , syusyokuCd19 != null ? syusyokuCd19 : "" },
                {"SyusyokuCd20" , syusyokuCd20 != null ? syusyokuCd20 : "" },
                {"SyusyokuCd21" , syusyokuCd21 != null ? syusyokuCd21 : "" }
            };
            Byomei = byomei;
            IsSuspect = 0;
            foreach (var item in SyusyokuCd.Select(e => e.Value)) {
                if (item != null && item.Equals("8002"))
                {
                        IsSuspect = 1;
                }
            }
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = nanbyoCd;
            IsNodspRece = isNodspRece;
            IsNodspKarte = isNodspKarte;
            IsDeleted = isDeleted;
            Id = id;
            IsImportant = isImportant;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long SeqNo { get; private set; }
        public string ByomeiCd { get; private set; }
        public int SortNo { get; private set; }
        public Dictionary<string, string>? SyusyokuCd { get; private set; }
        public string Byomei { get; private set; }
        public int IsSuspect { get; private set; }
        public int StartDate { get; private set; }
        public int TenkiKbn { get; private set; }
        public int TenkiDate { get; private set; }
        public int SyubyoKbn { get; private set; }
        public int SikkanKbn { get; private set; }
        public int NanbyoCd { get; private set; }
        public int IsNodspRece { get; private set; }
        public int IsNodspKarte { get; private set; }
        public int IsDeleted { get; private set; }
        public long Id { get; private set; }
        public int IsImportant { get; private set; }
        public int SinDate { get; private set; }
    }
}
