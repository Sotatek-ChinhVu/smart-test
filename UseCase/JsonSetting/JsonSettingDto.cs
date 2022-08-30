namespace UseCase.JsonSetting;

public class JsonSettingDto
{
    public JsonSettingDto(int userId, string key, object? value)
    {
        UserId = userId;
        Key = key;
        Value = value;
    }

    public int UserId { get; private set; }
    public string Key { get; private set; }
    public object? Value { get; private set; }
}
