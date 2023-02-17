namespace Domain.Models.Receipt;

public class InsuranceReceInfModel
{
    public InsuranceReceInfModel()
    {
        SeikyuYm = 0;
        PtId = 0;
        SinYm = 0;
        HokenId = 0;
        HokenId2 = 0;
        Kohi1Id = 0;
        Kohi2Id = 0;
        Kohi3Id = 0;
        Kohi4Id = 0;
        HokenKbn = 0;
        ReceSbt = string.Empty;
        HokensyaNo = string.Empty;
        HokenReceTensu = 0;
        HokenReceFutan = 0;
        Kohi1ReceTensu = 0;
        Kohi1ReceFutan = 0;
        Kohi1ReceKyufu = 0;
        Kohi2ReceTensu = 0;
        Kohi2ReceFutan = 0;
        Kohi2ReceKyufu = 0;
        Kohi3ReceTensu = 0;
        Kohi3ReceFutan = 0;
        Kohi3ReceKyufu = 0;
        Kohi4ReceTensu = 0;
        Kohi4ReceFutan = 0;
        Kohi4ReceKyufu = 0;
        HokenNissu = 0;
        Kohi1Nissu = 0;
        Kohi2Nissu = 0;
        Kohi3Nissu = 0;
        Kohi4Nissu = 0;
        Kohi1ReceKisai = 0;
        Kohi2ReceKisai = 0;
        Kohi3ReceKisai = 0;
        Kohi4ReceKisai = 0;
        Tokki1 = string.Empty;
        Tokki2 = string.Empty;
        Tokki3 = string.Empty;
        Tokki4 = string.Empty;
        Tokki5 = string.Empty;
        RousaiIFutan = 0;
        RousaiRoFutan = 0;
        JibaiITensu = 0;
        JibaiRoTensu = 0;
        JibaiHaFutan = 0;
        JibaiNiFutan = 0;
        JibaiHoSindan = 0;
        JibaiHeMeisai = 0;
        JibaiAFutan = 0;
        JibaiBFutan = 0;
        JibaiCFutan = 0;
        JibaiDFutan = 0;
        JibaiKenpoFutan = 0;
        FutansyaNoKohi1 = string.Empty;
        FutansyaNoKohi2 = string.Empty;
        FutansyaNoKohi3 = string.Empty;
        FutansyaNoKohi4 = string.Empty;
        JyukyusyaNoKohi1 = string.Empty;
        JyukyusyaNoKohi2 = string.Empty;
        JyukyusyaNoKohi3 = string.Empty;
        JyukyusyaNoKohi4 = string.Empty;
        HokenInfRousaiKofuNo = string.Empty;
    }

    public InsuranceReceInfModel(int seikyuYm, long ptId, int sinYm, int hokenId, int hokenId2, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int hokenKbn, string receSbt, string hokensyaNo, int hokenReceTensu, int hokenReceFutan, int kohi1ReceTensu, int kohi1ReceFutan, int kohi1ReceKyufu, int kohi2ReceTensu, int kohi2ReceFutan, int kohi2ReceKyufu, int kohi3ReceTensu, int kohi3ReceFutan, int kohi3ReceKyufu, int kohi4ReceTensu, int kohi4ReceFutan, int kohi4ReceKyufu, int hokenNissu, int kohi1Nissu, int kohi2Nissu, int kohi3Nissu, int kohi4Nissu, int kohi1ReceKisai, int kohi2ReceKisai, int kohi3ReceKisai, int kohi4ReceKisai, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int rousaiIFutan, int rousaiRoFutan, int jibaiITensu, int jibaiRoTensu, int jibaiHaFutan, int jibaiNiFutan, int jibaiHoSindan, int jibaiHeMeisai, int jibaiAFutan, int jibaiBFutan, int jibaiCFutan, int jibaiDFutan, int jibaiKenpoFutan, string futansyaNoKohi1, string futansyaNoKohi2, string futansyaNoKohi3, string futansyaNoKohi4, string jyukyusyaNoKohi1, string jyukyusyaNoKohi2, string jyukyusyaNoKohi3, string jyukyusyaNoKohi4, string hokenInfRousaiKofuNo)
    {
        SeikyuYm = seikyuYm;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        HokenId2 = hokenId2;
        Kohi1Id = kohi1Id;
        Kohi2Id = kohi2Id;
        Kohi3Id = kohi3Id;
        Kohi4Id = kohi4Id;
        HokenKbn = hokenKbn;
        ReceSbt = receSbt;
        HokensyaNo = hokensyaNo;
        HokenReceTensu = hokenReceTensu;
        HokenReceFutan = hokenReceFutan;
        Kohi1ReceTensu = kohi1ReceTensu;
        Kohi1ReceFutan = kohi1ReceFutan;
        Kohi1ReceKyufu = kohi1ReceKyufu;
        Kohi2ReceTensu = kohi2ReceTensu;
        Kohi2ReceFutan = kohi2ReceFutan;
        Kohi2ReceKyufu = kohi2ReceKyufu;
        Kohi3ReceTensu = kohi3ReceTensu;
        Kohi3ReceFutan = kohi3ReceFutan;
        Kohi3ReceKyufu = kohi3ReceKyufu;
        Kohi4ReceTensu = kohi4ReceTensu;
        Kohi4ReceFutan = kohi4ReceFutan;
        Kohi4ReceKyufu = kohi4ReceKyufu;
        HokenNissu = hokenNissu;
        Kohi1Nissu = kohi1Nissu;
        Kohi2Nissu = kohi2Nissu;
        Kohi3Nissu = kohi3Nissu;
        Kohi4Nissu = kohi4Nissu;
        Kohi1ReceKisai = kohi1ReceKisai;
        Kohi2ReceKisai = kohi2ReceKisai;
        Kohi3ReceKisai = kohi3ReceKisai;
        Kohi4ReceKisai = kohi4ReceKisai;
        Tokki1 = tokki1;
        Tokki2 = tokki2;
        Tokki3 = tokki3;
        Tokki4 = tokki4;
        Tokki5 = tokki5;
        RousaiIFutan = rousaiIFutan;
        RousaiRoFutan = rousaiRoFutan;
        JibaiITensu = jibaiITensu;
        JibaiRoTensu = jibaiRoTensu;
        JibaiHaFutan = jibaiHaFutan;
        JibaiNiFutan = jibaiNiFutan;
        JibaiHoSindan = jibaiHoSindan;
        JibaiHeMeisai = jibaiHeMeisai;
        JibaiAFutan = jibaiAFutan;
        JibaiBFutan = jibaiBFutan;
        JibaiCFutan = jibaiCFutan;
        JibaiDFutan = jibaiDFutan;
        JibaiKenpoFutan = jibaiKenpoFutan;
        FutansyaNoKohi1 = futansyaNoKohi1;
        FutansyaNoKohi2 = futansyaNoKohi2;
        FutansyaNoKohi3 = futansyaNoKohi3;
        FutansyaNoKohi4 = futansyaNoKohi4;
        JyukyusyaNoKohi1 = jyukyusyaNoKohi1;
        JyukyusyaNoKohi2 = jyukyusyaNoKohi2;
        JyukyusyaNoKohi3 = jyukyusyaNoKohi3;
        JyukyusyaNoKohi4 = jyukyusyaNoKohi4;
        HokenInfRousaiKofuNo = hokenInfRousaiKofuNo;
    }

    public int SeikyuYm { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int HokenId2 { get; private set; }

    public int Kohi1Id { get; private set; }

    public int Kohi2Id { get; private set; }

    public int Kohi3Id { get; private set; }

    public int Kohi4Id { get; private set; }

    public int HokenKbn { get; private set; }

    public string ReceSbt { get; private set; }

    public string HokensyaNo { get; private set; }

    public int HokenReceTensu { get; private set; }

    public int HokenReceFutan { get; private set; }

    public int Kohi1ReceTensu { get; private set; }

    public int Kohi1ReceFutan { get; private set; }

    public int Kohi1ReceKyufu { get; private set; }

    public int Kohi2ReceTensu { get; private set; }

    public int Kohi2ReceFutan { get; private set; }

    public int Kohi2ReceKyufu { get; private set; }

    public int Kohi3ReceTensu { get; private set; }

    public int Kohi3ReceFutan { get; private set; }

    public int Kohi3ReceKyufu { get; private set; }

    public int Kohi4ReceTensu { get; private set; }

    public int Kohi4ReceFutan { get; private set; }

    public int Kohi4ReceKyufu { get; private set; }

    public int HokenNissu { get; private set; }

    public int Kohi1Nissu { get; private set; }

    public int Kohi2Nissu { get; private set; }

    public int Kohi3Nissu { get; private set; }

    public int Kohi4Nissu { get; private set; }

    public int Kohi1ReceKisai { get; private set; }

    public int Kohi2ReceKisai { get; private set; }

    public int Kohi3ReceKisai { get; private set; }

    public int Kohi4ReceKisai { get; private set; }

    public string Tokki1 { get; private set; }

    public string Tokki2 { get; private set; }

    public string Tokki3 { get; private set; }

    public string Tokki4 { get; private set; }

    public string Tokki5 { get; private set; }

    public int RousaiIFutan { get; private set; }

    public int RousaiRoFutan { get; private set; }

    public int JibaiITensu { get; private set; }

    public int JibaiRoTensu { get; private set; }

    public int JibaiHaFutan { get; private set; }

    public int JibaiNiFutan { get; private set; }

    public int JibaiHoSindan { get; private set; }

    public int JibaiHeMeisai { get; private set; }

    public int JibaiAFutan { get; private set; }

    public int JibaiBFutan { get; private set; }

    public int JibaiCFutan { get; private set; }

    public int JibaiDFutan { get; private set; }

    public int JibaiKenpoFutan { get; private set; }

    public string FutansyaNoKohi1 { get; private set; }

    public string FutansyaNoKohi2 { get; private set; }

    public string FutansyaNoKohi3 { get; private set; }

    public string FutansyaNoKohi4 { get; private set; }

    public string JyukyusyaNoKohi1 { get; private set; }

    public string JyukyusyaNoKohi2 { get; private set; }

    public string JyukyusyaNoKohi3 { get; private set; }

    public string JyukyusyaNoKohi4 { get; private set; }

    public string HokenInfRousaiKofuNo { get; private set; }
}
