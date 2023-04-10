using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetDrugCheckSetting;
public class GetDrugCheckSettingOutputData : IOutputData
{
    public GetDrugCheckSettingOutputData(DrugCheckSettingItem drugCheckSettingData, GetDrugCheckSettingStatus status)
    {
        DrugCheckSettingData = drugCheckSettingData;
        Status = status;
    }

    public DrugCheckSettingItem DrugCheckSettingData { get; private set; }

    public GetDrugCheckSettingStatus Status { get; private set; }
}
