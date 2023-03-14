namespace Domain.Models.Receipt;

public class ReceInfModel
{
    public ReceInfModel(int seikyuYm, long ptId, int sinYm, int hokenId, int hokenId2, int kaId, int tantoId, string receSbt, int hokenKbn, int hokenSbtCd, string houbetu, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string kohi1Houbetu, string kohi2Houbetu, string kohi3Houbetu, string kohi4Houbetu, int honkeKbn)
    {
        SeikyuYm = seikyuYm;
        PtId = ptId;
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
    }

    public int SeikyuYm { get; private set; }

    public long PtId { get; private set; }

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
}
