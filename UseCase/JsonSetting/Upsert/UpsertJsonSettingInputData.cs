using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.Upsert;

public class UpsertJsonSettingInputData : IInputData<UpsertJsonSettingOutputData>
{
    public UpsertJsonSettingInputData(int hpId, JsonSettingDto setting)
    {
        HpId = hpId;
        Setting = setting;
    }
    public int HpId { get; private set; }

    public JsonSettingDto Setting { get; private set; }
}
