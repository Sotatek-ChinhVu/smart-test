namespace EmrCloudApi.Requests.Receipt;

public class GetReceStatusRequest
{
    public long PtId { get; set; }

    public int SeikyuYm { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }
}
