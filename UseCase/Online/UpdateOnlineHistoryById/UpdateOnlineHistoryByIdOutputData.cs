using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineHistoryById;

public class UpdateOnlineHistoryByIdOutputData : IOutputData
{
    public UpdateOnlineHistoryByIdOutputData(UpdateOnlineHistoryByIdStatus status)
    {
        Status = status;
    }

    public UpdateOnlineHistoryByIdStatus Status { get; private set; }
}
