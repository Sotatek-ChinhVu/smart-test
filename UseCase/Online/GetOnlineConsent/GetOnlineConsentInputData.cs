using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetOnlineConsent;

public class GetOnlineConsentInputData : IInputData<GetOnlineConsentOutputData>
{
    public GetOnlineConsentInputData(long ptId)
    {
        PtId = ptId;
    }

    public long PtId { get; private set; }
}
