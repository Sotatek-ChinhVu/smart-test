using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineInRaiinInf;

public class UpdateOnlineInRaiinInfInputData : IInputData<UpdateOnlineInRaiinInfOutputData>
{
    public UpdateOnlineInRaiinInfInputData(int hpId, int userId, long ptId, string onlineConfirmationDate, int confirmationType, string infConsFlg)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        OnlineConfirmationDate = onlineConfirmationDate;
        ConfirmationType = confirmationType;
        InfConsFlg = infConsFlg;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public string OnlineConfirmationDate { get; private set; }

    public int ConfirmationType { get; private set; }

    public string InfConsFlg { get; private set; }
}
