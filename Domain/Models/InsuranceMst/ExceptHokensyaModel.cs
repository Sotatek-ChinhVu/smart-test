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

        public long Id { get; set; }

        public int HpId { get; set; }

        public int PrefNo { get; set; }

        public int HokenNo { get; set; }

        public int HokenEdaNo { get; set; }

        public int StartDate { get; set; }

        public string HokensyaNo { get; set; }
    }
}
