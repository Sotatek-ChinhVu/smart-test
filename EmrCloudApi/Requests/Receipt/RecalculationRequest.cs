namespace EmrCloudApi.Requests.Receipt;

public class RecalculationRequest
{
    public int SinYm { get; set; }

    public List<long> PtIdList { get; set; } = new();

    public bool IsStopCalc { get; set; } = false;
}
