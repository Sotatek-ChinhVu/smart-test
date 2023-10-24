using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveOdrSet;

public class SaveOdrSetInputData : IInputData<SaveOdrSetOutputData>
{
    public SaveOdrSetInputData(int hpId, int userId, int sinDate, List<OdrSetNameModel> setNameModelList)
    {
        HpId = hpId;
        UserId = userId;
        SinDate = sinDate;
        SetNameModelList = setNameModelList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinDate { get; private set; }

    public List<OdrSetNameModel> SetNameModelList { get; private set; }
}
