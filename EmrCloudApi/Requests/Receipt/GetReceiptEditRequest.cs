namespace EmrCloudApi.Requests.Receipt;

public class GetReceiptEditRequest
{
    public int SeikyuYm { get; set; }

    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }
}
