using Domain.Models.Online;
using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetListOnlineConfirmationHistoryModel;

public class GetListOnlineConfirmationHistoryModelOutputData : IOutputData
{
    public GetListOnlineConfirmationHistoryModelOutputData(List<OnlineConfirmationHistoryModel> onlineConfirmationHistoryList, GetListOnlineConfirmationHistoryModelStatus status)
    {
        OnlineConfirmationHistoryList = onlineConfirmationHistoryList;
        Status = status;
    }

    public List<OnlineConfirmationHistoryModel> OnlineConfirmationHistoryList { get; private set; }

    public GetListOnlineConfirmationHistoryModelStatus Status { get; private set; }
}
