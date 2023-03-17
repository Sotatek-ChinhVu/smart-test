namespace Domain.Models.Receipt;

public class ReceiptEditModel
{
    public ReceiptEditModel()
    {
        Tokki1 = string.Empty;
        Tokki2 = string.Empty;
        Tokki3 = string.Empty;
        Tokki4 = string.Empty;
        Tokki5 = string.Empty;
    }

    public ReceiptEditModel(long seqNo, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, int hokenNissu, int kohi1Nissu, int kohi2Nissu, int kohi3Nissu, int kohi4Nissu, int kohi1ReceKyufu, int kohi2ReceKyufu, int kohi3ReceKyufu, int kohi4ReceKyufu, int hokenReceTensu, int hokenReceFutan, int kohi1ReceTensu, int kohi1ReceFutan, int kohi2ReceTensu, int kohi2ReceFutan, int kohi3ReceTensu, int kohi3ReceFutan, int kohi4ReceTensu, int kohi4ReceFutan)
    {
        SeqNo = seqNo;
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

    public long SeqNo { get; private set; }

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
}
