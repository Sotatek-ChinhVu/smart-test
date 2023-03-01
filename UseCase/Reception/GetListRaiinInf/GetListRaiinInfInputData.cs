using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetListRaiinInfs;
public class GetListRaiinInfInputData : IInputData<GetListRaiinInfOutputData>
{
    public GetListRaiinInfInputData(int hpId, long ptId, int pageIndex, int pageSize)
    {
        HpId = hpId;
        PtId = ptId;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int PageIndex { get; private set;}

    public int PageSize { get; private set;}
}