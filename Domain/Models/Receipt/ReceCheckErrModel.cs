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

    public ReceCheckErrModel(string errCd, int sinDate, string aCd, string bCd, int isChecked)
    {
        ErrCd = errCd;
        SinDate = sinDate;
        ACd = aCd;
        BCd = bCd;
        Message1 = string.Empty;
        Message2 = string.Empty;
        IsChecked = isChecked;
    }

    public ReceCheckErrModel(int hpId, long ptId, int sinYm, int hokenId, string errCd, int sinDate, string aCd, string bCd, string message1, string message2, int isChecked)
    {
        HpId = hpId;
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

    public int HpId { get; private set; }

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

    public ReceCheckErrModel ChangeMessage1(string message)
    {
        Message1 = message;
        return this;
    }

    public ReceCheckErrModel ChangeMessage2(string message)
    {
        Message2 = message;
        return this;
    }

    public ReceCheckErrModel ChangeIsChecked(int isChecked)
    {
        IsChecked = isChecked;
        return this;
    }
}
