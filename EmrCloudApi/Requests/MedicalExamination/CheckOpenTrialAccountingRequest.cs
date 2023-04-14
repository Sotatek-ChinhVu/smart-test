namespace EmrCloudApi.Requests.MedicalExamination
{
    public class CheckOpenTrialAccountingRequest
    {
        public int HpId { get; set; }
        public int PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public int SyosaiKbn { get; set; }
        public List<Tuple<string, string>> AllOdrInfItem { get; set; } = new();
        public List<int> OdrInfHokenPid { get; set; } = new();
        public List<string> ItemCds { get; set; } = new();
    }
}
