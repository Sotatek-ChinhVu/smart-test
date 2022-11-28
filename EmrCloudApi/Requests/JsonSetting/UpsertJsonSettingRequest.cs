using UseCase.JsonSetting;

namespace EmrCloudApi.Requests.JsonSetting;

public class UpsertJsonSettingRequest
{
    public JsonSettingDto Setting { get; set; } = null!;
}
