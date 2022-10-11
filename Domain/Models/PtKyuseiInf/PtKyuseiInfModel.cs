namespace Domain.Models.PtKyuseiInf
{
    public class PtKyuseiInfModel
    {
        public PtKyuseiInfModel(int hpId, long ptId, string kanaName, string name, int endDate, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            KanaName = kanaName;
            Name = name;
            EndDate = endDate;
            IsDeleted = isDeleted;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public string KanaName { get; set; }
        public string Name { get; set; }
        public int EndDate { get; set; }
        public int IsDeleted { get; set; }
    }
}
