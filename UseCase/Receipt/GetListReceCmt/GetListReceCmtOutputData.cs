using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceCmt;

public class GetListReceCmtOutputData : IOutputData
{
    public GetListReceCmtOutputData(List<ReceCmtModel> receCmtList, GetListReceCmtStatus status)
    {
        ReceCmtList = receCmtList.Select(item => new ReceCmtItem(item)).ToList();
        Status = status;
    }

    public List<ReceCmtItem> ReceCmtList { get; private set; }

    public GetListReceCmtStatus Status { get; private set; }
}
