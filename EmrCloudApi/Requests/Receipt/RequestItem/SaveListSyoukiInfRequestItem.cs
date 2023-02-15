namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveListSyoukiInfRequestItem
{
    public SaveListSyoukiInfRequestItem(int seqNo, int sortNo, int syoukiKbn, int syoukiKbnStartYm, string syouki, bool isDeleted)
    {
        SeqNo = seqNo;
        SortNo = sortNo;
        SyoukiKbn = syoukiKbn;
        SyoukiKbnStartYm = syoukiKbnStartYm;
        Syouki = syouki;
        IsDeleted = isDeleted;
    }

    public int SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public int SyoukiKbn { get; private set; }

    public int SyoukiKbnStartYm { get; private set; }

    public string Syouki { get; private set; }

    public bool IsDeleted { get; private set; }
}
