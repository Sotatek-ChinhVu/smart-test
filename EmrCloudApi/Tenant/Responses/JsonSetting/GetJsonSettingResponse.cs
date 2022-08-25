using UseCase.JsonSetting;

namespace EmrCloudApi.Tenant.Responses.JsonSetting;

public class GetJsonSettingResponse
{
    public GetJsonSettingResponse(JsonSettingDto? setting)
    {
        Setting = setting;
    }

    public JsonSettingDto? Setting { get; private set; }
}
