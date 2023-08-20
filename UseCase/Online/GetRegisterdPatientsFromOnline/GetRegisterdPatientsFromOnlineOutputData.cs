using Domain.Models.Online;
using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetRegisterdPatientsFromOnline;

public class GetRegisterdPatientsFromOnlineOutputData : IOutputData
{
    public GetRegisterdPatientsFromOnlineOutputData(List<OnlineConfirmationHistoryModel> onlineConfirmationHistoryList, GetRegisterdPatientsFromOnlineStatus status)
    {
        OnlineConfirmationHistoryList = onlineConfirmationHistoryList;
        Status = status;
    }

    public List<OnlineConfirmationHistoryModel> OnlineConfirmationHistoryList { get; private set; }

    public GetRegisterdPatientsFromOnlineStatus Status { get; private set; }
}
