namespace Domain.Models.InsuranceMst
{
    public class HokensyaMstModel
    {
        public HokensyaMstModel(int hpId, string name, string kanaName, string houbetuKbn, string houbetu, int hokenKbn, int prefNo, string hokensyaNo, string kigo, string bango, int rateHonnin, int rateKazoku, string postCode, string address1, string address2, string tel1, int isKigoNa)
        {
            HpId = hpId;
            Name = name;
            KanaName = kanaName;
            HoubetuKbn = houbetuKbn;
            Houbetu = houbetu;
            HokenKbn = hokenKbn;
            PrefNo = prefNo;
            HokensyaNo = hokensyaNo;
            Kigo = kigo;
            Bango = bango;
            RateHonnin = rateHonnin;
            RateKazoku = rateKazoku;
            PostCode = postCode;
            Address1 = address1;
            Address2 = address2;
            Tel1 = tel1;
            IsKigoNa = isKigoNa;
        }

        public HokensyaMstModel(int isKigoNa)
        {
            Name = string.Empty;
            KanaName = string.Empty;
            HoubetuKbn = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            Name = string.Empty;
            Kigo = string.Empty;
            Bango = string.Empty;
            PostCode = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            Tel1 = string.Empty;
            IsKigoNa = isKigoNa;
        }

        public HokensyaMstModel()
        {
            Name = string.Empty;
            KanaName = string.Empty;
            HoubetuKbn = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            Name = string.Empty;
            Kigo = string.Empty;
            Bango = string.Empty;
            PostCode = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            Tel1 = string.Empty;
        }

        public int HpId { get; private set; }

        public string Name { get; private set; }

        public string KanaName { get; private set; }

        public string HoubetuKbn { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenKbn { get; private set; }

        public int PrefNo { get; private set; }

        public string HokensyaNo { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public int RateHonnin { get; private set; }

        public int RateKazoku { get; private set; }

        public string PostCode { get; private set; }

        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string Tel1 { get; private set; }

        public int IsKigoNa { get; private set; }

        public bool IsReadOnlyHokenSyaNo
        {
            get { return !string.IsNullOrEmpty(HokensyaNo); }
        }

        public string PostCdDisplay
        {
            get
            {
                if (!string.IsNullOrEmpty(PostCode))
                {
                    if (PostCode.Length > 3)
                    {
                        return PostCode.Substring(0, 3) + "-" + PostCode.Substring(3);
                    }
                }
                return PostCode;
            }
        }
    }
}
