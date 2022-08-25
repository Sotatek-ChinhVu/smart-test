namespace Domain.Models.JsonSetting;

public class JsonSettingModel
{
    public JsonSettingModel(int userId, string key, string value)
    {
        UserId = userId;
        Key = key;
        Value = value;
    }

    public int UserId { get; private set; }
    public string Key { get; private set; }
    public string Value { get; private set; }
}
