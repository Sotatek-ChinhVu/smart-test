using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOQConfirmation;

public class UpdateOQConfirmationInputData : IInputData<UpdateOQConfirmationOutputData>
{
    public UpdateOQConfirmationInputData(int hpId, int userId, long onlineHistoryId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg, int prescriptionIssueType)> onlQuaConfirmationTypeDict)
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

    public Dictionary<string, (int confirmationType, string infConsFlg, int prescriptionIssueType)> OnlQuaConfirmationTypeDict { get; private set; }
}
