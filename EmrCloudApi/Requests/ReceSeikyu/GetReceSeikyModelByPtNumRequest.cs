namespace EmrCloudApi.Requests.ReceSeikyu;

public class GetReceSeikyModelByPtNumRequest
{
    public int SinDate { get; set; }

    public int SinYm { get; set; }

    public long PtNum { get; set; }
}
