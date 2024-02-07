using UseCase.Core.Sync.Core;

namespace UseCase.Ka.GetKacodeMstYossi;

public class GetKacodeMstYossiInputData : IInputData<GetKacodeMstYossiOutputData>
{
    public GetKacodeMstYossiInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
