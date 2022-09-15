using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputData : IInputData<SaveSuperSetDetailOutputData>
{
    public SaveSuperSetDetailInputData(int setCd, int userId, int hpId, SaveSuperSetDetailInputItem saveSuperSetDetailInput)
    {
        SetCd = setCd;
        UserId = userId;
        HpId = hpId;
        SaveSuperSetDetailInput = saveSuperSetDetailInput;
    }

    public int SetCd { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public SaveSuperSetDetailInputItem SaveSuperSetDetailInput { get; private set; }
}
