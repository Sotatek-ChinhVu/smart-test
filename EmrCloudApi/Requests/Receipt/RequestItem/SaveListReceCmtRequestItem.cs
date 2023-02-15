namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveListReceCmtRequestItem
{
    public long Id { get; set; }

    public int SeqNo { get; set; }

    public int CmtKbn { get; set; }

    public int CmtSbt { get; set; }

    public string Cmt { get; set; } = string.Empty;

    public string CmtData { get; set; } = string.Empty;

    public string ItemCd { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
