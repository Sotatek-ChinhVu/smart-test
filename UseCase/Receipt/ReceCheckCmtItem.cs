namespace UseCase.Receipt;

public class ReceCheckCmtItem
{
    public ReceCheckCmtItem(int seqNo, int statusColor, string cmt, int isChecked, int sortNo, bool isDeleted)
    {
        SeqNo = seqNo;
        StatusColor = statusColor;
        Cmt = cmt;
        IsChecked = isChecked;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public int SeqNo { get; private set; }

    public int StatusColor { get; private set; }

    public string Cmt { get; private set; }

    public int IsChecked { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }
}
