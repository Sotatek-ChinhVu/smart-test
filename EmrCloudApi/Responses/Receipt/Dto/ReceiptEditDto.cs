using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class ReceiptEditDto
{
    public ReceiptEditDto(ReceiptEditItem item)
    {
        Tokki1Id = item.Tokki1Id;
        Tokki2Id = item.Tokki2Id;
        Tokki3Id = item.Tokki3Id;
        Tokki4Id = item.Tokki4Id;
        Tokki5Id = item.Tokki5Id;
        HokenNissu = item.HokenNissu;
        Kohi1Nissu = item.Kohi1Nissu;
        Kohi2Nissu = item.Kohi2Nissu;
        Kohi3Nissu = item.Kohi3Nissu;
        Kohi4Nissu = item.Kohi4Nissu;
        Kohi1ReceKyufu = item.Kohi1ReceKyufu;
        Kohi2ReceKyufu = item.Kohi2ReceKyufu;
        Kohi3ReceKyufu = item.Kohi3ReceKyufu;
        Kohi4ReceKyufu = item.Kohi4ReceKyufu;
        HokenReceTensu = item.HokenReceTensu;
        HokenReceFutan = item.HokenReceFutan;
        Kohi1ReceTensu = item.Kohi1ReceTensu;
        Kohi1ReceFutan = item.Kohi1ReceFutan;
        Kohi2ReceTensu = item.Kohi2ReceTensu;
        Kohi2ReceFutan = item.Kohi2ReceFutan;
        Kohi3ReceTensu = item.Kohi3ReceTensu;
        Kohi3ReceFutan = item.Kohi3ReceFutan;
        Kohi4ReceTensu = item.Kohi4ReceTensu;
        Kohi4ReceFutan = item.Kohi4ReceFutan;
    }

    public string Tokki1Id { get; private set; }

    public string Tokki2Id { get; private set; }

    public string Tokki3Id { get; private set; }

    public string Tokki4Id { get; private set; }

    public string Tokki5Id { get; private set; }

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
