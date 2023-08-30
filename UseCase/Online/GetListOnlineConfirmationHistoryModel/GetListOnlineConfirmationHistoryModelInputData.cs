using UseCase.Core.Sync.Core;

namespace UseCase.Online.GetListOnlineConfirmationHistoryModel;

public class GetListOnlineConfirmationHistoryModelInputData : IInputData<GetListOnlineConfirmationHistoryModelOutputData>
{
    public GetListOnlineConfirmationHistoryModelInputData(long ptId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict)
    {
        PtId = ptId;
        OnlQuaResFileDict = onlQuaResFileDict;
        OnlQuaConfirmationTypeDict = onlQuaConfirmationTypeDict;
    }

    public long PtId { get; private set; }

    public Dictionary<string, string> OnlQuaResFileDict { get; private set; }

    public Dictionary<string, (int confirmationType, string infConsFlg)> OnlQuaConfirmationTypeDict { get; private set; }
}
