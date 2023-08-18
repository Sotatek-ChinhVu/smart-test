using UseCase.Core.Sync.Core;

namespace UseCase.Online.InsertOnlineConfirmHistory;

public class InsertOnlineConfirmHistoryOutputData : IOutputData
{
    public InsertOnlineConfirmHistoryOutputData(InsertOnlineConfirmHistoryStatus status)
    {
        Status = status;
    }

    public InsertOnlineConfirmHistoryStatus Status { get; private set; }
}
