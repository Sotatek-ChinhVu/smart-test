using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.ConvertStringChkJISKj;

public class ConvertStringChkJISKjInputData : IInputData<ConvertStringChkJISKjOutputData>
{
    public ConvertStringChkJISKjInputData(List<string> inputList)
    {
        InputList = inputList;
    }

    public List<string> InputList { get; private set; }
}
