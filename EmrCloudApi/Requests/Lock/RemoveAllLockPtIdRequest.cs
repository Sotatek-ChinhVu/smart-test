namespace EmrCloudApi.Requests.Lock;

public class RemoveAllLockPtIdRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public string FunctionCd { get; set; } = string.Empty;

    public string TabKey { get; set; } = string.Empty;
}
