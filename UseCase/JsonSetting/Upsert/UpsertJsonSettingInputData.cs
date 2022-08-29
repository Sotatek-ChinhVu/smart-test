using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.Upsert;

public class UpsertJsonSettingInputData : IInputData<UpsertJsonSettingOutputData>
{
    public UpsertJsonSettingInputData(JsonSettingDto setting)
    {
        Setting = setting;
    }

    public JsonSettingDto Setting { get; private set; }
}
