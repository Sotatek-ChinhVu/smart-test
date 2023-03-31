using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceStatus;

public class GetReceStatusOutputData : IOutputData
{
    public GetReceStatusOutputData(GetReceStatusStatus status, ReceStatusModel model)
    {
        Status = status;
        ReceStatus = new ReceStatusItem(model);
    }

    public GetReceStatusStatus Status { get; private set; }

    public ReceStatusItem ReceStatus { get; private set; }
}
