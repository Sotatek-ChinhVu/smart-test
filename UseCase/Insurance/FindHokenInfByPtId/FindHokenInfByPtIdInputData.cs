using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.FindHokenInfByPtId;

public class FindHokenInfByPtIdInputData : IInputData<FindHokenInfByPtIdOutputData>
{
    public FindHokenInfByPtIdInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
