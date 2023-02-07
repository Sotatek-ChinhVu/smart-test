using UseCase.Core.Sync.Core;
using UseCase.Reception.GetListRaiinInfs;

public class GetListRaiinInfsOutputData : IOutputData
{
    public GetListRaiinInfsOutputData(List<GetListRaiinInfsInputItem> raiinInfs, GetListRaiinInfsStatus status)
    {
        Status = status;
        RaiinInfs = raiinInfs;
    }

    public GetListRaiinInfsStatus Status { get; private set; }
    public List<GetListRaiinInfsInputItem> RaiinInfs { get; private set; }
}