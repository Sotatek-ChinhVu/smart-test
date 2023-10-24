using Domain.Models.Online;
using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetOnlineConsent;

public class GetOnlineConsentOutputData : IOutputData
{
    public GetOnlineConsentOutputData(GetOnlineConsentStatus status, List<OnlineConsentModel> onlineConsentList)
    {
        Status = status;
        OnlineConsentList = onlineConsentList;
    }

    public GetOnlineConsentStatus Status { get; private set; }

    public List<OnlineConsentModel> OnlineConsentList { get; private set; }
}
