using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SaveDrugCheckSetting;

public class SaveDrugCheckSettingInputData : IInputData<SaveDrugCheckSettingOutputData>
{
    public SaveDrugCheckSettingInputData(int hpId, int userId, DrugCheckSettingItem drugCheckSetting)
    {
        HpId = hpId;
        UserId = userId;
        DrugCheckSetting = drugCheckSetting;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public DrugCheckSettingItem DrugCheckSetting { get; private set; }
}
