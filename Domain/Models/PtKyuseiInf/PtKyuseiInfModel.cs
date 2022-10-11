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

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public string KanaName { get; private set; }
        public string Name { get; private set; }
        public int EndDate { get; private set; }
        public int IsDeleted { get; private set; }

        public string FirstName
        {
            get => Name.Substring(0, Name.IndexOf("　"));
        }

        public string LastName
        {
            get => Name.Substring(Name.IndexOf("　") + 1);
        }

        public string FirstKanaName
        {
            get => KanaName.Substring(0, KanaName.IndexOf(" "));
        }

        public string LastKanaName
        {
            get => KanaName.Substring(KanaName.IndexOf(" ") + 1);
        }
    }
}
