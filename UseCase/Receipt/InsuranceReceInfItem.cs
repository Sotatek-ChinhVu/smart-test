using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class InsuranceReceInfItem
{
    public InsuranceReceInfItem(InsuranceReceInfModel model, string insuranceName)
    {
        SeikyuYm = model.SeikyuYm;
        PtId = model.PtId;
        SinYm = model.SinYm;
        HokenId = model.HokenId;
        HokenId2 = model.HokenId2;
        Kohi1Id = model.Kohi1Id;
        Kohi2Id = model.Kohi2Id;
        Kohi3Id = model.Kohi3Id;
        Kohi4Id = model.Kohi4Id;
        HokenKbn = model.HokenKbn;
        ReceSbt = model.ReceSbt;
        HokensyaNo = model.HokensyaNo;
        HokenReceTensu = model.HokenReceTensu;
        HokenReceFutan = model.HokenReceFutan;
        Kohi1ReceTensu = model.Kohi1ReceTensu;
        Kohi1ReceFutan = model.Kohi1ReceFutan;
        Kohi1ReceKyufu = model.Kohi1ReceKyufu;
        Kohi2ReceTensu = model.Kohi2ReceTensu;
        Kohi2ReceFutan = model.Kohi2ReceFutan;
        Kohi2ReceKyufu = model.Kohi2ReceKyufu;
        Kohi3ReceTensu = model.Kohi3ReceTensu;
        Kohi3ReceFutan = model.Kohi3ReceFutan;
        Kohi3ReceKyufu = model.Kohi3ReceKyufu;
        Kohi4ReceTensu = model.Kohi4ReceTensu;
        Kohi4ReceFutan = model.Kohi4ReceFutan;
        Kohi4ReceKyufu = model.Kohi4ReceKyufu;
        HokenNissu = model.HokenNissu;
        Kohi1Nissu = model.Kohi1Nissu;
        Kohi2Nissu = model.Kohi2Nissu;
        Kohi3Nissu = model.Kohi3Nissu;
        Kohi4Nissu = model.Kohi4Nissu;
        Kohi1ReceKisai = model.Kohi1ReceKisai;
        Kohi2ReceKisai = model.Kohi2ReceKisai;
        Kohi3ReceKisai = model.Kohi3ReceKisai;
        Kohi4ReceKisai = model.Kohi4ReceKisai;
        Tokki1 = model.Tokki1;
        Tokki2 = model.Tokki2;
        Tokki3 = model.Tokki3;
        Tokki4 = model.Tokki4;
        Tokki5 = model.Tokki5;
        RousaiIFutan = model.RousaiIFutan;
        RousaiRoFutan = model.RousaiRoFutan;
        JibaiITensu = model.JibaiITensu;
        JibaiRoTensu = model.JibaiRoTensu;
        JibaiHaFutan = model.JibaiHaFutan;
        JibaiNiFutan = model.JibaiNiFutan;
        JibaiHoSindan = model.JibaiHoSindan;
        JibaiHeMeisai = model.JibaiHeMeisai;
        JibaiAFutan = model.JibaiAFutan;
        JibaiBFutan = model.JibaiBFutan;
        JibaiCFutan = model.JibaiCFutan;
        JibaiDFutan = model.JibaiDFutan;
        JibaiKenpoFutan = model.JibaiKenpoFutan;
        FutansyaNoKohi1 = model.FutansyaNoKohi1;
        FutansyaNoKohi2 = model.FutansyaNoKohi2;
        FutansyaNoKohi3 = model.FutansyaNoKohi3;
        FutansyaNoKohi4 = model.FutansyaNoKohi4;
        JyukyusyaNoKohi1 = model.JyukyusyaNoKohi1;
        JyukyusyaNoKohi2 = model.JyukyusyaNoKohi2;
        JyukyusyaNoKohi3 = model.JyukyusyaNoKohi3;
        JyukyusyaNoKohi4 = model.JyukyusyaNoKohi4;
        HokenInfRousaiKofuNo = model.HokenInfRousaiKofuNo;
        InsuranceName = insuranceName;
        Kigo = model.Kigo;
        Bango = model.Bango;
        EdaNo = (HokenKbn > 0 && (HokenKbn < 11 || HokenKbn > 14)) ? model.EdaNo : string.Empty;
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

    public string InsuranceName { get; private set; }

    public string Kigo { get; private set; }

    public string Bango { get; private set; }

    public string EdaNo { get; private set; }
}
