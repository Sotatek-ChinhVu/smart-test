using Domain.Models.JsonSetting;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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

    public void Upsert(JsonSettingModel model)
    {
        var existingEntity = _tenantDataContext.JsonSettings.AsTracking()
            .FirstOrDefault(e => e.UserId == model.UserId && e.Key == model.Key);
        if (existingEntity is null)
        {
            _tenantDataContext.JsonSettings.Add(new JsonSetting
            {
                UserId = model.UserId,
                Key = model.Key,
                Value = model.Value
            });
        }
        else
        {
            existingEntity.Value = model.Value;
        }

        _tenantDataContext.SaveChanges();
    }

    private JsonSettingModel ToModel(JsonSetting entity)
    {
        return new JsonSettingModel(entity.UserId, entity.Key, entity.Value);
    }
}
