using UseCase.Core.Sync.Core;

namespace UseCase.Online.InsertOnlineConfirmHistory;

public class InsertOnlineConfirmHistoryInputData : IInputData<InsertOnlineConfirmHistoryOutputData>
{
    public InsertOnlineConfirmHistoryInputData(int userId, List<OnlineConfirmationHistoryItem> onlineList)
    {
        UserId = userId;
        OnlineList = onlineList;
    }

    public int UserId { get; private set; }

    public List<OnlineConfirmationHistoryItem> OnlineList { get; private set; }
}
