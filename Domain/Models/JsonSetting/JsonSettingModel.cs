namespace Domain.Models.JsonSetting;

public class JsonSettingModel
{
    public JsonSettingModel(int hpId, int userId, string key, string value)
    {
        HpId = hpId;
        UserId = userId;
        Key = key;
        Value = value;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public string Key { get; private set; }
    public string Value { get; private set; }
}
