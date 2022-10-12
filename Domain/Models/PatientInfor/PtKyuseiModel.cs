namespace Domain.Models.PatientInfor
{
    public class PtKyuseiModel
    {
        public PtKyuseiModel(int hpId, long ptId, long seqNo, string kanaName, string name, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            KanaName = kanaName;
            Name = name;
            EndDate = endDate;
        }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public long SeqNo { get; set; }

        public string KanaName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int EndDate { get; set; }
    }
}
