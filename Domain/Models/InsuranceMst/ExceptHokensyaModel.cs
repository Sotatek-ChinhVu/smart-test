namespace Domain.Models.InsuranceMst
{
    public class ExceptHokensyaModel
    {
        public ExceptHokensyaModel(long id, int hpId, int prefNo, int hokenNo, int hokenEdaNo, int startDate, string hokensyaNo)
        {
            Id = id;
            HpId = hpId;
            PrefNo = prefNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            StartDate = startDate;
            HokensyaNo = hokensyaNo;
        }

        public long Id { get; private set; }

        public int HpId { get; private set; }

        public int PrefNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int StartDate { get; private set; }

        public string HokensyaNo { get; private set; }
    }
}
