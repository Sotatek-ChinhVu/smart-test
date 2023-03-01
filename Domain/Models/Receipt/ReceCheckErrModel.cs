namespace Domain.Models.Receipt;

public class ReceCheckErrModel
{
    public ReceCheckErrModel(long ptId, int sinYm, int hokenId, string errCd, int sinDate, string aCd, string bCd, string message1, string message2, int isChecked)
    {
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        ErrCd = errCd;
        SinDate = sinDate;
        ACd = aCd;
        BCd = bCd;
        Message1 = message1;
        Message2 = message2;
        IsChecked = isChecked;
    }

    public ReceCheckErrModel ChangeMessage(string message1, string message2)
    {
        Message1 = message1;
        Message2 = message2;
        return this;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public string ErrCd { get; private set; }

    public int SinDate { get; private set; }

    public string ACd { get; private set; }

    public string BCd { get; private set; }

    public string Message1 { get; private set; }

    public string Message2 { get; private set; }

    public int IsChecked { get; private set; }
}
