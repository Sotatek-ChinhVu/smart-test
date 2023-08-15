using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetOutDrugOrderList;

public class GetOutDrugOrderListOutputData : IOutputData
{
    public GetOutDrugOrderListOutputData(List<RaiinInfToPrintModel> raiinInfToPrintList, GetOutDrugOrderListStatus status)
    {
        RaiinInfToPrintList = raiinInfToPrintList;
        Status = status;
    }

    public List<RaiinInfToPrintModel> RaiinInfToPrintList { get; private set; }

    public GetOutDrugOrderListStatus Status { get; private set; }
}
