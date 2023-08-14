using Helper.Enum;

namespace UseCase.Receipt;

public class ReceiptCheckCmtErrListItem
{
    public ReceiptCheckCmtErrListItem(int seqNo, int isPending, int sortNo, bool isChecked, string textDisplay1)
    {
        SeqNo = seqNo;
        SortNo = sortNo;
        IsChecked = isChecked;
        TextDisplay1 = textDisplay1;
        TextDisplay2 = string.Empty;
        StatusColor = isPending;
        ReceiptCheckIsErrItem = false;
        ErrorCd = string.Empty;
        ACd = string.Empty;
        BCd = string.Empty;
    }

    public ReceiptCheckCmtErrListItem(bool isChecked, string textDisplay1, string textDisplay2, string errorCd, string acd, string bcd, int sinDate)
    {
        SeqNo = 0;
        SortNo = 0;
        IsChecked = isChecked;
        TextDisplay1 = textDisplay1;
        TextDisplay2 = textDisplay2;
        StatusColor = (int)ReceiptCheckCmtStatusColorEnum.SystemHold;
        ReceiptCheckIsErrItem = true;
        ErrorCd = errorCd;
        ACd = acd;
        BCd = bcd;
        SinDate = sinDate;
    }

    public int SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public bool IsChecked { get; private set; }

    public string TextDisplay1 { get; private set; }

    public string TextDisplay2 { get; private set; }

    public string ErrorCd { get; private set; }

    public string ACd { get; private set; }

    public string BCd { get; private set; }

    public int SinDate { get; private set; }

    public int StatusColor { get; private set; }

    public bool ReceiptCheckIsErrItem { get; private set; }
}
