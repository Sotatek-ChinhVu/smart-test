using Domain.Common;

namespace Domain.Models.JsonSetting;

public interface IJsonSettingRepository : IRepositoryBase
{
    JsonSettingModel? Get(int userId, string key);
    void Upsert(JsonSettingModel model);
}
