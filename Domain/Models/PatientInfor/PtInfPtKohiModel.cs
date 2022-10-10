namespace Domain.Models.PatientInfor
{
    public class PtInfPtKohiModel
    {
        public PtInfPtKohiModel(long seqNo, int hokenId, int prefNo, int hokenNo, int hokenEdaNo, string futansyaNo, string jyukyusyaNo, string tokusyuNo, int sikakuDate, int kofuDate, int startDate, int endDate, int rate, int gendogaku, int hokenSbtKbn, string houbetu, List<PtHokenCheckModel> hokenChecks)
        {
            SeqNo = seqNo;
            HokenId = hokenId;
            PrefNo = prefNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
            TokusyuNo = tokusyuNo;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            StartDate = startDate;
            EndDate = endDate;
            Rate = rate;
            GendoGaku = gendogaku;
            HokenSbtKbn = hokenSbtKbn;
            Houbetu = houbetu;
            HokenChecks = hokenChecks;
        }

        public long SeqNo { get; private set; }
        public int HokenId { get; private set; }
        public int PrefNo { get; private set; }
        public int HokenNo { get; private set; }
        public int HokenEdaNo { get; private set; }
        public string FutansyaNo { get; private set; }
        public string JyukyusyaNo { get; private set; }
        public string TokusyuNo { get; private set; }
        public int SikakuDate { get; private set; }
        public int KofuDate { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public int Rate { get; private set; }
        public int GendoGaku { get; private set; }
        public int HokenSbtKbn { get; private set; }
        public string Houbetu { get; private set; }
        public List<PtHokenCheckModel> HokenChecks { get; private set; } = new List<PtHokenCheckModel>();
    }
}