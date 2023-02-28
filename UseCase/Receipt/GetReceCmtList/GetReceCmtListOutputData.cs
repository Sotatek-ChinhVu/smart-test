using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceCmt;

public class GetReceCmtListOutputData : IOutputData
{
    public GetReceCmtListOutputData(List<ReceCmtModel> receCmtList, GetReceCmtListStatus status)
    {
        ReceCmtList = receCmtList.Select(item => new ReceCmtItem(item)).ToList();
        Status = status;
    }

    public List<ReceCmtItem> ReceCmtList { get; private set; }

    public GetReceCmtListStatus Status { get; private set; }
}
