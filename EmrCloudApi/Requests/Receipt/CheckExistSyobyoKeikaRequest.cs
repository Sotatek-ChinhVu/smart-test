namespace EmrCloudApi.Requests.Receipt;

public class CheckExistSyobyoKeikaRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public int SinDay { get; set; }
}
