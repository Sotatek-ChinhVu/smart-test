namespace Domain.Models.Receipt;

public class ReceCheckOptionModel
{
    public ReceCheckOptionModel(string errCd, int checkOpt, string biko, bool isValid)
    {
        ErrCd = errCd;
        CheckOpt = checkOpt;
        Biko = biko;
        IsValid = isValid;
    }

    public string ErrCd { get; private set; }

    public int CheckOpt { get; private set; }

    public string Biko { get; private set; }

    public bool IsValid { get; private set; }
}
