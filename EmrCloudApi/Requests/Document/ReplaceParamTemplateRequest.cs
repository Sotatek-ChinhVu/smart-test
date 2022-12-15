namespace EmrCloudApi.Requests.Document;

public class ReplaceParamTemplateRequest
{
    public int HpId { get; set; }

    public int UserId { get; set; }

    public long PtId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public int HokenPId { get; set; }

    public string LinkFile { get; set; } = string.Empty;
}
