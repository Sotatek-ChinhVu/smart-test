using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListRaiinInf;

public class GetListRaiinInfOutputData : IOutputData
{
    public GetListRaiinInfOutputData(List<RaiinInfModel> raiinInfList, GetListRaiinInfStatus status)
    {
        RaiinInfList = raiinInfList.Select(item => new RaiinInfItem(item)).ToList();
        Status = status;
    }

    public List<RaiinInfItem> RaiinInfList { get; private set; }

    public GetListRaiinInfStatus Status { get; private set; }
}
