namespace EmrCloudApi.Requests.Receipt;

public class GetReceHenReasonRequest
{
    public int SeikyuYm { get; set; }

    public int SinDate { get; set; }

    public long PtId { get; set; }

    public int HokenId { get; set; }
}
