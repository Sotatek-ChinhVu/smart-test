using UseCase.Core.Sync.Core;

namespace UseCase.Ka.GetKaCodeList;

public class GetKaCodeMstInputData : IInputData<GetKaCodeMstListOutputData>
{
    public GetKaCodeMstInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
