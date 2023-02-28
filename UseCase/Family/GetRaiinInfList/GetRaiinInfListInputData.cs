using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetRaiinInfList;

public class GetRaiinInfListInputData : IInputData<GetRaiinInfListOutputData>
{
    public GetRaiinInfListInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
