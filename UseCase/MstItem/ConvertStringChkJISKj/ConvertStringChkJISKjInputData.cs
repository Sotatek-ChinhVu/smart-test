using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.ConvertStringChkJISKj;

public class ConvertStringChkJISKjInputData : IInputData<ConvertStringChkJISKjOutputData>
{
    public ConvertStringChkJISKjInputData(string inputString, string sOut)
    {
        InputString = inputString;
        SOut = sOut;
    }

    public string InputString { get; private set; }

    public string SOut { get; private set; }
}
