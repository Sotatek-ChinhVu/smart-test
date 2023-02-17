namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveSyobyoKeikaRequestItem
{
    public int SeqNo { get; set; }

    public int SinDay { get; set; }

    public string Keika { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
