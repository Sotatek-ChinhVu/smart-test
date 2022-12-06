namespace EmrCloudApi.Requests.Document;

public class SaveDocInfRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public int SeqNo { get; set; }

    public int CategoryCd { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string DisplayFileName { get; set; } = string.Empty;
}
