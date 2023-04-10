using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SaveDrugCheckSetting;

public class SaveDrugCheckSettingOutputData : IOutputData
{
    public SaveDrugCheckSettingOutputData(SaveDrugCheckSettingStatus status)
    {
        Status = status;
    }

    public SaveDrugCheckSettingStatus Status { get; private set; }
}
