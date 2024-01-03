using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.IsHokenInfInUsed;

public class IsHokenInfInUsedInputData : IInputData<IsHokenInfInUsedOutputData>
{
    public IsHokenInfInUsedInputData(int hpId, long ptId, int hokenId)
    {
        HpId = hpId;
        PtId = ptId;
        HokenId = hokenId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }
}
