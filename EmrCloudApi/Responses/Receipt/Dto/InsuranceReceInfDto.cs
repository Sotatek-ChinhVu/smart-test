using UseCase.Receipt.GetInsuranceReceInfList;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class InsuranceReceInfDto
{
    public InsuranceReceInfDto(GetInsuranceReceInfListOutputData output)
    {
        SeikyuYm = output.SeikyuYm;
        PtId = output.PtId;
        SinYm = output.SinYm;
        HokenId = output.HokenId;
        HokenId2 = output.HokenId2;
        Kohi1Id = output.Kohi1Id;
        Kohi2Id = output.Kohi2Id;
        Kohi3Id = output.Kohi3Id;
        Kohi4Id = output.Kohi4Id;
        HokenKbn = output.HokenKbn;
        ReceSbt = output.ReceSbt;
        HokensyaNo = output.HokensyaNo;
        HokenReceTensu = output.HokenReceTensu;
        HokenReceFutan = output.HokenReceFutan;
        Kohi1ReceTensu = output.Kohi1ReceTensu;
        Kohi1ReceFutan = output.Kohi1ReceFutan;
        Kohi1ReceKyufu = output.Kohi1ReceKyufu;
        Kohi2ReceTensu = output.Kohi2ReceTensu;
        Kohi2ReceFutan = output.Kohi2ReceFutan;
        Kohi2ReceKyufu = output.Kohi2ReceKyufu;
        Kohi3ReceTensu = output.Kohi3ReceTensu;
        Kohi3ReceFutan = output.Kohi3ReceFutan;
        Kohi3ReceKyufu = output.Kohi3ReceKyufu;
        Kohi4ReceTensu = output.Kohi4ReceTensu;
        Kohi4ReceFutan = output.Kohi4ReceFutan;
        Kohi4ReceKyufu = output.Kohi4ReceKyufu;
        HokenNissu = output.HokenNissu;
        Kohi1Nissu = output.Kohi1Nissu;
        Kohi2Nissu = output.Kohi2Nissu;
        Kohi3Nissu = output.Kohi3Nissu;
        Kohi4Nissu = output.Kohi4Nissu;
        Kohi1ReceKisai = output.Kohi1ReceKisai;
        Kohi2ReceKisai = output.Kohi2ReceKisai;
        Kohi3ReceKisai = output.Kohi3ReceKisai;
        Kohi4ReceKisai = output.Kohi4ReceKisai;
        Tokki1 = output.Tokki1;
        Tokki2 = output.Tokki2;
        Tokki3 = output.Tokki3;
        Tokki4 = output.Tokki4;
        Tokki5 = output.Tokki5;
        RousaiIFutan = output.RousaiIFutan;
        RousaiRoFutan = output.RousaiRoFutan;
        JibaiITensu = output.JibaiITensu;
        JibaiRoTensu = output.JibaiRoTensu;
        JibaiHaFutan = output.JibaiHaFutan;
        JibaiNiFutan = output.JibaiNiFutan;
        JibaiHoSindan = output.JibaiHoSindan;
        JibaiHeMeisai = output.JibaiHeMeisai;
        JibaiAFutan = output.JibaiAFutan;
        JibaiBFutan = output.JibaiBFutan;
        JibaiCFutan = output.JibaiCFutan;
        JibaiDFutan = output.JibaiDFutan;
        JibaiKenpoFutan = output.JibaiKenpoFutan;
        FutansyaNoKohi1 = output.FutansyaNoKohi1;
        FutansyaNoKohi2 = output.FutansyaNoKohi2;
        FutansyaNoKohi3 = output.FutansyaNoKohi3;
        FutansyaNoKohi4 = output.FutansyaNoKohi4;
        JyukyusyaNoKohi1 = output.JyukyusyaNoKohi1;
        JyukyusyaNoKohi2 = output.JyukyusyaNoKohi2;
        JyukyusyaNoKohi3 = output.JyukyusyaNoKohi3;
        JyukyusyaNoKohi4 = output.JyukyusyaNoKohi4;
        HokenInfRousaiKofuNo = output.HokenInfRousaiKofuNo;
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
