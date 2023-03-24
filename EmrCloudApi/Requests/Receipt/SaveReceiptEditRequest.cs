namespace EmrCloudApi.Requests.Receipt;

public class SaveReceiptEditRequest
{
    public long PtId { get; set; }

    public int SeikyuYm { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public int SeqNo { get; set; }

    public int Tokki1Id { get; set; }

    public int Tokki2Id { get; set; }

    public int Tokki3Id { get; set; }

    public int Tokki4Id { get; set; }

    public int Tokki5Id { get; set; }

    public int HokenNissu { get; set; }

    public int Kohi1Nissu { get; set; }

    public int Kohi2Nissu { get; set; }

    public int Kohi3Nissu { get; set; }

    public int Kohi4Nissu { get; set; }

    public int Kohi1ReceKyufu { get; set; }

    public int Kohi2ReceKyufu { get; set; }

    public int Kohi3ReceKyufu { get; set; }

    public int Kohi4ReceKyufu { get; set; }

    public int HokenReceTensu { get; set; }

    public int HokenReceFutan { get; set; }

    public int Kohi1ReceTensu { get; set; }

    public int Kohi1ReceFutan { get; set; }

    public int Kohi2ReceTensu { get; set; }

    public int Kohi2ReceFutan { get; set; }

    public int Kohi3ReceTensu { get; set; }

    public int Kohi3ReceFutan { get; set; }

    public int Kohi4ReceTensu { get; set; }

    public int Kohi4ReceFutan { get; set; }

    public bool IsDeleted { get; set; }
}
