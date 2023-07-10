using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.ConvertStringChkJISKj;

public class ConvertStringChkJISKjOutputData : IOutputData
{
    public ConvertStringChkJISKjOutputData(string result, string sOut, ConvertStringChkJISKjStatus status)
    {
        Result = result;
        SOut = sOut;
        Status = status;
    }

    public string Result { get; private set; }

    public string SOut { get; private set; }

    public ConvertStringChkJISKjStatus Status { get; private set; }
}
