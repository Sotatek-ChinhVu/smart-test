using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetOdrSetName;

public class GetOdrSetNameInputData : IInputData<GetOdrSetNameOutputData>
{
    public GetOdrSetNameInputData(int hpId, SetCheckBoxStatusModel checkBoxStatus, int timeExpired, string itemName)
    {
        HpId = hpId;
        CheckBoxStatus = checkBoxStatus;
        TimeExpired = timeExpired;
        ItemName = itemName;
    }

    public int HpId { get; private set; }

    public SetCheckBoxStatusModel CheckBoxStatus { get; private set; }

    public int TimeExpired { get; private set; }

    public string ItemName { get; private set; }
}
