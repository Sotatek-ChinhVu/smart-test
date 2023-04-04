namespace EmrCloudApi.Requests.Receipt;

public class SaveReceStatusRequest
{
    public long PtId { get; set; }

    public int SeikyuYm { get; set; }

    public int HokenId { get; set; }

    public int SinYm { get; set; }

    public int FusenKbn { get; set; }

    public bool IsPaperRece { get; set; }

    public int StatusKbn { get; set; }

    public bool IsPrechecked { get; set; }
}
