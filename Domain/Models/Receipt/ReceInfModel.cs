using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.Receipt;

public class ReceInfModel
{
    public ReceInfModel()
    {
        ReceSbt = string.Empty;
        Houbetu = string.Empty;
        Kohi1Houbetu = string.Empty;
        Kohi2Houbetu = string.Empty;
        Kohi3Houbetu = string.Empty;
        Kohi4Houbetu = string.Empty;
        Tokki1 = string.Empty;
        Tokki2 = string.Empty;
        Tokki3 = string.Empty;
        Tokki4 = string.Empty;
        Tokki5 = string.Empty;
    }

    public ReceInfModel(int hpId, int seikyuYm, long ptId, long ptNum, int sinYm, int hokenId, int hokenId2, int kaId, int tantoId, string receSbt, int hokenKbn, int hokenSbtCd, string houbetu, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string kohi1Houbetu, string kohi2Houbetu, string kohi3Houbetu, string kohi4Houbetu, int honkeKbn, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int hokenNissu, int kohi1Nissu, int kohi2Nissu, int kohi3Nissu, int kohi4Nissu, int kohi1ReceKyufu, int kohi2ReceKyufu, int kohi3ReceKyufu, int kohi4ReceKyufu, int hokenReceTensu, int hokenReceFutan, int kohi1ReceTensu, int kohi1ReceFutan, int kohi2ReceTensu, int kohi2ReceFutan, int kohi3ReceTensu, int kohi3ReceFutan, int kohi4ReceTensu, int kohi4ReceFutan)
    {
        HpId = hpId;
        SeikyuYm = seikyuYm;
        PtId = ptId;
        PtNum = ptNum;
        SinYm = sinYm;
        HokenId = hokenId;
        HokenId2 = hokenId2;
        KaId = kaId;
        TantoId = tantoId;
        ReceSbt = receSbt;
        HokenKbn = hokenKbn;
        HokenSbtCd = hokenSbtCd;
        Houbetu = houbetu;
        Kohi1Id = kohi1Id;
        Kohi2Id = kohi2Id;
        Kohi3Id = kohi3Id;
        Kohi4Id = kohi4Id;
        Kohi1Houbetu = kohi1Houbetu;
        Kohi2Houbetu = kohi2Houbetu;
        Kohi3Houbetu = kohi3Houbetu;
        Kohi4Houbetu = kohi4Houbetu;
        HonkeKbn = honkeKbn;
        Tokki1 = tokki1;
        Tokki2 = tokki2;
        Tokki3 = tokki3;
        Tokki4 = tokki4;
        Tokki5 = tokki5;
        HokenNissu = hokenNissu;
        Kohi1Nissu = kohi1Nissu;
        Kohi2Nissu = kohi2Nissu;
        Kohi3Nissu = kohi3Nissu;
        Kohi4Nissu = kohi4Nissu;
        Kohi1ReceKyufu = kohi1ReceKyufu;
        Kohi2ReceKyufu = kohi2ReceKyufu;
        Kohi3ReceKyufu = kohi3ReceKyufu;
        Kohi4ReceKyufu = kohi4ReceKyufu;
        HokenReceTensu = hokenReceTensu;
        HokenReceFutan = hokenReceFutan;
        Kohi1ReceTensu = kohi1ReceTensu;
        Kohi1ReceFutan = kohi1ReceFutan;
        Kohi2ReceTensu = kohi2ReceTensu;
        Kohi2ReceFutan = kohi2ReceFutan;
        Kohi3ReceTensu = kohi3ReceTensu;
        Kohi3ReceFutan = kohi3ReceFutan;
        Kohi4ReceTensu = kohi4ReceTensu;
        Kohi4ReceFutan = kohi4ReceFutan;
    }

    public int HpId { get; private set; }

    public int SeikyuKbn { get; private set; }

    public int SeikyuYm { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int HokenId2 { get; private set; }

    public int KaId { get; private set; }

    public int TantoId { get; private set; }

    public string ReceSbt { get; private set; }

    public int HokenKbn { get; private set; }

    public int HokenSbtCd { get; private set; }

    public string Houbetu { get; private set; }

    public int Kohi1Id { get; private set; }

    public int Kohi2Id { get; private set; }

    public int Kohi3Id { get; private set; }

    public int Kohi4Id { get; private set; }

    public string Kohi1Houbetu { get; private set; }

    public string Kohi2Houbetu { get; private set; }

    public string Kohi3Houbetu { get; private set; }

    public string Kohi4Houbetu { get; private set; }

    public int HonkeKbn { get; private set; }

    public string Tokki1 { get; private set; }

    public string Tokki2 { get; private set; }

    public string Tokki3 { get; private set; }

    public string Tokki4 { get; private set; }

    public string Tokki5 { get; private set; }

    public int HokenNissu { get; private set; }

    public int Kohi1Nissu { get; private set; }

    public int Kohi2Nissu { get; private set; }

    public int Kohi3Nissu { get; private set; }

    public int Kohi4Nissu { get; private set; }

    public int Kohi1ReceKyufu { get; private set; }

    public int Kohi2ReceKyufu { get; private set; }

    public int Kohi3ReceKyufu { get; private set; }

    public int Kohi4ReceKyufu { get; private set; }

    public int HokenReceTensu { get; private set; }

    public int HokenReceFutan { get; private set; }

    public int Kohi1ReceTensu { get; private set; }

    public int Kohi1ReceFutan { get; private set; }

    public int Kohi2ReceTensu { get; private set; }

    public int Kohi2ReceFutan { get; private set; }

    public int Kohi3ReceTensu { get; private set; }

    public int Kohi3ReceFutan { get; private set; }

    public int Kohi4ReceTensu { get; private set; }

    public int Kohi4ReceFutan { get; private set; }

    public PatientInforModel PtInf { get; private set; }

    public HokenInfModel PtHokenInf { get; private set; }

    public KohiInfModel PtKohi1 { get; private set; }

    public KohiInfModel PtKohi2 { get; private set; }

    public KohiInfModel PtKohi3 { get; private set; }

    public KohiInfModel PtKohi4 { get; private set; }

    public List<ConfirmDateModel> HokenChecks { get; private set; }

    public List<ConfirmDateModel> Kohi1Checks { get; private set; }

    public List<ConfirmDateModel> Kohi2Checks { get; private set; }

    public List<ConfirmDateModel> Kohi3Checks { get; private set; }

    public List<ConfirmDateModel> Kohi4Checks { get; private set; }

    public ReceStatusModel ReceStatus { get; private set; }

    public bool IsNashi => Houbetu == HokenConstant.HOUBETU_NASHI;

    public bool IsJihi => HokenKbn == 0 && (Houbetu == HokenConstant.HOUBETU_JIHI_108 || Houbetu == HokenConstant.HOUBETU_JIHI_109);

    public bool IsHokenConfirmed => IsJihi || IsNashi || (HokenChecks.Exists(p => CIUtil.DateTimeToInt(DateTime.SpecifyKind(CIUtil.IntToDate(p.ConfirmDate), DateTimeKind.Utc)) / 100 == SinYm));

    public bool IsKohi1Confirmed => Kohi1Checks.Exists(p => p.ConfirmDate / 100 == SinYm);

    public bool IsKohi2Confirmed => Kohi2Checks.Exists(p => p.ConfirmDate / 100 == SinYm);

    public bool IsKohi3Confirmed => Kohi3Checks.Exists(p => p.ConfirmDate / 100 == SinYm);

    public bool IsKohi4Confirmed => Kohi4Checks.Exists(p => p.ConfirmDate / 100 == SinYm);

    public int FirstDateOfThisMonth => (SinYm + "01").AsInteger();

    public int LastDateOfThisMonth => (SinYm + "31").AsInteger();

    public int IsPaperRece => (ReceStatus != null && ReceStatus.IsPaperRece) ? 1 : 0;

    public ReceInfModel(ReceInfModel receInf)
    {
        SeikyuYm = receInf.SeikyuYm;
        PtId = receInf.PtId;
        SinYm = receInf.SinYm;
        HokenId = receInf.HokenId;
        HokenId2 = receInf.HokenId2;
        KaId = receInf.KaId;
        TantoId = receInf.TantoId;
        ReceSbt = receInf.ReceSbt;
        HokenKbn = receInf.HokenKbn;
        HokenSbtCd = receInf.HokenSbtCd;
        Houbetu = receInf.Houbetu;
        Kohi1Id = receInf.Kohi1Id;
        Kohi2Id = receInf.Kohi2Id;
        Kohi3Id = receInf.Kohi3Id;
        Kohi4Id = receInf.Kohi4Id;
        Kohi1Houbetu = receInf.Kohi1Houbetu;
        Kohi2Houbetu = receInf.Kohi2Houbetu;
        Kohi3Houbetu = receInf.Kohi3Houbetu;
        Kohi4Houbetu = receInf.Kohi4Houbetu;
        HonkeKbn = receInf.HonkeKbn;
        Tokki1 = receInf.Tokki1;
        Tokki2 = receInf.Tokki2;
        Tokki3 = receInf.Tokki3;
        Tokki4 = receInf.Tokki4;
        Tokki5 = receInf.Tokki5;
        HokenNissu = receInf.HokenNissu;
        Kohi1Nissu = receInf.Kohi1Nissu;
        Kohi2Nissu = receInf.Kohi2Nissu;
        Kohi3Nissu = receInf.Kohi3Nissu;
        Kohi4Nissu = receInf.Kohi4Nissu;
        Kohi1ReceKyufu = receInf.Kohi1ReceKyufu;
        Kohi2ReceKyufu = receInf.Kohi2ReceKyufu;
        Kohi3ReceKyufu = receInf.Kohi3ReceKyufu;
        Kohi4ReceKyufu = receInf.Kohi4ReceKyufu;
        HokenReceTensu = receInf.HokenReceTensu;
        HokenReceFutan = receInf.HokenReceFutan;
        Kohi1ReceTensu = receInf.Kohi1ReceTensu;
        Kohi1ReceFutan = receInf.Kohi1ReceFutan;
        Kohi2ReceTensu = receInf.Kohi2ReceTensu;
        Kohi2ReceFutan = receInf.Kohi2ReceFutan;
        Kohi3ReceTensu = receInf.Kohi3ReceTensu;
        Kohi3ReceFutan = receInf.Kohi3ReceFutan;
        Kohi4ReceTensu = receInf.Kohi4ReceTensu;
        Kohi4ReceFutan = receInf.Kohi4ReceFutan;
    }
}
