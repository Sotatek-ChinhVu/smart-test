using UseCase.Core.Async.Core;

namespace UseCase.Online.SaveAllOQConfirmation;

public class SaveAllOQConfirmationInputData : IInputData<SaveAllOQConfirmationOutputData>
{
    public SaveAllOQConfirmationInputData(int hpId, int userId, long onlineHistoryId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict)
    {
        HpId = hpId;
        UserId = userId;
        OnlineHistoryId = onlineHistoryId;
        OnlQuaResFileDict = onlQuaResFileDict;
        OnlQuaConfirmationTypeDict = onlQuaConfirmationTypeDict;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long OnlineHistoryId { get; private set; }

    public Dictionary<string, string> OnlQuaResFileDict { get; private set; }

    public Dictionary<string, (int confirmationType, string infConsFlg)> OnlQuaConfirmationTypeDict { get; private set; }
}
