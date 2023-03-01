using UseCase.Core.Sync.Core;
using UseCase.Reception.GetListRaiinInf;
using UseCase.Reception.GetListRaiinInfs;

public class GetListRaiinInfOutputData : IOutputData
{
    public GetListRaiinInfOutputData(List<GetListRaiinInfOutputItem> raiinInfs, GetListRaiinInfStatus status)
    {
        Status = status;
        RaiinInfs = raiinInfs;
    }

    public GetListRaiinInfStatus Status { get; private set; }
    public List<GetListRaiinInfOutputItem> RaiinInfs { get; private set; }
}