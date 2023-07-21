namespace UseCase.Receipt;

public class ReceCheckErrorItem
{
    public ReceCheckErrorItem(string errCd, int sinDate, string aCd, string bCd, string message1, string message2, int isChecked)
    {
        ErrCd = errCd;
        SinDate = sinDate;
        ACd = aCd;
        BCd = bCd;
        Message1 = message1;
        Message2 = message2;
        IsChecked = isChecked;
    }

    public string ErrCd { get; private set; }

    public int SinDate { get; private set; }

    public string ACd { get; private set; }

    public string BCd { get; private set; }

    public string Message1 { get; private set; }

    public string Message2 { get; private set; }

    public int IsChecked { get; private set; }
}
