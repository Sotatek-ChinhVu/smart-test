using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.ConvertStringChkJISKj;

public class ConvertStringChkJISKjOutputData : IOutputData
{
    public ConvertStringChkJISKjOutputData(string result, string sOut, string itemError, ConvertStringChkJISKjStatus status)
    {
        Result = result;
        SOut = sOut;
        ItemError = itemError;
        Status = status;
    }

    public string Result { get; private set; }

    public string SOut { get; private set; }

    public string ItemError { get; private set; }

    public ConvertStringChkJISKjStatus Status { get; private set; }
}
