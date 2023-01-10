using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetListRaiinInfs;
public class GetListRaiinInfsInputData : IInputData<GetListRaiinInfsOutputData>
{
    public GetListRaiinInfsInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }
    public int HpId { get; private set; }
    public long PtId { get; private set; }
}