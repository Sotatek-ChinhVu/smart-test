using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.IsHokenInfInUsed;

public class IsHokenInfInUsedInputData : IInputData<IsHokenInfInUsedOutputData>
{
    public IsHokenInfInUsedInputData(int hpId, long ptId, int hokenPId)
    {
        HpId = hpId;
        PtId = ptId;
        HokenPId = hokenPId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int HokenPId { get; private set; }
}
