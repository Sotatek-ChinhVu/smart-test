using UseCase.JsonSetting;

namespace EmrCloudApi.Responses.JsonSetting;

public class GetAllJsonSettingResponse
{
    public GetAllJsonSettingResponse(List<JsonSettingDto> settings)
    {
        Settings = settings;
    }

    public List<JsonSettingDto> Settings { get; private set; }
}
