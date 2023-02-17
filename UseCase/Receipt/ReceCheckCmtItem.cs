namespace UseCase.Receipt;

public class ReceCheckCmtItem
{
    public ReceCheckCmtItem(int seqNo, int isPending, string cmt, int isChecked, int sortNo, bool isDeleted)
    {
        SeqNo = seqNo;
        IsPending = isPending;
        Cmt = cmt;
        IsChecked = isChecked;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public int SeqNo { get; private set; }

    public int IsPending { get; private set; }

    public string Cmt { get; private set; }

    public int IsChecked { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }
}
