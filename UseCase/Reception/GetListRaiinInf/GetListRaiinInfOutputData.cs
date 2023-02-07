using UseCase.Core.Sync.Core;
using UseCase.Reception.GetListRaiinInfs;

public class GetListRaiinInfOutputData : IOutputData
{
    public GetListRaiinInfOutputData(List<GetListRaiinInfInputItem> raiinInfs, GetListRaiinInfStatus status)
    {
        Status = status;
        RaiinInfs = raiinInfs;
    }

    public GetListRaiinInfStatus Status { get; private set; }
    public List<GetListRaiinInfInputItem> RaiinInfs { get; private set; }
}