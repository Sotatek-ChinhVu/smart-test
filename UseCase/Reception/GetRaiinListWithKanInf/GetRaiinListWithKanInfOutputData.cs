using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetRaiinListWithKanInf;

public class GetRaiinListWithKanInfOutputData: IOutputData
{
    public GetRaiinListWithKanInfOutputData(List<ReceptionModel> raiinInfList, GetRaiinListWithKanInfStatus status)
    {
        RaiinInfList = raiinInfList.Select(item => new RaiinInfItem(item)).ToList();
        Status = status;
    }

    public List<RaiinInfItem> RaiinInfList { get;private set; }

    public GetRaiinListWithKanInfStatus Status { get;private set; }
}
