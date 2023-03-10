namespace EmrCloudApi.Responses.Receipt.Dto;

public class ReceCheckOptionDto
{
    public ReceCheckOptionDto(string checkboxKey, string errCd, int checkOpt, bool isInvalid)
    {
        CheckboxKey = checkboxKey;
        ErrCd = errCd;
        CheckOpt = checkOpt;
        IsInvalid = isInvalid;
    }

    public string CheckboxKey { get; private set; }

    public string ErrCd { get; private set; }

    public int CheckOpt { get; private set; }

    public bool IsInvalid { get; private set; }
}
