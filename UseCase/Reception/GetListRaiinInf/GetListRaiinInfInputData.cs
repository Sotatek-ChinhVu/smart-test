using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetListRaiinInf;
public class GetListRaiinInfInputData : IInputData<GetListRaiinInfOutputData>
{
    public GetListRaiinInfInputData(int hpId, long ptId, int pageIndex, int pageSize, int isDeleted, bool isAll)
    {
        HpId = hpId;
        PtId = ptId;
        PageIndex = pageIndex;
        PageSize = pageSize;
        IsDeleted = isDeleted;
        IsAll = isAll;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int PageIndex { get; private set;}

    public int PageSize { get; private set;}

    public int IsDeleted { get; private set;}

    public bool IsAll { get; private set;}
}