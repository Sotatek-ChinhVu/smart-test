using UseCase.Core.Sync.Core;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class SaveSuperSetDetailInputData : IInputData<SaveSuperSetDetailOutputData>
{
    public SaveSuperSetDetailInputData(int setCd, int userId, int hpId, List<SaveSetByomeiInputItem> setByomeiModelInputs, SaveSetKarteInputItem saveSetKarteInputItem, List<SaveSetOrderInfInputItem> saveSetOrderInputItems)
    {
        SetCd = setCd;
        UserId = userId;
        HpId = hpId;
        SetByomeiModelInputs = setByomeiModelInputs;
        SaveSetKarteInputItem = saveSetKarteInputItem;
        SaveSetOrderInputItems = saveSetOrderInputItems;
    }

    public int SetCd { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<SaveSetByomeiInputItem> SetByomeiModelInputs { get; private set; }

    public SaveSetKarteInputItem SaveSetKarteInputItem { get; private set; }

    public List<SaveSetOrderInfInputItem> SaveSetOrderInputItems { get; private set; }
}
