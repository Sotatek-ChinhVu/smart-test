using UseCase.Core.Sync.Core;

namespace UseCase.JsonSetting.Get;

public class GetJsonSettingOutputData : IOutputData
{
    public GetJsonSettingOutputData(GetJsonSettingStatus status, JsonSettingDto? setting)
    {
        Status = status;
        Setting = setting;
    }

    public GetJsonSettingStatus Status { get; private set; }
    public JsonSettingDto? Setting { get; private set; }
}
