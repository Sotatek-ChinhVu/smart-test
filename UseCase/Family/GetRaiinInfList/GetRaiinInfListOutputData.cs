using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetRaiinInfList;

public class GetRaiinInfListOutputData : IOutputData
{
    public GetRaiinInfListOutputData(List<RaiinInfOutputItem> raiinInfList, GetRaiinInfListStatus status)
    {
        RaiinInfList = raiinInfList;
        Status = status;
    }

    public GetRaiinInfListOutputData(GetRaiinInfListStatus status)
    {
        RaiinInfList = new();
        Status = status;
    }

    public List<RaiinInfOutputItem> RaiinInfList { get; private set; }

    public GetRaiinInfListStatus Status { get; private set; }
}
