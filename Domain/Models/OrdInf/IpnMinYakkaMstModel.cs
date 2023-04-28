namespace Domain.Models.OrdInf
{
    public class IpnMinYakkaMstModel
    {
        public IpnMinYakkaMstModel(int id, int hpId, string ipnNameCd, int startDate, int endDate, double yakka, int seqNo, int isDeleted, bool modelModified)
        {
            Id = id;
            HpId = hpId;
            IpnNameCd = ipnNameCd;
            StartDate = startDate;
            EndDate = endDate;
            Yakka = yakka;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
            ModelModified = modelModified;
        }
        public IpnMinYakkaMstModel()
        {
            IpnNameCd = string.Empty;
        }

        public int Id { get; private set; }
        public int HpId { get; private set; }
        public string IpnNameCd { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public double Yakka { get; private set; }
        public int SeqNo { get; private set; }
        public int IsDeleted { get; private set; }

        public bool ModelModified { get; private set; }

        public bool CheckDefaultValue() => StartDate == 0 && Yakka == 0;
    }
}
