namespace EmrCloudApi.Requests.Receipt;

public class GetSyobyoKeikaListRequest
{
    public int SinYm { get; set; }

    public long PtId { get; set; }

    public int HokenId { get; set; }

    public int HokenKbn { get; set; }
}
