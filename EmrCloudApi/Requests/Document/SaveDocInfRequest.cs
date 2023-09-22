namespace EmrCloudApi.Requests.Document;

public class SaveDocInfRequest
{
    public long PtId { get; set; }

    public long FileId { get; set; }

    public int GetDate { get; set; }

    public int CategoryCd { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string DisplayFileName { get; set; } = string.Empty;
}
