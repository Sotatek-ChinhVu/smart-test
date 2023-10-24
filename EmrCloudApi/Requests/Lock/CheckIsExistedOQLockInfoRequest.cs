namespace EmrCloudApi.Requests.Lock;

public class CheckIsExistedOQLockInfoRequest
{
    public long PtId { get; set; }

    public string FunctionCd { get; set; } = string.Empty;

    public long RaiinNo { get; set; }

    public int SinDate { get; set; }
}
