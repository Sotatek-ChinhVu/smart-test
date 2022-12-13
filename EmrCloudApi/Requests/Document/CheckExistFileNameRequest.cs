namespace EmrCloudApi.Requests.Document;

public class CheckExistFileNameRequest
{
    public string FileName { get; set; } = string.Empty;

    public int CategoryCd { get; set; }

    public int PtId { get; set; }

    public bool IsCheckDocInf { get; set; }
}
