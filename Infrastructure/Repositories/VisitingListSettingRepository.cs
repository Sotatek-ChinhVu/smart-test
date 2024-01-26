using Domain.Models.SystemConf;
using Domain.Models.VisitingListSetting;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class VisitingListSettingRepository : RepositoryBase, IVisitingListSettingRepository
{
    private readonly IDatabase _cache;
    private readonly string getListSystemConfigKey;
    private readonly IConfiguration _configuration;
    public VisitingListSettingRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        getListSystemConfigKey = GetDomainKey() + CacheKeyConstant.GetListSystemConf;
        _configuration = configuration;
        GetRedis();
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    public void GetRedis()
    {
        string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public void Save(List<SystemConfModel> systemConfModels, int hpId, int userId)
    {
        ModifySystemConfs(systemConfModels, hpId, userId);
        TrackingDataContext.SaveChanges();

        // Remove cache when save system setting
        if (_cache.KeyExists(getListSystemConfigKey))
        {
            _cache.KeyDelete(getListSystemConfigKey);
        }
    }

    private void ModifySystemConfs(List<SystemConfModel> confModels, int hpId, int userId)
    {
        var existingConfigs = TrackingDataContext.SystemConfs
            .Where(s => s.HpId == hpId && (s.GrpCd == SystemConfGroupCodes.ReceptionTimeColor
                || s.GrpCd == SystemConfGroupCodes.ReceptionStatusColor)).ToList();
        var configsToInsert = new List<SystemConf>();

        foreach (var model in confModels)
        {
            var configToUpdate = existingConfigs.FirstOrDefault(c => c.GrpCd == model.GrpCd && c.GrpEdaNo == model.GrpEdaNo);
            if (configToUpdate is not null)
            {
                if (configToUpdate.Param != model.Param)
                {
                    configToUpdate.Param = model.Param;
                    configToUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    configToUpdate.UpdateId = userId;
                }
            }
            else
            {
                configsToInsert.Add(new SystemConf
                {
                    HpId = hpId,
                    GrpCd = model.GrpCd,
                    GrpEdaNo = model.GrpEdaNo,
                    Val = model.Val,
                    Param = model.Param,
                    Biko = model.Biko,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateId = userId
                });
            }
        }

        TrackingDataContext.SystemConfs.AddRange(configsToInsert);

        var configsToDelete = existingConfigs.Where(c => !confModels.Exists(m => m.GrpCd == c.GrpCd && m.GrpEdaNo == c.GrpEdaNo));
        TrackingDataContext.SystemConfs.RemoveRange(configsToDelete);
    }
}
