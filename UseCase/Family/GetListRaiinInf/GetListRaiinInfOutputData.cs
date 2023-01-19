using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetListRaiinInf;

public class GetListRaiinInfOutputData : IOutputData
{
    public GetListRaiinInfOutputData(List<RaiinInfOutputItem> listRaiinInf, GetListRaiinInfStatus status)
    {
        ListRaiinInf = listRaiinInf;
        Status = status;
    }

    public GetListRaiinInfOutputData(GetListRaiinInfStatus status)
    {
        ListRaiinInf = new();
        Status = status;
    }

    public List<RaiinInfOutputItem> ListRaiinInf { get; private set; }

    public GetListRaiinInfStatus Status { get; private set; }
}
