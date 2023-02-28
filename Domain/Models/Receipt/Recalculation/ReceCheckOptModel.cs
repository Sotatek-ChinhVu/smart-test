namespace Domain.Models.Receipt.Recalculation;

public class ReceCheckOptModel
{
    public ReceCheckOptModel(string errCd, int checkOpt, string biko, int isInvalid)
    {
        ErrCd = errCd;
        CheckOpt = checkOpt;
        Biko = biko;
        IsInvalid = isInvalid;
    }

    public ReceCheckOptModel(string errCd, int checkOpt)
    {
        ErrCd = errCd;
        CheckOpt = checkOpt;
        Biko = string.Empty;
    }

    public ReceCheckOptModel(string errCd)
    {
        ErrCd = errCd;
        Biko = string.Empty;
    }

    public string ErrCd { get; private set; }

    public int CheckOpt { get; private set; }

    public string Biko { get; private set; }

    public int IsInvalid { get; private set; }
}
