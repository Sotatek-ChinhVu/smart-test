using Domain.Models.JsonSetting;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class JsonSettingRepository : IJsonSettingRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public JsonSettingRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public JsonSettingModel? Get(int userId, string key)
    {
        var entity = _tenantDataContext.JsonSettings.FirstOrDefault(e => e.UserId == userId && e.Key == key);
        return entity is null ? null : ToModel(entity);
    }

    private JsonSettingModel ToModel(JsonSetting entity)
    {
        return new JsonSettingModel(entity.UserId, entity.Key, entity.Value);
    }
}
