using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceCmt;

public class GetListReceCmtOutputData : IOutputData
{
    public GetListReceCmtOutputData(List<ReceCmtModel> listReceCmt, GetListReceCmtStatus status)
    {
        ListReceCmt = listReceCmt.Select(item => new ReceCmtItem(item)).ToList();
        Status = status;
    }
    public GetListReceCmtOutputData(GetListReceCmtStatus status)
    {
        ListReceCmt = new();
        Status = status;
    }

    public List<ReceCmtItem> ListReceCmt { get; private set; }

    public GetListReceCmtStatus Status { get; private set; }
}
