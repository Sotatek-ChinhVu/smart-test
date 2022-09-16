using UseCase.Core.Sync.Core;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputData : IInputData<SaveSuperSetDetailOutputData>
{
    public SaveSuperSetDetailInputData(int setCd, int userId, int hpId, List<SaveSetByomeiInputItem> setByomeiModelInputs, SaveSetKarteInputItem saveSetKarteInputItem)
    {
        SetCd = setCd;
        UserId = userId;
        HpId = hpId;
        SetByomeiModelInputs = setByomeiModelInputs;
        SaveSetKarteInputItem = saveSetKarteInputItem;
    }

    public int SetCd { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<SaveSetByomeiInputItem> SetByomeiModelInputs { get; private set; }

    public SaveSetKarteInputItem SaveSetKarteInputItem { get; private set; }
}
