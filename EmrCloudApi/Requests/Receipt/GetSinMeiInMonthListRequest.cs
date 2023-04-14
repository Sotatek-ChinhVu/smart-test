namespace EmrCloudApi.Requests.Receipt;

public class GetSinMeiInMonthListRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public int SeikyuYm { get; set; }
}
