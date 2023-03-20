using Helper.Common;

namespace UseCase.Receipt.GetInsuranceInf
{
    public class InsuranceInfDto
    {
        public InsuranceInfDto(string insuranceName, int hokenKbn, int nissu, int tensu, int ichibuFutan, string edaNo, string kigo, string bango, bool kohi1ReceKisai, int kohi1Id, bool kohi2ReceKisai, int kohi2Id, bool kohi3ReceKisai, int kohi3Id, bool kohi4ReceKisai, string kohi1FutansyaNo, string kohi1JyukyusyaNo, string kohi2FutansyaNo, string kohi2JyukyusyaNo, string kohi3FutansyaNo, string kohi3JyukyusyaNo, string kohi4FutansyaNo, string kohi4JyukyusyaNo, string hokensyaNo)
        {
            InsuranceName = insuranceName;
            HokenKbn = hokenKbn;
            Nissu = nissu;
            Tensu = CIUtil.FormatIntToString(tensu);
            IchibuFutan = CIUtil.FormatIntToString(ichibuFutan);
            EdaNo = (HokenKbn > 0 && (HokenKbn < 11 || HokenKbn > 14)) ? edaNo : string.Empty;
            Kigo = kigo;
            Bango = bango;
            Kohi1ReceKisai = kohi1ReceKisai;
            Kohi1Id = kohi1Id;
            Kohi2ReceKisai = kohi2ReceKisai;
            Kohi2Id = kohi2Id;
            Kohi3ReceKisai = kohi3ReceKisai;
            Kohi3Id = kohi3Id;
            Kohi4ReceKisai = kohi4ReceKisai;
            Kohi1FutansyaNo = kohi1FutansyaNo;
            Kohi1JyukyusyaNo = kohi1JyukyusyaNo;
            Kohi2FutansyaNo = kohi2FutansyaNo;
            Kohi2JyukyusyaNo = kohi2JyukyusyaNo;
            Kohi3FutansyaNo = kohi3FutansyaNo;
            Kohi3JyukyusyaNo = kohi3JyukyusyaNo;
            Kohi4FutansyaNo = kohi4FutansyaNo;
            Kohi4JyukyusyaNo = kohi4JyukyusyaNo;
            HokensyaNo = hokensyaNo;
        }

        public string InsuranceName { get; private set; }
        public int HokenKbn { get; private set; }
        public int Nissu { get; private set; }
        public string Tensu { get; private set; }
        public string IchibuFutan { get; private set; }
        public string EdaNo { get; private set; }
        public string Kigo { get; private set; }
        public string Bango { get; private set; }
        public bool Kohi1ReceKisai { get; private set; }
        public int Kohi1Id { get; private set; }
        public bool Kohi2ReceKisai { get; private set; }
        public int Kohi2Id { get; private set; }
        public bool Kohi3ReceKisai { get; private set; }
        public int Kohi3Id { get; private set; }
        public bool Kohi4ReceKisai { get; private set; }
        public string Kohi1FutansyaNo { get; private set; }
        public string Kohi1JyukyusyaNo { get; private set; }
        public string Kohi2FutansyaNo { get; private set; }
        public string Kohi2JyukyusyaNo { get; private set; }
        public string Kohi3FutansyaNo { get; private set; }
        public string Kohi3JyukyusyaNo { get; private set; }
        public string Kohi4FutansyaNo { get; private set; }
        public string Kohi4JyukyusyaNo { get; private set; }
        public string HokensyaNo { get; private set; }

    }
}
