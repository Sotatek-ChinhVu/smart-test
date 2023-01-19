using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetListRaiinInf;

public class GetListRaiinInfInputData : IInputData<GetListRaiinInfOutputData>
{
    public GetListRaiinInfInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
