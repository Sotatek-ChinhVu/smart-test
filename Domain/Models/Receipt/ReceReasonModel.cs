namespace Domain.Models.Receipt;

public class ReceReasonModel
{
    public ReceReasonModel(int hokenId, int sinYm, string henreiJiyuuCd, string henreiJiyuu, string hosoku)
    {
        HokenId = hokenId;
        SinYm = sinYm;
        HenreiJiyuuCd = henreiJiyuuCd;
        HenreiJiyuu = henreiJiyuu;
        Hosoku = hosoku;
    }

    public int HokenId { get; private set; }

    public int SinYm { get; private set; }

    public string HenreiJiyuuCd { get; private set; }

    public string HenreiJiyuu { get; private set; }

    public string Hosoku { get; private set; }
}
