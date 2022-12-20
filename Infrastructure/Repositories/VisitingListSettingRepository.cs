using Domain.Models.SystemConf;
using Domain.Models.VisitingListSetting;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class VisitingListSettingRepository : RepositoryBase, IVisitingListSettingRepository
{
    public VisitingListSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {

    }

    public void Save(List<SystemConfModel> systemConfModels, int hpId, int userId)
    {
        ModifySystemConfs(systemConfModels, hpId, userId);
        TrackingDataContext.SaveChanges();
    }

    private void ModifySystemConfs(List<SystemConfModel> confModels, int hpId, int userId)
    {
        var existingConfigs = TrackingDataContext.SystemConfs
            .Where(s => s.GrpCd == SystemConfGroupCodes.ReceptionTimeColor
                || s.GrpCd == SystemConfGroupCodes.ReceptionStatusColor).ToList();
        var configsToInsert = new List<SystemConf>();

        foreach (var model in confModels)
        {
            var configToUpdate = existingConfigs.FirstOrDefault(c => c.GrpCd == model.GrpCd && c.GrpEdaNo == model.GrpEdaNo);
            if (configToUpdate is not null)
            {
                if (configToUpdate.Param != model.Param)
                {
                    configToUpdate.Param = model.Param;
                    configToUpdate.UpdateDate = DateTime.UtcNow;
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
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
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
