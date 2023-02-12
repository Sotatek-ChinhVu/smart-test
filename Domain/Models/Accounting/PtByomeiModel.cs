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

        public int StartDate { get; set; }

        public int TenkiDate { get; set; }

        public string HosokuCmt { get; set; }

        public int SortNo { get; set; }

        public int SeqNo { get; set; }

        public string SyusyokuCd1 { get; set; }

        public string SyusyokuCd2 { get; set; }

        public string SyusyokuCd3 { get; set; }

        public string SyusyokuCd4 { get; set; }

        public string SyusyokuCd5 { get; set; }

        public string SyusyokuCd6 { get; set; }

        public string SyusyokuCd7 { get; set; }

        public string SyusyokuCd8 { get; set; }

        public string SyusyokuCd9 { get; set; }

        public string SyusyokuCd10 { get; set; }

        public string SyusyokuCd11 { get; set; }

        public string SyusyokuCd12 { get; set; }

        public string SyusyokuCd13 { get; set; }

        public string SyusyokuCd14 { get; set; }

        public string SyusyokuCd15 { get; set; }

        public string SyusyokuCd16 { get; set; }

        public string SyusyokuCd17 { get; set; }

        public string SyusyokuCd18 { get; set; }

        public string SyusyokuCd19 { get; set; }

        public string SyusyokuCd20 { get; set; }

        public string SyusyokuCd21 { get; set; }

        public string ByomeiCd { get; set; }

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
