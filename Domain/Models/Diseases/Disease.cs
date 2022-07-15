using Domain.CommonObject;

namespace Domain.Models.Diseases
{
    public class Disease
    {
        public Disease(int hpId, long ptId, long seqNo, string byomeiCd, int sortNo, 
            string syusyokuCd1, string syusyokuCd2, string syusyokuCd3, string syusyokuCd4, 
            string syusyokuCd5, string syusyokuCd6, string syusyokuCd7, string syusyokuCd8, 
            string syusyokuCd9, string syusyokuCd10, string syusyokuCd11, string syusyokuCd12, 
            string syusyokuCd13, string syusyokuCd14, string syusyokuCd15, string syusyokuCd16, 
            string syusyokuCd17, string syusyokuCd18, string syusyokuCd19, string syusyokuCd20, 
            string syusyokuCd21, string byomei, int startDate, int tenkiKbn, int tenkiDate, 
            int syubyoKbn, int sikkanKbn, int nanbyoCd, string hosokuCmt, int hokenPid, 
            int togetuByomei, int isNodspRece, int isNodspKarte, int isDeleted, long id, 
            int isImportant, int sinDate)
        {
            HpId = HpId.From(hpId);
            PtId = PtId.From(ptId);
            SeqNo = SeqNo.From(seqNo);
            ByomeiCd = ByomeiCd.From(byomeiCd);
            SortNo = sortNo;
            SyusyokuCd1 = syusyokuCd1;
            SyusyokuCd2 = syusyokuCd2;
            SyusyokuCd3 = syusyokuCd3;
            SyusyokuCd4 = syusyokuCd4;
            SyusyokuCd5 = syusyokuCd5;
            SyusyokuCd6 = syusyokuCd6;
            SyusyokuCd7 = syusyokuCd7;
            SyusyokuCd8 = syusyokuCd8;
            SyusyokuCd9 = syusyokuCd9;
            SyusyokuCd10 = syusyokuCd10;
            SyusyokuCd11 = syusyokuCd11;
            SyusyokuCd12 = syusyokuCd12;
            SyusyokuCd13 = syusyokuCd13;
            SyusyokuCd14 = syusyokuCd14;
            SyusyokuCd15 = syusyokuCd15;
            SyusyokuCd16 = syusyokuCd16;
            SyusyokuCd17 = syusyokuCd17;
            SyusyokuCd18 = syusyokuCd18;
            SyusyokuCd19 = syusyokuCd19;
            SyusyokuCd20 = syusyokuCd20;
            SyusyokuCd21 = syusyokuCd21;
            Byomei = byomei;
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = nanbyoCd;
            HosokuCmt = hosokuCmt;
            HokenPid = hokenPid;
            TogetuByomei = togetuByomei;
            IsNodspRece = isNodspRece;
            IsNodspKarte = isNodspKarte;
            IsDeleted = isDeleted;
            Id = id;
            IsImportant = isImportant;
            SinDate = SinDate.From(sinDate);
        }

        public HpId HpId { get; private set; }
        public PtId PtId { get; private set; }
        public SeqNo SeqNo { get; private set; }
        public ByomeiCd ByomeiCd { get; private set; }
        public int SortNo { get; private set; }
        public string SyusyokuCd1 { get; private set; }
        public string SyusyokuCd2 { get; private set; }
        public string SyusyokuCd3 { get; private set; }
        public string SyusyokuCd4 { get; private set; }
        public string SyusyokuCd5 { get; private set; }
        public string SyusyokuCd6 { get; private set; }
        public string SyusyokuCd7 { get; private set; }
        public string SyusyokuCd8 { get; private set; }
        public string SyusyokuCd9 { get; private set; }
        public string SyusyokuCd10 { get; private set; }
        public string SyusyokuCd11 { get; private set; }
        public string SyusyokuCd12 { get; private set; }
        public string SyusyokuCd13 { get; private set; }
        public string SyusyokuCd14 { get; private set; }
        public string SyusyokuCd15 { get; private set; }
        public string SyusyokuCd16 { get; private set; }
        public string SyusyokuCd17 { get; private set; }
        public string SyusyokuCd18 { get; private set; }
        public string SyusyokuCd19 { get; private set; }
        public string SyusyokuCd20 { get; private set; }
        public string SyusyokuCd21 { get; private set; }
        public string Byomei { get; private set; }
        public int StartDate { get; private set; }
        public int TenkiKbn { get; private set; }
        public int TenkiDate { get; private set; }
        public int SyubyoKbn { get; private set; }
        public int SikkanKbn { get; private set; }
        public int NanbyoCd { get; private set; }
        public string HosokuCmt { get; private set; }
        public int HokenPid { get; private set; }
        public int TogetuByomei { get; private set; }
        public int IsNodspRece { get; private set; }
        public int IsNodspKarte { get; private set; }
        public int IsDeleted { get; private set; }
        public long Id { get; private set; }
        public int IsImportant { get; private set; }
        public SinDate SinDate { get; private set; }
    }
}
