using UseCase.Core.Sync.Core;

namespace UseCase.Online.InsertOnlineConfirmHistory;

public class InsertOnlineConfirmHistoryOutputData : IOutputData
{
    public InsertOnlineConfirmHistoryOutputData(List<long> idList, InsertOnlineConfirmHistoryStatus status)
    {
        Status = status;
        IdList = idList;
    }

    public List<long> IdList { get; private set; }

    public InsertOnlineConfirmHistoryStatus Status { get; private set; }
}
