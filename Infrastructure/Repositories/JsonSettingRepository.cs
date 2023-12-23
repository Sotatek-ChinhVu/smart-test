using Domain.Models.JsonSetting;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class JsonSettingRepository : RepositoryBase, IJsonSettingRepository
{
    public JsonSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public JsonSettingModel? Get(int userId, string key)
    {
        var entity = NoTrackingDataContext.JsonSettings.FirstOrDefault(e => e.UserId == userId && e.Key == key);
        return entity is null ? null : ToModel(entity);
    }

    public List<JsonSettingModel> GetListFollowUserId(int userId)
    {
        var entities = NoTrackingDataContext.JsonSettings.Where(e => e.UserId == userId).ToList();
        return entities?.Select(e => ToModel(e)).ToList() ?? new();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public void Upsert(JsonSettingModel model)
    {
        var existingEntity = TrackingDataContext.JsonSettings.AsTracking()
            .FirstOrDefault(e => e.UserId == model.UserId && e.Key == model.Key);
        if (existingEntity is null)
        {
            TrackingDataContext.JsonSettings.Add(new JsonSetting
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

        TrackingDataContext.SaveChanges();
    }

    private JsonSettingModel ToModel(JsonSetting entity)
    {
        return new JsonSettingModel(entity.UserId, entity.Key, entity.Value);
    }
}
