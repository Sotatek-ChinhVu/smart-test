using Domain.Common;

namespace Domain.Models.JsonSetting;

public interface IJsonSettingRepository : IRepositoryBase
{
    JsonSettingModel? Get(int hpId, int userId, string key);

    void Upsert(JsonSettingModel model);

    List<JsonSettingModel> GetListFollowUserId(int hpId, int userId);
}
