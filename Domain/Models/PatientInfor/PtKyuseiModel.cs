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
            IsDeleted = false;
        }

        public PtKyuseiModel(int hpId, long ptId, long seqNo, string kanaName, string name, int endDate, bool isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            KanaName = kanaName;
            Name = name;
            EndDate = endDate;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string KanaName { get; private set; }

        public string Name { get; private set; }

        public int EndDate { get; private set; }

        public bool IsDeleted { get; private set; }
    }
}
