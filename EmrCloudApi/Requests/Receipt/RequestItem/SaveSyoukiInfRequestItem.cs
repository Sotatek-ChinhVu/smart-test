namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveSyoukiInfRequestItem
{
    public int SeqNo { get; set; }

    public int SortNo { get; set; }

    public int SyoukiKbn { get; set; }

    public int SyoukiKbnStartYm { get; set; }

    public string Syouki { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
