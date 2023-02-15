namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveListSyobyoKeikaRequestItem
{
    public int SeqNo { get; set; }

    public int SinDay { get; set; }

    public string Keika { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
