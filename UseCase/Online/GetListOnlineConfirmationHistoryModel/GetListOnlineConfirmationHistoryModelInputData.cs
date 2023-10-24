using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetListOnlineConfirmationHistoryModel;

public class GetListOnlineConfirmationHistoryModelInputData : IInputData<GetListOnlineConfirmationHistoryModelOutputData>
{
    public GetListOnlineConfirmationHistoryModelInputData(int userId, long ptId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict)
    {
        UserId = userId;
        PtId = ptId;
        OnlQuaResFileDict = onlQuaResFileDict;
        OnlQuaConfirmationTypeDict = onlQuaConfirmationTypeDict;
    }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public Dictionary<string, string> OnlQuaResFileDict { get; private set; }

    public Dictionary<string, (int confirmationType, string infConsFlg)> OnlQuaConfirmationTypeDict { get; private set; }
}
