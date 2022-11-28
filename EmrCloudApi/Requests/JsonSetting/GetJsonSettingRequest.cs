namespace EmrCloudApi.Requests.JsonSetting;

public class GetJsonSettingRequest
{
    public int UserId { get; set; }
    public string Key { get; set; } = string.Empty;
}
