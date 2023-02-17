namespace Domain.Models.Receipt;

public class ReceCheckCmtModel
{
    public ReceCheckCmtModel(long ptId, int seqNo, int sinYm, int hokenId, int isPending, string cmt, int isChecked, int sortNo)
    {
        PtId = ptId;
        SeqNo = seqNo;
        SinYm = sinYm;
        HokenId = hokenId;
        IsPending = isPending;
        Cmt = cmt;
        IsChecked = isChecked;
        SortNo = sortNo;
        IsDeleted = false;
    }

    public ReceCheckCmtModel(long ptId, int seqNo, int sinYm, int hokenId, int isPending, string cmt, int isChecked, int sortNo, bool isDeleted)
    {
        PtId = ptId;
        SeqNo = seqNo;
        SinYm = sinYm;
        HokenId = hokenId;
        IsPending = isPending;
        Cmt = cmt;
        IsChecked = isChecked;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public long PtId { get; private set; }

    public int SeqNo { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int IsPending { get; private set; }

    public string Cmt { get; private set; }

    public int IsChecked { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }
}
