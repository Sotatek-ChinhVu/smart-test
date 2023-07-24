using Helper.Common;

namespace UseCase.Receipt.GetInsuranceInf
{
    public class InsuranceInfDto
    {
        public InsuranceInfDto(int hokenId, string insuranceName, int hokenKbn, int nissu, int tensu, int ichibuFutan, int ptFutan, string edaNo, string kigo, string bango, bool kohi1ReceKisai, int kohi1Id, bool kohi2ReceKisai, int kohi2Id, bool kohi3ReceKisai, int kohi3Id, bool kohi4ReceKisai, string futansyaNoKohi1, string jyukyusyaNoKohi1, string futansyaNoKohi2, string jyukyusyaNoKohi2, string futansyaNoKohi3, string jyukyusyaNoKohi3, string futansyaNoKohi4, string jyukyusyaNoKohi4, string hokensyaNo)
        {
            HokenId = hokenId;
            InsuranceName = insuranceName;
            HokenKbn = hokenKbn;
            Nissu = nissu;
            Tensu = CIUtil.FormatIntToString(tensu);
            IchibuFutan = CIUtil.FormatIntToString(ichibuFutan);
            PtFutan = CIUtil.FormatIntToString(ptFutan);
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
            FutansyaNoKohi1 = futansyaNoKohi1;
            JyukyusyaNoKohi1 = jyukyusyaNoKohi1;
            FutansyaNoKohi2 = futansyaNoKohi2;
            JyukyusyaNoKohi2 = jyukyusyaNoKohi2;
            FutansyaNoKohi3 = futansyaNoKohi3;
            JyukyusyaNoKohi3 = jyukyusyaNoKohi3;
            FutansyaNoKohi4 = futansyaNoKohi4;
            JyukyusyaNoKohi4 = jyukyusyaNoKohi4;
            HokensyaNo = hokensyaNo;
        }

        public int HokenId { get; private set; }
        public string InsuranceName { get; private set; }
        public int HokenKbn { get; private set; }
        public int Nissu { get; private set; }
        public string Tensu { get; private set; }
        public string IchibuFutan { get; private set; }
        public string PtFutan { get; private set; }
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
        public string FutansyaNoKohi1 { get; private set; }
        public string JyukyusyaNoKohi1 { get; private set; }
        public string FutansyaNoKohi2 { get; private set; }
        public string JyukyusyaNoKohi2 { get; private set; }
        public string FutansyaNoKohi3 { get; private set; }
        public string JyukyusyaNoKohi3 { get; private set; }
        public string FutansyaNoKohi4 { get; private set; }
        public string JyukyusyaNoKohi4 { get; private set; }
        public string HokensyaNo { get; private set; }
    }
}
