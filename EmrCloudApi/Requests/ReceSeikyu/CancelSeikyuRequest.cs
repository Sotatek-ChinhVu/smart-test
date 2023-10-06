namespace EmrCloudApi.Requests.ReceSeikyu;

public class CancelSeikyuRequest
{
    public int SeikyuYm { get; set; }

    public int SeikyuKbn { get; set; }

    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }
}
