using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.DoReceCmt;

public class DoReceCmtOutputData : IOutputData
{
    public DoReceCmtOutputData(List<ReceCmtModel> receCmtList, DoReceCmtStatus status)
    {
        ReceCmtList = receCmtList.Select(item => new ReceCmtItem(item)).ToList();
        Status = status;
    }

    public List<ReceCmtItem> ReceCmtList { get; private set; }

    public DoReceCmtStatus Status { get; private set; }
}
