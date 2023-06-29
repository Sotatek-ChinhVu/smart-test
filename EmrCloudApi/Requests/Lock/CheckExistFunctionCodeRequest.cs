namespace EmrCloudApi.Requests.Lock;

public class CheckExistFunctionCodeRequest
{
    public long PtId { get; set; }

    public string FunctionCod { get; set; } = string.Empty;
}
