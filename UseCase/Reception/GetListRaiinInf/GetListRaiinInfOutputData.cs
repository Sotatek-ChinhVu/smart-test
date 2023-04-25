using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetListRaiinInf;

public class GetListRaiinInfOutputData : IOutputData
{
    public GetListRaiinInfOutputData(List<GetListRaiinInfOutputItem> raiinInfs, GetListRaiinInfStatus status)
    {
        Status = status;
        RaiinInfs = raiinInfs;
    }

    public GetListRaiinInfOutputData(GetListRaiinInfStatus status)
    {
        Status = status;
        RaiinInfs = new();
    }

    public GetListRaiinInfStatus Status { get; private set; }

    public List<GetListRaiinInfOutputItem> RaiinInfs { get; private set; }
}