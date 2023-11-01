using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveOdrSet;

public class SaveOdrSetInputData : IInputData<SaveOdrSetOutputData>
{
    public SaveOdrSetInputData(int hpId, int userId, int sinDate, List<OdrSetNameModel> setNameModelList, List<OdrSetNameModel> updateSetNameList)
    {
        HpId = hpId;
        UserId = userId;
        SinDate = sinDate;
        SetNameModelList = setNameModelList;
        UpdateSetNameList = updateSetNameList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinDate { get; private set; }

    public List<OdrSetNameModel> SetNameModelList { get; private set; }

    public List<OdrSetNameModel> UpdateSetNameList { get; private set; }
}
