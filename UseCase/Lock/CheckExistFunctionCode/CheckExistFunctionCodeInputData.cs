using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckExistFunctionCode;

public class CheckExistFunctionCodeInputData : IInputData<CheckExistFunctionCodeOutputData>
{
    public CheckExistFunctionCodeInputData(int hpId, string functionCd, long ptId)
    {
        HpId = hpId;
        FunctionCd = functionCd;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public string FunctionCd { get; private set; }

    public long PtId { get; private set; }
}
