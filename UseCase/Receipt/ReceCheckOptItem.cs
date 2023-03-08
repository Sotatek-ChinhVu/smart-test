namespace UseCase.Receipt;

public class ReceCheckOptItem
{
    public ReceCheckOptItem(string errCd, int checkOpt, string biko, bool isInvalid)
    {
        ErrCd = errCd;
        CheckOpt = checkOpt;
        Biko = biko;
        IsInvalid = isInvalid;
    }

    public string ErrCd { get; private set; }

    public int CheckOpt { get; private set; }

    public string Biko { get; private set; }

    public bool IsInvalid { get; private set; }
}
