using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetRaiinListWithKanInf;

public class GetRaiinListWithKanInfInputData : IInputData<GetRaiinListWithKanInfOutputData>
{
    public GetRaiinListWithKanInfInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get;private set; }

    public long PtId { get;private set; }
}
