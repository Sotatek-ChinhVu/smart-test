using UseCase.JsonSetting;

namespace EmrCloudApi.Tenant.Requests.JsonSetting;

public class UpsertJsonSettingRequest
{
    public JsonSettingDto Setting { get; set; } = null!;
}
