namespace Domain.Models.JsonSetting;

public interface IJsonSettingRepository
{
    JsonSettingModel? Get(int userId, string key);
}
