using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetDrugCheckSetting;

public class GetDrugCheckSettingInputData : IInputData<GetDrugCheckSettingOutputData>
{
    public GetDrugCheckSettingInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
