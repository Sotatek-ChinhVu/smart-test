using UseCase.Core.Sync.Core;

namespace UseCase.Ka.GetKacodeYousikiMst;

public class GetKaCodeYousikiMstInputData : IInputData<GetKaCodeYousikiMstOutputData>
{
    public GetKaCodeYousikiMstInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
