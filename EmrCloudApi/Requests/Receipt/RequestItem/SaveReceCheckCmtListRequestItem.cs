namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveReceCheckCmtListRequestItem
{
    public int SeqNo { get; set; }

    public int StatusColor { get; set; }

    public string Cmt { get; set; } = string.Empty;

    public bool IsChecked { get; set; }

    public int SortNo { get; set; }

    public bool IsDeleted { get; set; }
}
