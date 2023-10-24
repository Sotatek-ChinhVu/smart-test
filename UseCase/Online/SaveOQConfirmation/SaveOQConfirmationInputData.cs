using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOQConfirmation;

public class SaveOQConfirmationInputData : IInputData<SaveOQConfirmationOutputData>
{
    public SaveOQConfirmationInputData(int hpId, int userId, long onlineHistoryId, long ptId, string confirmationResult, string onlineConfirmationDateString, int confirmationType, string infConsFlg, int uketukeStatus, bool isUpdateRaiinInf)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        ConfirmationResult = confirmationResult;
        OnlineConfirmationDateString = onlineConfirmationDateString;
        ConfirmationType = confirmationType;
        InfConsFlg = infConsFlg;
        UketukeStatus = uketukeStatus;
        OnlineHistoryId = onlineHistoryId;
        IsUpdateRaiinInf = isUpdateRaiinInf;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long OnlineHistoryId { get; private set; }

    public long PtId { get; private set; }

    public string ConfirmationResult { get; private set; }

    public string OnlineConfirmationDateString { get; private set; }

    public int ConfirmationType { get; private set; }

    public string InfConsFlg { get; private set; }

    public int UketukeStatus { get; private set; }

    public bool IsUpdateRaiinInf { get; private set; }
}
