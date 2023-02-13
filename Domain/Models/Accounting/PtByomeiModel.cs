using Helper.Extension;

namespace Domain.Models.Accounting
{
    public class PtByomeiModel
    {
        private const string SUSPECTED = "の疑い";
        private const string SUSPECTED_CD = "8002";
        private const string FREE_WORD = "0000999";
        private const string MODIFIER_CD = "SyusyokuCd";
        private const string MODIFIER_NM = "SyusyokuName";

        public PtByomeiModel(string byomei, int startDate, int tenkiDate)
        {
            Byomei = byomei;
            StartDate = startDate;
            TenkiDate = tenkiDate;
        }

        public string Byomei { get; set; }

        public int StartDate { get; private set; }

        public int TenkiDate { get; private set; }

        public string HosokuCmt { get; private set; }

        public int SortNo { get; private set; }

        public int SeqNo { get; private set; }

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

        public string ByomeiCd { get; private set; }

        public string Icd10 { get; set; }

        public int SikkanCd { get; set; }
        public string Icd102013 { get; set; }
        public string Icd1012013 { get; set; }
        public string Icd1022013 { get; set; }
        public int IsNodspRece { get; set; }
        public int TenkiKbn { get; set; }
        public string FullByomei { get => string.Concat(Byomei, HosokuCmt); }
        public bool IsFreeWord
        {
            get => ByomeiCd.AsString().Equals(FREE_WORD);
            set { }
        }
    }
}
